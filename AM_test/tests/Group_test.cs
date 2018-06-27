using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AM_test.utilities;
using AM_test.objects;

namespace AM_test.tests
{
    [TestClass]
    public class Group_test
    {
        public static Random rnd = new Random();

        [TestMethod]
        public void getGroup()
        {
            string groupId = AMGroupHelpers.getRandomGroupId();
            AMGroupHelpers.getGroupById(groupId);
        }

        [TestMethod]
        public void getGroups()
        {
            AMGroupHelpers.getGroups();
        }

        [TestMethod]
        public void getGroupNotExist()
        {
            AMGroupHelpers.getGroupNotExist(Guid.NewGuid().ToString());
        }


        [TestMethod]
        public void deleteGroup()
        {
            Group group = new Group("group_test" + rnd.Next(10000), true);
            group = AMGroupHelpers.createGroup(group);
            AMGroupHelpers.deleteGroup(group.Id);
            AMGroupHelpers.getGroupNotExist(group.Id);
        }

        /// <summary>
        /// удаление группы, в которую входят пользователи
        /// </summary>
        [TestMethod]
        public void deleteGroupWithMembers()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            AMGroupHelpers.deleteGroup(group.Id);
            Groups groupsList = AMMembershipsHelpers.getUserGroup(user.Id);
            Assert.AreEqual(0, groupsList.TotalResults, "Пользователь входит в состав группы");
            AMUserHelpers.deleteUser(user.Id);
        }

        [TestMethod]
        public void deleteGroupNotExist()
        {
            AMGroupHelpers.deleteGroupNotExist(Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void createGroup()
        {
            Group group = new Group("group_test" + rnd.Next(10000), true);
            group = AMGroupHelpers.createGroup(group);
            AMGroupHelpers.checkGroupField(group);
        }

        [TestMethod]
        public void createFullGroup()
        {
            Group group = new Group();
            group = AMGroupHelpers.createFullGroup();
            AMGroupHelpers.checkGroupField(group);
        }

        [TestMethod]
        public void updateGroup()
        {
            Group group = new Group("group_test" + rnd.Next(10000), true);
            group = AMGroupHelpers.createGroup(group);
            group = AMGroupHelpers.updateGroup(group);
            AMGroupHelpers.checkGroupField(group);
            AMGroupHelpers.deleteGroup(group.Id);
        }

        [TestMethod]
        public void updateGroupNotExist()
        {
            AMGroupHelpers.updateGroupNotExist(Guid.NewGuid().ToString());

        }

        [TestMethod]
        public void createGroupSameName()
        {
            Group group = new Group("group_test" + rnd.Next(10000), true);
            group = AMGroupHelpers.createGroup(group);
            AMGroupHelpers.createGroupSameName(group.Name);
            AMGroupHelpers.deleteGroup(group.Id);
        }

        [TestMethod]
        public void createGroupNoName()
        {
            AMGroupHelpers.createGroupNoName();
        }

        [TestMethod]
        public void createGroupNoEnable()
        {
            string groupName = "group_test" + rnd.Next(10000);
            Group group = AMGroupHelpers.createGroupNoEnable(groupName);
            Group item = new Group();
            item = AMGroupHelpers.getGroupById(group.Id);
            Assert.IsFalse(item.Enabled, "Enabled is not FALSE");
            AMGroupHelpers.deleteGroup(group.Id);
        }
    }

}
