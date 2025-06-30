using BettySlotGame.Services.Abtractions;
using System.Text;

namespace BettySlotGame.Services
{
    public class SlotGameService : ISlotGameService
    {
        private readonly ICommandService _commandService;
        private readonly CancellationTokenSource _cts;

        public SlotGameService(ISlotEngine gameEngine, IWalletService wallet, ICommandService commandServices, CancellationTokenSource cts)
        {
            _commandService = commandServices ?? throw new ArgumentNullException(nameof(commandServices), "Command service cannot be null.");
            _cts = cts;
        }

        public void StartGame()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("Please, submimt action:");
                var input = Console.ReadLine();
                
                _commandService.HandleInput(input, _cts.Token);

                Console.WriteLine();
            }

            Console.WriteLine("Press any key to exit.");
        }
    }
}
