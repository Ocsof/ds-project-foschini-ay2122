using System;
using RethinkDb.Driver.Model;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    /// <summary>
    /// Classe che accoppia un "observable di notifiche" con un Guid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotificationSubscription<T> where T : Notification
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// Observable di notifiche
        /// </summary>
        public IObservable<Change<T>> Observable { get; }

        public NotificationSubscription(Guid guid, IObservable<Change<T>> observable)
        {
            this.Guid = guid;
            this.Observable = observable;
        }
    }
}
