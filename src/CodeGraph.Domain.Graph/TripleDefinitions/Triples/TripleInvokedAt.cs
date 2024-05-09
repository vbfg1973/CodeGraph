﻿using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleInvokedAt : Triple
    {
        public TripleInvokedAt(
            InvocationNode invocationA,
            InvocationLocationNode invocationLocationNodeB)
            : base(invocationA, invocationLocationNodeB, new InvokedAtRelationship())
        {
        }
    }
}