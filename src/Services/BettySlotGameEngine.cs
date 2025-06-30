using BettySlotGame.Services.Abtractions;

namespace BettySlotGame.Services
{
    public class BettySlotGameEngine : ISlotEngine
    {
        private readonly Random _random;

        public BettySlotGameEngine(Random? radom = null)
        {
            _random = radom ?? new Random();
        }

        public decimal Bet(decimal bet)
        {
            // Generates a random number between 1 and 100
            var roll = _random.Next(1, 101); 

            //50% chance to lose
            if (roll <= 50) 
            {
                return 0;
            }
            //40% chance to win a small amount
            else if (roll <= 90)
            {
                return bet * 2;
            }
            //10% chance to win a medium amount (roll > 90)
            else
            {
                //_random.NextDouble() * 8.0 sets the multiplier to a random value between 0.0 and 8.0,
                //then adding 2.0 ensures the multiplier is always at least 2.0,
                //resulting in a final multiplier range of [2.0, 10.0).
                var multiplier = (decimal)(_random.NextDouble() * 8.0 + 2.0);
                return Math.Round(bet * multiplier, 2);
            }
        }
    }
}
