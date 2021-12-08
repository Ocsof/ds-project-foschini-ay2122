using System;
using RethinkDbLib.src.TablesManager.Notifications;

namespace RethinkDbLib
{
    public interface INotificationProvider
    {
        /// <summary>
        /// Gestore per la tabella "Notification", ha in se la funzionalità di query e di notificators apposta per la tabella Notification
        /// </summary>
        /// <returns>Oggetto che ha in se le funzionalità di query e listening sulla tabella "Notification"</returns>
        public INotificationsManager GetNotificationsManager();

        /// <summary>
        /// Da chiamare ogni volta che si termina una sessione con Rethink per chiudere in sicurezza la connessione
        /// </summary>
        public void CloseConnection();
    }
}
