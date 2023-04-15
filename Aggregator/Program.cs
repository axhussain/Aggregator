using System;

namespace Aggregator;

internal static class Program
{
    static void Main(string[] args)
    {
        //Omitted for brevity - Collect input (risk data from the user)
        //Hardcoded here, but would normally be created from the user input above
        var riskData = new RiskData("John", "Smith", 500, "Cool New Phone", DateTime.Parse("1980-01-01"));
        var request = new PriceRequest(riskData);

        var priceEngine = new PriceEngine();
        var response = priceEngine.GetPrice(request);

        if (response.Price == -1)
        {
            Console.Out.WriteLine($"There was an error - {response.Error}");
        }
        else
        {
            Console.Out.WriteLine($"You price is {response.Price}, from insurer: {response.Insurer}. This includes tax of {response.Tax}");
        }
    }
}