namespace OrderHub.Core.Models;

public class OrderResult
{
    public bool Success { get; set; }

    public string? OrderReference { get; set; }

    public string? FailureReason { get; set; }

    public static OrderResult Ok(string reference)
    {
        return new OrderResult
        {
            Success = true,
            OrderReference = reference
        };
    }

    public static OrderResult Fail(string reason)
    {
        return new OrderResult
        {
            Success = false,
            FailureReason = reason
        };
    }
}