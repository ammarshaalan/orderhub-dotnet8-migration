namespace OrderHub.Core.Interfaces;

public interface IPaymentService
{
    Task<string> CreateIntentAsync(
        decimal amount,
        string email,
        CancellationToken cancellationToken = default);
}