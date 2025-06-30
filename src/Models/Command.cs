namespace BettySlotGame.Models
{
    public class Command
    {
        public string? CommandName { get; set; }
        public decimal Value { get; set; }
        public decimal MinBet { get; set; }
        public decimal MaxBet { get; set; }
    }
}
