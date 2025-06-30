using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BettySlotGame.Commands
{
    public class BetCommand : ICommand
    {
        private readonly IWalletService _wallet;
        private readonly ISlotEngine _engine;
        private readonly IValidator<Command> _validator;
        private readonly ILogger<BetCommand> _logger;

        public BetCommand(IWalletService wallet, ISlotEngine engine, IValidator<Command> validator, ILogger<BetCommand> logger)
        {
            _wallet = wallet;
            _engine = engine;
            _validator = validator;
        }

        public string Name => CommandEnum.Bet.ToString();

        public void Execute(string[] args, CancellationToken token)
        {
            try
            {
                if (args.Length < 2 || !decimal.TryParse(args[1], out var amount))
                {
                    Console.WriteLine("Unknow command");
                    return;
                }

                var command = new Command { CommandName = "bet", Value = amount };
                var result = _validator.Validate(command);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"{error.ErrorMessage}");
                    }

                    return;
                }

                var win = _engine.Bet(amount);
                _wallet.ApplyGameResult(amount, win);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the bet command.");
            }
        }
    }
}
