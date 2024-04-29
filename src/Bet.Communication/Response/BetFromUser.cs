using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet.Communication.Response;
public class BetFromUser
{
    public double Odd { get; set; }
    public long BetId { get; set; }
    public double BetAmount { get; set; }
    public long ChosenTeam { get; set; }
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
