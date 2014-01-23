using System;
using Joker.Domain.Entities.Interfaces;

namespace Joker.Domain.Entities
{
    /// <summary>
    /// Instances of this class represent user scores
    /// </summary>
    public class RoundsScore : IRoundScore
    {
        public ushort Round { get; set; }
        public short Score { get; set; }
    }
}