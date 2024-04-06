namespace Bet.Domain.Entities;
public class Bet : BaseEntity
{
    public DateTime ExpiryTime { get; set; } 
    public bool Paid { get; set; }
    public ICollection<UserBet>? UserBets { get; set; }
    public Team? Winner { get; set; } = null;
}


