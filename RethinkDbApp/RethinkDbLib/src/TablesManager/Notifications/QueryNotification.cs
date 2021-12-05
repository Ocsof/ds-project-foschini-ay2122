using System;
using System.Collections.Generic;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    public class QueryNotification : IQueryNotifications
    {
        public QueryNotification()
        {

        }

        public void DeleteNotification(Guid id)
        {
            throw new NotImplementedException();
        }

        public T GetNotificationOrNull<T>(Guid id) where T : Notification
        {
            throw new NotImplementedException();
        }

        public IList<T> GetNotifications<T>(DateTime date) where T : Notification
        {
            throw new NotImplementedException();
        }

        public IList<T> GetNotificationsWithArg<T>(string arg) where T : Notification
        {
            throw new NotImplementedException();
        }

        public IList<T> GetNotificationsWithText<T>(string text) where T : Notification
        {
            throw new NotImplementedException();
        }

        public void NewNotification<T>(T notification) where T : Notification
        {
            throw new NotImplementedException();
        }
    }
}
