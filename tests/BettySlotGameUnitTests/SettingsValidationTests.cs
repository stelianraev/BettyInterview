using BettySlotGame.Models;
using BettySlotGame.Services;

namespace BettySlotGameUnitTests
{
    public class SettingsValidationTests
    {
        [Fact]
        public void SettingsValidationSuccessWhenPercentagesSumTo100()
        {
            var settings = new BettySlotSettings
            {
                LoseBetPercentage = 50,
                NormalWinBetPercentage = 40,
                BigWinsBetPercentage = 10
            };

            var validator = new BettySlotSettingsValidator();

            var result = validator.Validate(nameof(BettySlotSettings), settings);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public void SettingsValidationFailedWhenPercentagesDoNotSum100()
        {
            var settings = new BettySlotSettings
            {
                LoseBetPercentage = 50,
                NormalWinBetPercentage = 30,
                BigWinsBetPercentage = 10
            };

            var validator = new BettySlotSettingsValidator();

            var result = validator.Validate(nameof(BettySlotSettings), settings);

            Assert.False(result.Succeeded);
            Assert.Contains("Bet percentages must sum to 100", result.FailureMessage);
        }
    }
}
