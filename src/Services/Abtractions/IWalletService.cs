namespace BettySlotGame.Services.Abtractions
{
    public interface IWalletService
    {
        decimal Balance { get; }

        bool CanAfford(decimal amount);

        public void Withdraw(decimal amount);

        public void Deposit(decimal amount);

        void ApplyGameResult(decimal betAmount, decimal winAmount);
    }
}
