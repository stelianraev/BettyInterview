namespace BettySlotGame.Services.Abtractions
{
    public interface IWalletService
    {
        decimal Balance { get; }

        public void Withdraw(decimal amount);

        public void Deposit(decimal amount);

        void ApplyGameResult(decimal betAmount, decimal winAmount);
    }
}
