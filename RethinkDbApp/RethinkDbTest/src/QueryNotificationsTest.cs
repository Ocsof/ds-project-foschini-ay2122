using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using System.Collections.Generic;
using RethinkDbLib.src.TablesManager.Notifications;

namespace RethinkDbTest.src
{
    [TestClass]
    public class QueryNotificationsTest
    {
        private IList<string> hostPortsOneNode;
        private IList<string> hostPortsOneNodeWrong;
        private IUtilityRethink utilityRethink;
        private IUtilityRethink utilityRethinkWrong;
        private IQueryNotifications queryNotifications;

        private NotificationNewData notificationNewData;
        private NotificationExec notificationExecution;
        private Guid idExecution;
        private Guid idNewData;


        [TestInitialize]
        public void TestInitialize()
        {
            this.hostPortsOneNode = new List<String>() { "192.168.1.57:28016" };
            this.hostPortsOneNodeWrong = new List<String>() { "192.168.1.57:29016" };

            this.utilityRethink = new UtilityRethink("test", hostPortsOneNode);

            this.queryNotifications = utilityRethink.GetNotificationsManager().GetQueryService();

            this.idExecution = Guid.NewGuid();
            Guid idExec = Guid.NewGuid();
            this.notificationExecution = new NotificationExec
            {
                Id = idExecution,
                Date = DateTime.Now,//new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Text = CreateRandomString(),
                Arg = CreateRandomString(),
                IdExec = idExec
            };

            this.idNewData = Guid.NewGuid();
            this.notificationNewData = new NotificationNewData
            {
                Id = idNewData,
                Date = DateTime.Now,
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
            notificationExecution = this.queryNotifications.GetNotificationOrNull<NotificationExec>(idNewData);
            Assert.IsNull(notificationExecution);

            //non esiste notifica con questo id casuale per ora
            notificationExecution = this.queryNotifications.GetNotificationOrNull<NotificationExec>(new Guid());
            Assert.IsNull(notificationExecution);

            //qui è ok quindi entra nell'if
            notificationNewData = this.queryNotifications.GetNotificationOrNull<NotificationNewData>(idNewData);
            if (notificationNewData != null)
            {
                Console.WriteLine("Notifica con id: " + idNewData.ToString() + " : ");
                Console.WriteLine(notificationNewData.ToString());
            }

            //qui tutto ok ed entra nell'if
            notificationExecution = this.queryNotifications.GetNotificationOrNull<NotificationExec>(idExecution);
            if (notificationExecution != null)
            {
                Console.WriteLine("Notifica con id: " + idExecution.ToString() + " : ");
                Console.WriteLine(notificationExecution.ToString());
            }

            Console.WriteLine();
            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);
        }


        [TestMethod]
        public void TestNewNotification()
        {      
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);

            //qui è ok quindi entra nell'if
            notificationNewData = this.queryNotifications.GetNotificationOrNull<NotificationNewData>(idNewData);
            if (notificationNewData != null)
            {
                Console.WriteLine("Notifica con id: " + idNewData.ToString() + " : ");
                Console.WriteLine(notificationNewData.ToString());
            }

            //qui tutto ok ed entra nell'if
            notificationExecution = this.queryNotifications.GetNotificationOrNull<NotificationExec>(idExecution);
            if (notificationExecution != null)
            {
                Console.WriteLine("Notifica con id: " + idExecution.ToString() + " : ");
                Console.WriteLine(notificationExecution.ToString());
            }

            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);

            //dato che l'ho cancellata deve dare true
            notificationNewData = this.queryNotifications.GetNotificationOrNull<NotificationNewData>(idNewData);
            Assert.IsNull(notificationNewData);

            //dato che l'ho cancellata deve dare true
            notificationExecution = this.queryNotifications.GetNotificationOrNull<NotificationExec>(idExecution);
            Assert.IsNull(notificationExecution);
        }


        [TestMethod]
        public void TestGetNotificationByData()
        {
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);
            DateTime newDataDate = notificationNewData.Date;
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.GetNotifications<NotificationNewData>(newDataDate);
            if (listNotificationNewData.Count != 0)
            {
                Console.WriteLine("Notifiche di new data in data: " + newDataDate.ToString() + ": ");
                foreach (NotificationNewData not in listNotificationNewData)
                {
                    Console.WriteLine(not.ToString());
                    Console.WriteLine();
                }
            }

            newDataDate = new DateTime(1995, 1, 1);
            IList<NotificationExec> listNotificationExecution = this.queryNotifications.GetNotifications<NotificationExec>(newDataDate);
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
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.GetNotificationsWithText<NotificationNewData>(textNewData);
            if (listNotificationNewData.Count != 0)
            {
                Console.WriteLine("Notifiche di new data con text: " + textNewData);
                foreach (NotificationNewData not in listNotificationNewData)
                {
                    Console.WriteLine(not.ToString());
                    Console.WriteLine();
                }
            }

            string textExec = CreateRandomString();
            IList<NotificationExec> listNotificationExecution = this.queryNotifications.GetNotificationsWithText<NotificationExec>(textExec);
            Assert.IsTrue(listNotificationExecution.Count == 0);
            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);
        }

        [TestMethod]
        public void TestGetNotificationByArg()
        {
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);
            string argNewData = notificationNewData.Arg;
            string argExec = notificationExecution.Arg;
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.GetNotificationsWithArg<NotificationNewData>(argNewData);
            if (listNotificationNewData.Count != 0)
            {
                Console.WriteLine("Notifica di new data con Arg: " + argNewData);
                foreach (NotificationNewData not in listNotificationNewData)
                {
                    Console.WriteLine(not.ToString());
                    Console.WriteLine();
                }
            }
            IList<NotificationExec> listNotificationExecution = this.queryNotifications.GetNotificationsWithArg<NotificationExec>(argExec);
            if (listNotificationExecution.Count != 0)
            {
                Console.WriteLine("Notifica di exec con Arg: " + argExec);
                foreach (NotificationExec not in listNotificationExecution)
                {
                    Console.WriteLine(not.ToString());
                    Console.WriteLine();
                }
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
