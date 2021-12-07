using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using RethinkDbLib.src.Connection;
using RethinkDbLib.src.Exception;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    /// <summary>
    /// Implementazione di Notifier che interroga la tabella di sistema "INotificationsManager.TABLE";
    /// </summary>
    class Notifier<T> : INotifier<T> where T: Notification
    {
        private readonly IConnectionNodes rethinkDbConnection;
        private readonly string dbName;
        private readonly static RethinkDB R = RethinkDB.R;
        private readonly ConcurrentDictionary<Guid, Cursor<Change<T>>> changesDict;  //dizionario Thread safe
        private readonly string tableName;

        public Notifier(IConnectionNodes rethinkDbConnection)
        {
            this.rethinkDbConnection = rethinkDbConnection;
            this.dbName = this.rethinkDbConnection.GetNodi().ElementAt(0).Database;
            changesDict = new ConcurrentDictionary<Guid, Cursor<Change<T>>>();  //Rappresenta una raccolta thread-safe di coppie chiave/valore a cui è possibile accedere contemporaneamente da più thread.
            this.tableName = INotificationsManager.TABLE;
        }

        public NotificationSubscription<T> ListenWithOneOfTheArguments(params string[] argsList)
        {
            var conn = this.rethinkDbConnection.GetConnection();

            var changes = R.Db(dbName).Table(this.tableName)
             .Filter(notification =>
                R.Expr(R.Array(argsList.ToArray())).Contains(notification.G("Arg"))
             )
             .Changes()
             .RunChanges<T>(conn);

            NotificationSubscription<T> subscription = new NotificationSubscription<T>(Guid.NewGuid(), changes.ToObservable());
            if (changesDict.TryAdd(subscription.Guid, changes))
            {
                return subscription;
            }
            //se non riesce ad aggiungere al dizionario perchè Guid già presente:
            throw new NewGuidException();
        }

        public void StopListening(Guid id)
        {
            if (this.changesDict.TryGetValue(id, out Cursor<Change<T>> change))
            {
                change.Close(); //chiude la listening
                Thread.Sleep(3000);
            }
            else //se non entra non trova il change con guid specificato:
            {
                throw new GetGuidException();
            }
        }

        public void StopListening(NotificationSubscription<T> notificationSubscription)
        {
            if (this.changesDict.TryGetValue(notificationSubscription.Guid, out Cursor<Change<T>> change))
            {
                change.Close(); //chiude la listening
                Thread.Sleep(3000);
            }
            else //se non entra non trova il change con guid specificato:
            {
                throw new GetGuidException();
            }
        }
    }
}
