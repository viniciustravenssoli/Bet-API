using Bet.Domain.Entities;

namespace Bet.Communication.Request;
public class RequestRegisterBet
{
    public DateTime ExpiryTime { get; set; }
}
