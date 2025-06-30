using BettySlotGame.Models;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Commands
{
    public class ExitCommand : ICommand
    {
        private readonly CancellationTokenSource _cts;
        private readonly ILogger<ExitCommand> _logger;
        public ExitCommand(CancellationTokenSource cts, ILogger<ExitCommand> logger)
        {
            _cts = cts;
            _logger = logger;
        }

        public string Name => CommandEnum.Exit.ToString();

        public void Execute(string[] args, CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation("Thank you for playing! Hoe to see you again soon.");
            return;
        }
    }
}
