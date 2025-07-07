
using BettySlotGame.Models;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Commands
{
    public class ExitCommand : ISlotCommand
    {
        private readonly CancellationTokenSource _cts;
        private readonly ILogger<ExitCommand> _logger;

        public ExitCommand(CancellationTokenSource cts, ILogger<ExitCommand> logger)
        {
            _cts = cts;
            _logger = logger;
        }

        public string Name => CommandEnum.Exit.ToString();

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _logger.LogInformation("Exit command executed. Application will terminate.");
            _cts.Cancel();
        }
    }
}
