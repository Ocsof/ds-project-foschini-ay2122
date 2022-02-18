using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using System.Collections.Generic;
using RethinkDbLib.src.TablesManager.Notifications;
using RethinkDbLib;

namespace RethinkDbTest.src
{
    [TestClass]
    public class QueryNotificationsTest
    {
        private IList<string> hostPortsOneNode;
        private IList<string> hostPortsOneNodeWrong;
        private INotificationProviderDBMS utilityRethink;
        private INotificationProviderDBMS utilityRethinkWrong;
        private IQueryNotifications queryNotifications;

        private NotificationNewData notificationNewData;
        private NotificationExec notificationExecution;
        private Guid idExecution;
        private Guid idNewData;


        [TestInitialize]
        public void TestInitialize()
        {
            //ATTENZIONE: Cambiare l'indirizzo IP con il proprio locale
            this.hostPortsOneNode = new List<String>() { "192.168.1.57:28016" };
            this.hostPortsOneNodeWrong = new List<String>() { "192.168.1.57:29016" };

            this.utilityRethink = new UtilityRethink("test", hostPortsOneNode);

            this.queryNotifications = utilityRethink.NotificationsManager.QueryService;

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
            notificationExecution = this.queryNotifications.NotificationOrNull<NotificationExec>(idNewData);
            Assert.IsNull(notificationExecution);

            //non esiste notifica con questo id casuale per ora
            notificationExecution = this.queryNotifications.NotificationOrNull<NotificationExec>(new Guid());
            Assert.IsNull(notificationExecution);

            //qui è ok quindi entra nell'if
            notificationNewData = this.queryNotifications.NotificationOrNull<NotificationNewData>(idNewData);
            if (notificationNewData != null)
            {
                Console.WriteLine("Notification with id: " + idNewData.ToString() + " : ");
                Console.WriteLine(notificationNewData.ToString());
            }

            //qui tutto ok ed entra nell'if
            notificationExecution = this.queryNotifications.NotificationOrNull<NotificationExec>(idExecution);
            if (notificationExecution != null)
            {
                Console.WriteLine("Notification with id: " + idExecution.ToString() + " : ");
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
            notificationNewData = this.queryNotifications.NotificationOrNull<NotificationNewData>(idNewData);
            if (notificationNewData != null)
            {
                Console.WriteLine("Notification with id: " + idNewData.ToString() + " : ");
                Console.WriteLine(notificationNewData.ToString());
            }

            //qui tutto ok ed entra nell'if
            notificationExecution = this.queryNotifications.NotificationOrNull<NotificationExec>(idExecution);
            if (notificationExecution != null)
            {
                Console.WriteLine("Notification with id: " + idExecution.ToString() + " : ");
                Console.WriteLine(notificationExecution.ToString());
            }

            this.queryNotifications.DeleteNotification(idNewData);
            this.queryNotifications.DeleteNotification(idExecution);

            //dato che l'ho cancellata deve dare true
            notificationNewData = this.queryNotifications.NotificationOrNull<NotificationNewData>(idNewData);
            Assert.IsNull(notificationNewData);

            //dato che l'ho cancellata deve dare true
            notificationExecution = this.queryNotifications.NotificationOrNull<NotificationExec>(idExecution);
            Assert.IsNull(notificationExecution);
        }


        [TestMethod]
        public void TestGetNotificationByData()
        {
            this.queryNotifications.NewNotification(notificationNewData);
            this.queryNotifications.NewNotification(notificationExecution);
            DateTime newDataDate = notificationNewData.Date;
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.NotificationsDate<NotificationNewData>(newDataDate);
            if (listNotificationNewData.Count != 0)
            {
                Console.WriteLine("New data Notification in date: " + newDataDate.ToString() + ": ");
                foreach (NotificationNewData not in listNotificationNewData)
                {
                    Console.WriteLine(not.ToString());
                    Console.WriteLine();
                }
            }

            newDataDate = new DateTime(1995, 1, 1);
            IList<NotificationExec> listNotificationExecution = this.queryNotifications.NotificationsDate<NotificationExec>(newDataDate);
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
            if (listNotificationNewData.Count != 0)
            {
                Console.WriteLine("New Data Notification with text: " + textNewData);
                foreach (NotificationNewData not in listNotificationNewData)
                {
                    Console.WriteLine(not.ToString());
                    Console.WriteLine();
                }
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
            string argNewData = notificationNewData.Arg;
            string argExec = notificationExecution.Arg;
            IList<NotificationNewData> listNotificationNewData = this.queryNotifications.NotificationsWithArg<NotificationNewData>(argNewData);
            if (listNotificationNewData.Count != 0)
            {
                Console.WriteLine("New data notification with arg: " + argNewData);
                foreach (NotificationNewData not in listNotificationNewData)
                {
                    Console.WriteLine(not.ToString());
                    Console.WriteLine();
                }
            }
            IList<NotificationExec> listNotificationExecution = this.queryNotifications.NotificationsWithArg<NotificationExec>(argExec);
            if (listNotificationExecution.Count != 0)
            {
                Console.WriteLine("Execution Notification with Arg: " + argExec);
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
