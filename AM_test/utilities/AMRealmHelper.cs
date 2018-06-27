using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using AM_test.objects;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace AM_test.utilities
{
    class AMRealmHelper : Test_Base
    {

        public static Realms getRealms()
        {
            Realms realmList = new Realms();
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms?api-version=1.0");
            createGetRQ();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    realmList = JsonConvert.DeserializeObject<Realms>(objText);
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
            return realmList;
        }


        public static Realm getRealm(string id)
        {
            Realm realm = new Realm();
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/" + id + "?api-version=1.0");
            createGetRQ();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    realm = JsonConvert.DeserializeObject<Realm>(objText);
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
            return realm;
        }


        public static void getRealmNotExist(string id)
        {
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/" + id + "?api-version=1.0");
            createGetRQ();
            getError500(request);           
        }

        public static async Task addRealm(string realmName)
        {    
            StringContent queryString = new StringContent("{\"active\":true,\"id\":\"" + realmName + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(connect.AMUri + "/Admin/RealmManagement/Add", queryString);
            Assert.AreEqual("Created", response.StatusCode.ToString(), "Status Code is not Created");
        }

        public static async Task updateRealm(string realmName)
        {
            StringContent queryString = new StringContent("{\"active\":false,\"id\":\"" + realmName + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(connect.AMUri + "/Admin/RealmManagement/Edit", queryString);
            Assert.AreEqual("OK", response.StatusCode.ToString(), "Status Code is not OK");
        }

        public static async Task addRealmSameName(string realmName)
        {
            StringContent queryString = new StringContent("{\"active\":true,\"id\":\"" + realmName + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(connect.AMUri + "/Admin/RealmManagement/Add", queryString);
            Assert.AreEqual("BadRequest", response.StatusCode.ToString(), "Status Code is not BadRequest");
        }

        public static async Task deleteRealm(string realmName)
        {
            StringContent queryString = new StringContent("{\"active\":true,\"id\":\"" + realmName + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(connect.AMUri + "/Admin/RealmManagement/Delete", queryString);
            Assert.AreEqual("OK", response.StatusCode.ToString(), "Status Code is not OK");
        }


        public static bool checkRealmExist(string realmName)
        {
            bool checkFlag = false;
            Realms realmList = getRealms();
            for (int i = 0; i< realmList.Items.Count; i++)
            {
                if (realmList.Items[i].Id == realmName) checkFlag = true;
            }
            return checkFlag;
        }

        public static bool checkRealmIsActive(string realmName)
        {
            bool checkFlag = false;
            Realms realmList = getRealms();
            for (int i = 0; i < realmList.Items.Count; i++)
            {
                if (realmList.Items[i].Id == realmName)
                {
                    Realm realm = getRealm(realmName);
                    if (realm.IsActive == true) checkFlag = true;
                }
            }
            return checkFlag;
        }
    }
}
