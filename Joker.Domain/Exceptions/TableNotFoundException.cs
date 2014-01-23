using System;

namespace Joker.Domain.Exceptions
{
    /// <summary>
    /// Exception that states that a playing table is not found in any of the current collections
    /// </summary>
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException()
        {
        }

        public TableNotFoundException(string message)
            :base(message)
        {          
        }
    }
}