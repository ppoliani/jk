using System.Collections.Generic;

namespace Joker.Domain.Entities.BoardGame
{

    #region inner members

    /// <summary>
    /// Enumeration of the game rounds
    /// </summary>
    public enum GameRound
    {
        // first block 1-8
        BlockOneRoundOne,
        BlockOneRoundTwo,
        BlockOneRoundThree,
        BlockOneRoundFour,
        BlockOneRoundFive,
        BlockOneRoundSix,
        BlockOneRoundSeven,
        BlockOneRoundEight,

        // second block four 9s
        BlockTwoRoundOne,
        BlockTwoRoundTwo,
        BlockTwoRoundThree,
        BlockTwoRoundFour,

        // third block 8-1
        BlockThreeRoundOne,
        BlockThreeRoundTwo,
        BlockThreeRoundThree,
        BlockThreeRoundFour,
        BlockThreeRoundFive,
        BlockThreeRoundSix,
        BlockThreeRoundSeven,
        BlockThreeRoundEight,

        // fourth block four 9s
        BlockFourRoundOne,
        BlockFourRoundTwo,
        BlockFourRoundThree,
        BlockFourRoundFour
    }

    #endregion

    public static class Config
    {
        public static readonly IDictionary<GameRound, ushort> NumOfCardsInEachRound = new Dictionary<GameRound, ushort>()
            {
                // first block
                { GameRound.BlockOneRoundOne, 1 },
                { GameRound.BlockOneRoundTwo, 2},
                { GameRound.BlockOneRoundThree, 3 },
                { GameRound.BlockOneRoundFour, 4 },
                { GameRound.BlockOneRoundFive, 5 },
                { GameRound.BlockOneRoundSix, 6 },
                { GameRound.BlockOneRoundSeven, 7 },
                { GameRound.BlockOneRoundEight, 8},

                // second block
                { GameRound.BlockTwoRoundOne, 9 },
                { GameRound.BlockTwoRoundTwo, 9 },
                { GameRound.BlockTwoRoundThree, 9 },
                { GameRound.BlockTwoRoundFour, 9 },

                // third block
                { GameRound.BlockThreeRoundOne, 8 },
                { GameRound.BlockThreeRoundTwo, 7 },
                { GameRound.BlockThreeRoundThree, 6 },
                { GameRound.BlockThreeRoundFour, 5 },
                { GameRound.BlockThreeRoundFive, 4 },
                { GameRound.BlockThreeRoundSix, 3 },
                { GameRound.BlockThreeRoundSeven, 2 },
                { GameRound.BlockThreeRoundEight, 1 },

                // fourth block
                { GameRound.BlockFourRoundOne, 9 },
                { GameRound.BlockFourRoundTwo, 9 },
                { GameRound.BlockFourRoundThree, 9 },
                { GameRound.BlockFourRoundFour, 9 },
            };
    }
}