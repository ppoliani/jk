using System.Collections.Generic;

namespace Joker.Domain.Entities.BoardGame
{
    /// <summary>
    /// Instances of this class store information about 
    /// each round of the game
    /// </summary>
    public class GameRoundInfo
    {
        #region private fields

        #endregion

        #region Constructors

        public GameRoundInfo(short numOfCards)
        {
            this.NumOfCards = numOfCards;
        }

        #endregion

        #region properties

        public short NumOfCards { get; set; }

        public IList<GameRoundStep> GameRoundSteps { get; set; }

        #endregion
    }
}