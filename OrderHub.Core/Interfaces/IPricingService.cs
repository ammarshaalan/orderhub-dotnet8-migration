using OrderHub.Core.Models;

namespace OrderHub.Core.Interfaces;

public interface IPricingService
{
    Task<decimal> CalculatePriceAsync(
        OrderTier tier,
        OrderLine orderLine,
        CancellationToken cancellationToken = default);
}