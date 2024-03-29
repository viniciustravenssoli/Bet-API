namespace Bet.Domain.Entities;
public class UserBet : BaseEntity
{
    public double Odd { get; set; }
    public long BetId { get; set; }
    public Bet Bet { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public double BetAmount { get; set; }
    public Team ChosenTeam { get; set; }

    public void CalculateOdd(double totalAmount, double amountOnTeamA, double amountOnTeamB)
    {
        // Verifica se o totalAmount é zero para definir uma odd padrão
        if (totalAmount == 0 || amountOnTeamA == 0 || amountOnTeamB == 0)
        {
            // Definir odd padrão
            Odd = 1.1; // Ou qualquer outro valor que você desejar
            return;
        }

        // Calcula as proporções
        double proportionA = amountOnTeamA / totalAmount;
        double proportionB = amountOnTeamB / totalAmount;

        // Calcula a odd com base na proporção da equipe escolhida
        Odd = ChosenTeam == Team.TeamA ? 1 / proportionA : 1 / proportionB;
        Odd = Math.Round(Odd, 1);
    }
}



public enum Team
{
    TeamA,
    TeamB
}