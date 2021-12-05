using System;
namespace RethinkDbLib.src.TablesManager.Notifications
{
    class Notifier<T> : INotifier<T> where T: Notification
    {
        public Notifier()
        {
        }

        public NotificationSubscription<T> ListenWithOneOfTheArguments(params string[] argsList)
        {
            throw new NotImplementedException();
        }

        public void StopListening(Guid id)
        {
            throw new NotImplementedException();
        }

        public void StopListening(NotificationSubscription<T> notificationSubscription)
        {
            throw new NotImplementedException();
        }
    }
}
