using System;

namespace Joker.Domain.Exceptions
{
    /// <summary>
    /// Exception that is raised when a user wants to join 
    /// a table with no free spaces
    /// </summary>
    public class TableIsFullException : Exception
    {
        public TableIsFullException()
        {
        }

        public TableIsFullException(string message)
            :base(message)
        {          
        }
    }
}