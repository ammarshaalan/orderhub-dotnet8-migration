using OrderHub.Core.Interfaces;
using OrderHub.Core.Models;

namespace OrderHub.Core.Services;

public class PricingService : IPricingService
{
    private readonly IProductRepository _productRepository;

    public PricingService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<decimal> CalculatePriceAsync(
        OrderTier tier,
        OrderLine orderLine,
        CancellationToken cancellationToken = default)
    {
        var basePrice = await _productRepository.GetBasePriceAsync(
            orderLine.Sku,
            cancellationToken);

        if (!basePrice.HasValue)
        {
            throw new InvalidOperationException(
                $"Product not found: {orderLine.Sku}");
        }

        var price = ApplyTierDiscount(
            basePrice.Value,
            tier);

        return ApplyEmbroiderySurcharge(
            price,
            orderLine.Embroidery);
    }

    private static decimal ApplyTierDiscount(
        decimal basePrice,
        OrderTier tier)
    {
        switch (tier)
        {
            case OrderTier.Gold:
                return basePrice * 0.85m;

            case OrderTier.Silver:
                return basePrice * 0.92m;

            default:
                return basePrice;
        }
    }

    private static decimal ApplyEmbroiderySurcharge(
        decimal price,
        string embroidery)
    {
        if (string.IsNullOrWhiteSpace(embroidery))
        {
            return price;
        }

        return embroidery.Length <= 3
            ? price + 4.50m
            : price + 8.00m;
    }
}