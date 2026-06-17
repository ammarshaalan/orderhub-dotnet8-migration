namespace OrderHub.Core.Interfaces;

public interface IProductRepository
{
    Task<decimal?> GetBasePriceAsync(
        string sku,
        CancellationToken cancellationToken = default);
}