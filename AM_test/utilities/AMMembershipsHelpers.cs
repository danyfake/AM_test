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
    class AMMembershipsHelpers : Test_Base
    {
        /// <summary>
        /// получение пользователей, содержащихся в группе
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>список пользователей</returns>
        public static Users getGroupUser(string groupId)
        {
            Users userList = new Users();
            createGetGroupUserRQ(groupId);
            char[] bytes = new char[100];
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    userList = JsonConvert.DeserializeObject<Users>(objText);
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
            Assert.AreEqual("User", userList.ItemType);
            return userList;
        }

        /// <summary>
        /// получение групп, в которых есть пользователь
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <returns>список групп</returns>
        public static Groups getUserGroup(string userId)
        {
            Groups groupList = new Groups();
            createGetUserGroupRQ(userId);
            char[] bytes = new char[100];
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Assert.AreEqual(response.StatusCode.ToString(), "OK", "StatusCode is not ОК");
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    var objText = stream.ReadLine();
                    groupList = JsonConvert.DeserializeObject<Groups>(objText);
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not process response to request " + e);
            }
            Assert.AreEqual("Group", groupList.ItemType);
            return groupList;
        }

        public static bool checkMemberGroupUser(string groupId, string userId)
        {
            Users userList = getGroupUser(groupId);
            bool check = false;
            for (int i = 0; i < userList.Items.Count; i++)
            {
                if (userList.Items[i].Id == userId) check = true;
            }
            return check;
        }

        public static bool checkMemberUserGroup(string groupId, string userId)
        {
            Groups groupList = getUserGroup(userId);
            bool check = false;
            for (int i = 0; i < groupList.Items.Count; i++)
            {
                if (groupList.Items[i].Id == groupId) check = true;
            }
            return check;
        }

        public static void getGroupNotExist(string groupId)
        {
            createGetUserGroupRQ(groupId);
            getError404(request);
        }

        public static void getMemberUserNotExist(string userId)
        {
            createGetGroupUserRQ(userId);
            getError404(request);
        }


        public static void addMemberGroupUserNotExist(string groupId, string userId)
        {
            createPostGroupUserRQ(groupId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + userId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError500(request);
        }

        public static void addMemberUserNotExistGroup(string groupId, string userId)
        {
            createPostUserGroupRQ(userId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + groupId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError404(request);
        }

        public static void addMemberGroupNotExistUser(string groupId, string userId)
        {
            createPostGroupUserRQ(groupId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + userId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError404(request);
        }

        public static void addMemberUserGroupNotExist(string groupId, string userId)
        {
            createPostUserGroupRQ(userId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + groupId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError500(request);
        }

        /// <summary>
        /// добавление пользователей в группу
        /// </summary>
        /// <param name="groupId">id группы</param>
        /// <param name="usersId">массив с id пользователей</param>
        public static void addMemberGroupUsers(string groupId, string[] usersId)
        {
            createPostGroupUserRQ(groupId);
            string postData = "[";
            for (int i = 0; i < usersId.Length; i++)
            {
                postData = postData + "\"" + usersId[i] + "\", ";
            }
            postData = postData + "]";
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
        }


        /// <summary>
        /// добавление пользователей в группу
        /// </summary>
        /// <param name="groupId">id группы</param>
        /// <param name="usersId">массив с id пользователей</param>
        public static void addMemberUserGroups(string userId, string[] groupsId)
        {
            createPostUserGroupRQ(userId);
            string postData = "[";
            for (int i = 0; i < groupsId.Length; i++)
            {
                postData = postData + "\"" + groupsId[i] + "\", ";
            }
            postData = postData + "]";
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
        }

        public static void deleteMemberGroupUsers(string groupId, string[] usersId)
        {
            createDeleteGroupUserRQ(groupId);
            string postData = "[";
            for (int i = 0; i < usersId.Length; i++)
            {
                postData = postData + "\"" + usersId[i] + "\", ";
            }
            postData = postData + "]";
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
        }

        public static void deleteMemberUserGroups(string usersId, string[] groupId )
        {
            createDeleteUserGroupRQ(usersId);
            string postData = "[";
            for (int i = 0; i < groupId.Length; i++)
            {
                postData = postData + "\"" + groupId[i] + "\", ";
            }
            postData = postData + "]";
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
        }

        public static void deleteMemberGroupNotExistUser(string groupId, string userId)
        {
            createDeleteGroupUserRQ(groupId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + userId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError404(request);
        }

        public static void deleteMemberGroupUserNotExist(string groupId, string userId)
        {
            createDeleteGroupUserRQ(groupId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + userId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
        }

        public static void deleteMemberUserGroupNotExist(string groupId, string userId)
        {
            createDeleteUserGroupRQ(userId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + groupId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getNoContentResponse(request);
        }


        public static void deleteMemberUserNotExistGroup(string groupId, string userId)
        {
            createDeleteUserGroupRQ(userId);
            Stream dataStream = request.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes("[\"" + groupId + "\"]");
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            getError404(request);
        }

        private static void createDeleteGroupUserRQ(string groupId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLGroupMember(groupId));
            createDeleteRQ();    
        }

        private static void createPostGroupUserRQ(string groupId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLGroupMember(groupId));
            createPostRQ();
        }

        private static void createGetGroupUserRQ(string groupId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLGroupMember(groupId));
            createGetRQ();
        }

        private static void createDeleteUserGroupRQ(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLUserMember(userId));
            createDeleteRQ();
        }

        private static void createPostUserGroupRQ(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLUserMember(userId));
            createPostRQ();
        }

        private static void createGetUserGroupRQ(string userId)
        {
            request = (HttpWebRequest)WebRequest.Create(getURLUserMember(userId));
            createGetRQ();
        }
    }
}
