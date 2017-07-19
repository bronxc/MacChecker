using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MacChecker
{
    class OUIGrabber
    {
        static string OUI_URL = "https://code.wireshark.org/review/gitweb?p=wireshark.git;a=blob_plain;f=manuf";

        public static string getOUIList()
        {
            WebRequest request = WebRequest.Create(System.Uri.EscapeUriString(OUI_URL));
            //request.Credentials = CredentialCache.DefaultCredentials;

            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            return reader.ReadToEnd();
        }

        //for testing purpose only, accept any dodgy certificate... 
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
