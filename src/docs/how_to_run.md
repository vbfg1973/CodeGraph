# How to Run

At present, only Neo4j is inside a docker container. When an API is added that too will be put inside a container and will be runnable in the same way. The command line interface project, which will be used for importing solutions into the database, will also be containerised.

To start the database:

`docker-compose up`

# Building and running

Until the CLI is containerised, the only option is to build and run manually. It is not possible to run with `dotnet run` as numerous switches are supported by the application and this will only lead to confusion. It must be built in advance, and then run.

The command line interface is defined in the project CodeGraph, located at `src/CodeGraph`. 

Navigate to this folder and run `dotnet build`. 

Run

`./bin/Debug/net8.0/CodeGraph`

This will list the currently available features.

# Importing a solution

`./bin/Debug/net8.0/CodeGraph solution -s <path_to_solution.sln>`

# Generating a PlantUML Sequence diagram

`./bin/Debug/net8.0/CodeGraph sequence -m {namespace}.{classname}.{method}`

Find the class with the method you are interested in. Let's say it is a WebAPI controller, `StudentController` in a project called `UniversityEnrollments.Api`. We are interested in the method, `EnrolOnCourse`.

Probable invocation is then:

`./bin/Debug/net8.0/CodeGraph sequence -m UniversityEnrollments.Api.Controllers.StudentController.EnrolOnCourse`

The `EnrolOnCourse` method will be found in the database. Any method invocations made by `EnrolOnCourse` will be found, and this will then be repeated recursively to build a full call stack of the entire supporting cast of services and methods involved in enrolling a student on a  course. 