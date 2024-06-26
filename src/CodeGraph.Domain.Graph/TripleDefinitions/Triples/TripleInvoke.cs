﻿using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleInvoke : Triple
    {
        public TripleInvoke(
            MethodNode methodA,
            InvocationNode invocationNodeB)
            : base(methodA, invocationNodeB, new InvokesRelationship())
        {
        }
    }
}