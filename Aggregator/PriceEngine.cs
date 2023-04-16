using EnsureThat;
using System.Dynamic;

namespace Aggregator;

public class PriceEngine
{
    private readonly PriceRequest _request;
    private readonly List<object> _quotationSystems = new();

    //Pass request with risk data with details of a gadget, return the best price retrieved from 3 external quotation engines
    public PriceEngine(PriceRequest request)
    {
        EnsureArg.IsNotNull(request, nameof(request));
        _request = request;

        RegisterQuotationSystems();
    }

    private void RegisterQuotationSystems()
    {
        _quotationSystems.Add(new QuotationSystem1("http://quote-system-1.com", "1234"));
        dynamic systemRequest2 = BuildSystem2Request();
        _quotationSystems.Add(new QuotationSystem2("http://quote-system-2.com", "1235", systemRequest2));
        _quotationSystems.Add(new QuotationSystem3("http://quote-system-3.com", "100"));
    }

    public PriceResponse GetPrice()
    {
        var responses = _quotationSystems.Select(x => GetQuotation(x)).ToList();

        var response = responses?.Where(x => x != null).OrderBy(x => x!.Price).FirstOrDefault();

        return response ?? new PriceResponse()
        {
            Error = "No quotes found.",
            Price = -1
        };
    }

    private PriceResponse? GetQuotation(object quotationSystem)
    {
        var response = quotationSystem switch
        {
            QuotationSystem1 system1 when RequestHasDoB() => CallSystem1(system1),
            QuotationSystem2 system2 when RequestHasSystem2Makes() => CallSystem2(system2),
            QuotationSystem3 system3 => CallSystem3(system3),
            _ => null
        };

        return response;
    }

    private PriceResponse? CallSystem1(QuotationSystem1 system1)
    {
        dynamic systemRequest1 = BuildSystem1Request();
        dynamic system1Response = system1.GetPrice(systemRequest1);

        if (system1Response.IsSuccess)
        {
            return new PriceResponse
            {
                Price = system1Response.Price,
                Insurer = system1Response.Name,
                Tax = system1Response.Tax
            };
        }
        return null;
    }

    private PriceResponse? CallSystem2(QuotationSystem2 system2)
    {
        dynamic system2Response = system2.GetPrice();

        if (system2Response.HasPrice)
        {
            return new PriceResponse
            {
                Price = system2Response.Price,
                Insurer = system2Response.Name,
                Tax = system2Response.Tax
            };
        }
        return null;
    }

    private PriceResponse? CallSystem3(QuotationSystem3 system3)
    {
        dynamic systemRequest3 = BuildSystem3Request();
        var system3Response = system3.GetPrice(systemRequest3);

        if (system3Response.IsSuccess)
        {
            return new PriceResponse
            {
                Price = system3Response.Price,
                Insurer = system3Response.Name,
                Tax = system3Response.Tax
            };
        }
        return null;
    }

    private bool RequestHasDoB()
    {
        return _request.RiskData.DOB != null;
    }

    private bool RequestHasSystem2Makes()
    {
        var validMakes = new List<string>
        {
            "examplemake1",
            "examplemake2",
            "examplemake3"
        };

        return validMakes.Contains(_request.RiskData.Make ?? "");
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