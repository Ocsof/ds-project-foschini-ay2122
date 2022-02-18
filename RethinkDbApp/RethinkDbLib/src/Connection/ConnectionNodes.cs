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
        private readonly IList<DbOptions> listNodes;
        private readonly int timeout;

        //il timeout come paramtro opzionalmente configurabile
        public ConnectionNodes(IList<DbOptions> listNodes, int timeout = 20) 
        {
            this.listNodes = listNodes;
            this.timeout = timeout;
        }

        public virtual IConnection Connection()
        {
            if (conn == null)
            {
                var R = RethinkDB.R;
                string[] nodi = new string[this.listNodes.Count];
                int position = 0;
                foreach(DbOptions node in listNodes)
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
                this.Connection();
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

        public IList<DbOptions> Nodes
        {
            get => this.listNodes;
        }

        public int Timeout
        {
            get => this.timeout;
            
        }
    }
}
