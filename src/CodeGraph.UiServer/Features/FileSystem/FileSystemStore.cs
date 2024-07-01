using System.Text.Json;
using CodeGraph.Clients;
using CodeGraph.Clients.Dto.FileSystem;
using Fluxor;

namespace CodeGraph.UiServer.Features.FileSystem
{
    public record FileSystemState
    {
        public bool Initialized { get; init; }
        public bool Loading { get; init; }
        public FileSystemHierarchyDto[] FileSystemHierarchies { get; init; }
    }

    public class FileSystemFeature : Feature<FileSystemState>
    {
        public override string GetName()
        {
            return "FileSystem";
        }

        protected override FileSystemState GetInitialState()
        {
            return new FileSystemState
            {
                Initialized = false,
                Loading = false,
                FileSystemHierarchies = []
            };
        }
    }

    public static class FileSystemReducers
    {
        [ReducerMethod]
        public static FileSystemState OnSetForecasts(FileSystemState state, FileSystemSetHierarchyAction action)
        {
            return state with
            {
                FileSystemHierarchies = action.Hierarchies,
                Loading = false
            };
        }

        [ReducerMethod(typeof(FileSystemSetInitializedAction))]
        public static FileSystemState OnSetInitialized(FileSystemState state)
        {
            return state with
            {
                Initialized = true
            };
        }

        [ReducerMethod(typeof(FileSystemLoadHierarchyAction))]
        public static FileSystemState OnLoadForecasts(FileSystemState state)
        {
            return state with
            {
                Loading = true
            };
        }
    }

    public class FileSystemEffects(CodeGraphFileSystemClient fileSystemClient, ILogger<FileSystemEffects> logger)
    {
        [EffectMethod(typeof(FileSystemLoadHierarchyAction))]
        public async Task LoadFileSystemHierarchies(IDispatcher dispatcher)
        {
            List<FileSystemHierarchyDto> hierarchies = await fileSystemClient.GetHierarchy();
            
            dispatcher.Dispatch(new FileSystemSetHierarchyAction(hierarchies.ToArray()));
        }
    }

    #region FileSystemActions

    public class FileSystemSetInitializedAction
    {
    }

    public class FileSystemLoadHierarchyAction
    {
    }

    public class FileSystemSetHierarchyAction
    {
        public FileSystemSetHierarchyAction(FileSystemHierarchyDto[] hierarchies)
        {
            Hierarchies = hierarchies;
        }

        public FileSystemHierarchyDto[] Hierarchies { get; }
    }

    #endregion
}