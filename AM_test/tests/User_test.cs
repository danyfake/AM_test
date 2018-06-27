using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AM_test.utilities;
using AM_test.objects;
using System.Net.Http;

namespace AM_test.tests
{
    [TestClass]
    public class User_test
    {
        public static Random rnd = new Random();

        /// <summary>
        /// получение пользователя
        /// </summary>
        [TestMethod]
        public void getUser()
        {
            string userId = AMUserHelpers.getRandomUserId();
            User user = AMUserHelpers.getUserById(userId);
        }

        /// <summary>
        /// получение не существующего пользователя
        /// </summary>
        [TestMethod]
        public void getUserNotExist()
        {
            AMUserHelpers.getUserNotExist(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// получение списка пользователей
        /// </summary>
        [TestMethod]
        public void getUsers()
        {
            AMUserHelpers.getUsers();        
        }

        /// <summary>
        /// удаление пользователя, проверяем, что в ответ на запрос удаленного пользователя вернулось 404
        /// </summary>
        [TestMethod]
        public void deleteUser()
        {
            User user = new User("user_test" + rnd.Next(10000), true);
            user = AMUserHelpers.createUser(user);
            AMUserHelpers.deleteUser(user.Id);
            AMUserHelpers.getUserNotExist(user.Id);
        }

        /// <summary>
        /// удаление несуществующего пользователя
        /// </summary>
        [TestMethod]
        public void deleteUserNotExist()
        {
            AMUserHelpers.deleteUserNotExist(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// удаление пользователя, который входит в группы
        /// </summary>
        [TestMethod]
        public void deleteUserWithGroup()
        {
            User user = new User("user_test" + rnd.Next(100000), true);
            Group group = new Group("group_test" + rnd.Next(100000), true);
            user = AMUserHelpers.createUser(user);
            group = AMGroupHelpers.createGroup(group);
            AMMembershipsHelpers.addMemberUserGroups(user.Id, new string[] { group.Id });
            Assert.IsTrue(AMMembershipsHelpers.checkMemberGroupUser(group.Id, user.Id), "Пользователя " + user.Id + " нет в группе " + group.Id);
            AMUserHelpers.deleteUser(user.Id);
            Users usersList = AMMembershipsHelpers.getGroupUser(group.Id);
            Assert.AreEqual(0, usersList.TotalResults, "В группе остались пользователи");
            AMGroupHelpers.deleteGroup(group.Id);
        }

        /// <summary>
        /// создание пользователя с заполнением основных полей: логин, Enabled = true, время изменения пользователя
        /// </summary>
        [TestMethod]
        public void createUser()
        {
            User user = new User("user_test" + rnd.Next(10000), true);
            user = AMUserHelpers.createUser(user);
            AMUserHelpers.checkUserField(user);
        }

        /// <summary>
        /// создание пользователя с заполнением всех полей
        /// </summary>
        [TestMethod]
        public void createFullUser()
        {
            User user = new User();
            user = AMUserHelpers.createFullUser();
            AMUserHelpers.checkUserField(user);
            AMUserHelpers.deleteUser(user.Id);
        }

        /// <summary>
        /// обновления данных существующего пользователя
        /// </summary>
        [TestMethod]
        public void updateUser()
        {
            User user = new User("user_test" + rnd.Next(10000), true);
            user = AMUserHelpers.createUser(user);
            user = AMUserHelpers.updateUser(user);
            AMUserHelpers.checkUserField(user);
            AMUserHelpers.deleteUser(user.Id);
        }

        /// <summary>
        /// обновления данных существующего пользователя методом patch
        /// </summary>
        //[TestMethod]
        public void updateUserByPatch()
        {
            User user = new User("user_test" + rnd.Next(10000), true);
            user = AMUserHelpers.createUser(user);
            user = AMUserHelpers.updateUserByPatch(user);
            AMUserHelpers.checkUserField(user);
            //AMUserHelpers.deleteUser(user.Id);
        }


        /// <summary>
        /// проверка, что возвращается ошибка 404, при обновлении пользователя, который не существует в АМ
        /// </summary>
        [TestMethod]
        public void updateUserNotExist()
        {
            AMUserHelpers.updateUserNotExist(Guid.NewGuid().ToString());
            
        }

        /// <summary>
        /// проверка, что возвращается ошибка 409, если пользователь с таким логином уже есть
        /// </summary>
        [TestMethod]
        public void createUserSameLogin()
        {
            User user = new User("user_test" + rnd.Next(10000), true);
            user = AMUserHelpers.createUser(user);
            AMUserHelpers.createUserSameLogin(user.Login);
            AMUserHelpers.deleteUser(user.Id);
        }

        /// <summary>
        /// проверка, что возвращается ошибка 400, если не указан логин
        /// </summary>
        [TestMethod]
        public void createUserNoLogin()
        {
            AMUserHelpers.createUserNoLogin();
        }

        /// <summary>
        /// если не заполнен обязательный параметр Enable, автоматически подставляется false.
        /// </summary>
        [TestMethod]
        public void createUserNoEnable()
        {
            string login = "user_test" + rnd.Next(10000);
            User user = AMUserHelpers.createUserNoEnable(login);
            User item = new User();
            item = AMUserHelpers.getUserById(user.Id);
            Assert.IsFalse(item.Enabled, "Enabled is not FALSE");
            AMUserHelpers.deleteUser(user.Id);
        }
    }
}
