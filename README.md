# CodeGraph

CodeGraph is a comprehensive code analysis tool for dotnet code bases. It is intended to help with:

* Understanding large projects
* Documenting large projects
* Finding aspects of a system which multiple features depend upon
* Discovery of domain terms and concepts encapsulated within a large project
* Understanding which features rely on complex, hard to maintain code
* Provide assistance with:
    * migrating monoliths to a modular monoliths
    * break up monoliths into microservices based on Domain Driven Design's *bounded contexts*

CodeGraph uses Roslyn to read and understand the contents of a dotnet solution and store the results in a graph database, Neo4j.

It is very much a work in progress. 

## Completed 
* The [data model](src/docs/data_model.md) exists and is well tested. 
* Fully importing a wholly CSharp solution
* Generating sequence diagrams from method invocations

## To Do
* Support for multiple solutions
* Visual Basic Support
* An API with Blazor UI for interactive analysis

## Documentation

* [How to Run](src/docs/how_to_run.md)
* [Data Model](src/docs/data_model.md)


