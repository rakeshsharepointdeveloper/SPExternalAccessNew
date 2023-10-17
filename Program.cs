using IdentityModel.Client;
using Newtonsoft.Json;
using SPExternalAccessNew.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPExternalAccessNew
{
    internal class Program
    {
       
        static void Main(string[] args)
        {

            CallAccessSPFilesUsingGraphAPI();

        }

        private static void CallAccessSPFilesUsingGraphAPI()
        {
           
            var TokenEndpoint = ConfigurationManager.AppSettings["TokenEndpoint"];
            var ClientID = ConfigurationManager.AppSettings["client_id"];
            var ClientSecret = ConfigurationManager.AppSettings["client_secret"];
            var resource = ConfigurationManager.AppSettings["resource"];
            var GrantType = ConfigurationManager.AppSettings["grant_type"];
            var Tenant = ConfigurationManager.AppSettings["tenantid"];
            var scope = ConfigurationManager.AppSettings["scope"];
            TokenEndpoint = string.Format(TokenEndpoint, Tenant);

            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("client_id", ClientID),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("resource", resource),
                new KeyValuePair<string, string>("scope", scope),
                new KeyValuePair<string, string>("tenant", Tenant),
            };

            HttpContent content = new FormUrlEncodedContent(keyValues);

            var httpClient = new HttpClient();
            var response = httpClient.PostAsync(TokenEndpoint, content).Result;
            var token = response.Content.ReadAsStringAsync().Result;
            var accessToken = (JsonConvert.DeserializeObject<AccessToken>(token)).access_token;


            var SiteDataEndPoint = ConfigurationManager.AppSettings["SiteDataEndPoint"];

            httpClient.SetBearerToken(accessToken);
            response = httpClient.GetAsync(SiteDataEndPoint).Result;
            var siteData = response.Content.ReadAsStringAsync().Result;
            var sharepointSite = JsonConvert.DeserializeObject<SharePointSite>(siteData);


            //var ListsEndPoint = ConfigurationManager.AppSettings["ListsEndPoint"];
            //ListsEndPoint = string.Format(ListsEndPoint, sharepointSite.id);


            //httpClient.SetBearerToken(accessToken);
            //response = httpClient.GetAsync(ListsEndPoint).Result;
            //var listData = response.Content.ReadAsStringAsync().Result;
            //var sharePointList = JsonConvert.DeserializeObject<SharePointList>(listData);
            //var listid = sharePointList.value.FirstOrDefault(obj => obj.displayName == "Documents").id;

            //var ListDataEndPoint = ConfigurationManager.AppSettings["ListDataByFilterLib"];
            //ListDataEndPoint = string.Format(ListDataEndPoint, sharepointSite.id, listid);
            //httpClient.SetBearerToken(accessToken);
            //response = httpClient.GetAsync(ListDataEndPoint).Result;

            //Below logic is to handle TooManyRequests Error. We wait for seconds mentioned in Header with name "Retry-After" and try to call the endpoint again.
            //int maxRetryCount = 3;
            //int retriesCount = 0;

            //if (response.StatusCode == HttpStatusCode.TooManyRequests)
            //{
            //    do
            //    {
            //        // Determine the retry after value - use the "Retry-After" header
            //        var retryAfterInterval = Int32.Parse(response.Headers.GetValues("Retry-After").FirstOrDefault());

            //        //we get retryAfterInterval in seconds. We need to pass milliseconds to Thread.Sleep method, hence we multiply retryAfterInterval with 1000
            //        System.Threading.Thread.Sleep(retryAfterInterval * 1000);
            //        response = httpClient.GetAsync(ListDataEndPoint).Result;
            //        retriesCount += 1;
            //    } while (response.StatusCode == HttpStatusCode.TooManyRequests && retriesCount <= maxRetryCount);
            //}

            //var ListData = response.Content.ReadAsStringAsync().Result;

            //https://graph.microsoft.com/v1.0/drives/{{DriveID}}/root:/ERP-CRM Enhancement (ERP Dev)/BC.png:/content
            //https://graph.microsoft.com/v1.0/sites/{{SiteID}} /Drives.
            //https://graph.microsoft.com/v1.0/sites/{site-id}/drives

            var GetDrives = ConfigurationManager.AppSettings["GetDrives"];
            GetDrives = string.Format(GetDrives, sharepointSite.id);


            httpClient.SetBearerToken(accessToken);
            response = httpClient.GetAsync(GetDrives).Result;
            var getDrives = response.Content.ReadAsStringAsync().Result;


            //https://graph.microsoft.com/v1.0/Drives/{{DriveID}}/root:/ERP-CRM Enhancement (ERP Dev):/Children.


            var path = "https://graph.microsoft.com/v1.0/Drives/b!TTTR5HPR00qKMbhd4Ua5kqnQZpNBJrJAg4NkBP-KwGwT1AmTV73TRqdNngV2bl_H/root:/321:/Children";

            httpClient.SetBearerToken(accessToken);
            response = httpClient.GetAsync(path).Result;
            var getChildFiles = response.Content.ReadAsStringAsync().Result;
            var finalJSON = JsonConvert.DeserializeObject<Downlaod>(getChildFiles);

            var FINALLINK = finalJSON.value.FirstOrDefault(obj => obj.name == "Passport_123.pdf").microsoftgraphdownloadUrl;


            WebClient webClient = new WebClient();
            webClient.DownloadFile(FINALLINK, @"C:\\Download\\Passport_123.pdf");

        }
    }
}
