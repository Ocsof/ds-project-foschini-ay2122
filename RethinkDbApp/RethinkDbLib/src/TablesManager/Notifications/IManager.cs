

namespace RethinkDbLib.src.TablesManager.Notifications
{
    /// <summary>
    /// Manager di una tabella di sistema
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Restituisce la sua tabella di sistema
        /// </summary>
        /// <returns>Tabella di sistema</returns>
        string WellKnownTable { get; }

        //string GetWellKnownTable();

    }
}
