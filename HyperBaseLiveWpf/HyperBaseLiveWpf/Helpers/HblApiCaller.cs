using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;
using RestSharp;
using System.Threading.Tasks;
using HyperBaseLiveWpf.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HyperBaseLiveWpf
{
     
    public static class HblApiCaller
    {
        public static HBLToken Token { get; set; }
        public static async Task<bool> Authenticate(string clientId, string clientSecret)
        {
            var hblApi = "https://api.hyperbase-live.com/token";
            var request = string.Format("grant_type=password&userName={0}&password={1}", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
            try { Token = HttpPost(hblApi, request);
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine("ERROR: "  + e.Message);
                return false;

            }

        }
        private static HBLToken HttpPost(string datamarketAccessUri, string requestDetails)
        {

            //Prepare OAuth request 
            var webRequest = WebRequest.Create(datamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            var bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (var outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (var webResponse = webRequest.GetResponse())
            {
                var serializer = new DataContractJsonSerializer(typeof(HBLToken));
                //Get deserialized object from JSON stream
                var token = (HBLToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }

        public static async Task<string> ValidateID(string id)
        {
            try
            {
                var HblApi = "https://api.hyperbase-live.com/api";
                var client = new RestClient(HblApi);
                var request = new RestRequest("/ClientInstances", Method.GET);
                request.AddParameter("instanceId", id);              
                var response = client.Execute(request);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    System.Windows.MessageBox.Show("Error Validating ID, please check your internet connection");
                }
                //client name
                return response.Content;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error Validating ID, please check your internet connection");
                return null;
            }
        }

        public static async Task<string> CheckStatus()
        {
            ///////////////////////////////ToDo
            //try {
            //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.hyperbase-live.com");

            //    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            //    if (response == null || response.StatusCode != HttpStatusCode.OK)
            //        return "Down";
            //    return "UP";
            //}
            //catch ( Exception e)
            //{
            //    return "Down";
            //}
            return "UP";
        }

        
      
        public static async Task<ServiceVersionsResponse> GetServiceVersions()
        {
            try
            {
                var HblApi = "https://api.hyperbase-live.com/api";
                var client = new RestClient(HblApi);
               
                var request = new RestRequest("/ServiceVersions", Method.GET);
                ///////////////////////////////////////
                request.AddHeader("apikey", "7a4901de-eb4c-48ba-9be3-aa36f0cda888");
               //////////////////////////////////////////
                var response = client.Execute(request);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    System.Windows.MessageBox.Show("Error in GetServiceVersions");
                }
                return JsonConvert.DeserializeObject<ServiceVersionsResponse>(response.Content);

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error in GetServiceVersions Exception");
                return null;
            }
        }



    }
    [DataContract]
    public class HBLToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
               }
}