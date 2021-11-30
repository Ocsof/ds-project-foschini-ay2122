using Rethink.Connection;
using Rethink.Model;
using RethinkDb.Driver;
using System;
using System.Collections.Generic;
using RethinkDbApp.Model;

namespace Rethink
{
    /// <summary>
    /// Libreria per la gestione del Db, della tabella "Notifiche" e per rimanere in ascolto sui cambiamenti della tabella "Notifications"
    /// </summary>
    public class UtilityRethink : IUtilityRethink
    {
        private readonly static RethinkDB R = RethinkDB.R;
        private readonly IConnectionNodes connection;
        private readonly INotificationsManager notificationsManager;
        private readonly IDbManager dbManager;
        private readonly List<string> WellKnownTables = new List<string>();
        private readonly IList<DbOptions> listNodi;

        /// <summary>
        /// Connettere l'app al cluster Rethinkdb in esecuzione
        /// </summary>
        /// <param name="dbName">Nome del Db a cui ci si vuole connettere, se non esiste viene creato</param>
        /// <param name="hostsPorts">Lista di stringhe del tipo: "indirizzoip:porta"</param>
        public UtilityRethink(string dbName, IList<String> hostsPorts)
        {
            this.listNodi = new List<DbOptions>();
            foreach (String hostPort in hostsPorts)
            {
                listNodi.Add(new DbOptions { Database = dbName, HostPort = hostPort, Timeout = 20 });
            }
            this.connection = new ConnectionNodes(listNodi);
            
            //vari manager supportati
            this.notificationsManager = new NotificationsManager(this.connection);
            this.WellKnownTables.Add(notificationsManager.GetWellKnownTable());
            //..altri ?

            this.dbManager = new DbManager(this.connection,this.WellKnownTables.ToArray());
            this.CreateDb(dbName);

            //per ogni manager con tabelle "note"
            this.dbManager.CreateTable(INotificationsManager.TABLE);
        }

        /// <summary>
        /// Metodo che viene chiamato alla creazione dell'istanza "UtilityRethink"
        /// Il db su cui ci si vuole connetter viene creato se non esiste 
        /// </summary>
        /// <param name="dbName"></param>
        private void CreateDb(string dbName)
        {
            var conn = this.connection.GetConnection();
            var exists = R.DbList().Contains(db => db == dbName).Run(conn);
            if (!exists)
            {
                R.DbCreate(dbName).Run(conn);
                R.Db(dbName).Wait_().Run(conn);
            }
        }

        public IDbManager GetDbManager()
        {
            return this.dbManager;
        }

        public INotificationsManager GetNotificationsManager()
        {
            return this.notificationsManager;
        }

        public void CloseConnection()
        {
            this.connection.CloseConnection();
        }

    }
}
