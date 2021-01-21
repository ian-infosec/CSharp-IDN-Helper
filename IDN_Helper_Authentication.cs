using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_IDN_Helper
{
    public class IDN_Helper_Authentication
    {
        private string GrantType { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        public string OrgName { get; set; }
        public string BearerToken { get; set; }


        public IDN_Helper_Authentication(string grantType, string clientId, string clientSecret, string orgName)
        {
            this.GrantType = grantType;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.OrgName = orgName;

            
            SetBearerToken();
        }
        private void SetBearerToken()
        {
            var client = new RestClient("https://" + this.OrgName + ".api.identitynow.com/oauth/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", this.GrantType);
            request.AddParameter("client_id", this.ClientId);
            request.AddParameter("client_secret", this.ClientSecret);
            IRestResponse response = client.Execute(request);
            JObject jResponse = JObject.Parse(response.Content);
            this.BearerToken = jResponse["access_token"].ToString();
        }
    }

    
}
