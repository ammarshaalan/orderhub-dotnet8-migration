# MIGRATION.md

## Migration Approach

The current `OrderProcessor` handles multiple responsibilities including pricing, inventory validation, payment processing, email notifications, and database access. To support a zero-downtime migration to .NET 8, I would first separate these concerns into individual services while preserving the existing entry point used by the WebForms and ASMX applications.

### Phase 1 - Extract Business Logic

Introduce a new .NET 8 implementation of `IOrderProcessor` and move business rules into dedicated services:

* `PricingService`
* `PaymentService`
* `ConfirmationService`

Database access is moved behind repository interfaces:

* `ISchoolRepository`
* `IProductRepository`
* `IInventoryRepository`

The new `OrderProcessor` becomes an orchestration layer responsible for coordinating these components.

### Phase 2 - Compatibility Layer

The existing `OrderProcessor` contract remains unchanged so current WebForms pages and ASMX services continue working without modification.

A compatibility layer forwards requests from the legacy implementation to the new .NET 8 services. This allows functionality to be migrated incrementally while keeping the application online.

### Phase 3 - Modernize Infrastructure

As functionality is migrated:

* Replace SQL string concatenation with parameterized queries.
* Replace direct `HttpClient` creation with dependency injection.
* Introduce structured logging.
* Replace synchronous blocking calls with async operations.

Each change can be deployed independently and verified through automated tests.

### Phase 4 - Final Cutover

Once all consumers have been migrated to the .NET 8 implementation, the legacy `OrderProcessor` can be retired and the compatibility layer removed.

## Why This Approach

This approach minimizes risk by preserving existing integrations while gradually moving functionality into smaller, testable services. It avoids a large rewrite and supports continuous delivery throughout the migration.

## Key Risk

The largest risk is undocumented behavior inside the existing `OrderProcessor`. Other parts of the application may rely on edge cases or side effects that are not immediately visible. Before migration begins, I would create characterization tests around pricing, inventory, payment, and order-processing scenarios to establish a baseline for expected behavior.
