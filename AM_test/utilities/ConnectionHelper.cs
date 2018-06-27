using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AM_test.utilities
{
    public class ConnectionHelper
    {
        public string AMUriAPI = "";
        public string AMUri = "";
        public string TestRealmName = "";   
        public string certThumbprint = "";

        private ResourceManager rm = null;

        public ConnectionHelper()
        {
            string machineName = Environment.MachineName;
            this.SetConnectionString(machineName);
        }

        public ConnectionHelper(string server)
        {
            this.SetConnectionString(server);
        }
        private void SetConnectionString(string machineName)
        {

            switch (machineName.ToUpper())
            {
                case "QA034":
                    rm = new ResourceManager(typeof(ConnectionSettingsQA034));
                    break;
                case "WS-QA-16":
                    rm = new ResourceManager(typeof(ConnectionSettingsQA034));
                    break;
                default:
                    Assert.Fail("Для машины: {0} нет настроек", machineName);
                    break;
            }
            this.SetConnectionSettings();

        }

        private void SetConnectionSettings()
        {
            this.AMUriAPI = rm.GetString("AMUri") + "/_apis";
            this.AMUri = rm.GetString("AMUri");
            this.TestRealmName = rm.GetString("TestRealmName");
            this.certThumbprint = rm.GetString("certThumbprint");
        }
    }
}
