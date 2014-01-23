using System;

namespace Joker.Domain.Exceptions
{
    /// <summary>
    /// Exception that indicates that the user has
    /// entered the wrong password when joing a table
    /// </summary>
    public class WrongTablePasswordEntered : Exception
    {
        public WrongTablePasswordEntered()
        {
        }

        public WrongTablePasswordEntered(string msg) 
            :base(msg)
        {    
        }
    }
}