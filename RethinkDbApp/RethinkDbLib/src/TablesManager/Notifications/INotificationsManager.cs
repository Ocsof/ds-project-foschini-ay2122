using Rethink.Model;
using Rethink.ReactiveExtension;

namespace RethinkDbApp.Model
{
    /// <summary>
    /// Manager delle Notifiche ---> Query Notifiche e Listener sulle Notifiche
    /// </summary>
    public interface INotificationsManager : IManager
    {
        public const string TABLE = "Notifications";  //Nome della tabella su Rethink
        /// <summary>
        /// Metodo per gestire le notifiche sul Db
        /// </summary>
        /// <returns>Oggetto di gestione delle notifiche presenti sul db, permette di effettuare query</returns>
        public IQueryNotifications GetQueryService();

        /// <summary>
        /// Metodo per ottenere un notificatore di notifiche di un certo tipo specificato 
        /// </summary>
        /// <returns>Listener di eventi su tabella "Notification"</returns>
        public IRXNotifier<T> GetNotifier<T>() where T : Notification;
    }
}
