using Microsoft.VisualStudio.TestTools.UnitTesting;
using RethinkDbLib;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using System;
using System.Collections.Generic;

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
            hostPortsOneNode  = new List<String>() { "192.168.1.57:28016" };
            hostPortsOneNodeWrong = new List<String>() { "192.168.1.57:29016" };

            utilityRethink = new UtilityRethink("test", hostPortsOneNode);
            //utilityRethinkWrong = new UtilityRethink("test", hostPortsOneNodeWrong);
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
            var dbManager = utilityRethink.GetDbManager();
            Assert.ThrowsException<DeleteTableSystemException>(() => dbManager.DeleteTable("Notifications") );
        }
    }
}
