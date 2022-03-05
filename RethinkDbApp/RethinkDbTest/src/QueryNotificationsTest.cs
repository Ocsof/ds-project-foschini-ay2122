using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using System.Collections.Generic;
using RethinkDbLib.src.TablesManager.Notifications;
using RethinkDbLib;
using System.Net.Sockets;
using System.Net;

namespace RethinkDbTest.src
{
    [TestClass]
    public class QueryNotificationsTest
    {
        private IList<string> hostPortsOneNode;
        private IList<string> hostPortsOneNodeWrong;
        private INotificationProviderDBMS utilityRethink;
        private IQueryNotifications queryNotifications;

        private NotificationNewData notificationNewData;
        private NotificationExec notificationExecution;
        private Guid idExecution;
        private Guid idNewData;
        private DateTime date;


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

            this.queryNotifications = utilityRethink.NotificationsManager.QueryService;

            this.idExecution = Guid.NewGuid();
            Guid idExec = Guid.NewGuid();
            this.date = DateTime.Now;
            this.notificationExecution = new NotificationExec
            {
                Id = idExecution,
                Date = date,
                Text = CreateRandomString(),
                Arg = CreateRandomString(),
                IdExec = idExec
            };

            this.idNewData = Guid.NewGuid();
            this.notificationNewData = new NotificationNewData
            {
                Id = idNewData,
                Date = date,
                Text = CreateRandomString(),
                Arg = CreateRandomString(),
                Table = CreateRandomString()
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.utilityRethink.CloseConnection();
        }

        [TestMethod]
        public void TestConnectionFailureAfterTimeOut()
        {
            Assert.ThrowsException<ConnectionFailureException>(() => new UtilityRethink("test", this.hostPortsOneNodeWrong));
        }

        [TestMethod]
        public void TestGetNotificationByID()
        {
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);

            //la notifica con id "NewDate" sarebbe di tipo NewDate quindi la variabile restituita è null
            Assert.IsNull(this.queryNotifications.NotificationOrNull<NotificationExec>(idNewData));

            //non esiste notifica con questo id casuale per ora
            Assert.IsNull(this.queryNotifications.NotificationOrNull<NotificationExec>(new Guid()));

            Assert.AreEqual(this.notificationNewData.ToString(), this.queryNotifications.NotificationOrNull<NotificationNewData>(idNewData).ToString());

            Assert.AreEqual(this.notificationExecution.ToString(), this.queryNotifications.NotificationOrNull<NotificationExec>(idExecution).ToString());

            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);
        }


        [TestMethod]
        public void TestNewNotification()
        {      
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);

            Assert.AreEqual(this.notificationNewData.ToString(), this.queryNotifications.NotificationOrNull<NotificationNewData>(idNewData).ToString());

            Assert.AreEqual(this.notificationExecution.ToString(), this.queryNotifications.NotificationOrNull<NotificationExec>(idExecution).ToString());

            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);

            //dato che l'ho cancellata deve dare true
            Assert.IsNull(this.queryNotifications.NotificationOrNull<NotificationNewData>(idNewData));

            //dato che l'ho cancellata deve dare true
            Assert.IsNull(this.queryNotifications.NotificationOrNull<NotificationExec>(idExecution));
        }


        [TestMethod]
        public void TestGetNotificationByDate()
        {
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.NotificationsDate<NotificationNewData>(this.date);
            
            foreach (NotificationNewData not in listNotificationNewData)
            {
                Console.WriteLine(not.Date.ToString());
                Assert.AreEqual(this.date.ToString(), not.Date.ToString());
            }
            
            this.date = new DateTime(1995, 1, 1);
            IList<NotificationExec> listNotificationExecution = this.queryNotifications.NotificationsDate<NotificationExec>(this.date);
            Assert.IsTrue(listNotificationExecution.Count == 0); 
            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);
        }


        [TestMethod]
        public void TestGetNotificationByText()
        {
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);
            string textNewData = notificationNewData.Text;
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.NotificationsWithText<NotificationNewData>(textNewData);

            foreach (NotificationNewData not in listNotificationNewData)
            {
                Assert.AreEqual(textNewData, not.Text);
            }

            string textExec = CreateRandomString();
            IList<NotificationExec> listNotificationExecution = this.queryNotifications.NotificationsWithText<NotificationExec>(textExec);
            Assert.IsTrue(listNotificationExecution.Count == 0);
            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);
        }

        [TestMethod]
        public void TestGetNotificationByArg()
        {
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);
            string argNewData = this.notificationNewData.Arg;
            string argExec = this.notificationExecution.Arg;
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.NotificationsWithArg<NotificationNewData>(argNewData);

            foreach (NotificationNewData not in listNotificationNewData)
            {
                Assert.AreEqual(argNewData, not.Arg);
            }

            IList<NotificationExec> listNotificationExecution = this.queryNotifications.NotificationsWithArg<NotificationExec>(argExec);
            foreach (NotificationExec not in listNotificationExecution)
            {
                Assert.AreEqual(argExec, not.Arg);
            }

            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);
        }

        private static String CreateRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }


    }
}
