namespace BettySlotGame.Services.Abtractions
{
    public interface ISlotEngine
    {
        (bool, decimal) Bet(decimal bet);
    }
}
