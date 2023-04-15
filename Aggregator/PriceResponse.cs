namespace Aggregator;

public class PriceResponse
{
    public decimal Tax { get; set; }
    public string Insurer { get; set;}
    public string Error { get; set; }
    public decimal Price { get; set; }
}