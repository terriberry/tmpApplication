# Introduction

For our GitBook guide on building APIs with the Core Template, please look [here](https://app.gitbook.com/o/-MhAHQRNbXRJJAmyAX--/s/wlBDkDmB9NnayT8MEoDa/the-delta-code-smith/the-template-runbooks/custom-logic-apis/core-api).

Below are summaries of the basic concepts relevant to the project:
## Core API High Level Architecture
<img src="https://user-images.githubusercontent.com/98895450/181212910-4e28d10b-b944-4c8b-bb56-52f424480aaa.png" />

**Presentation (API) Layer:** This layer is the part where interaction with external systems happens. Requests will be accepted from this layer and the response will be shaped in this layer and displayed to the user.

**Application Layer:** Mediates interactions between the Presentation and Domain Layers. Orchestrates domain entities to perform specific application tasks. Implements use cases as the application logic.

**Domain Layer:** Includes (domain) entities and the core (domain) business rules. This is the heart of the application.

**Infrastructure Layer:** Provides generic technical capabilities that support higher layers mostly using 3rd-party libraries or services. This includes gateways, repositories and/or providers.  

## Dependency Rules

Layers should only have inward dependencies. This ensures that layers are decoupled.

**Presentation (API) Layer:** May depend on Infrastructure / Application / Domain

**Infrastructure Layer:** May depend on Application / Domain

**Application Layer:** May depend on the Domain

**Domain Layer:** May have no other dependencies

Through inversion of control and dependency injection, all Domain and Application level "dependencies" will only be leveraged though Interface contracts.

[Dependency Inversion Principle](https://deviq.com/principles/dependency-inversion-principle)

[Dependency Injection](https://deviq.com/practices/dependency-injection)

## Presentation (API) Layer

The presentation layer in an API project defines how other services interact with your service. Although most implementations will be REST based APIs with JSON payloads, other interfaces should still be able to function without any rework of the Application and Domain layers.

As an example, REST considerations that should not bleed into any other parts of the solutions are:

Http verbs such as PUT, POST, GET etc.

Http response codes such as 200, 201, 400 or 500

JSON as strings or any pre-serialised data

## Infrastructure Layer

This is the layer where Gateways (connections to 3rd party APIs) and Persistence (Database) concerns are handled. If you want to create a new gateway, this should be done as a new project in the Infrastructure folder. Repository implementations are defined in the Persistence project. You can generate a readonly or readwrite repository depending on if your application should have access to write to the data backed by the repository.

## Application Layer

This is the layer where business process flows are handled. To maintain a neat and clear Domain, it is vitally important to keep UI, UX and Processes excluded from the Domain. If you have a requirement that involves consolidating requests and responses, the Application layer allows for the addition of process specific implementation without adding additional complexity to your Domain.

**Example: Registering a User**

If your domain includes different concepts for a user, a password and an organisation: you may need to create all 3 upon registration. An application layer registration service could provide the necessary process to safely execute all 3 steps with a single call (and also ensure that it safely fails if there is partial execution).

## Domain Layer

Following Persistence and Infrastructure Ignorance principles: the Domain layer must completely ignore data persistence concepts. Persistency should be performed by the infrastructure layer. Therefore, this layer should not have direct dependencies on the infrastructure, which means that your domain model entity classes should be POCOs.

Domain layer entities represent key domain concepts which may or may not be directly related to persisted objects. In the core template, 2 types of entities are distinguished:

**Id Based Entities**

Id based entities are typically entities that have a generated Identifier, being int/long/guid, and represents user data of the system:

Numeric IDs (int or long): Although more performant, numeric based identifiers make it difficult to transfer data between environments and tenants. They also become significantly more difficult to manage when introducing document based storage. Ideally, they should be reserved for transactional data, or any place where data sequencing is required.

GUIDs: The default for most user related data, GUIDs provide a conflict-free way to generate unique identifiers. Use of sequential GUIDs, where supported, allows for performance increases in relational databases as pages are filled better.

**Reference Entities**

Reference entities are used to store configuration and cacheable entities that leverage CODES as their primary keys. String based identifiers are easier to read, identify and reference.

***Benefits:***

Easily understand data without having to join to additional sources.
e.g. Having a StatusCode of "ACTIVE" on a User table is much better understood than a foreign key that reads as "1"

Easier to export and import data. Having the ability to export and import configuration between environments where a numeric or GUID identifier is present becomes significantly more difficult.

More flexible and extensible than Enums. Configuration entities allows you to quickly add and extend behavioural properties for reporting requirements.

***Examples:***

UserStatus (Code = "ACTIVE", Name = "Active", AllowedToLogin = true)

OrderStatus (Code = "COMPLETED", Name = "Order Processed", FollowsOn = [ "NEW", "PENDING" ])

**Entity Filters**

Filters are mapped off of entity attributes. They allow for defining selection criteria when handling an entity which can be useful repository queries. The following entity attribute filter types are supported:

* Equals
* Contains
* In
* GreaterThan
* GreaterThanEquals
* LessThan
* LessThanEquals
* Search

**Enums**

Enums should be used to represent an entity attribute that can be one of several states. Enums are used for a simpler use case than Reference Entities as Reference Entities allow for several attributes to be declared along with the key where as enums just represent a key (i.e. state). 

