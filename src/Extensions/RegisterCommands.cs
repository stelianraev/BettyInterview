using BettySlotGame.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace BettySlotGame.Extensions
{
    public static class RegisterCommands
    {
        public static void RegisterAllCommands(this IServiceCollection services)
        {
            var commandInterface = typeof(ISlotCommand);

            var commands = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => commandInterface.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            foreach (var command in commands)
            {
                services.AddSingleton(commandInterface, command);
            }
        }
    }
}
