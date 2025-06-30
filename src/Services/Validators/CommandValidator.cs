using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using FluentValidation;

namespace BettySlotGame.Services.Validators
{
    public class CommandValidator : AbstractValidator<Command>
    {
        private readonly IWalletService _wallet;
        private readonly int _minBetAmount = Constants.BettySlotMinBetAmount;
        private readonly int _maxBetAmmount = Constants.BettySlotMaxBetAmount;
        public CommandValidator(IWalletService wallet)
        {
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet), "Wallet cannot be null.");

            RuleFor(x => x.CommandName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Command cannot be empty or null.");

            RuleFor(x => x.CommandName)
                .Must(x => Enum.TryParse<CommandEnum>(x, true, out _))
                .WithMessage("Invalid command.");

            RuleFor(x => x.Value)
                .InclusiveBetween(_minBetAmount, _maxBetAmmount)
                .WithMessage($"Value must be between {_minBetAmount} and {_maxBetAmmount}.")
                .When(x => x.CommandName!.Equals(CommandEnum.Bet.ToString(), StringComparison.InvariantCultureIgnoreCase));

            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Value must be greater than zero.");
        }
    }
}
