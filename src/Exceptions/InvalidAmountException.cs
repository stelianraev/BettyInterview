namespace BettySlotGame.Exceptions
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException(string exceptionText)
            : base(exceptionText)
        {
        }
    }
}
