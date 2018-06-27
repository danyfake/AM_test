using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using AM_test.objects;
using Newtonsoft.Json;
using System.Text;

namespace AM_test.utilities
{
    class AMGroupHelpers : Test_Base
    {

        public static void getGroups()
        {
            Groups item = getGroupList();
            Assert.AreEqual("Group", item.ItemType);
        }



        public static Groups getGroupList()
        {
            Groups item = new Groups();
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/groups?api-version=1.0");
            createGetRQ();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    item = JsonConvert.DeserializeObject<Groups>(objText);
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
            return item;
        }

        public static Group getGroupById(string groupId)
        {
            Group item = new Group();
            createGetGroupRQ(groupId);
            item = createGroupOk();
            Assert.AreEqual(groupId, item.Id, "Id пользователей не совпали");
            return item;
        }

        public static string getRandomGroupId()
        {
            string id = "";
            Groups item = getGroupList();
            Assert.AreNotEqual(0, item.Items.Count, "No groups at realm " + connect.TestRealmName.ToString().ToUpper());
            id = item.Items[rnd.Next(item.Items.Count)].Id;
            Assert.AreNotEqual("", id);
            return id;
        }

        internal static void getGroupNotExist(string Id)
        {
            createGetGroupRQ(Id);
            getError404(request);
        }


        internal static Group createGroup(Group group)
        {
            Group item = new Group();
            createPostGroupRQ();
            group.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(group);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            item = createGroupOk();
            Assert.AreEqual(group.Name, item.Name, "Names do not match");
            Assert.IsNotNull(item.Id, "Group GUID is null");
            return item;
        }

        internal static Group createGroupNoEnable(string groupName)
        {
            Group item = new Group();
            createPostGroupRQ();
            string postData = "{\"Name\": \"" + groupName + "\"}";
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            item = createGroupOk();
            Assert.AreEqual(groupName, item.Name, "Names do not match");
            Assert.IsNotNull(item.Id, "Group GUID is null");
            return item;
        }

        internal static Group createFullGroup()
        {
            Group item = new Group();
            createPostGroupRQ();
            Group group = new Group();
            group = group.groupGenerate();
            string postData = JsonConvert.SerializeObject(group);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            item = createGroupOk();            
            Assert.AreEqual(group.Name, item.Name, "Names do not match");
            return item;
        }

        internal static void createGroupSameName(string name)
        {
            createPostGroupRQ();
            Group group = new Group(name, true);
            group.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(group);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Assert.AreEqual(response.StatusCode.ToString(), "Conflict", "StatusCode is not Conflict");
                response.Close();
            }
            catch (Exception e)
            {
                Assert.AreEqual("The remote server returned an error: (409) Conflict.", e.Message, "StatusCode is not Conflict = 409");
            }
        }

        internal static void createGroupNoName()
        {
            createPostGroupRQ();
            Group group = new Group("", true);
            group.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(group);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getBadRequestResponse(request);
        }

        internal static void deleteGroup(string Id)
        {
            createDeleteGroupRQ(Id);
            getNoContentResponse(request);
        }

        internal static void deleteGroupNotExist(string Id)
        {
            createDeleteGroupRQ(Id);
            getError404(request);
        }

        internal static Group updateGroup(Group group)
        {
            createPutGroupRQ(group.Id.ToString());
            group.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(group);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
            return group;
        }

        internal static void updateGroupNotExist(string groupId)
        {
            createPutGroupRQ(groupId);
            Group group = new Group("group_test" + rnd.Next(10000), true);
            group.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(group);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError404(request);
        }

        private static void createGetGroupRQ(string id)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLGroup(id));
            createGetRQ();
        }

        private static void createPostGroupRQ()
        {
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/groups?api-version=1.0");
            createPostRQ();
        }

        private static void createPutGroupRQ(string id)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLGroup(id));
            createPutRQ();
        }

        private static void createDeleteGroupRQ(string id)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLGroup(id));
            createDeleteRQ();
        }
        
        static SoftAssert _softAssertions = new SoftAssert();
        /// <summary>
        /// проверка параметров группы путем отправки запроса get и сравнения полученной группы, с имеющимся
        /// </summary>
        /// <param name="user">пользователь</param>
        internal static void checkGroupField(Group group)
        {
            Group item = new Group();
            item = getGroupById(group.Id);
            CheckField(group.Id, item.Id, "Id");
            CheckField(group.Enabled.ToString(), item.Enabled.ToString(), "Enabled");
            CheckField(group.Name, item.Name, "Name");
            CheckField(group.Description, item.Description, "Description");
            _softAssertions.AssertAll();
        }

        static void CheckField(string fieldSource, string fieldDest, string fieldName)
        {
            if (fieldSource != fieldDest)
                _softAssertions.AddTrue("Field " + fieldName.ToUpper() + " are not same. At created object is " + fieldSource + ", " +
                    "at getted object is " + fieldDest);
        }

        private static Group createGroupOk()
        {
            Group item = new Group();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); ;
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    item = JsonConvert.DeserializeObject<Group>(objText);
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
            return item;
        }
    }
}
