namespace OrderHub.Core.Interfaces;

public interface IInventoryRepository
{
    Task<int> GetStockAsync(
        string sku,
        CancellationToken cancellationToken = default);
}