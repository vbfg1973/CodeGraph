﻿using CodeGraph.Domain.Graph.TripleDefinitions.Nodes;
using CodeGraph.Domain.Graph.TripleDefinitions.Relationships;
using CodeGraph.Domain.Graph.TripleDefinitions.Triples.Abstract;

namespace CodeGraph.Domain.Graph.TripleDefinitions.Triples
{
    public class TripleDependsOnProject : Triple
    {
        public TripleDependsOnProject(
            ProjectNode projectA,
            ProjectNode projectB)
            : base(projectA, projectB, new DependsOnRelationship())
        {
        }
    }
}