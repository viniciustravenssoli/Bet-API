namespace Bet.Domain.Entities;
public class Bet : BaseEntity
{
    public long VisitorId { get; set; }
    public Team Visitor { get; set; }
    public long HomeId { get; set; }
    public Team Home { get; set; }
    public DateTime ExpiryTime { get; set; }
    public bool Paid { get; set; }
    public ICollection<UserBet>? UserBets { get; set; }
    public long? WinnerId { get; set; } 
    public Team Winner { get; set; }
}
