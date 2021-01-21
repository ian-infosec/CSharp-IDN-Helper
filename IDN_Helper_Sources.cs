using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_IDN_Helper
{
    public class IDN_Helper_Sources
    {
        public JArray Get_SourceCollection(IDN_Helper_Authentication authentication)
        {

            var client = new RestClient("https://" + authentication.OrgName + ".api.identitynow.com/cc/api/source/list");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ("Bearer " + authentication.BearerToken).ToString());
            IRestResponse response = client.Execute(request);
            JArray jResponse = JArray.Parse(response.Content);

            return jResponse;

        }
    }
}
