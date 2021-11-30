using Microsoft.VisualStudio.TestTools.UnitTesting;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using System;
using System.Collections.Generic;

namespace RethinkDbTest.src
{
    [TestClass]
    public class DbManagerTest
    {
        private IList<string> hostPortsOneNode = new List<String>() { "192.168.1.57:28016" };
        private IList<string> hostPortsOneNodeWrong = new List<String>() { "192.168.1.57:29016" };
        

        [TestMethod]
        public void TestConnectionFailureAfterTimeOut()
        {
            //IUtilityRethink utilityRethink = new UtilityRethink("test", hostPortsOneNode);
            //IUtilityRethink utilityRethinkWrong = new UtilityRethink("test", hostPortsOneNodeWrong);

           //var dbManager = utilityRethink.GetDbManager();
            //new UtilityRethink("test", hostPortsOneNodeWrong);


            Assert.ThrowsException<ConnectionFailureException>( () => new UtilityRethink("test", hostPortsOneNodeWrong) );
        }

        public void TestDeleteTableList()
        {

        }
    }
}
