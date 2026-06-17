using FluentAssertions;
using NSubstitute;
using OrderHub.Core.Interfaces;
using OrderHub.Core.Models;
using OrderHub.Core.Services;

namespace OrderHub.Tests;

public class PricingServiceTests
{
    [Fact]
    public async Task CalculatePriceAsync_GoldTierWithLongEmbroidery_AppliesDiscountBeforeSurcharge()
    {
        // Arrange
        var productRepository = Substitute.For<IProductRepository>();

        productRepository
            .GetBasePriceAsync("SKU1", Arg.Any<CancellationToken>())
            .Returns(20m);

        var service = new PricingService(productRepository);

        var orderLine = new OrderLine
        {
            Sku = "SKU1",
            Quantity = 1,
            Embroidery = "SMITH"
        };

        // Act
        var result = await service.CalculatePriceAsync(
            "GOLD",
            orderLine);

        // Assert
        result.Should().Be(25m);
    }
}