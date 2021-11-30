using System;

namespace RethinkDbApp.Exception
{
    /// <summary>
    /// Se si tenta di eliminare una tabella di sistema
    /// </summary>
    [Serializable]
    class DeleteTableSystemException : System.Exception
    {
        private readonly static string message = "Impossibile eliminare la tabella di sistema ";
        public DeleteTableSystemException(string table) : base(message + table)
        {

        }
    }
}
