using System;

namespace Joker.Domain.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a player tries to join a table
    /// whish he has already joined
    /// </summary>
    public class PlayerHasAlreadyJoinedTableException : Exception
    {
        public PlayerHasAlreadyJoinedTableException()
        {
        }

        public PlayerHasAlreadyJoinedTableException( string message )
            :base(message)
        {          
        }
    }
}