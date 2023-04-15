using System;
using System.Dynamic;

namespace Aggregator;

public class PriceEngine
{
    //Pass request with risk data with details of a gadget, return the best price retrieved from 3 external quotation engines
    public PriceResponse GetPrice(PriceRequest request)
    {
        if (request.RiskData == null)
        {
            return new PriceResponse() { Error = "Risk Data is missing", Price = -1 };
        }

        if (String.IsNullOrEmpty(request.RiskData.FirstName))
        {
            return new PriceResponse() { Error = "First name is required", Price = -1 };
        }

        if (String.IsNullOrEmpty(request.RiskData.LastName))
        {
            return new PriceResponse() { Error = "Surname is required", Price = -1 };
        }

        if (request.RiskData.Value == 0)
        {
            return new PriceResponse() { Error = "Value is required", Price = -1 };
        }
        
        decimal price = 0;
        decimal tax = 0;
        string insurerName = String.Empty;

        //System 1 requires DOB to be specified
        if (request.RiskData.DOB != null)
        {
            QuotationSystem1 system1 = new QuotationSystem1("http://quote-system-1.com", "1234");

            dynamic systemRequest1 = new ExpandoObject();
            systemRequest1.FirstName = request.RiskData.FirstName;
            systemRequest1.Surname = request.RiskData.LastName;
            systemRequest1.DOB = request.RiskData.DOB;
            systemRequest1.Make = request.RiskData.Make;
            systemRequest1.Amount = request.RiskData.Value;

            dynamic system1Response = system1.GetPrice(systemRequest1);
            if (system1Response.IsSuccess)
            {
                price = system1Response.Price;
                insurerName = system1Response.Name;
                tax = system1Response.Tax;
            }
        }

        //System 2 will only provide quotes for some makes
        if (request.RiskData.Make == "examplemake1" || request.RiskData.Make == "examplemake2" ||
            request.RiskData.Make == "examplemake3")
        {
            dynamic systemRequest2 = new ExpandoObject();
            systemRequest2.FirstName = request.RiskData.FirstName;
            systemRequest2.LastName = request.RiskData.LastName;
            systemRequest2.Make = request.RiskData.Make;
            systemRequest2.Value = request.RiskData.Value;

            QuotationSystem2 system2 = new QuotationSystem2("http://quote-system-2.com", "1235", systemRequest2);

            dynamic system2Response = system2.GetPrice();
            if (system2Response.HasPrice && system2Response.Price < price)
            {
                price = system2Response.Price;
                insurerName = system2Response.Name;
                tax = system2Response.Tax;
            }
        }

        //System 3 has not limitations on what it can quote for
        QuotationSystem3 system3 = new QuotationSystem3("http://quote-system-3.com", "100");
        dynamic systemRequest3 = new ExpandoObject();

        systemRequest3.FirstName = request.RiskData.FirstName;
        systemRequest3.Surname = request.RiskData.LastName;
        systemRequest3.DOB = request.RiskData.DOB;
        systemRequest3.Make = request.RiskData.Make;
        systemRequest3.Amount = request.RiskData.Value;

        var system3Response = system3.GetPrice(systemRequest3);
        if (system3Response.IsSuccess && system3Response.Price < price)
        {
            price = system3Response.Price;
            insurerName = system3Response.Name;
            tax = system3Response.Tax;
        }

        if (price == 0)
        {
            price = -1;
        }

        return new PriceResponse(){Insurer = insurerName, Price = price, Tax = tax};
    }
}