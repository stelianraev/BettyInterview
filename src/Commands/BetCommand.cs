using BettySlotGame.Exceptions;
using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BettySlotGame.Commands
{
    public class BetCommand : ISlotCommand
    {
        private readonly ISlotEngine _slotEngine;
        private readonly IWalletService _walletService;
        private readonly IConsoleService _consoleService;
        private readonly BettySlotSettings _slotSettings;
        private readonly ILogger<BetCommand> _logger;

        public BetCommand(ISlotEngine slotEngine, IWalletService walletService, IConsoleService consoleService, IOptions<BettySlotSettings> slotSettings, ILogger<BetCommand> logger)
        {
            _slotEngine = slotEngine;
            _walletService = walletService;
            _consoleService = consoleService;
            _slotSettings = slotSettings.Value;
            _logger = logger;
        }
        public string Name => CommandEnum.Bet.ToString();

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (parameter is InputCommand command)
            {
                if (!(command.Value >= _slotSettings.MinBet && command.Value <= _slotSettings.MaxBet))
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

                if(!canExecute)
                {
                    throw new InvalidBetPlacementException($"Bet amount must be between {_slotSettings.MinBet} and {_slotSettings.MaxBet}");
                }
                else
                {
                    var bet = (decimal)command.Value!;
                    _walletService.Withdraw(bet);

                    var (isWin, returnAmount) = _slotEngine.Bet(bet);

                    if(isWin)
                    {
                        _walletService.Deposit(returnAmount);
                        _consoleService.WriteLine($"Congrats - you won ${returnAmount}! Your current balance is: ${_walletService.Balance.ToString("0.##")}");
                        _logger.LogInformation($"Successfully won ${returnAmount}. Current balance: ${_walletService.Balance.ToString("0.##")}");
                        return;
                    }
                    else
                    {                        
                        _consoleService.WriteLine($"No luck this time! Your cuurent balance is: ${_walletService.Balance.ToString("0.##")}");
                        _logger.LogInformation($"Bet of ${bet} was lost. Current balance: ${_walletService.Balance.ToString("0.##")}");
                    }
                }
            }
        }
    }
}
