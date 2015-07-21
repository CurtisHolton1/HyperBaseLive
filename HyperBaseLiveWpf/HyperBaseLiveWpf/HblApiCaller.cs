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
namespace HyperBaseLiveWpf
{
     
    public static class HblApiCaller
    {
        public static HBLToken Token { get; set; }
        public static async Task<bool> Authenticate(string clientId, string clientSecret)
        {
            var HblApi = "https://api.hyperbase-live.com/token";
            var request = string.Format("grant_type=password&userName={0}&password={1}", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
            //Prepare OAuth request 
            var webRequest = WebRequest.Create(HblApi);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            var bytes = Encoding.ASCII.GetBytes(request);
            webRequest.ContentLength = bytes.Length;
            try
            {
                using (var outputStream = webRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }
                var webResponse = webRequest.GetResponse();
                var serializer = new DataContractJsonSerializer(typeof(HBLToken));
                //Get deserialized object from JSON stream
                Token = (HBLToken)serializer.ReadObject(webResponse.GetResponseStream());
                return true;
            }
            catch (Exception e)
            {
                if (!e.Message.Equals("The remote server returned an error: (400) Bad Request."))
                System.Windows.MessageBox.Show("Error Logging in, please check your internet connection");
                return false;

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
                return response.Content;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error Validating ID, please check your internet connection");
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