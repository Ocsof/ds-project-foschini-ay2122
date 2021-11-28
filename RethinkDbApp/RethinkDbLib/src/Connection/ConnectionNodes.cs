using Rethink.Model;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using RethinkDb.Driver.Net.Clustering;
using RethinkDbApp.Exception;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Rethink.Connection
{
    class ConnectionNodes : IConnectionNodes
    {
        //private RethinkDB R = RethinkDB.R;
        private ConnectionPool conn;  //IConnection
        private readonly IList<DbOptions> listNodi;


        public ConnectionNodes(IList<DbOptions> listNodi)
        {
            this.listNodi = listNodi;
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
                        .InitialTimeout(listNodi.First().Timeout)
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


    }
}
