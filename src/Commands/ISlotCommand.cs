using System.Windows.Input;

namespace BettySlotGame.Commands
{
    public interface ISlotCommand : ICommand
    {
        string Name { get; }
    }
}
