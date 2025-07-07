using BettySlotGame.Extensions;
using BettySlotGame.Models;
using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using BettySlotGame.Services.BettySlotGame.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                    services.Configure<BettySlotSettings>(context.Configuration.GetSection("BettySlotSettings"));
                    services.Configure<WalletSettings>(context.Configuration.GetSection("WalletSettings"));

                    services.AddSingleton(cts);
                    services.AddSingleton<ISlotEngine, BettySlotGameEngine>();
                    services.AddSingleton<IWalletService, WalletService>();
                    services.AddSingleton<ISlotGameService, SlotGameService>();
                    services.AddSingleton<ICommandRegistry, CommandRegistry>();
                    services.AddSingleton<IConsoleService, ConsoleService>();
                    services.AddSingleton<IRandomNumberProvider, RandomNumberProvider>();
                    services.AddSingleton<IValidateOptions<BettySlotSettings>, BettySlotSettingsValidator>();

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
