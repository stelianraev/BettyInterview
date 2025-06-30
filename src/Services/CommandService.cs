using BettySlotGame.Commands;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Services
{
    public class CommandService : ICommandService
    {

        private readonly IEnumerable<ICommand> _commands;
        private readonly ILogger<CommandService> _logger;
        private readonly IConsoleService _consoleService;

        public CommandService(IEnumerable<ICommand> commands, ILogger<CommandService> logger, IConsoleService consoleService)
        {
            _commands = commands;
            _logger = logger;
            _consoleService = consoleService;
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
                _consoleService.WriteLine($"Unknown command: {parts[0]}");
                _logger.LogInformation($"Invalid input {parts[0]}");

                return;
            }

            command.Execute(parts, token);
        }
    }
}
