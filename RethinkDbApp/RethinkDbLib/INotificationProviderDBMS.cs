using System;
using RethinkDbLib.src.DBManager;

namespace RethinkDbLib
{
    public interface INotificationProviderDBMS : INotificationProvider
    {
        /// <summary>
        /// Metodo per gestire il db precedentemente specificato nel costruttore di Utility Rethink
        /// </summary>
        /// <returns>Oggetto che gestisce il database, permette di creare tabelle nuove, indici, riconfigurare il numero di shards e repliche</returns>
        IDbManager DBManager { get; }

        //public IDbManager GetDbManager();
    }
}
