using EnsureThat;
using System;
using System.Dynamic;

namespace Aggregator;

public class PriceEngine
{
    private readonly PriceRequest _request;

    public PriceEngine(PriceRequest request)
    {
        EnsureArg.IsNotNull(request, nameof(request));

        _request = request;
    }

    //Pass request with risk data with details of a gadget, return the best price retrieved from 3 external quotation engines
    public PriceResponse GetPrice()
    {
        decimal price = 0;
        decimal tax = 0;
        string insurerName = String.Empty;

        //System 1 requires DOB to be specified
        if (_request.RiskData.DOB != null)
        {
            QuotationSystem1 system1 = new QuotationSystem1("http://quote-system-1.com", "1234");

            dynamic systemRequest1 = new ExpandoObject();
            systemRequest1.FirstName = _request.RiskData.FirstName;
            systemRequest1.Surname = _request.RiskData.LastName;
            systemRequest1.DOB = _request.RiskData.DOB;
            systemRequest1.Make = _request.RiskData.Make;
            systemRequest1.Amount = _request.RiskData.Value;

            dynamic system1Response = system1.GetPrice(systemRequest1);
            if (system1Response.IsSuccess)
            {
                price = system1Response.Price;
                insurerName = system1Response.Name;
                tax = system1Response.Tax;
            }
        }

        //System 2 will only provide quotes for some makes
        if (_request.RiskData.Make == "examplemake1" || _request.RiskData.Make == "examplemake2" ||
            _request.RiskData.Make == "examplemake3")
        {
            dynamic systemRequest2 = new ExpandoObject();
            systemRequest2.FirstName = _request.RiskData.FirstName;
            systemRequest2.LastName = _request.RiskData.LastName;
            systemRequest2.Make = _request.RiskData.Make;
            systemRequest2.Value = _request.RiskData.Value;

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

        systemRequest3.FirstName = _request.RiskData.FirstName;
        systemRequest3.Surname = _request.RiskData.LastName;
        systemRequest3.DOB = _request.RiskData.DOB;
        systemRequest3.Make = _request.RiskData.Make;
        systemRequest3.Amount = _request.RiskData.Value;

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