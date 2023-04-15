using System;

namespace Aggregator;

internal static class Program
{
    static void Main(string[] args)
    {
        //Omitted for brevity - Collect input (risk data from the user)
        var request = new PriceRequest()
        {
            //Hardcoded here, but would normally be created from the user input above
            RiskData = new RiskData() 
            {
                DOB = DateTime.Parse("1980-01-01"),
                FirstName = "John",
                LastName = "Smith",
                Make = "Cool New Phone",
                Value = 500
            }
        };

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