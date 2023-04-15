using System;
using System.Reflection.Metadata.Ecma335;

namespace Aggregator;

internal static class Program
{
    static void Main(string[] args)
    {
        try
        {
            PriceRequest request = BuildRequest(args);
            string quote = GetPrice(request);

            Console.Out.WriteLine(quote);
        }
        catch (Exception ex) 
        {
            Console.Out.WriteLine($"There was an error - {ex.Message}");
        }
    }

    private static PriceRequest BuildRequest(string[] args)
    {
        //Omitted for brevity - Collect input (risk data from the user)
        //Hardcoded here, but would normally be created from the user input above
        var riskData = new RiskData("John", "Smith", 500, "Cool New Phone", DateTime.Parse("1980-01-01"));
        var request = new PriceRequest(riskData);
        return request;
    }

    private static string GetPrice(PriceRequest request)
    {
        PriceEngine priceEngine = new(request);
        PriceResponse response = priceEngine.GetPrice();

        if (response.Price == -1)
        {
            return $"There was an error - {response.Error}";
        }

        return $"Your price is {response.Price}, from insurer: {response.Insurer}. This includes tax of {response.Tax}";
    }
}