namespace Bet.Domain.Entities;
public class UserBet : BaseEntity
{
    public UserBet(Team chosenTeam)
    {
        ChosenTeam = chosenTeam;
    }

    public UserBet(Team chosenTeam, Bet bet)
    {
        ChosenTeam = chosenTeam;
        Bet = bet;
    }

    public UserBet()
    {
        
    }

    public double Odd { get; set; }
    public long BetId { get; set; }
    public Bet Bet { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public double BetAmount { get; set; }
    public long ChosenTeamId { get; set; }
    public Team ChosenTeam { get; set; }


    public void CalculateOddOnRequest(double totalAmount, double amountOnTeamA, double amountOnTeamB)
    {
        // Verifica se o totalAmount é zero para definir uma odd padrão
        if (totalAmount == 0 || amountOnTeamA == 0 || amountOnTeamB == 0)
        {
            // Definir odd padrão
            Odd = 1.1; // Ou qualquer outro valor que você desejar
            return;
        }

        double proportionA = amountOnTeamA / totalAmount;
        double proportionB = amountOnTeamB / totalAmount;

        // Calcula a odd com base na proporção da equipe escolhida
        Odd = ChosenTeam.Id == Bet.HomeId ? 1 / proportionA : 1 / proportionB;
        Odd = Math.Round(Odd, 1);
    }

    public void CalculateOddOnCreate(double totalAmount, double amountOnTeamA, double amountOnTeamB)
    {
        // Verifica se o totalAmount é zero para definir uma odd padrão
        if (totalAmount == 0 || amountOnTeamA == 0 || amountOnTeamB == 0)
        {
            // Definir odd padrão
            Odd = 1.1; // Ou qualquer outro valor que você desejar
            return;
        }

        double proportionA = amountOnTeamA / totalAmount;
        double proportionB = amountOnTeamB / totalAmount;

        // Calcula a odd com base na proporção da equipe escolhida
        Odd = ChosenTeamId == Bet.HomeId ? 1 / proportionA : 1 / proportionB;
        Odd = Math.Round(Odd, 1);
    }
}

