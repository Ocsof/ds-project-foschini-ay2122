using System;

namespace RethinkDbLib.src.Exception
{
    /// <summary>
    /// Se non ci si riesce a connettere con il server Rethink in un tempo ragionevole (oltre il Timeout)
    /// </summary>
    [Serializable]
    public class ConnectionFailureException : System.Exception
    {
        private readonly static string message = "Connessione fallita, server non trovato";
        public ConnectionFailureException() : base(message)
        {

        }
    }
}
