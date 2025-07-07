namespace BettySlotGame.Models
{
    public class BettySlotSettings
    {
        public decimal MinBet { get; set; }
        public decimal MaxBet { get; set; }
        public decimal BigWinMinMultiplier { get; set; }
        public decimal BigWinMaxMultiplier { get; set; }
        public decimal LoseBetPercentage { get; set; }
        public decimal NormalWinBetPercentage { get; set; }
        public decimal NormalWinBetMultiplier { get; set; }
        public decimal BigWinsBetPercentage { get; set; }
    }
}