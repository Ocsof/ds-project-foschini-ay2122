using Newtonsoft.Json;
using System;

namespace Rethink.Model
{
    public abstract class Notification
    {
        /// <summary>
        /// Notifica base
        /// </summary>
        protected Notification()
        {
            this.Type = this.GetType().Name;  //Uso della Reflection --> Assegno a type una stringa che rappresenta il nome della classe
        }
        /// <summary>
        /// Id della notifica: es 0f8fad5b-d9cb-469f-a165-70867728950e
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]  //penso sia qualcosa relativo al fatto che id no null
        public Guid Id { get; set; }

        /// <summary>
        /// Data generazione notifica
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Argomento della notifica
        /// </summary>
        public string Arg { get; set; }

        /// <summary>
        /// Descrizione della notifica
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Una stringa che rappresenta il tipo della notifica
        /// </summary>
        public string Type { get; }

        public override String ToString()
        {
            string result = "Id: " + this.Id.ToString() + " Date: " + this.Date.ToString() 
                    + " Text: " + this.Text + " Type: " + this.Type + " Arg: " + this.Arg;
            return result;
        }
    }
}
