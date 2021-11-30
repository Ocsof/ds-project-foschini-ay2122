using System;
using RethinkDbLib.src.Connection;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    class NotificationsManager : INotificationsManager
    {
        private readonly IConnectionNodes connection;

        public NotificationsManager(IConnectionNodes connection)
        {
            this.connection = connection;
        }

        public INotifier<T> GetNotifier<T>() where T : Notification
        {
            throw new NotImplementedException();
        }

        public IQueryNotifications GetQueryService()
        {
            throw new NotImplementedException();
        }

        public string GetWellKnownTable()
        {
            return INotificationsManager.TABLE;
        }
    }
}
