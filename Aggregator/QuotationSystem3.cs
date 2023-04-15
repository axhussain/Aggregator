﻿using System.Dynamic;

namespace Aggregator;

public class QuotationSystem3
{
    public QuotationSystem3(string url, string port)
    {

    }

    public dynamic GetPrice(dynamic request)
    {
        //Omitted - Call to an external service
        //var response = _someExternalService.PostHttpRequest(requestData);

        dynamic response = new ExpandoObject();
        response.Price = 92.67M;
        response.IsSuccess = true;
        response.Name = "zxcvbnm";
        response.Tax = 92.67M * 0.12M;

        return response;
    }
}