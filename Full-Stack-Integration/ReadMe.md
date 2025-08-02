# Module 1: Connecting Front-End and Back-End Components
## Learning Objectives
Describe the components and architecture of full-stack development
Explain the steps to set up a full-stack development environment
Describe how to build RESTful APIs for front-end integration
Explain the process of creating and consuming APIs in front-end applications
Identify real-world scenarios for Integrating front-end and back-end components

## Architectural Patterns in Full-Stack Development
- Client-Server Model: Separates the front-end (client) and back-end (server), ensuring efficient data retrieval and updates.

- MVC (Model-View-Controller): Divides applications into three components—Model (data and logic), View (UI), and Controller (input handling)—to simplify development and scaling.

- Monolithic Architecture: Combines front-end, back-end, and logic in a single project, suitable for small applications but challenging to scale.

- Microservices: Decomposes applications into independent services, each handling a specific function and communicating via APIs, ideal for complex, scalable systems.

```C++
dotnet new blazorwasm -o FrontEndApp

dotnet new webapi -o ServerApi
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet new sln -o FullStackSolution
dotnet sln add FrontEndApp
dotnet sln add ServerApi
```
