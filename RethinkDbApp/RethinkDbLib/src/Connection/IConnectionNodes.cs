
using RethinkDb.Driver.Net;
using System.Collections.Generic;

namespace RethinkDbLib.src.Connection
{
    interface IConnectionNodes
    {
        /// <summary>
        /// Ritorna la connessione verso il server
        /// </summary>
        /// <returns>La connessione</returns>
        public IConnection GetConnection();

        /// <summary>
        /// Chiusura connessione
        /// </summary>
        public void CloseConnection();

        /// <summary>
        /// Ritorna i Nodi Del Cluster
        /// </summary>
        /// <returns>I nodi Rethink presenti sul server</returns>
        public IList<DbOptions> GetNodi();

        /// <summary>
        /// Timeout per la connessione
        /// </summary>
        public int GetTimeout();

    }
}
