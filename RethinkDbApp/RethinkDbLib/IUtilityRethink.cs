using Rethink.Model;
using RethinkDbApp.Model;

namespace Rethink
{
    /// <summary>
    /// Libreria per la gestione del Db Rethink, della tabella "Notifiche" e per rimanere in ascolto sui cambiamenti della tabella "Notifications"
    /// </summary>
    public interface IUtilityRethink
    {       
        /// <summary>
        /// Metodo per gestire il db precedentemente specificato nel costruttore di Utility Rethink
        /// </summary>
        /// <returns>Oggetto che gestisce il database, permette di creare tabelle nuove, indici, riconfigurare il numero di shards e repliche</returns>
        public IDbManager GetDbManager();

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
