using System;

namespace RethinkDbLib.src.Exception
{
    /// <summary>
    /// Se il Guid creato è già presente
    /// </summary>
    [Serializable]
    public class NewGuidException : System.Exception
    {
        private readonly static string message = "Il Guid creato è già presente";
        public NewGuidException() : base(message)
        {

        }
    }
}
