# Chat Events

## What's implemented
I implemented `GET v1/chat-rooms/{chatRoomId:long}/aggregated-events/{granularity}` endpoint that serves the events for a given `Granularity`.
The frontend code is simple, it lists granularity options in a select box, which are displayed in a simple table.

## What's in it

### chatevents-client
There's a React based client ran by npm vite package, bound to a ASP .NET Core Web API using minimal APIs (`ChatEvents.API`).

### ChatEvents.API
This project is running on .NET Core 8. The API project exposes minimal APIs exposed through Swagger Open API for easier testing. API project is basic, only other thing added in the project is a `GlobalExceptionHandler` and `DatabaseOptions` registration. Services registration is done by calling `RegisterServices` on `ChatEvents.Infrastructure`.

For the backend code I used this `nullable` setup:
```csharp
<Nullable>enable</Nullable>
<WarningsAsErrors>Nullable</WarningsAsErrors>
```
When you honor this setup, you rarely get to see `System.NullReferenceException` in production.

### ChatEvents
This class library holds main models, DB entities, interfaces for repositories and services.

### ChatEvents.Infrastructure
This class library holds `IServiceCollection` extension `RegisterServices` for registering app services. Repositories are implemented using Entity Framework Core based implementation of `IGenericOrmRepository`. EF is configured to use in memory database. This project contains `DbInitializer` which is triggered from the API to seed the database.

### ChatEvents.IntegrationTests
This test project uses `WebApplicationFactory` to test the response types of the endpoints, and the response itself.

### ChatEvents.UnitTests
This test project unit tests different classes of the project.

## What's missing
In real app paging has to be implemented for the endpoint.
Local timezone should be sent from the client in order to convert event time to it. For the same reason the results are inaccurate on daily level compared to local timezone.
Not all unit tests are written, I tested main model classes and `ChatEventsService` which is crucial for what's implemented.
In a prod ready app `ILogger<ClassName>` should be injected and the information and errors should be logged.
There is no authentication setup.

## Setup
You need to install following dependencies:
* .NET Core 8 SDK for your OS - https://dotnet.microsoft.com/en-us/download/dotnet/8.0 
* Node package manager (npm) for your OS - https://docs.npmjs.com/downloading-and-installing-node-js-and-npm
* Set up local Asp .NET Core Dev SSL certificate. Usually the IDE will support you, but here's the command: `dotnet dev-certs https --trust`

## Running the project
* Build the project.
* Run the project using `ChatEvents.API https` from `launchSettings.json` profile.
* The profile will start the backend, expose Swagger for it on `https://localhost:7008/swagger/index.html`, and then launch the React app using `vite`, the address is `https://localhost:5173/`.