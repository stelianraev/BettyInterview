using BettySlotGame.Models;
using BettySlotGame.Services;
using Microsoft.Extensions.Options;

namespace BettySlotGameIntegrationTests
{
    public class MonteCarloTest
    {
        [Theory]
        [InlineData(1000000)]
        public void MonteCarloSimulation(long numTrials)
        {

            var settings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var random = new RandomNumberProvider();

            var engine = new BettySlotGameEngine(random, settings);

            decimal betAmount = 10;
            int lossCount = 0;
            int winCount = 0;
            int bigWinCount = 0;
            decimal totalReturn = 0;

            for (int i = 0; i < numTrials; i++)
            {
                var (isWin, payout) = engine.Bet(betAmount);

                totalReturn += payout;

                if (payout == 0)
                {
                    lossCount++;
                }                    
                else if (payout == betAmount * 2)
                {
                    winCount++;
                }
                else
                {
                    bigWinCount++;
                }
            }

            var lossPct = (decimal)lossCount / numTrials * 100;
            var winPct = (decimal)winCount / numTrials * 100;
            var bigWinPct = (decimal)bigWinCount / numTrials * 100;

            Assert.InRange(lossPct, 48, 52);
            Assert.InRange(winPct, 38, 42);
            Assert.InRange(bigWinPct, 8, 12);
        }
    }
}
