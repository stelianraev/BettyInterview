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
            try
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

                _logger.LogInformation($"Command {command.Name} is ready for execute");
                command.Execute(parts, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the command.");
                _consoleService.WriteLine("An error occurred while processing the command. Please try again.");
            }
        }
    }
}
