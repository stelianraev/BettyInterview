using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Commands
{
    public class DepositCommand : ICommand
    {
        private readonly IWalletService _wallet;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<DepositCommand> _logger;

        public DepositCommand(IValidator<Command> valdator, IWalletService wallet, ILogger<DepositCommand> logger)
        {
            _validator = valdator;
            _wallet = wallet;
            _logger = logger;
        }

        public string Name => CommandEnum.Deposit.ToString();

        public void Execute(string[] args, CancellationToken cancellationToken)
        {
            try
            {
                if (args.Length < 2 || !decimal.TryParse(args[1], out var amount))
                {
                    Console.WriteLine("Unknow command");
                    return;
                }

                var command = new Command { CommandName = "deposit", Value = amount };
                var result = _validator.Validate(command);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"{error.ErrorMessage}");
                    }

                    return;
                }

                _wallet.Deposit(amount);

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the deposit command.");
                return;
            }
        }
    }
}
