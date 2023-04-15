using System.Dynamic;

namespace Aggregator;

public class QuotationSystem1
{
    public QuotationSystem1(string url, string port)
    {
        
    }

    public dynamic GetPrice(dynamic request)
    {
        //Omitted - Call to an external service
        //var response = _someExternalService.PostHttpRequest(requestData);

        dynamic response = new ExpandoObject();
        response.Price = 123.45M;
        response.IsSuccess = true;
        response.Name = "Test Name";
        response.Tax = 123.45M * 0.12M;

        return response;
    }
}