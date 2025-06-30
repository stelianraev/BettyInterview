using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Commands
{
    public class WithdrawalCommand : ICommand
    {
        private readonly IWalletService _wallet;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<WithdrawalCommand> _logger;
        public WithdrawalCommand(IValidator<Command> validator, IWalletService wallet, ILogger<WithdrawalCommand> logger)
        {
            _validator = validator;
            _wallet = wallet;
            _logger = logger;
        }
        public string Name => CommandEnum.Withdraw.ToString();
        public void Execute(string[] args, CancellationToken cancellationToken)
        {
            try
            {
                if (args.Length < 2 || !decimal.TryParse(args[1], out var amount))
                {
                    Console.WriteLine("Unknow command");
                    return;
                }

                var command = new Command { CommandName = "withdraw", Value = amount };
                var result = _validator.Validate(command);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"{error.ErrorMessage}");
                    }

                    return;
                }
                _wallet.Withdraw(amount);

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the withdrawal command.");
                return;
            }
        }
    }
}
