namespace OrderHub.Core.Interfaces;

public interface IConfirmationService
{
    Task SendConfirmationAsync(
        string email,
        decimal total,
        CancellationToken cancellationToken = default);
}