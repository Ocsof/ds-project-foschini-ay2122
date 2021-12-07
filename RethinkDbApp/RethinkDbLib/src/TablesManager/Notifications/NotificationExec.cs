using System;

namespace RethinkDbLib.src.TablesManager.Notifications
{
    /// <summary>
    /// Le Notifiche di Esecuzione servono per capire e intercettare la conclusione di un processo/task.
    /// </summary>
    public class NotificationExec : Notification
    {
        public NotificationExec() : base()
        {

        }
        /// <summary>
        /// Id di esecuzione
        /// </summary>
        public Guid IdExec { get; set; }

        public override String ToString()
        {
            string result = base.ToString() + " IdExec: " + this.IdExec.ToString();
            return result;
        }
    }
}
