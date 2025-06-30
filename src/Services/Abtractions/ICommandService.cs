namespace BettySlotGame.Services.Abtractions
{
    public interface ICommandService
    {
        void HandleInput(string input, CancellationToken token);
    }
}
