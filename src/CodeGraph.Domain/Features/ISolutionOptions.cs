﻿using CommandLine;

namespace CodeGraph.Domain.Features
{
    public interface ISolutionOptions
    {
        [Option('s', nameof(Solution), Required = true, HelpText = "Path to solution file")]
        public string Solution { get; set; }
    }
}