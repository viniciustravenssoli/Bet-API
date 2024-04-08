using Microsoft.VisualBasic;

namespace Bet.Domain.Entities;
public class User : BaseEntity
{
    public string Name { get; set; }
    public double Balance { get; set; } = 0;
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public ICollection<UserBet>? UserBets { get; set; }
    public string Role { get; set; }
}
