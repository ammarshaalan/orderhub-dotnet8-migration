using OrderHub.Core.Models;

namespace OrderHub.Core.Interfaces;

public interface IOrderProcessor
{
    Task<OrderResult> ProcessOrderAsync(
        int schoolId,
        IReadOnlyCollection<OrderLine> lines,
        string parentEmail,
        CancellationToken cancellationToken = default);
}