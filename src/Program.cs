using BettySlotGame.Extensions;
using BettySlotGame.Models;
using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using BettySlotGame.Services.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BettySlotGame
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(cts);
                    services.AddSingleton<ISlotEngine, BettySlotGameEngine>();
                    services.AddSingleton<IWalletService, WalletService>();
                    services.AddSingleton<ISlotGameService, SlotGameService>();
                    services.AddSingleton<ICommandService, CommandService>();
                    services.AddSingleton<IValidator<Command>, CommandValidator>();
                    services.AddSingleton<IConsoleService, ConsoleService>();

                    services.RegisterAllCommands();

                    services.AddLogging(builder =>
                    {
                        builder.ClearProviders();
                    });

                })
                .Build();

            var app = host.Services.GetRequiredService<ISlotGameService>();

            try
            {
                app.StartGame();
            }
            catch (Exception ex)
            {
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while starting the game.");
            }       
        }
    }
}
