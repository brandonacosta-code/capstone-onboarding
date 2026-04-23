# Capstone Onboarding

E-commerce style onboarding solution built on .NET Framework 4.7.2. A SQL Server back end exposes product catalog and order creation through an ASP.NET Web API. A separate MVC site hosts a client-side TypeScript application that consumes the API.

## Functionality

- *Products*: List products with optional name search, sort by Name or UnitPrice, and ascending or descending order. Retrieve a single product by identifier.
- *Orders*: Create an order from a list of line items. The application computes subtotal, a fixed tax rate, and fixed shipping, persists the order and lines in the database, and returns an order summary with totals.

## Solution layout

| Project | Role |
| -------- | ---- |
| Capstone.Client | ASP.NET MVC host for the SPA, Razor partial views, TypeScript build (npm scripts compile SPA sources). |
| Capstone.Api | ASP.NET Web API 2, HTTP endpoints, CORS, JSON, Unity dependency injection. |
| Capstone.Application | Use cases: MediatR requests, query and command handlers, IMessageBus facade. |
| Capstone.Core | Shared contracts: DTOs and repository interfaces. No infrastructure references. |
| Capstone.Infrastructure | Dapper-based SQL Server access implementing Capstone.Core interfaces. |
| Capstone.Tests (sources under Capstone.Tests1) | NUnit unit tests with Moq for handlers. |
| Capstone.UITests | NUnit end-to-end tests with Playwright and Boa Constrictor Screenplay. |

The Visual Studio solution file is Capstone.sln.

## Application flow and patterns

- *Web API* controllers depend on IMessageBus only. They do not call repositories directly.
- *IMessageBus* (Capstone.Application.MediatR) wraps MediatR’s IMediator and exposes synchronous and asynchronous Send methods. Controllers use SendAsync to run queries and commands.
- *CQRS-style organization*: Capstone.Application.CQRS.Queries holds IRequest<TResponse> types and their IRequestHandler<,> implementations. Capstone.Application.CQRS.Commands holds commands and handlers for write operations.
- *Handlers* resolve repository interfaces from Capstone.Core.Interfaces and implement business rules (for example, order total calculation in CreateOrderCommandHandler).

## DTOs

All API-facing data shapes live in Capstone.Core.DTOs and use the DTO suffix:

- ProductDTO: catalog fields (Id, Name, UnitPrice, Description, ImageUrl, Stock).
- OrderItemDTO: line item (ProductId, Name, Qty, UnitPrice, Amount).
- CreateOrderDTO: Items collection for new orders.
- OrderSummaryDTO: persisted OrderId, repeated Items, and computed Subtotal, Tax, Shipping, Total.

Repositories in Core accept and return these types where applicable, so the same shapes flow from SQL mapping through handlers to the API and tests.

## Validations

- *API layer* (OrderController): rejects null bodies, null item lists, or empty lists with HTTP 400 and a short message before dispatching a command.
- *Command handler* (CreateOrderCommandHandler): throws ArgumentException if there are no items, if any Qty is not positive, or if any Amount is not positive. These rules are covered by unit tests.
- *Infrastructure* (ProductRepository): restricts dynamic ORDER BY to an allowed set of column names to avoid invalid or unsafe sort inputs.

## Naming and organization

- *Namespaces* mirror project boundaries: Capstone.Api.Controllers, Capstone.Application.CQRS.Queries, Capstone.Core.DTOs, Capstone.Core.Interfaces, Capstone.Infrastructure.Repositories.
- *Queries* are named Get{Entity}Query or Get{Entities}Query with handlers named ...QueryHandler (for example, GetProductByIdQuery / GetProductByIdQueryHandler). The product list type uses GetProductsQuery with handler class GetProductsQueryHandle in the current codebase.
- *Commands* pair Create{Action}Command with Create{Action}CommandHandler.
- *Repositories* are I{Entity}Repository with implementations {Entity}Repository in Infrastructure.

## Dependency injection and configuration

- *Unity* is configured in Capstone.Api WebApiConfig: registers MediatR (including handler discovery from the Application assembly), IMessageBus / MessageBus, and repository implementations with the connection string injected into their constructors.
- *Connection string* name: CapstoneDB in Capstone.Api\Web.config (default example uses Server=localhost;Database=Capstone;Integrated Security=True). Database tables used include dbo.tblProducts, tblOrders, and tblOrderItems.
- *CORS* on the API allows the MVC client origin matching http://localhost:53019, consistent with the UI test base URL.

## Client application

- *Capstone.Client* uses TypeScript under SPA\, a Kendo-oriented wrapper under SPA\0-kendo-wrapper\, view models and controllers for routes such as home, product grid, product detail, and cart. ViewController serves Razor partials as strings for the SPA shell.
- *Build*: from Capstone.Client, npm run dev or npm run build runs the TypeScript compiler per tsconfig.json.

## Tests

- *Unit tests* (Capstone.Tests1): NUnit + Moq. Handlers are tested in isolation with mocked IProductRepository and IOrderRepository. Tests assert returned DTOs, sort and search pass-through, repository call counts, and validation failures for CreateOrderCommandHandler.
- *UI tests* (Capstone.UITests): NUnit, Microsoft Playwright, Boa Constrictor. TestBase starts Chromium, navigates to the configured products URL, and exposes an Actor and IPage. Tests are grouped with attributes such as BoundedContext and Allure suite metadata. Folders follow Screenplay style: Actions, Pages, and Tests per area (for example, checkout and product search).

## Build and run (overview)

1. Ensure SQL Server is available and the schema matches the repositories’ expected tables, or adjust the connection string in Web.config.
2. Build the solution in Visual Studio or with MSBuild.
3. Run Capstone.Api and Capstone.Client (IIS Express or your host of choice) so the client can call the API. Align ports and CORS if you change host URLs.
4. For UI tests, run the client at the URL expected by Capstone.UITests.Core.TestBase or update that constant to your environment.
5. For TypeScript changes in the client, run the npm build scripts in Capstone.Client so compiled output matches your deployment setup.
