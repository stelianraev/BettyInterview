using BettySlotGame.Services.Abtractions;

namespace BettySlotGame.Services
{
    public class SlotGameService : ISlotGameService
    {
        private readonly ICommandService _commandService;
        private readonly CancellationTokenSource _cts;
        private readonly IConsoleService _consoleService;

        public SlotGameService(ISlotEngine gameEngine, IWalletService wallet, ICommandService commandServices, IConsoleService consoleService, CancellationTokenSource cts)
        {
            _commandService = commandServices;
            _consoleService = consoleService;
            _cts = cts;
        }

        public void StartGame()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                _consoleService.WriteLine("Please, submimt action:");
                var input = _consoleService.ReadLine();
                
                _commandService.HandleInput(input, _cts.Token);

                _consoleService.WriteLine();
            }

            _consoleService.WriteLine("Press any key to exit.");
        }
    }
}
