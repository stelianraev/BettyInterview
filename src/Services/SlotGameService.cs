using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;

namespace BettySlotGame.Services
{
    public class SlotGameService : ISlotGameService
    {
        private readonly ICommandRegistry _commandRegistry;
        private readonly CancellationTokenSource _cts;
        private readonly IConsoleService _consoleService;


        public SlotGameService(ISlotEngine gameEngine, IWalletService wallet, ICommandRegistry commandRegistry, IConsoleService consoleService, CancellationTokenSource cts)
        {
            _commandRegistry = commandRegistry;
            _consoleService = consoleService;
            _cts = cts;
        }

        public void StartGame()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    _consoleService.WriteLine("Please, submimt action:");

                    var input = _consoleService.ReadLine();
                    var splitInput = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (splitInput == null || splitInput.Length == 0)
                    {
                        _consoleService.WriteLine("No command entered.");
                        continue;
                    }

                    var commandName = splitInput[0];

                    var command = _commandRegistry.GetCommand(commandName);

                    if (command == null)
                    {
                        _consoleService.WriteLine($"Unknown command: {commandName}");
                        continue;
                    }

                    var inputCommand = new InputCommand
                    {
                        Name = commandName,
                        Value = splitInput.Length > 1 ? decimal.TryParse(splitInput[1], out var value) ? value : null : null,
                    };

                    command.Execute(inputCommand);

                    _consoleService.WriteLine();
                }
                catch (Exception ex)
                {
                    _consoleService.WriteLine(ex.Message);
                    _consoleService.WriteLine();
                }                
            }

            _consoleService.WriteLine("Press any key to exit.");
        }
    }
}
