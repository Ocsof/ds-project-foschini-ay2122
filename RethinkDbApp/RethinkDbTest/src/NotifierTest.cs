using Microsoft.VisualStudio.TestTools.UnitTesting;
using RethinkDb.Driver.Model;
using RethinkDbLib;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using RethinkDbLib.src.TablesManager.Notifications;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RethinkDbTest.src
{
    [TestClass]
    public class NotifierTest
    {
        private IList<string> hostPortsOneNode;
        private IList<string> hostPortsOneNodeWrong;
        private INotificationProviderDBMS utilityRethink;

        private INotifier<NotificationExec> notifierExec;
        private INotifier<NotificationNewData> notifierNewData;
        private IQueryNotifications queryNotifications;
        private NotificationExec notificationExec;
        private NotificationNewData notificationNewData;

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
            this.notifierExec = this.utilityRethink.NotificationsManager.Notifier<NotificationExec>();
            this.notifierNewData = this.utilityRethink.NotificationsManager.Notifier<NotificationNewData>();
            this.queryNotifications = utilityRethink.NotificationsManager.QueryService;
        }


        [TestCleanup]
        public void TestCleanup()
        {
            this.utilityRethink.CloseConnection();
        }

        [TestMethod]
        public void TestConnectionFailureAfterTimeOut()
        {
            Assert.ThrowsException<ConnectionFailureException>(() => new UtilityRethink("test", hostPortsOneNodeWrong));
        }

        [TestMethod]
        public void TestClientCanBeNotified()
        {
            var onCompleted = 0;
            var onError = 0;
            var onNext = 0;

            NotificationSubscription<NotificationExec> subscription = this.notifierExec.ListenWithOneOfTheArguments("ciao", "ciuppa");
            IObservable<Change<NotificationExec>> observervableExecForArgs = subscription.Observable;
            observervableExecForArgs.SubscribeOn(NewThreadScheduler.Default)
                .Subscribe(
                    x => OnNext(x, ref onNext),
                    e => OnError(e, ref onError),
                    () => OnCompleted(ref onCompleted)
                );


            Task.Run(() =>
            {
                this.notificationExec = new NotificationExec
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Text = CreateRandomString(),
                    Arg = "ciao",
                    IdExec = Guid.NewGuid()
                };

                this.queryNotifications.NewNotification<NotificationExec>(this.notificationExec);
            });


            this.notifierExec.StopListening(subscription.Guid);
            //notificatorsExec.StopListening(pair);

            //notificatorsNewData.StopListening();



        }


        private static void OnCompleted(ref int onCompleted)
        {
            onCompleted++;
        }

        private static void OnError(Exception obj, ref int onError)
        {
            onError++;
        }

        private void OnNext<T>(Change<T> obj, ref int onNext) where T : Notification
        {
            onNext++;
            Assert.AreEqual(this.notificationExec.ToString(), obj.NewValue.ToString());
 
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
