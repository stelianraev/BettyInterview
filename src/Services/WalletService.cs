using BettySlotGame.Services.Abtractions;

namespace BettySlotGame.Services
{
    public class WalletService : IWalletService
    {
        private decimal _balance;

        public WalletService()
        {
            _balance = 0.0m;
        }

        public decimal Balance => _balance;

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.", nameof(amount));
            }

            _balance += amount;

            Console.WriteLine($"Your deposit of ${amount} was successful. Your current balance is: ${_balance.ToString("0.##")}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.", nameof(amount));
            }
            else if (amount > _balance)
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }
            
            _balance -= amount;

            Console.WriteLine($"Your withdrawal of ${amount} was successful. Your current balance is: ${_balance.ToString("0.##")}");
        }

        public void ApplyGameResult(decimal betAmount, decimal winAmount)
        {
            _balance = (_balance - betAmount) + winAmount;

            if(winAmount > 0)
            {
                Console.WriteLine($"Congrats - you won ${winAmount}! Your current balance is: ${_balance.ToString("0.##")}");
            }
            else
            {
                Console.WriteLine($"No luck this time! Your cuurent balance is: ${_balance.ToString("0.##")}");
            }
        }
    }
}
