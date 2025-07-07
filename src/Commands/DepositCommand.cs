using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Commands
{
    public class DepositCommand : ISlotCommand
    {
        private readonly IWalletService _walletService;
        private readonly IConsoleService _consoleService;
        private readonly ILogger<DepositCommand> _logger;

        public DepositCommand(IWalletService walletService, IConsoleService consoleService, ILogger<DepositCommand> logger)
        {
            _walletService = walletService;
            _consoleService = consoleService;
            _logger = logger;
        }

        public string Name => CommandEnum.Deposit.ToString();

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
                    throw new ArgumentException("Invalid deposit");
                }
                else
                {
                    var depositAmount = (decimal)command.Value!;

                    _walletService.Deposit(depositAmount);
                    _consoleService.WriteLine($"Your deposit of ${depositAmount.ToString("0.##")} was successful. Your current balance is: ${_walletService.Balance.ToString("0.##")}");
                    _logger.LogInformation($"Successfully deposited ${depositAmount}. Current balance: ${_walletService.Balance.ToString("0.##")}");
                }
            }
        }
    }
}
