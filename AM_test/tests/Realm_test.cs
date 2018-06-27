using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AM_test.utilities;
using AM_test.objects;
using System.Threading.Tasks;

namespace AM_test.tests
{
    [TestClass]
    public class Realm_test: Test_Base
    {
        
        [TestMethod]
        public void getRealms()
        {
            Realms realmList = AMRealmHelper.getRealms();
            Assert.AreEqual("Realm", realmList.ItemType, "Не верный тип объектов");
        }

        [TestMethod]
        public void getRealm()
        {
            Realms realmList = AMRealmHelper.getRealms();
            string id = realmList.Items[rnd.Next(realmList.Items.Count)].Id;
            Realm realm = AMRealmHelper.getRealm(id);
            Assert.AreEqual(id, realm.Id, "Вернулся другой realm");
        }

        [TestMethod]
        public void getRealmNotExist()
        {
            string id = "realm_name" + rnd.Next(100000);
            AMRealmHelper.getRealmNotExist(id);
        }

        [TestMethod]
        public async Task addRealm()
        {
            await LoginHelper.AMloginAsyncSuccess("admin", "123456");
            string realmName = "realmtest" + rnd.Next(100000);
            await AMRealmHelper.addRealm(realmName);
            Assert.IsTrue(AMRealmHelper.checkRealmExist(realmName), "Realm " + realmName + " not Exist");
            Assert.IsTrue(AMRealmHelper.checkRealmIsActive(realmName), "Realm " + realmName + " is not active");
            await LoginHelper.AMlogOutAsync();
        }

        [TestMethod]
        public async Task addRealmSameName()
        {
            await LoginHelper.AMloginAsyncSuccess("admin", "123456");
            string realmName = "realmtest" + rnd.Next(100000);
            await AMRealmHelper.addRealm(realmName);
            await AMRealmHelper.addRealmSameName(realmName);
            await AMRealmHelper.deleteRealm(realmName);
            await LoginHelper.AMlogOutAsync();
        }

        [TestMethod]
        public async Task deleteRealm()
        {
            await LoginHelper.AMloginAsyncSuccess("admin", "123456");
            string realmName = "realmtest" + rnd.Next(100000);
            //await AMRealmHelper.addRealm(realmName);
            await AMRealmHelper.deleteRealm("realmtest37012");
            Assert.IsFalse(AMRealmHelper.checkRealmExist(realmName), "Realm " + realmName + " is not deleted");
            await LoginHelper.AMlogOutAsync();
        }

        [TestMethod]
        public async Task updateActiveRealm()
        {
            await LoginHelper.AMloginAsyncSuccess("admin", "123456");
            string realmName = "realmtest" + rnd.Next(100000);
            await AMRealmHelper.addRealm(realmName);
            Assert.IsTrue(AMRealmHelper.checkRealmExist(realmName), "Realm " + realmName + " not Exist");
            await AMRealmHelper.updateRealm(realmName);
            Assert.IsFalse(AMRealmHelper.checkRealmIsActive(realmName), "Realm " + realmName + " is active");
            await AMRealmHelper.deleteRealm(realmName);
        }
    }
}
