namespace BettySlotGame.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(string exceptionText)
            : base(exceptionText)
        {
        }
    }
}
