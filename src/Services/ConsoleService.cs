using BettySlotGame.Services.Abtractions;

namespace BettySlotGame.Services
{
    public class ConsoleService : IConsoleService
    {
        public string? ReadLine() => Console.ReadLine();
        public void WriteLine(string message) => Console.WriteLine(message);
        public void WriteLine() => Console.WriteLine();
    }
}
