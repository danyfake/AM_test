using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using AM_test.objects;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace AM_test.utilities
{
    class AMUserHelpers : Test_Base
    {
        
        /// <summary>
        /// получение id любого пользователя из списка полученных
        /// </summary>
        /// <returns>id пользователя</returns>
        public static string getRandomUserId()
        {
            string id = "";
            Users item = getUsersList();
            Assert.AreNotEqual(0, item.Items.Count, "No users at realm "+ connect.TestRealmName.ToString().ToUpper());
            id = item.Items[rnd.Next(item.Items.Count)].Id;
            Assert.AreNotEqual("", id);
            return id;
        }

        internal static void getUserNotExist(string userId)
        {
            createGetUserRQ(userId);
            getError404(request);
        }

        internal static void deleteUserNotExist(string userId)
        {
            createDeleteUserRQ(userId);
            getError404(request);
        }


        /// <summary>
        /// удаление пользователя по id
        /// </summary>
        /// <param name="userId">id пользователя</param>
        internal static void deleteUser(string userId)
        {
            createDeleteUserRQ(userId);
            getNoContentResponse(request);
        }

        internal static void deleteRootUser(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/root/users/" + userId + "?api-version=1.0");
            createDeleteRQ();
            getNoContentResponse(request);
        }

        internal static void createUserNoLogin()
        {
            createPostUserRQ();
            User user = new User("", true);
            user.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(user);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getBadRequestResponse(request);
        }


        internal static void updateUserNotExist(string userId)
        {
            createPutUserRQ(userId);
            User user = new User("user_test" + rnd.Next(10000), true);
            user.LastModifiedOn = DateTime.Now;
            user.Email = "testMail" + rnd.Next(10000);
            string postData = JsonConvert.SerializeObject(user);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError404(request);
        }

        internal static User createFullUser()
        {
            User item = new User();
            createPostUserRQ();
            User user = new User();
            user = user.userGenerate();           
            string postData = JsonConvert.SerializeObject(user);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            item = createUserOk();
            Assert.AreEqual(user.Login, item.Login, "Logins do not match");
            return item;
        }


        /// <summary>
        /// обновление почты
        /// </summary>
        /// <param name="user">пользователь</param>
        internal static User updateUser(User user)
        {
            createPutUserRQ(user.Id.ToString());
            user.LastModifiedOn = DateTime.Now;
            string newMail = "testMail" + rnd.Next(10000);
            user.Email = newMail;
            string postData = JsonConvert.SerializeObject(user);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
            return user;
        }

        /// <summary>
        /// обновление почты
        /// </summary>
        /// <param name="user">пользователь</param>
        internal static User updateUserByPatch(User user)
        {
            createPatchUserRQ(user.Id.ToString());
            user.LastModifiedOn = DateTime.Now;
            string newMail = "testMail" + rnd.Next(10000);
            user.Email = newMail;
            string postData = "[{\"value\" : \"" + newMail + "\", \"path\":\"email\", \"op\":\"replace\"}]";
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
            return user;
        }


        /// <summary>
        /// получение списка пользователей
        /// </summary>
        public static void getUsers()
        {
            Users item = getUsersList();
            Assert.AreEqual("User", item.ItemType);
        }

        /// <summary>
        /// получение списка пользователей
        /// </summary>
        /// <returns>список пользователей</returns>
        private static Users getUsersList()
        {
            Users item = new Users();
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/users?api-version=1.0");
            createGetRQ();
            char[] bytes = new char[100];
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    item = JsonConvert.DeserializeObject<Users>(objText);
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
            return item;
        }

      
        public static User createUser(User user)
        {
            User item = new User();
            createPostUserRQ();
            user.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(user);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            item = createUserOk();
            Assert.AreEqual(user.Login, item.Login, "Logins do not match");
            Assert.IsNotNull(item.Id, "User GUID is null");
            return item;
        }

        public static User createRootUser(User user)
        {
            User item = new User();
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/root/users?api-version=1.0");
            createPostRQ();
            user.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(user);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            item = createUserOk();
            Assert.AreEqual(user.Login, item.Login, "Logins do not match");
            Assert.IsNotNull(item.Id, "User GUID is null");
            return item;
        }

        public static User createUserNoEnable(string login)
        {
            User item = new User();
            createPostUserRQ();
            string postData = "{\"Login\": \"" + login + "\"}";
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            item = createUserOk();
            Assert.AreEqual(login, item.Login, "Logins do not match");
            Assert.IsNotNull(item.Id, "User GUID is null");
            return item;
        }

        internal static void createUserSameLogin(string login)
        {
            createPostUserRQ();
            User user = new User(login, true);
            user.LastModifiedOn = DateTime.Now;
            string postData = JsonConvert.SerializeObject(user);
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

        internal static User getUserById(string userId)
        {
            User item = new User();
            createGetUserRQ(userId);
            item = createUserOk();
            Assert.AreEqual(userId, item.Id, "Id пользователей не совпали");
            return item;
        }

        static  SoftAssert _softAssertions = new SoftAssert();
        /// <summary>
        /// проверка параметров пользователя путем отправки запроса get и сравнения полученного пользователя, с имеющимся
        /// </summary>
        /// <param name="user">пользователь</param>
        internal static void checkUserField(User user)
        {
            User item = new User();
            item = getUserById(user.Id);
            CheckField(user.Id, item.Id, "Id");
            CheckField(user.Enabled.ToString(), item.Enabled.ToString(), "Enabled");
            CheckField(user.Login, item.Login, "Login");
            CheckField(user.Email, item.Email, "Email");
            CheckField(user.FirstName, item.FirstName, "FirstName");
            CheckField(user.LastName, item.LastName, "LastName");
            CheckField(user.Description, item.Description, "Description");
            CheckField(user.DisplayName, item.DisplayName, "DisplayName");
            CheckField(user.TelephoneNumber, item.TelephoneNumber, "TelephoneNumber");
            _softAssertions.AssertAll();
        }

        static void CheckField(string fieldSource, string fieldDest, string fieldName)
        {
            if (fieldSource != fieldDest)
                _softAssertions.AddTrue("Field " + fieldName.ToUpper() + " are not same. At created object is " + fieldSource + ", " +
                    "at getted object is " + fieldDest);
        }


        private static void createGetUserRQ(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLUser(userId));
             createGetRQ();
        }

        private static void createPostUserRQ()
        {
            request = (HttpWebRequest)WebRequest.Create(connect.AMUriAPI + "/realms/" + connect.TestRealmName + "/users?api-version=1.0");
            createPostRQ();
        }

        private static void createPutUserRQ(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLUser(userId));
             createPutRQ();
        }

        private static void createPatchUserRQ(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLUser(userId));
            createPatchRQ();
        }

        private static void createDeleteUserRQ(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLUser(userId));
            createDeleteRQ();
        }

        private static User createUserOk()
        {
            User item = new User();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); ;
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    item = JsonConvert.DeserializeObject<User>(objText);
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
