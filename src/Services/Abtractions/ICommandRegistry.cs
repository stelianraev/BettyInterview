using BettySlotGame.Commands;

namespace BettySlotGame.Services.Abtractions
{
    public interface ICommandRegistry
    {
        ISlotCommand GetCommand(string command);
    }
}
