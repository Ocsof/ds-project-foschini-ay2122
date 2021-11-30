using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using RethinkDb.Driver.Net.Clustering;
using RethinkDbLib.src.Exception;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RethinkDbLib.src.Connection
{
    class ConnectionNodes : IConnectionNodes
    {
        //private RethinkDB R = RethinkDB.R;
        private ConnectionPool conn;  //IConnection
        private readonly IList<DbOptions> listNodi;
        private readonly int timeout;

        //il timeout come paramtro opzionalmente configurabile
        public ConnectionNodes(IList<DbOptions> listNodi, int timeout = 20) 
        {
            this.listNodi = listNodi;
            this.timeout = timeout;
        }

        public virtual IConnection GetConnection()
        {
            if (conn == null)
            {
                var R = RethinkDB.R;
                string[] nodi = new string[this.listNodi.Count];
                int position = 0;
                foreach(DbOptions node in listNodi)
                {
                    nodi[position] = node.HostPort;
                    position++;
                }
                try
                {
                    this.conn = R.ConnectionPool()
                        .Seed(nodi)
                        .PoolingStrategy(new RoundRobinHostPool())
                        .Discover(true)
                        .InitialTimeout(this.timeout)
                        .Connect();
                }
                catch (ReqlDriverError)  //viene catturata se dopo 20 secondi non è riuscito a connettersi
                {
                    throw new ConnectionFailureException();
                }
                R.Now().Run<DateTimeOffset>(conn);  // forse è da togliere
            }

            if (!conn.AnyOpen)
            {
                //conn.Reconnect();
                conn = null;
                this.GetConnection();
            }
            
            return conn;
        }

        public void CloseConnection()
        {
            if (conn != null && conn.AnyOpen)
            {

                // conn.Close(false);
                conn.Shutdown();
            }
        }

        public IList<DbOptions> GetNodi()
        {
            return this.listNodi;
        }

        public int GetTimeout()
        {
            return this.timeout;
        }
    }
}
