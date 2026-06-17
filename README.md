# OrderHub

A .NET 8 refactoring of the legacy OrderProcessor.

## Design

- OrderProcessor orchestrates the order workflow.
- PricingService contains pricing rules.
- Repositories abstract data access.
- Payment and email integrations are represented by interfaces.

## Testing

Run:

dotnet test

## Migration

See MIGRATION.md