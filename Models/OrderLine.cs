namespace OrderHub.Core.Models;

public class OrderLine
{
    public string Sku { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string Embroidery { get; set; } = string.Empty;
}