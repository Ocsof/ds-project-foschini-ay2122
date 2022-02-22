using System;

namespace RethinkDbLib.src.Exception
{
    /// <summary>
    /// Se si tenta di eliminare una tabella di sistema
    /// </summary>
    [Serializable]
    public class DeleteTableSystemException : System.Exception
    {
        private readonly static string message = "Could not delete system table ";
        public DeleteTableSystemException(string table) : base(message + table)
        {

        }
    }
}
