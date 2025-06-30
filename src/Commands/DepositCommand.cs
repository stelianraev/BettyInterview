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
        private readonly IConsoleService _consoleService;

        public DepositCommand(IValidator<Command> valdator, IWalletService wallet, IConsoleService consoleService, ILogger<DepositCommand> logger)
        {
            _validator = valdator;
            _wallet = wallet;
            _logger = logger;
            _consoleService = consoleService;
        }

        public string Name => CommandEnum.Deposit.ToString();

        public void Execute(string[] args, CancellationToken cancellationToken)
        {
            try
            {
                if (args.Length < 2 || !decimal.TryParse(args[1], out var amount))
                {
                    _consoleService.WriteLine("Unknow command");
                    _logger.LogInformation($"Invalid input {args.ToString()}");
                    return;
                }

                var command = new Command { CommandName = "deposit", Value = amount };
                var result = _validator.Validate(command);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        _consoleService.WriteLine($"{error.ErrorMessage}");
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
