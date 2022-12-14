using RethinkDbLib.src.Connection;
using RethinkDb.Driver;
using RethinkDbLib.src.Exception;
using System.Linq;

namespace RethinkDbLib.src.DBManager
{
    class DbManager : IDbManager
    {
        private readonly IConnectionNodes connection;
        private readonly static RethinkDB R = RethinkDB.R;
        private readonly string dbName;
        private string[] wellKnowns;

        public DbManager(IConnectionNodes connection, string[] wellKnown)  //IConnectionPooling connectionFactory  // ---> per connessione con un cluster + nodi
        {
            this.connection = connection;
            this.dbName = connection.Nodes.ElementAt(0).Database;
            this.wellKnowns = wellKnown;
        }

        public string TablesList
        {
            get
            {
                var conn = this.connection.Connection();
                var tableList = R.Db(dbName).TableList().Run(conn);

                return tableList.ToString();
            }
            
        }

        public void CreateTable(string tableName)
        {
            var conn = this.connection.Connection();

            var exists = R.Db(dbName).TableList().Contains(t => t == tableName).Run(conn);
            if (!exists)
            {
                R.Db(this.dbName).TableCreate(tableName).Run(conn);
                R.Db(this.dbName).Table(tableName).Wait_().Run(conn);
            }
        }

        public void DeleteTable(string tableName)
        {
            if (wellKnowns.Contains(tableName))
            {
                throw new DeleteTableSystemException(tableName);
            }
            var conn = this.connection.Connection();
            var exists = R.Db(this.dbName).TableList().Contains(t => t == tableName).Run(conn);
            if (exists)
            {
                R.Db(this.dbName).TableDrop(tableName).Run(conn);
            }
        }

        public string IndexList(string tableName) 
        {
            var conn = this.connection.Connection();
            try
            {
                var indexList = R.Db(this.dbName).Table(tableName).IndexList().Run(conn);
                return indexList.ToString();
            }
            catch (ReqlOpFailedError)
            {
                throw new TableNotFoundException(tableName);
            }
        }

        public void CreateIndex(string tableName, string indexName)
        {
            var conn = this.connection.Connection();
            try
            {
                var exists = R.Db(this.dbName).Table(tableName).IndexList().Contains(t => t == indexName).Run(conn);
                if (!exists)
                {
                    R.Db(this.dbName).Table(tableName).IndexCreate(indexName).Run(conn);
                    R.Db(this.dbName).Table(tableName).IndexWait(indexName).Run(conn);
                }
            }
            catch (ReqlOpFailedError)
            {
                throw new TableNotFoundException(tableName);
            }
        }

        public void DeleteIndex(string tableName, string indexName)
        {
            var conn = this.connection.Connection();
            try
            {
                var exists = R.Db(this.dbName).Table(tableName).IndexList().Contains(t => t == indexName).Run(conn);
                if (exists)
                {
                    R.Db(this.dbName).Table(tableName).IndexDrop(indexName).Run(conn);
                }
            }
            catch (ReqlOpFailedError)
            {
                throw new TableNotFoundException(tableName);
            }
        }

        public void ReconfigureTable(string tableName, int shards, int replicas)
        {
            var conn = this.connection.Connection();
            try
            {
                R.Db(this.dbName).Table(tableName).Reconfigure().OptArg("shards", shards).OptArg("replicas", replicas).Run(conn);
                R.Db(this.dbName).Table(tableName).Wait_().Run(conn);
            }
            catch (ReqlOpFailedError)
            {
                throw new TableNotFoundException(tableName);
            }
        }
 
    }
}
