using Annytab.Stemmer;
using CodeGraph.Domain.Common;
using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Nodes.Abstract;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGraph.Domain.Dotnet.Walkers.CSharp
{
    public abstract class CSharpBaseTypeWalker(WalkerOptions walkerOptions) : CSharpSyntaxWalker
    {
        private readonly IStemmer _stemmer = new EnglishStemmer();
        protected readonly WalkerOptions _walkerOptions = walkerOptions;

        protected TypeNode GetTypeNode(TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return _walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(typeDeclarationSyntax)!
                .CreateTypeNode(typeDeclarationSyntax);
        }

        protected MethodNode GetMethodNode(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return _walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(methodDeclarationSyntax)!
                .CreateMethodNode();
        }

        protected PropertyNode GetPropertyNode(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return _walkerOptions
                .DotnetOptions
                .SemanticModel
                .GetDeclaredSymbol(propertyDeclarationSyntax)!
                .CreatePropertyNode();
        }

        protected IEnumerable<Triple> WordTriples(CodeNode node)
        {
            IEnumerable<string> words = node.Name.SplitStringOnCapitals();

            foreach (string word in words.Select(w => w.ToLower()))
            {
                WordNode wordNode = new(word, word);
                yield return new TripleUsesWord(node, wordNode);

                string root = (_stemmer.GetSteamWord(word) ?? word).ToLower();
                yield return new TripleWordDerivation(wordNode, new WordRootNode(root, root));
            }
        }
    }
}