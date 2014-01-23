using System;

namespace Joker.Domain.Exceptions
{
    /// <summary>
    /// Exception that is raised when a user is
    /// not found in the game
    /// </summary>
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException()
        {
        }

        public PlayerNotFoundException(string message)
            :base(message)
        {       
        }
    }
}