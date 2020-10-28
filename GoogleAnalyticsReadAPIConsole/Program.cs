//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Google.Apis.Analytics.v3;
//using Google.Apis.Auth.OAuth2;
//using System.Threading;
//using Google.Apis.Util.Store;
//using Google.Apis.Services;
//using System.Security.Cryptography.X509Certificates;
//using System.IO;
//using System.Threading.Tasks;
//using System.Net.Http;

//namespace GoogleAnalyticsReadAPIConsole
//{
//    class Program
//    {

//        // API Key = AIzaSyCpMxKETygX9dIILdlGS24UN8e2nqr_plU 
//        static void Main()
//        {
//            RunAsync().GetAwaiter().GetResult();
//        }

//        static void Auth()
//        {
//            string[] scopes = new string[] { AnalyticsService.Scope.Analytics }; // view and manage your Google Analytics data

//            var keyFilePath = @"c:\file.p12";    // Downloaded from https://console.developers.google.com
//            var serviceAccountEmail = "grp-it@mrsfields.com";  // found https://console.developers.google.com

//            //loading the Key file
//            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
//            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
//            {
//                Scopes = scopes
//            }.FromCertificate(certificate));
//        }
using Google.Apis.AnalyticsReporting.v4.Data;
using System;

namespace ConsoleApplication
    {
        class Program
        {
            static void Main(string[] args)
            {
                var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile("serviceAccount.json")
                    .CreateScoped(new[] { Google.Apis.AnalyticsReporting.v4.AnalyticsReportingService.Scope.AnalyticsReadonly });

                using (var analytics = new Google.Apis.AnalyticsReporting.v4.AnalyticsReportingService(new Google.Apis.Services.BaseClientService.Initializer
                {
                    HttpClientInitializer = credential
                }))
                {
                    var request = analytics.Reports.BatchGet(new GetReportsRequest
                    {
                        ReportRequests = new[] {
                        new ReportRequest{
                            DateRanges = new[] { new DateRange{ StartDate = "2019-01-01", EndDate = "2019-01-31" }},
                            Dimensions = new[] { new Dimension{ Name = "ga:date" }},
                            Metrics = new[] { new Metric{ Expression = "ga:sessions", Alias = "Sessions"}},
                            ViewId = "99999999"
                        }
                    }
                    });
                    var response = request.Execute();
                    foreach (var row in response.Reports[0].Data.Rows)
                    {
                        Console.Write(string.Join(",", row.Dimensions) + ": ");
                        foreach (var metric in row.Metrics) Console.WriteLine(string.Join(",", metric.Values));
                    }
                }

                Console.WriteLine("Done");
                Console.ReadKey(true);
            }

        }
    public class PersonalServiceAccountCred
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
    }
}

}
