using System;
using AM_test.utilities;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using AM_test.objects;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AM_test
{
    public class Test_Base
    {
        public static ConnectionHelper connect = new ConnectionHelper();
        public static Random rnd = new Random();
        public static X509Certificate2 certificate = GetCertificateByThumbprint(connect.certThumbprint);
        public static HttpWebRequest request;
        static SoftAssert _softAssertions = new SoftAssert();
        public static string refer = "";

        public static CookieContainer cookieContainer = new CookieContainer();
        public static HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        public static HttpClient client = new HttpClient(handler);
        
        public static CookieCollection collection;
        protected static void createDeleteRQ()
        {
            request.Method = "DELETE";
            addSettingsToRequest(request);         
        }

        protected static void createPostRQ()
        {
            request.Method = "POST";
            addSettingsToRequest(request);
        }

        protected static void createGetRQ()
        {
            request.Method = "GET";
            addSettingsToRequest(request);            
        }

        protected static void createPutRQ()
        {
            request.Method = "PUT";
            addSettingsToRequest(request);           
        }

        protected static void createPatchRQ()
        {
            request.Method = "PATCH";
            addSettingsToRequest(request);
        }

        protected static void addSettingsToRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.ClientCertificates.Clear();
            request.ClientCertificates.Add(certificate);
            request.ContentType = "application/json";
        }

        //Returns a certificate by searching through all likely places
        private static X509Certificate2 GetCertificateByThumbprint(string thumbprint)
        {
            X509Certificate2 certificate;
            //foreach likely certificate store name
            foreach (var name in new[] { StoreName.My, StoreName.Root })
            {
                //foreach store location
                foreach (var location in new[] { StoreLocation.CurrentUser, StoreLocation.LocalMachine })
                {
                    //see if the certificate is in this store name and location
                    certificate = FindThumbprintInStore(thumbprint, name, location);
                    if (certificate != null)
                    {
                        //return the resulting certificate
                        return certificate;
                    }
                }
            }
            //certificate was not found
            throw new Exception(string.Format("The certificate with thumbprint {0} was not found",
                                               thumbprint));
        }

        private static X509Certificate2 FindThumbprintInStore(string thumbprint, StoreName name, StoreLocation location)
        {
            //creates the store based on the input name and location e.g. name=My
            var certStore = new X509Store(name, location);
            certStore.Open(OpenFlags.ReadOnly);
            //finds the certificate in question in this store
            var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint,
                                                             thumbprint, false);
            certStore.Close();

            if (certCollection.Count > 0)
            {
                //if it is found return
                return certCollection[0];
            }
            else
            {
                //if the certificate was not found return null
                return null;
            }
        }


        public static void getError404(HttpWebRequest request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); ;
                Assert.AreEqual(response.StatusCode.ToString(), "NotFound", "StatusCode is not NotFound");
                response.Close();
            }
            catch (Exception e)
            {
                Assert.AreEqual("The remote server returned an error: (404) Not Found.", e.Message, "StatusCode is not Not Found= 404");
            }
        }

        public static void getError500(HttpWebRequest request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); ;
                Assert.AreEqual(response.StatusCode.ToString(), "InternalServerError", "StatusCode is not InternalServerError");
                response.Close();
            }
            catch (Exception e)
            {
                Assert.AreEqual("The remote server returned an error: (500) Internal Server Error.", e.Message, "StatusCode is not 500");
            }
        }

        public static void getNoContentResponse(HttpWebRequest request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); ;
                Assert.AreEqual(response.StatusCode.ToString(), "NoContent", "StatusCode is not NoContent");
                response.Close();
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
        }

        public static void getBadRequestResponse(HttpWebRequest request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); ;
                Assert.AreEqual(response.StatusCode.ToString(), "BadRequest", "StatusCode is not BadRequest");
            }
            catch (Exception e)
            {
                Assert.AreEqual("The remote server returned an error: (400) Bad Request.", e.Message, "StatusCode is not BadRequest= 400");
            }
        }

        public static string getURLGroupMember(string groupId)
        {
            return connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/groups/" + groupId + "/users?api-version=1.0";
        }

        public static string getURLUserMember(string userId)
        {
            return connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/users/" + userId + "/groups?api-version=1.0";
        }

        public static string getURLUser(string userId)
        {
            return connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/users/" + userId + "?api-version=1.0";
        }

        public static string getURLGroup(string groupId)
        {
            return connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/groups/" + groupId + "?api-version=1.0";
        }

        
    }
}
