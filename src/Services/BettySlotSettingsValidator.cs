using BettySlotGame.Models;
using Microsoft.Extensions.Options;

namespace BettySlotGame.Services
{
    public class BettySlotSettingsValidator : IValidateOptions<BettySlotSettings>
    {
        public ValidateOptionsResult Validate(string name, BettySlotSettings options)
        {
            var total = options.LoseBetPercentage + options.NormalWinBetPercentage + options.BigWinsBetPercentage;

            return total == 100.0M ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail("Bet percentages must sum to 100.");
        }
    }
}
