using System;
using RethinkDbLib.src.Connection;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    class NotificationsManager : INotificationsManager
    {
        private readonly IConnectionNodes connection;
        private readonly IQueryNotifications queryToNotifications;

        public NotificationsManager(IConnectionNodes connection)
        {
            this.connection = connection;
            this.queryToNotifications = new QueryNotifications(connection);
        }

        public INotifier<T> Notifier<T>() where T : Notification
        {
            INotifier<T> notifier = new Notifier<T>(this.connection);
            return notifier;
        }

        public IQueryNotifications QueryService { get => this.queryToNotifications; }

        /*
        public IQueryNotifications GetQueryService()
        {
            return this.queryToNotifications;
        }
        */

        public string WellKnownTable
        {
            get => INotificationsManager.TABLE;
        }
        
    }
}
