using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Commands
{
    public class WithdrawalCommand : ISlotCommand
    {
        private readonly IWalletService _walletService;
        private readonly IConsoleService _consoleService;
        private readonly ILogger<WithdrawalCommand> _logger;

        public WithdrawalCommand(IWalletService walletService, IConsoleService consoleService, ILogger<WithdrawalCommand> logger)
        {
            _walletService = walletService;
            _consoleService = consoleService;
            _logger = logger;
        }

        public string Name => CommandEnum.Withdraw.ToString();

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (parameter is InputCommand command)
            {
                if (command.Value == null)
                {
                    return false;
                }
            }

            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is InputCommand command)
            {
                var canExecute = CanExecute(command);

                if (!canExecute)
                {
                    throw new ArgumentException("Invalid withdraw");
                }
                else
                {
                    var withdrawAmount = (decimal)command.Value!;

                    _walletService.Withdraw(withdrawAmount);

                    _consoleService.WriteLine($"Your withdrawal of ${withdrawAmount.ToString("0.##")} was successful. Your current balance is: ${_walletService.Balance.ToString("0.##")}");

                    _logger.LogInformation($"Successfully withdrew ${withdrawAmount}. Current balance: ${_walletService.Balance.ToString("0.##")}");
                }
            }
        }
    }
}
