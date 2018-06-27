using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AM_test.objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AM_test.utilities
{
    class LoginHelper : Test_Base
    {
       
        static string __RequestVerificationToken_L2Ft0 = "8inR2ypjUV9zrgksK3mHPv-1IiD1MkH_nI9ORyzf9Ohh_ti88QmV4LX1GPqefUdTwlFuDq9ey4RF-Rjsy_Dz5vpnA7M1";
        static string __RequestVerificationToken = "WSgBVftbsu7Osh8kptbwa4rAlPyUmyDkmksYJV4Luq8IStngYD4WYnyMG5fua5uDnhm - If92ortQiXItKkrcNxohxE0AGfQcMnBFQSyhTMcxGA3KBB0NbZ45tCk - W9WINrxLiA2";
        static Uri uri = new Uri(connect.AMUri);
        static HttpResponseMessage response = new HttpResponseMessage();
        static loginPage item = new loginPage();

        public static async Task AMloginAsyncSuccess(string login, string pass)
        {
            await getStartPageAsync();
            await tryToLogin(login, pass);
            Assert.IsNull(item.errors, "Error during login.");
        }

        public static async Task AMloginAsyncWrongLogin()
        {
            await getStartPageAsync();
            await tryToLogin("admin1", "123456");    
            Assert.IsNotNull(item.errors, "Error during login: " + item.errors.ElementAt(0).ToString());

        }

        public static async Task AMloginAsyncWrongPassword()
        {
            await getStartPageAsync();
            await tryToLogin("admin", "1234567");
            Assert.IsNotNull(item.errors, "Error during login: " + item.errors.ElementAt(0).ToString());

        }

        public static async Task AMloginUserNoRights(string login, string password)
        {
            await getStartPageAsync();
            await tryToLogin(login, password);
            response = await client.GetAsync(connect.AMUri);
            Assert.AreEqual("Forbidden", response.StatusCode.ToString(), "Status Code is not Forbidden"); 
        }

        public static async Task AMloginAfterWrongPassword()
        {
            await getStartPageAsync();
            await tryToLogin("admin", "1234567");
            Assert.IsNotNull(item.errors, "Error during login: " + item.errors.ElementAt(0).ToString());
            await tryToLogin("admin", "123456");
            Assert.IsNull(item.errors, "Error during login.");
        }

        public static async Task AMlogOutAsync()
        {
            response = await client.GetAsync(connect.AMUri + "/logout");
            Assert.AreEqual("OK", response.StatusCode.ToString(), "Status Code is not OK");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36");

            string result = await response.Content.ReadAsStringAsync();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(result);

            var input = document.DocumentNode.SelectSingleNode("//div//input");
            string value = input.GetAttributeValue("value", "");
            
            StringContent queryString = new StringContent("am_xsrf=" + value + "&submit.Logout=logout", Encoding.UTF8, "application/json");
            response = await client.PostAsync(connect.AMUri + "/logout", queryString);
            Assert.AreEqual("OK", response.StatusCode.ToString(), "Status Code is not OK");
        }

        protected static async Task getStartPageAsync()
        {
            client.DefaultRequestHeaders.Clear();
            cookieContainer.Add(uri, new Cookie("__RequestVerificationToken_L2Ft0", __RequestVerificationToken_L2Ft0));
            response = await client.GetAsync(connect.AMUri);
            Assert.AreEqual("OK", response.StatusCode.ToString(), "Status Code is not OK");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Version", "1.0");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US, en");
            client.DefaultRequestHeaders.TryAddWithoutValidation("__RequestVerificationToken", __RequestVerificationToken);
        }

        protected static async Task tryToLogin(string login, string password)
        {
            StringContent queryString = new StringContent("", Encoding.UTF8, "application/json");
            response = await client.PostAsync(response.RequestMessage.RequestUri.AbsoluteUri, queryString);

            var jsonString = response.Content.ReadAsStringAsync();
            jsonString.Wait();

            item = JsonConvert.DeserializeObject<loginPage>(jsonString.Result);

            for (int i = 0; i < item.callbacks.Count; i++)
            {
                if (item.callbacks.ElementAt(i).name == "form_username") item.callbacks.ElementAt(i).value = login;
                if (item.callbacks.ElementAt(i).name == "form_password") item.callbacks.ElementAt(i).value = password;
                if (item.callbacks.ElementAt(i).name == "form_confirmation") item.callbacks.ElementAt(i).value = "0";
            }
            queryString = new StringContent(
                JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            response = await client.PostAsync(response.RequestMessage.RequestUri.AbsoluteUri, queryString);
            refer = response.RequestMessage.RequestUri.AbsoluteUri;
            Assert.AreEqual("OK", response.StatusCode.ToString(), "Status Code is not OK");

            jsonString = response.Content.ReadAsStringAsync();
            jsonString.Wait();
            item = JsonConvert.DeserializeObject<loginPage>(jsonString.Result);
        }
    }
}
