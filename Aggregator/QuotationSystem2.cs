using System.Dynamic;

namespace Aggregator;

public class QuotationSystem2
{
    public QuotationSystem2(string url, string port, dynamic request)
    {

    }

    public dynamic GetPrice()
    {
        //Omitted - Call to an external service
        //var response = _someExternalService.PostHttpRequest(requestData);

        dynamic response = new ExpandoObject();
        response.Price = 77.56M;
        response.HasPrice = true;
        response.Name = "qewtrywrh";
        response.Tax = 77.56M * 0.12M;

        return response;
    }
}