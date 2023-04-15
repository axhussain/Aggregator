using EnsureThat;
using System.Dynamic;

namespace Aggregator;

public class PriceEngine
{
    private readonly PriceRequest _request;

    //Pass request with risk data with details of a gadget, return the best price retrieved from 3 external quotation engines
    public PriceEngine(PriceRequest request)
    {
        EnsureArg.IsNotNull(request, nameof(request));

        _request = request;
    }

    public PriceResponse GetPrice()
    {
        List<PriceResponse> responses = new();

        //System 1 requires DOB to be specified
        if (RequestHasDoB())
        {
            QuotationSystem1 system1 = new QuotationSystem1("http://quote-system-1.com", "1234");
            dynamic systemRequest1 = BuildSystem1Request();

            dynamic system1Response = system1.GetPrice(systemRequest1);

            if (system1Response.IsSuccess)
            {
                responses.Add(new PriceResponse
                {
                    Price = system1Response.Price,
                    Insurer = system1Response.Name,
                    Tax = system1Response.Tax
                });
            }
        }

        //System 2 will only provide quotes for some makes
        if (RequestHasSystem2Makes())
        {
            dynamic systemRequest2 = BuildSystem2Request();
            QuotationSystem2 system2 = new QuotationSystem2("http://quote-system-2.com", "1235", systemRequest2);

            dynamic system2Response = system2.GetPrice();

            if (system2Response.HasPrice)
            {
                responses.Add(new PriceResponse
                {
                    Price = system2Response.Price,
                    Insurer = system2Response.Name,
                    Tax = system2Response.Tax
                });
            }
        }

        //System 3 has not limitations on what it can quote for
        QuotationSystem3 system3 = new QuotationSystem3("http://quote-system-3.com", "100");
        dynamic systemRequest3 = BuildSystem3Request();

        var system3Response = system3.GetPrice(systemRequest3);

        if (system3Response.IsSuccess)
        {
            responses.Add(new PriceResponse
            {
                Price = system3Response.Price,
                Insurer = system3Response.Name,
                Tax = system3Response.Tax
            });
        }

        var response = responses.OrderBy(x => x.Price).FirstOrDefault();
        return response ?? new PriceResponse()
        {
            Error = "No quotes found.",
            Price = -1
        };
    }

    private bool RequestHasDoB()
    {
        return _request.RiskData.DOB != null;
    }

    private bool RequestHasSystem2Makes()
    {
        return _request.RiskData.Make == "examplemake1"
            || _request.RiskData.Make == "examplemake2"
            || _request.RiskData.Make == "examplemake3";
    }

    private dynamic BuildSystem1Request()
    {
        dynamic systemRequest1 = new ExpandoObject();
        systemRequest1.FirstName = _request.RiskData.FirstName;
        systemRequest1.Surname = _request.RiskData.LastName;
        systemRequest1.DOB = _request.RiskData.DOB;
        systemRequest1.Make = _request.RiskData.Make;
        systemRequest1.Amount = _request.RiskData.Value;
        return systemRequest1;
    }

    private dynamic BuildSystem2Request()
    {
        dynamic systemRequest2 = new ExpandoObject();
        systemRequest2.FirstName = _request.RiskData.FirstName;
        systemRequest2.LastName = _request.RiskData.LastName;
        systemRequest2.Make = _request.RiskData.Make;
        systemRequest2.Value = _request.RiskData.Value;
        return systemRequest2;
    }

    private dynamic BuildSystem3Request()
    {
        dynamic systemRequest3 = new ExpandoObject();
        systemRequest3.FirstName = _request.RiskData.FirstName;
        systemRequest3.Surname = _request.RiskData.LastName;
        systemRequest3.DOB = _request.RiskData.DOB;
        systemRequest3.Make = _request.RiskData.Make;
        systemRequest3.Amount = _request.RiskData.Value;
        return systemRequest3;
    }
}