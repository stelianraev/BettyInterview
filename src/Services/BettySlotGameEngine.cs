using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace BettySlotGame.Services
{
    public class BettySlotGameEngine : ISlotEngine
    {
        private readonly IRandomNumberProvider _randomNumberProvider;
        private readonly BettySlotSettings _settings;
        public BettySlotGameEngine(IRandomNumberProvider randomNumberProvider, IOptions<BettySlotSettings> settings)
        {
            _settings = settings.Value;
            _randomNumberProvider = randomNumberProvider;
        }

        public (bool, decimal) Bet(decimal bet)
        {
            var roll = _randomNumberProvider.GetRandomNumber() * 100;

            var loseThreshold = _settings.LoseBetPercentage;
            var normalWinThreshold = loseThreshold + _settings.NormalWinBetPercentage;
            var bigWinThreshold = normalWinThreshold + _settings.BigWinsBetPercentage;

            if (roll <= loseThreshold)
            {
                return (false, 0);
            }
            else if (roll <= normalWinThreshold)
            {
                return (true, bet * _settings.NormalWinBetMultiplier);
            }
            else if(roll <= bigWinThreshold)
            {
                var betMultiplier = BigWinsRange();

                return (true, bet * betMultiplier);
            }
            else
            {
                throw new InvalidOperationException("Invalid roll distribution.");
            }
        }

        private decimal BigWinsRange()
        {
            var randomNumber = _randomNumberProvider.GetRandomNumber();
            var minMultiplier = _settings.BigWinMinMultiplier;
            var maxMultiplier = _settings.BigWinMaxMultiplier;

            var betMultiprier = minMultiplier + (decimal)randomNumber * (maxMultiplier - minMultiplier);

            return betMultiprier;
        }
    }
}
