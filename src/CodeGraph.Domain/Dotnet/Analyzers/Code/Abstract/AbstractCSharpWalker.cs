﻿using CodeGraph.Domain.Graph.Triples.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGraph.Domain.Dotnet.Analyzers.Code.Abstract
{
    public abstract class AbstractCSharpWalker : CSharpSyntaxWalker, ICodeWalker
    {
        private readonly Compilation _compilation;
        private readonly Document _document;

        protected AbstractCSharpWalker(ICodeWalkerFactory codeWalkerFactory, Document document, Compilation compilation)
        {
            _document = document;
            _compilation = compilation;
        }
        
        public abstract IEnumerable<Triple> Walk();
    }
}