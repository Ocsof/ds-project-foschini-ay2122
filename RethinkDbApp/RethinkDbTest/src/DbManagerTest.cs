using Microsoft.VisualStudio.TestTools.UnitTesting;
using RethinkDbLib;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace RethinkDbTest.src
{
    [TestClass]
    public class DbManagerTest
    {
        private IList<string> hostPortsOneNode; 
        private IList<string> hostPortsOneNodeWrong; 
        private INotificationProviderDBMS utilityRethink;
        private INotificationProviderDBMS utilityRethinkWrong;


        [TestInitialize]
        public void TestInitialize()
        {
            hostPortsOneNode = new List<String>();
            hostPortsOneNodeWrong = new List<String>();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    hostPortsOneNode.Add(ip.ToString() + ":28016");
                    hostPortsOneNodeWrong.Add(ip.ToString() + ":29016");
                }
            }

            utilityRethink = new UtilityRethink("test", hostPortsOneNode);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.utilityRethink.CloseConnection();
        }

        [TestMethod]
        public void TestConnectionFailureAfterTimeOut()
        {
            Assert.ThrowsException<ConnectionFailureException>( () => new UtilityRethink("test", hostPortsOneNodeWrong) );
        }

        [TestMethod]
        public void TestDeleteTableList()
        {
            var dbManager = utilityRethink.DBManager;
            Assert.ThrowsException<DeleteTableSystemException>(() => dbManager.DeleteTable("Notifications") );
        }
    }
}
