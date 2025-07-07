using BettySlotGame.Exceptions;
using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Options;

namespace BettySlotGame.Services
{
    public class WalletService : IWalletService
    {
        private readonly WalletSettings _walletSettings;
        private decimal _balance;

        public WalletService(IOptions<WalletSettings> walletSettings)
        {
            _walletSettings = walletSettings.Value;
            _balance = _walletSettings.StartingBalance;
        }

        public decimal Balance => _balance;

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAmountException($"Invalid deposit amount: {amount}. Amount must be greater than zero.");
            }

            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidAmountException($"Invalid withdraw amount: {amount}. Amount must be greater than zero.");
            }
            if (amount > _balance)
            {
                throw new InsufficientExecutionStackException("Insufficient balance.");
            }

            _balance -= amount;
        }
    }
}
