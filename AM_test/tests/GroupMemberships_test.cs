using System;
using AM_test.utilities;
using AM_test.objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AM_test.tests
{
    [TestClass]
    public class GroupMemberships_test
    {
        /// <summary>
        /// добавить удаление всех созданных групп и пользователей после выполнения тестов
        /// </summary>
        public static Random rnd = new Random();



        /// <summary>
        /// получить группу, в которой нет пользователей
        /// </summary>
        [TestMethod]
        public void getMemberGroupNoUser()
        {
            Group group = new Group("group_test" + rnd.Next(100000), true);
            group = AMGroupHelpers.createGroup(group);
            Users userList = AMMembershipsHelpers.getGroupUser(group.Id);
            Assert.AreEqual(0, userList.Items.Count, "Группа содержит пользователей");
            AMGroupHelpers.deleteGroup(group.Id);
        }


        [TestMethod]
        public void getMemberGroupNotExist()
        {
            AMMembershipsHelpers.getGroupNotExist(Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void addMemberGroupUser()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberGroupUsers(group.Id, new string[] { user.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
        }


        /// <summary>
        /// добавить в группу пользователя, которого нет
        /// </summary>
        [TestMethod]
        public void addMemberGroupUserNotExist()
        {
            Group group = new Group("group_test" + rnd.Next(100000), true);           
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberGroupUserNotExist(group.Id, Guid.NewGuid().ToString());
            AMGroupHelpers.deleteGroup(group.Id);
        }

        /// <summary>
        /// добавить в несуществующую группу пользователя
        /// </summary>
        [TestMethod]
        public void addMemberGroupNotExistUser()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            AMMembershipsHelpers.addMemberGroupNotExistUser(Guid.NewGuid().ToString(), user.Id);
            AMUserHelpers.deleteUser(user.Id);
        }

        /// <summary>
        /// добавление второго пользователя в ту же группу
        /// </summary>
        [TestMethod]
        public void addMemberGroupAnotherUser()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            User user2 = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            user2 = AMUserHelpers.createUser(user2);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberGroupUsers(group.Id, new string[] { user.Id });
            AMMembershipsHelpers.addMemberGroupUsers(group.Id, new string[] { user2.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user2.Id), "Пользователя " + user2.Id + " нет в группе " + group.Id);
            AMUserHelpers.deleteUser(user.Id);
            AMUserHelpers.deleteUser(user2.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            
        }


        /// <summary>
        /// добавление нескольких пользователей в одну группу
        /// </summary>
        [TestMethod]
        public void addMemberGroupFewUsers()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            User user2 = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            user2 = AMUserHelpers.createUser(user2);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberGroupUsers(group.Id, new string[] { user.Id, user2.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user2.Id), "Пользователя " + user2.Id + " нет в группе " + group.Id);    
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
            AMUserHelpers.deleteUser(user2.Id);
        }

        [TestMethod]
        public void deleteMemberGroupUser()
        {
            User user = new User("user_test" + rnd.Next(100000), true);        
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberGroupUsers(group.Id, new string[] { user.Id });
            AMMembershipsHelpers.deleteMemberGroupUsers(group.Id, new string[] { user.Id });
            Assert.IsFalse(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователь " + user.Id + " не удален из группы " + group.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
        }

        [TestMethod]
        public void deleteMemberGroupFewUsers()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            User user2 = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            user2 = AMUserHelpers.createUser(user2);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberGroupUsers(group.Id, new string[] { user.Id, user2.Id });
            AMMembershipsHelpers.deleteMemberGroupUsers(group.Id, new string[] { user.Id, user2.Id });
            Assert.IsFalse(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователь " + user.Id + " не удален из группы " + group.Id);
            Assert.IsFalse(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user2.Id), "Пользователь " + user2.Id + " не удален из группы " + group.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
            AMUserHelpers.deleteUser(user2.Id);
        }


        [TestMethod]
        public void deleteMemberGroupNotExistUser()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            AMMembershipsHelpers.deleteMemberGroupNotExistUser(Guid.NewGuid().ToString(), user.Id);        
            AMUserHelpers.deleteUser(user.Id);
        }


        [TestMethod]
        public void deleteMemberGroupUserNotExist()
        {
            Group group = new Group("group_test" + rnd.Next(100000), true);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.deleteMemberGroupUserNotExist(group.Id, Guid.NewGuid().ToString());
            AMGroupHelpers.deleteGroup(group.Id);
        }

        /// <summary>
        /// получить пользователя, который не состоит в группах
        /// </summary>
        [TestMethod]
        public void getMemberUserNoGroup()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            Groups groupList = AMMembershipsHelpers.getUserGroup(user.Id);
            Assert.AreEqual(0, groupList.Items.Count, "Пользователь состоит в группах");
            AMUserHelpers.deleteUser(user.Id);
        }

        [TestMethod]
        public void getMemberUserNotExist()
        {
            AMMembershipsHelpers.getMemberUserNotExist(Guid.NewGuid().ToString());
        }


        [TestMethod]
        public void addMemberUserGroup()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
        }

        
        /// <summary>
        /// добавить в группу пользователя, которого нет
        /// </summary>
        [TestMethod]
        public void addMemberUserNotExistGroup()
        {
            Group group = new Group("group_test" + rnd.Next(100000), true);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberUserNotExistGroup(group.Id, Guid.NewGuid().ToString());
            AMGroupHelpers.deleteGroup(group.Id);
        }
        
        /// <summary>
        /// добавить в несуществующую группу пользователя
        /// </summary>
        [TestMethod]
        public void addMemberUserGroupNotExist()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            AMMembershipsHelpers.addMemberUserGroupNotExist(Guid.NewGuid().ToString(), user.Id);
            AMUserHelpers.deleteUser(user.Id);
        }

        
        /// <summary>
        /// добавление пользователя во вторую группу
        /// </summary>
        [TestMethod]
        public void addMemberUserAnotherGroup()
        {
            User user = new User("user_test" + rnd.Next(100000), true);          
            Group group = new Group("group_test" + rnd.Next(100000), true);
            Group group2 = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            group2 = AMGroupHelpers.createGroup(group2);
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group.Id });
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group2.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group2.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group2.Id);
            AMUserHelpers.deleteUser(user.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMGroupHelpers.deleteGroup(group2.Id);
        }

        
        /// <summary>
        /// добавление пользователя в несколько групп
        /// </summary>
        [TestMethod]
        public void addMemberUserFewGroups()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            Group group2 = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            group2 = AMGroupHelpers.createGroup(group2);
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group.Id, group2.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberUserGroup(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            Assert.IsTrue(AMMembershipsHelpers.checkMemberUserGroup(group2.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group2.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
            AMGroupHelpers.deleteGroup(group2.Id);
        }

        
        [TestMethod]
        public void deleteMemberUserGroup()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group.Id });
            AMMembershipsHelpers.deleteMemberUserGroups(user.Id, new string[] { group.Id });
            Assert.IsFalse(AMMembershipsHelpers.checkMemberUserGroup(group.Id, user.Id), "Пользователь " + user.Id + " не удален из группы " + group.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
        }

        
        [TestMethod]
        public void deleteMemberUserFewGroups()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            Group group2 = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            group2 = AMGroupHelpers.createGroup(group2);
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group.Id, group2.Id });
            AMMembershipsHelpers.deleteMemberUserGroups(user.Id, new string[] { group.Id, group2.Id });
            Assert.IsFalse(AMMembershipsHelpers.checkMemberUserGroup(group.Id, user.Id), "Пользователь " + user.Id + " не удален из группы " + group.Id);
            Assert.IsFalse(AMMembershipsHelpers.checkMemberUserGroup(group2.Id, user.Id), "Пользователь " + user.Id + " не удален из группы " + group2.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            AMUserHelpers.deleteUser(user.Id);
            AMGroupHelpers.deleteGroup(group2.Id);
        }

        
        [TestMethod]
        public void deleteMemberUserGroupNotExist()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            AMMembershipsHelpers.deleteMemberUserGroupNotExist(Guid.NewGuid().ToString(), user.Id);
            AMUserHelpers.deleteUser(user.Id);
        }



        [TestMethod]
        public void deleteMemberUserNotExistGroup()
        {
            Group group = new Group("group_test" + rnd.Next(100000), true);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.deleteMemberUserNotExistGroup(group.Id, Guid.NewGuid().ToString());
            AMGroupHelpers.deleteGroup(group.Id);
        }
    }
}
