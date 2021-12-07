
using System;
using System.Collections.Generic;
using System.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;
using RethinkDbLib.src.Connection;
using RethinkDbLib.src.Exception;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    /// <summary>
    /// Implementazione di IQueryNotifications che interroga tabella di sistema "INotificationsManager.TABLE";
    /// </summary>
    class QueryNotifications : IQueryNotifications
    {
        private readonly IConnectionNodes connection;
        private readonly static RethinkDB R = RethinkDB.R;
        private readonly string dbName;
        private readonly string tableName;

        public QueryNotifications(IConnectionNodes connection)
        {
            this.connection = connection;
            this.dbName = connection.GetNodi().ElementAt(0).Database;
            this.tableName = INotificationsManager.TABLE;
        }

        public void DeleteNotification(Guid id)
        {
            var conn = this.connection.GetConnection();

            R.Db(this.dbName).Table(this.tableName).Get(id).Delete().Run(conn);
        }

        public T GetNotificationOrNull<T>(Guid id) where T : Notification
        {
            var conn = this.connection.GetConnection();
            Cursor<T> notification;
            try
            {
                notification = R.Db(this.dbName).Table(this.tableName)
                           .Filter(notification => notification.G("Type").Eq(typeof(T).Name).And(notification.G("id").Eq(id)))
                           .Run<T>(conn);

                return notification.First();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public IList<T> GetNotifications<T>(DateTime date) where T : Notification
        {
            var conn = this.connection.GetConnection();
            Cursor<T> notifications;

            notifications = R.Db(this.dbName).Table(this.tableName)
                       .Filter(notification => notification.G("Type").Eq(typeof(T).Name).And(notification.G("Date").Date().Eq(new Date(date))))
                       .Run<T>(conn);

            return notifications.ToList();

            /*
            //versione con indice date (su campo Date)
            Cursor<T> notificationss = R.Db(this.dbName).Table(nameof(Notification)).GetAll(date).OptArg("index", "date").Run(conn); ;
            */
        }

        public IList<T> GetNotificationsWithArg<T>(string arg) where T : Notification
        {
            var conn = this.connection.GetConnection();
            Cursor<T> notifications;

            notifications = R.Db(this.dbName).Table(this.tableName)
                       .Filter(notification => notification.G("Type").Eq(typeof(T).Name).And(notification.G("Arg").Eq(arg)))
                       .Run<T>(conn);
            /*
            //versione con indice arg (su campo Arg)
            Cursor<T> notificationss = R.Db(this.dbName).Table(nameof(Notification)).GetAll(arg).OptArg("index", "arg").Run(conn); ;
            */

            return notifications.ToList();
        }

        public IList<T> GetNotificationsWithText<T>(string text) where T : Notification
        {
            var conn = this.connection.GetConnection();
            Cursor<T> notifications;

            notifications = R.Db(this.dbName).Table(this.tableName)
                       .Filter(notification => notification.G("Type").Eq(typeof(T).Name).And(notification.G("Text").Eq(text)))
                       .Run<T>(conn);

            /*
            //versione con indice text (su campo Text)
            Cursor<T> notificationss = R.Db(this.dbName).Table(nameof(Notification)).GetAll(text).OptArg("index", "text").Run(conn); ;
            */

            return notifications.ToList();
        }

        public void NewNotification<T>(T notification) where T : Notification
        {
            var conn = this.connection.GetConnection();

            Cursor<T> all = R.Db(this.dbName).Table(this.tableName)
                .GetAll(notification.Id)//[new { index = nameof(Post.title) }]
                .Run<T>(conn);

            var notifications = all.ToList();
            if (notifications.Count > 0)
            {
                //se la notifica è già presente sul db:
                throw new NewGuidException();
            }
            else
            {
                // insert
                R.Db(this.dbName).Table(this.tableName)
                    .Insert(notification)
                    .RunWrite(conn);
            }
        }
    }
}
