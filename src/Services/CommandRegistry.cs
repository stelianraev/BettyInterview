using BettySlotGame.Commands;
using BettySlotGame.Services.Abtractions;

namespace BettySlotGame.Services
{
    namespace BettySlotGame.Services
    {
        public class CommandRegistry : ICommandRegistry
        {
            private readonly IEnumerable<ISlotCommand> _commands;

            public CommandRegistry(IEnumerable<ISlotCommand> commands)
            {
                _commands = commands;
            }

            public ISlotCommand? GetCommand(string commandName)
            {
                var command = _commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
                return command;
            }
        }
    }
}
