using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace CSharp_IDN_Helper
{
    public class IDN_Helper
    {
        private string GrantType;
        private string ClientId;
        private string ClientSecret;
        private string OrgName;

        public IDN_Helper_Authentication Token { get; set; }


        public IDN_Helper(IDN_Helper_Authentication authentication)
        {
            GetAccessProfiles(authentication);
        }

        private void GetAccessProfiles(IDN_Helper_Authentication authentication)
        {
            var client = new RestClient("https://" + this.OrgName + ".api.identitynow.com/cc/api/accessProfile/list?start=0&limit=3");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ("Bearer " + authentication.BearerToken).ToString());
            IRestResponse response = client.Execute(request);
            JObject jResponse = JObject.Parse(response.Content);
            Console.WriteLine("Access Profile Count: " + jResponse["count"].ToString());

            string APCount = jResponse["count"].ToString();

        }

        public JObject SearchByIdentityUsername(string username)
        {
            RequestBearerToken();

            JArray searchArray = new JArray();
            JObject result = new JObject();

            //lookup user via search (JArray)
            string searchQuery = "v2/search/identities?query=" + username;
            searchArray = ArrayIDNAPICall(searchQuery, "GET");

            foreach (JObject item in searchArray)
            {
                foreach(JObject account in item["accounts"]) {

                    Console.WriteLine("AccountId: " + account["accountId"].ToString());
                    Console.WriteLine("Name: " + account["name"].ToString());

                    if (account["accountId"].ToString() == username || account["name"].ToString() == username)
                    {
                        result = item;
                    }

                    //get user data via accountID (JObject)
                }

            }
            return result;
        }

        public JObject GetIdentityById(string identityId)
        {
            JObject identity = new JObject();

            return identity;
        }

        #region "Support Functions"
        private string RequestBearerToken()
        {
            try
            {
                string bearerToken;
                string query = "oauth/token?grant_type=" + GrantType + "&client_id=" + ClientId + "&client_secret=" + ClientSecret;
                JObject response = ObjectIDNAPICall(query, "POST");
                bearerToken = response["access_token"].ToString();
                
                return bearerToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.ToString());
                return "no token retrieved";
            }

        }
        private JObject ObjectIDNAPICall(string query, string method)
        {
            string urlGet = string.Format($"https://" + OrgName + ".api.identitynow.com/" + query + "&org=" + OrgName);
            Console.WriteLine("attempting to call API Endpoing: " + urlGet.ToString());
            //JArray searchArray = new JArray();
            var searchRequest = (HttpWebRequest)WebRequest.Create(urlGet);
            searchRequest.Method = method;
            if (!query.StartsWith("oauth"))
            {
                //searchRequest.Headers.Add("Authorization", "Bearer " + this.GetBearerToken());
            }
            var searchResponse = (System.Net.HttpWebResponse)searchRequest.GetResponse();
            JObject searchObject = ObjectResponseToJSON(searchResponse);

            return searchObject;
        }
        private static JObject ObjectResponseToJSON(WebResponse wr)
        {
            var response = (HttpWebResponse)(wr);
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var re = sr.ReadToEnd();
            var dobject = JsonConvert.DeserializeObject(re.ToString());
            var jobj = (JObject)dobject;
            return jobj;
        }
        private JArray ArrayIDNAPICall(string query, string method)
        {
            string urlGet = "https://" + OrgName + ".api.identitynow.com/" + query + "&org=" + OrgName;
            Console.WriteLine("attempting to call API Endpoing: " + urlGet.ToString());
            //JArray searchArray = new JArray();
            var searchRequest = (HttpWebRequest)WebRequest.Create(urlGet);
            searchRequest.Method = method;
            if (!query.StartsWith("oauth"))
            {
                //searchRequest.Headers.Add("Authorization", "Bearer " + this.GetBearerToken());
            }
            var searchResponse = (System.Net.HttpWebResponse)searchRequest.GetResponse();
            JArray searchArray = ArrayResponseToJSON(searchResponse);

            return searchArray;
        }
        private static JArray ArrayResponseToJSON(WebResponse wr)
        {
            var response = (HttpWebResponse)(wr);
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var re = sr.ReadToEnd();
            var dobject = JsonConvert.DeserializeObject(re.ToString());
            var jobj = (JArray)dobject;
            return jobj;
        }
    }
    #endregion

}
