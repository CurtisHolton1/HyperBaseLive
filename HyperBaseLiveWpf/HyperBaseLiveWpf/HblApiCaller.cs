﻿using System;
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
namespace HyperBaseLiveWpf
{
     
    public static class HblApiCaller
    {
        public static HBLToken Token { get; set; }
        public static bool Authenticate(string clientId, string clientSecret)
        {
           var HblApi = "https://api.hyperbase-live.com/token";
           var request = string.Format("grant_type=password&userName={0}&password={1}", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
           //Prepare OAuth request 
           var webRequest = WebRequest.Create(HblApi);
           webRequest.ContentType = "application/x-www-form-urlencoded";
           webRequest.Method = "POST";
           var bytes = Encoding.ASCII.GetBytes(request);
           webRequest.ContentLength = bytes.Length;
           using (var outputStream = webRequest.GetRequestStream())
           {
               outputStream.Write(bytes, 0, bytes.Length);
           }
           try
           {
               var webResponse = webRequest.GetResponse();
               var serializer = new DataContractJsonSerializer(typeof(HBLToken));
               //Get deserialized object from JSON stream
               Token = (HBLToken)serializer.ReadObject(webResponse.GetResponseStream());
               return true;
           }
           catch (Exception e)
           {
               Console.WriteLine(e.ToString());
               return false;
           }
        }


        public static string ValidateID(string id)
        {
             var HblApi = "https://api.hyperbase-live.com/api";
            var client = new RestClient(HblApi);
            var request = new RestRequest("/ClientInstances", Method.GET);         
            request.AddParameter("instanceId", id);
           var response= client.Execute(request);
           return response.Content;
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