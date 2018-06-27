using System;
using System.Threading.Tasks;
using AM_test.objects;
using AM_test.utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AM_test.tests
{
    [TestClass]
    public class Login : Test_Base
    {

        [TestMethod]
        public async Task loginSuccess()
        {
            await LoginHelper.AMloginAsyncSuccess("admin", "123456");
            await LoginHelper.AMlogOutAsync();
        }


        [TestMethod]
        public async Task loginWrongLogin()
        {
            await LoginHelper.AMloginAsyncWrongLogin();
        }

        [TestMethod]
        public async Task loginWrongPassword()
        {
            await LoginHelper.AMloginAsyncWrongPassword();
        }

        /// <summary>
        /// успешная аутентификация, после ввода неверного пароля
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task loginAfterWrongPassword()
        {
            await LoginHelper.AMloginAfterWrongPassword();
            await LoginHelper.AMlogOutAsync();
        }


        /// <summary>
        /// Залогиниться под пользователем, у которого нет прав для входа в приложение
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task loginUserNoRights()
        {
            string login = "user_no_rights" + rnd.Next(10000);
            User user = new User(login, true);
            Password pass = new Password();
            pass.Value = "123456";
            pass.EffectiveDate = DateTime.Now;
            pass.ExpirationDate = DateTime.Now.AddMonths(3);
            user.Password = pass;
            user = AMUserHelpers.createRootUser(user);
            await LoginHelper.AMloginUserNoRights(user.Login, pass.Value);
            AMUserHelpers.deleteRootUser(user.Id);
        }

        [TestMethod]
        public async Task loginByNewUser()
        {
            string login = "new_admin" + rnd.Next(10000);
            User user = new User(login, true);
            Password pass = new Password();
            pass.Value = "123456";
            pass.EffectiveDate = DateTime.Now;
            pass.ExpirationDate = DateTime.Now.AddMonths(3);
            user.Password = pass;
            user = AMUserHelpers.createRootUser(user);
            await LoginHelper.AMloginAsyncSuccess(user.Login, pass.Value);
            AMUserHelpers.deleteRootUser(user.Id);
        }
    }
}
