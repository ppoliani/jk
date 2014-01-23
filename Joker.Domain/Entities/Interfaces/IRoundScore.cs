namespace Joker.Domain.Entities.Interfaces
{
    public interface IRoundScore
    {
        ushort Round { get; set; }
        short Score { get; set; } 
    }
}