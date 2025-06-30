namespace BettySlotGame.Services
{
    public class ConsoleService
    {
        public string? ReadLine() => Console.ReadLine();
        public void WriteLine(string message) => Console.WriteLine(message);
        public void WriteLine() => Console.WriteLine();
    }
}
