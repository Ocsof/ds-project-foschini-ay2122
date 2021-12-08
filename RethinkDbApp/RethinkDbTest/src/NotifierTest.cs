using Microsoft.VisualStudio.TestTools.UnitTesting;
using RethinkDb.Driver.Model;
using RethinkDbLib;
using RethinkDbLib.src;
using RethinkDbLib.src.Exception;
using RethinkDbLib.src.TablesManager.Notifications;
using System;
using System.Collections.Generic;
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

        [TestInitialize]
        public void TestInitialize()
        {
            this.hostPortsOneNode = new List<String>() { "192.168.1.57:28016" };
            this.hostPortsOneNodeWrong = new List<String>() { "192.168.1.57:29016" };

            this.utilityRethink = new UtilityRethink("test", hostPortsOneNode);
            this.notifierExec = this.utilityRethink.GetNotificationsManager().GetNotifier<NotificationExec>();
            this.notifierNewData = this.utilityRethink.GetNotificationsManager().GetNotifier<NotificationNewData>();
            this.queryNotifications = utilityRethink.GetNotificationsManager().GetQueryService();
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
        public void TestNotifier()
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

            //Next simulate 3 inserts into Notification table.
            Thread.Sleep(3000);

            Task.Run(() =>
            {
                this.queryNotifications.NewNotification<NotificationExec>(new NotificationExec
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Text = CreateRandomString(),
                    Arg = "ciao",
                    IdExec = Guid.NewGuid()
                });
            });

            Console.WriteLine();
            Thread.Sleep(10000);

            Task.Run(() =>
            {
                this.queryNotifications.NewNotification<NotificationExec>(new NotificationExec
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Text = CreateRandomString(),
                    Arg = "ciuppa",
                    IdExec = Guid.NewGuid()
                });
            });

            Console.WriteLine();
            Thread.Sleep(10000);

            //qui non entra nella onNext perchè l'argomento "pappappero" non è nella lista 
            Task.Run(() =>
            {
                this.queryNotifications.NewNotification<NotificationExec>(new NotificationExec
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Text = CreateRandomString(),
                    Arg = "pappappero",
                    IdExec = Guid.NewGuid()
                });
            });

            Console.WriteLine();
            Thread.Sleep(10000);

            this.notifierExec.StopListening(subscription.Guid);
            //notificatorsExec.StopListening(pair);
            Console.WriteLine();

            //notificatorsNewData.StopListening();



        }


        private static void OnCompleted(ref int onCompleted)
        {
            Console.WriteLine("Stop listening");
            onCompleted++;
        }

        private static void OnError(Exception obj, ref int onError)
        {
            Console.WriteLine("On Error");
            Console.WriteLine(obj.Message);
            onError++;
        }

        private static void OnNext<T>(Change<T> obj, ref int onNext) where T : Notification
        {
            Console.WriteLine("On Next");
            var oldValue = obj.OldValue;

            onNext++;
            Console.WriteLine("New Value: " + obj.NewValue.ToString());
            if (oldValue != null)
            { //nel caso di un update
                Console.WriteLine("Old Value: " + oldValue.ToString());
            }
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
