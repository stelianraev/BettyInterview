namespace BettySlotGame.Exceptions
{
    public class InvalidBetPlacementException : Exception
    {
        public InvalidBetPlacementException(string message)
            : base(message)
        {
        }
    }
}
