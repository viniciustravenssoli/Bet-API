namespace Bet.Domain.Entities;
public class Bet : BaseEntity
{
    public double BetAmount { get; set; }
    public double Odd { get; set; }
    public double PossibleReturn => BetAmount * Odd;
    public long UserId { get; set; }
    public DateTime ExpiryTime { get; set; } 
    public bool Won { get; set; } 
}
