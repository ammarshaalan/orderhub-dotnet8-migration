using Microsoft.Extensions.Logging;
using OrderHub.Core.Interfaces;
using OrderHub.Core.Models;

namespace OrderHub.Core.Services;

public class OrderProcessor : IOrderProcessor
{
private readonly ISchoolRepository _schoolRepository;
private readonly IPricingService _pricingService;
private readonly IInventoryRepository _inventoryRepository;
private readonly IPaymentService _paymentService;
private readonly IConfirmationService _confirmationService;
private readonly ILogger<OrderProcessor> _logger;

public OrderProcessor(
    ISchoolRepository schoolRepository,
    IPricingService pricingService,
    IInventoryRepository inventoryRepository,
    IPaymentService paymentService,
    IConfirmationService confirmationService,
    ILogger<OrderProcessor> logger)
{
    _schoolRepository = schoolRepository;
    _pricingService = pricingService;
    _inventoryRepository = inventoryRepository;
    _paymentService = paymentService;
    _confirmationService = confirmationService;
    _logger = logger;
}

public async Task<OrderResult> ProcessOrderAsync(
    int schoolId,
    IReadOnlyCollection<OrderLine> lines,
    string parentEmail,
    CancellationToken cancellationToken = default)
{
    var school = await _schoolRepository.GetByIdAsync(
        schoolId,
        cancellationToken);

    if (school == null)
    {
        _logger.LogWarning(
            "School {SchoolId} was not found",
            schoolId);

        return OrderResult.Fail("School not found");
    }

    decimal subtotal = 0;

    foreach (var line in lines)
    {
        var stock = await _inventoryRepository.GetStockAsync(
            line.Sku,
            cancellationToken);

        if (stock < line.Quantity)
        {
            _logger.LogWarning(
                "Insufficient stock for SKU {Sku}",
                line.Sku);

            return OrderResult.Fail(
                $"Insufficient stock for {line.Sku}");
        }

        try
        {
            var unitPrice = await _pricingService.CalculatePriceAsync(
                school.Tier,
                line,
                cancellationToken);

            subtotal += unitPrice * line.Quantity;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Product lookup failed for SKU {Sku}",
                line.Sku);

            return OrderResult.Fail(ex.Message);
        }
    }

    string paymentReference;

    try
    {
        paymentReference = await _paymentService.CreateIntentAsync(
            subtotal,
            parentEmail,
            cancellationToken);
    }
    catch (Exception ex)
    {
        _logger.LogError(
            ex,
            "Payment failed for {Email}",
            parentEmail);

        return OrderResult.Fail("Payment failed");
    }

    try
    {
        await _confirmationService.SendConfirmationAsync(
            parentEmail,
            subtotal,
            cancellationToken);
    }
    catch (Exception ex)
    {
        _logger.LogWarning(
            ex,
            "Failed to send confirmation email to {Email}",
            parentEmail);
    }

    return OrderResult.Ok(paymentReference);
}

}
