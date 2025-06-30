namespace BettySlotGame.Services.Abtractions
{
    public interface IConsoleService
    {
        string? ReadLine();
        void WriteLine(string message);
        void WriteLine();
    }
}
