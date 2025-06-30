using BettySlotGame.Commands;
using BettySlotGame.Services.Abtractions;

namespace BettySlotGame.Services
{
    public class CommandService : ICommandService
    {

        private readonly IEnumerable<ICommand> _commands;

        public CommandService(IEnumerable<ICommand> commands)
        {
            _commands = commands;
        }

        public void HandleInput(string input, CancellationToken token)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return;
            }
                
            var command = _commands.FirstOrDefault(c => c.Name.Equals(parts[0], StringComparison.OrdinalIgnoreCase));

            if (command == null)
            {
                Console.WriteLine($"Unknown command: {parts[0]}");
                {
                    return;
                }
            }

            command.Execute(parts, token);
        }
    }
}
