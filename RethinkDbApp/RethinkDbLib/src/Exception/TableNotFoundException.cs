using System;

namespace RethinkDbLib.src.Exception
{
    /// <summary>
    /// Se la Tabella non esiste o non è stata trovata sul db Rethink
    /// </summary>
    [Serializable]
    public class TableNotFoundException : System.Exception
    {
        private readonly static string message = "Table ";
        private readonly static string message2 = " not found on db";
        public TableNotFoundException(string table) : base(message + table + message2)
        {

        }
    }
}
