﻿using Annytab.Stemmer;
using CodeGraph.Domain.Graph.Nodes;
using CodeGraph.Domain.Graph.Triples;
using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

namespace CodeGraph.Domain.Dotnet.OriginalImplementation
{
    public class CSharpCodeAnalyzer(SyntaxTree syntaxTree, SemanticModel semanticModel, FileNode fileNode) : IAnalyzer
    {
        private readonly IStemmer _stemmer = new EnglishStemmer();

        public async Task<IList<Triple>> Analyze()
        {
            List<Triple> triples = new();
            triples.AddRange(await AnalyzeInterfaces());
            triples.AddRange(await AnalyzeClasses());

            return triples;
        }

        private async Task<IList<Triple>> AnalyzeInterfaces()
        {
            List<Triple> triples = new();

            SyntaxNode root = await syntaxTree.GetRootAsync();
            IEnumerable<InterfaceDeclarationSyntax> declarations =
                root
                    .DescendantNodes()
                    .OfType<InterfaceDeclarationSyntax>();

            foreach (InterfaceDeclarationSyntax dec in declarations)
            {
                ISymbol? symbol = semanticModel.GetDeclaredSymbol(dec);
                if (symbol == null) continue;

                TypeNode? node = CreateTypeNode(symbol, dec);
                (string fullName, string name) = (symbol?.ContainingNamespace.ToString() + '.' + symbol?.Name,
                    symbol?.Name!);

                triples.Add(new TripleDeclaredAt(node!, fileNode));
                triples.AddRange(WordTriples(name, node));
                triples.AddRange(GetInherits(dec, semanticModel, node!));
                triples.AddRange(GetMethodsAll(dec, semanticModel, node));
            }

            return triples;
        }


        private async Task<IList<Triple>> AnalyzeClasses()
        {
            List<Triple> triples = new();

            SyntaxNode root = await syntaxTree.GetRootAsync();
            IEnumerable<ClassDeclarationSyntax> classDeclarations =
                root
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>();

            foreach (ClassDeclarationSyntax dec in classDeclarations)
            {
                ISymbol? symbol = semanticModel.GetDeclaredSymbol(dec);
                if (symbol == null) continue;

                TypeNode? node = CreateTypeNode(symbol, dec);
                (string fullName, string name) = (symbol?.ContainingNamespace.ToString() + '.' + symbol?.Name,
                    symbol?.Name!);

                triples.Add(new TripleDeclaredAt(node!, fileNode));
                triples.AddRange(WordTriples(name, node));
                triples.AddRange(GetInherits(dec, semanticModel, node));
                triples.AddRange(GetMethodsAll(dec, semanticModel, node));
            }

            return triples;
        }

        private static TypeNode? CreateTypeNode(ISymbol symbol, TypeDeclarationSyntax declaration)
        {
            (string fullName, string name) = (symbol.ContainingNamespace.ToString() + '.' + symbol.Name, symbol.Name);
            return declaration switch
            {
                ClassDeclarationSyntax _ => new ClassNode(fullName, name, MapModifiers(declaration.Modifiers)),
                InterfaceDeclarationSyntax _ => new InterfaceNode(fullName, name, MapModifiers(declaration.Modifiers)),
                _ => null
            };
        }

        private static IEnumerable<Triple> GetInherits(TypeDeclarationSyntax declaration,
            SemanticModel sem,
            TypeNode node)
        {
            if (declaration.BaseList == null) yield break;

            foreach (BaseTypeSyntax baseTypeSyntax in declaration.BaseList.Types)
            {
                TypeSyntax baseType = baseTypeSyntax.Type;
                TypeNode parentNode = sem.GetTypeInfo(baseType).CreateTypeNode();
                switch (node)
                {
                    case ClassNode classNode:
                        yield return new TripleOfType(classNode, parentNode);
                        break;
                    case InterfaceNode interfaceNode when parentNode is InterfaceNode parentInterfaceNode:
                        yield return new TripleOfType(interfaceNode, parentInterfaceNode);
                        break;
                }
            }
        }

        private IEnumerable<Triple> GetMethodsAll(TypeDeclarationSyntax declaration,
            SemanticModel sem,
            TypeNode node)
        {
            IEnumerable<MethodDeclarationSyntax> methods =
                declaration.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (MethodDeclarationSyntax method in methods)
            {
                IMethodSymbol methodSymbol = CSharpExtensions.GetDeclaredSymbol(sem, method)!;
                MethodNode methodNode = CreateMethodNode(methodSymbol, method);

                foreach (Triple wordTriple in WordTriples(methodNode.Name, methodNode))
                {
                    yield return wordTriple;
                }

                yield return new TripleHave(node, methodNode);

                // List<ExpressionSyntax> expressions = method
                //     .DescendantNodes()
                //     .OfType<ExpressionSyntax>()
                //     .ToList();
                //
                // foreach (ExpressionSyntax syntax in expressions)
                // {
                //     if (TryGetExpressionInvocation(sem, syntax, methodNode, out Triple triple)) yield return triple;
                // }
            }
        }

        private static bool TryGetExpressionInvocation(SemanticModel sem, ExpressionSyntax syntax,
            MethodNode methodNode,
            out Triple triple)
        {
            triple = null!;

            switch (syntax)
            {
                case ObjectCreationExpressionSyntax creation:
                    ClassNode classNode = sem.GetTypeInfo(creation).CreateClassNode();
                    triple = new TripleConstruct(methodNode, classNode);
                    return true;

                case InvocationExpressionSyntax invocation:
                    if (sem.GetSymbolInfo(invocation).Symbol is not IMethodSymbol invokedSymbol) return false;
                    MethodNode invokedMethod = CreateMethodNode(invokedSymbol);
                    triple = new TripleInvoke(methodNode, invokedMethod);

                    return true;
            }

            return false;
        }

        private static MethodNode CreateMethodNode(IMethodSymbol symbol, MethodDeclarationSyntax declaration = null)
        {
            string fullName =
                symbol
                    .ContainingNamespace
                    .GetNamespaceName($"{symbol.ContainingType.Name}.{symbol.Name}");

            (string name, string? type)[] args = symbol.Parameters.Select(x => (name: x.Name, type: x.Type.ToString()))
                .ToArray();

            string? returnType = symbol.ReturnType.ToString();

            return new MethodNode(fullName,
                symbol.Name,
                args,
                returnType,
                MapModifiers(declaration.Modifiers));
        }

        private static string[] MapModifiers(SyntaxTokenList syntaxTokens)
        {
            return syntaxTokens.Select(x => x.ValueText).ToArray();
        }

        private IEnumerable<Triple> WordTriples(string name, TypeNode? node)
        {
            IEnumerable<string> words = name.SplitStringOnCapitals();

            foreach (string word in words.Select(w => w.ToLower()))
            {
                WordNode wordNode = new(word, word);
                yield return new TripleUsesWord(node, wordNode);

                string root = (_stemmer.GetSteamWord(word) ?? word).ToLower();
                yield return new TripleWordDerivation(wordNode, new WordRootNode(root, root));
            }
        }

        private IEnumerable<Triple> WordTriples(string name, MethodNode? node)
        {
            IEnumerable<string> words = name.SplitStringOnCapitals();

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