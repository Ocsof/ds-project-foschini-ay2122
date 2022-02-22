using System;

namespace RethinkDbLib.src.Exception
{
    /// <summary>
    /// Se non ci si riesce a connettere con il server Rethink in un tempo ragionevole (oltre il Timeout)
    /// </summary>
    [Serializable]
    public class ConnectionFailureException : System.Exception
    {
        private readonly static string message = "Connection failed, server not found";
        public ConnectionFailureException() : base(message)
        {

        }
    }
}
