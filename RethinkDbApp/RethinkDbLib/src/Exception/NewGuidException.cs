using System;

namespace RethinkDbLib.src.Exception
{
    /// <summary>
    /// Se il Guid creato è già presente
    /// </summary>
    [Serializable]
    public class NewGuidException : System.Exception
    {
        private readonly static string message = "The Guid created is already on the db";
        public NewGuidException() : base(message)
        {

        }
    }
}
