using Bet.Domain.Entities;

namespace Bet.Communication.Request;
public class RequestRegisterBet
{
    public DateTime ExpiryTime { get; set; }
    public long VisitorId { get; set; }
    public long HomeId { get; set; }
}
