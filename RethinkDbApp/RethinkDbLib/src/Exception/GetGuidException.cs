using System;

namespace RethinkDbLib.src.Exception
{
    /// <summary>
    /// Se il Guid non è presente sul db Rethink
    /// </summary>
    [Serializable]
    public class GetGuidException : System.Exception
    {
        private readonly static string message = "Il Guid non è presente sul db";
        public GetGuidException() : base(message)
        {

        }
    }
}
