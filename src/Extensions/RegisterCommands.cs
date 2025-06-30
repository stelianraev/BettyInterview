using BettySlotGame.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace BettySlotGame.Extensions
{
    public static class RegisterCommands
    {
        public static void RegisterAllCommands(this IServiceCollection services)
        {
            services.AddSingleton<ICommand, WithdrawalCommand>();
            services.AddSingleton<ICommand, DepositCommand>();
            services.AddSingleton<ICommand, BetCommand>();
            services.AddSingleton<ICommand, ExitCommand>();
        }
    }
}
