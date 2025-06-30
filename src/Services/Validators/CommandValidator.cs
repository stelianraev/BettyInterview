using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using FluentValidation;

namespace BettySlotGame.Services.Validators
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IWalletService wallet)
        {
            RuleFor(x => x.CommandName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Command cannot be empty or null.");

            RuleFor(x => x.CommandName)
                .Must(x => Enum.TryParse<CommandEnum>(x, true, out _))
                .WithMessage("Invalid command.");

            RuleFor(x => x.Value)
                .Must((command, value) => value >= command.MinBet && value <= command.MaxBet)
                .When(x => x.CommandName!.Equals(CommandEnum.Bet.ToString(), StringComparison.InvariantCultureIgnoreCase))
                .WithMessage(command => $"Value must be between {command.MinBet} and {command.MaxBet}.");


            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Value must be greater than zero.");
        }
    }
}
