# CodeGraph Data Model

## Currently being stored

* Projects
* Packages
* Files and directories
* Types
* Classes
    * Interfaces
    * Records
    * Structs
    * Methods
    * Properties
* Invocations of methods
* Implementations of interfaces by classes defined in the project
* Words used in the names of any of the above types
* Stemmed words, so that “book”, “booking”, “booked”, etc can be linked as related concepts

## Relationships

All relationships are expressed in neo4j style syntax. Images to follow.

### Filesystem

* (Folder)-[:INCLUDED_IN]->(Folder)
* (File)-[:INCLUDED_IN]->(Folder)


* (Class)-[:DECLARED_AT]->(File)
* (Interface)-[:DECLARED_AT]->(File)
* (Record)-[:DECLARED_AT]->(File)

### Projects

* (Project)-[:DEPENDS_ON]->(Project)
* (Project)-[:DEPENDS_ON]->(Package)


* (Class)-[:BELONGS_TO]->(Project)
* (Interface)-[:BELONGS_TO]->(Project)
* (Record)-[:BELONGS_TO]->(Project)

### Dotnet Definitions

* (Class)-[:HAS]->(Method)
* (Interface)-[:HAS]->(Method)
* (Record)-[:HAS]->(Method)


* (Class)-[:HAS]->(Property)
* (Interface)-[:HAS]->(Property)
* (Record)-[:HAS]->(Property)


* (Class)-[:IMPLEMENTS]->(Method)

### Dotnet Code Flow

* (Method)-[:CONSTRUCTS]->(Class)
* (Method)-[:CONSTRUCTS]->(Record)


* (Method)-[:INVOKES]->(Invocation)
* (Invocation)-[:INVOKED_AT]->(InvocationLocation)
* (Invocation)-[:INVOCATION_OF]->(Method)

### Conceptual

* (Class)-[:HAS_WORD_IN_NAME]->(Word)
* (Interface)-[:HAS_WORD_IN_NAME]->(Word)
* (Record)-[:HAS_WORD_IN_NAME]->(Word)
* (Method)-[:HAS_WORD_IN_NAME]->(Word)
* (Property)-[:HAS_WORD_IN_NAME]->(Word)


* (Word)-[:WORD_DERIVES_FROM]->(WordRoot)