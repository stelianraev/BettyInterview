namespace BettySlotGame.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute(string[] args, CancellationToken token);
    }
}
