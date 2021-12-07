using System;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    /// <summary>
    /// Le notifiche di tipo NewData serviranno ad aggiornare la cache.
    /// Quando verrà fatta una insert/update sul db "vero" queste notifiche permetterano di aggiungere/aggiornare il nuovo dato in cache.
    /// </summary>
    public class NotificationNewData: Notification
    {
        /// <summary>
        /// Tabella del db "vero" in cui verrà fatta la insert/update
        /// </summary>
        public string Table { get; set; }

        public override String ToString()
        {
            string result = base.ToString() + " Table: " + this.Table;
            return result;
        }
    }
}
