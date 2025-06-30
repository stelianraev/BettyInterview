using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Services
{
    public class WalletService : IWalletService
    {
        private readonly IConsoleService _consoleService;
        private readonly ILogger<WalletService> _logger;

        private decimal _balance;

        public WalletService(IConsoleService consoleService, ILogger<WalletService> logger)
        {
            _balance = 0.0m;
            _consoleService = consoleService;
            _logger = logger;
        }

        public decimal Balance => _balance;

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                _logger.LogInformation("Deposit amount must be greater than zero.");
                throw new ArgumentException("Deposit amount must be greater than zero.", nameof(amount));
            }

            _balance += amount;

            _logger.LogInformation($"Successfully deposit ${amount}");
            _consoleService.WriteLine($"Your deposit of ${amount} was successful. Your current balance is: ${_balance.ToString("0.##")}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                _logger.LogInformation("Withdrawal amount must be greater than zero.");
                throw new ArgumentException("Withdrawal amount must be greater than zero.", nameof(amount));
            }
            else if (amount > _balance)
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }
            
            _balance -= amount;
            _logger.LogInformation($"Successfully withdrawal ${amount}");

            _consoleService.WriteLine($"Your withdrawal of ${amount} was successful. Your current balance is: ${_balance.ToString("0.##")}");
        }

        public void ApplyGameResult(decimal betAmount, decimal winAmount)
        {
            _balance = (_balance - betAmount) + winAmount;

            if(winAmount > 0)
            {
                _consoleService.WriteLine($"Congrats - you won ${winAmount}! Your current balance is: ${_balance.ToString("0.##")}");
            }
            else
            {
                _consoleService.WriteLine($"No luck this time! Your cuurent balance is: ${_balance.ToString("0.##")}");
            }
        }
    }
}
