using System.Text;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Models;
using CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Abstract;

namespace CodeGraph.Domain.Features.SequenceUml.SequenceGenerator.Services.Strategies
{
    public class PlantUmlSequenceUmlGenerator : ISequenceUmlGenerator
    {
        private readonly HashSet<string> _namesHashSet = new();

        public async Task<string> GenerateUmlDescription(MethodInvocationHierarchy methodInvocationHierarchy)
        {
            StringBuilder sb = new();

            sb.AppendLine("@startuml");

            sb.AppendLine(GetParticipantDefinitions(methodInvocationHierarchy));

            sb.AppendLine(GetCalls(methodInvocationHierarchy));

            sb.AppendLine("@enduml");

            return sb.ToString();
        }

        private static string GetName(string dottedName)
        {
            return dottedName.Split('.').Last();
        }

        #region Participants

        private string GetParticipantDefinitions(MethodInvocationHierarchy methodInvocationHierarchy)
        {
            GetParticipantNamesInOrder(methodInvocationHierarchy);

            StringBuilder sb = new();

            foreach (string participant in _namesHashSet)
            {
                sb.AppendLine($"participant {participant} as {participant}");
            }

            return sb.ToString();
        }

        private void GetParticipantNamesInOrder(MethodInvocationHierarchy methodInvocationHierarchy)
        {
            string name = GetName(methodInvocationHierarchy.ParentTypeFullname);

            _namesHashSet.Add(name);

            foreach (MethodInvocationHierarchy invocationHierarchy in methodInvocationHierarchy.MethodInvocations)
            {
                GetParticipantNamesInOrder(invocationHierarchy);
            }
        }

        #endregion

        #region Calls

        private string GetCalls(MethodInvocationHierarchy methodInvocationHierarchy)
        {
            StringBuilder sb = new();
            foreach (MethodInvocationHierarchy invocationHierarchy in methodInvocationHierarchy.MethodInvocations)
            {
                sb.Append(GetCalls(methodInvocationHierarchy, invocationHierarchy));
            }

            return sb.ToString();
        }

        private string GetCalls(MethodInvocationHierarchy parent, MethodInvocationHierarchy called)
        {
            StringBuilder sb = new();

            sb.AppendLine($"{GetName(parent.ParentTypeFullname)} -> {GetName(called.ParentTypeFullname)} : {GetName(called.MethodFullName)}");

            foreach (var invocationHierarchy in called.MethodInvocations)
            {
                sb.Append(GetCalls(called, invocationHierarchy));
            }
            
            if (!parent.ParentTypeFullname.Equals(called.ParentTypeFullname))
            {
                sb.AppendLine($"{GetName(called.ParentTypeFullname)} --> {GetName(parent.ParentTypeFullname)} : {called.MethodReturnType}");
            }
            return sb.ToString();
        }

        #endregion
    }
}