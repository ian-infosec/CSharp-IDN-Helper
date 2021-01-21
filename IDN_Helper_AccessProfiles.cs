using Newtonsoft.Json.Linq;
using RestSharp;
using Json.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_IDN_Helper
{
    public class IDN_Helper_AccessProfiles
    {
        public JArray Get_AccessProfileCollectionFromSource(IDN_Helper_Authentication authentication, string sourceID)
        {
            var client = new RestClient("https://" + authentication.OrgName + ".api.identitynow.com/v3/search?count=true&offset=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ("Bearer " + authentication.BearerToken).ToString());
            request.AddParameter("application/json", "{\n\t\"query\": {\n\t\t\"query\": \"source.id:" + sourceID + "\"\n\t},\n\t\"indices\": [\n\t\t\"accessprofiles\"\n\t],\n\t\"sort\": [\n\t\t\"name\"\n\t],\n\t\"includeNested\": false\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JArray jResponse = JArray.Parse(response.Content);

            return jResponse;
        }

        public JArray Get_AccessProfileEntitlements(IDN_Helper_Authentication authentication, string apID)
        {
            string url = "https://" + authentication.OrgName + ".api.identitynow.com/v2/access-profiles/" + apID.ToString() + "/entitlements";
            Console.WriteLine(url);
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ("Bearer " + authentication.BearerToken).ToString());
            IRestResponse response = client.Execute(request);
            JArray jResponse = JArray.Parse(response.Content);

            return jResponse;
        }

        public JObject Get_AccessProfileDetails(IDN_Helper_Authentication authentication, string apID)
        {
            string url = "https://" + authentication.OrgName + ".api.identitynow.com/v2/access-profiles/" + apID.ToString() ;
            Console.WriteLine(url);
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ("Bearer " + authentication.BearerToken));
            IRestResponse response = client.Execute(request);
            JObject jResponse = JObject.Parse(response.Content);

            return jResponse;
        }

        public void Patch_AccesProfile(IDN_Helper_Authentication authentication, JArray entitlements, string apID)
        {
            string url = "https://ogeprod.api.identitynow.com/v2/access-profiles/" + apID.ToString();
            var client = new RestClient(url);
            client.Timeout = -1;
            string body = "{ \"entitlements\": " + entitlements + " }";
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Authorization", "Bearer " + authentication.BearerToken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
