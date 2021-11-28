

namespace Rethink.src.Model
{
    class DbOptions
    {
        /// <summary>
        /// Stringa del tipo: "indirizzoip:porta"
        /// </summary>
        public string HostPort { get; set; }

        /// <summary>
        /// Nome database
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Timeout per la connessione
        /// </summary>
        public int Timeout { get; set; }
    }
}
