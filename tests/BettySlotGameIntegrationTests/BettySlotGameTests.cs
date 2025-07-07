using BettySlotGame.Commands;
using BettySlotGame.Models;
using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using BettySlotGame.Services.BettySlotGame.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Globalization;

namespace BettySlotGameIntegrationTests
{
    public class BettySlotGameTests
    {
        [Theory]
        [InlineData("bet 10")]
        [InlineData("Bet 5")]
        [InlineData("BET 8")]
        [InlineData("BeT 3")]
        public void BetCommandShouldBePlacedIfInputIsValid(string inputCommand)
        {
            var startingBalance = 100M;
            var walletSettings = Options.Create(new WalletSettings { StartingBalance = startingBalance });

            var gameSettings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var mockConsole = new Mock<IConsoleService>();
            var inputQueue = new Queue<string>(new[] { inputCommand, "exit" });

            mockConsole.Setup(c => c.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()));

            var mockRandom = new Mock<IRandomNumberProvider>();
            mockRandom.Setup(r => r.GetRandomNumber()).Returns(0.60m);

            var wallet = new WalletService(walletSettings);
            var engine = new BettySlotGameEngine(mockRandom.Object, gameSettings);
            var cts = new CancellationTokenSource();

            var commands = new List<ISlotCommand>
            {
                new BetCommand(engine, wallet, mockConsole.Object, gameSettings, Mock.Of<ILogger<BetCommand>>()),
                new ExitCommand(cts, Mock.Of<ILogger<ExitCommand>>())
            };

            var registry = new CommandRegistry(commands);
            var game = new SlotGameService(engine, wallet, registry, mockConsole.Object, cts);

            game.StartGame();

            var splitInput = inputCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var inputValue = decimal.Parse(splitInput[1]);

            var expected = startingBalance - inputValue + (inputValue * 2);
            Assert.Equal(expected , wallet.Balance);
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains($"Congrats - you won ${inputValue * 2}! Your current balance is: ${wallet.Balance.ToString("0.##", CultureInfo.InvariantCulture)}"))), Times.Once);
        }

        [Theory]
        [InlineData("bet 0")]
        [InlineData("Bet -5")]
        [InlineData("BET 11")]
        [InlineData("BeT 8")]
        [InlineData("beT 300")]
        public void BetCommandShouldNotBePlacedIfInputIsInValid(string inputCommand)
        {
            var startingBalance = 5;
            var walletSettings = Options.Create(new WalletSettings { StartingBalance = startingBalance });

            var gameSettings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var mockConsole = new Mock<IConsoleService>();
            var inputQueue = new Queue<string>(new[] { inputCommand, "exit" });

            mockConsole.Setup(c => c.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()));

            var mockRandom = new Mock<IRandomNumberProvider>();
            mockRandom.Setup(r => r.GetRandomNumber()).Returns(0.60m);

            var wallet = new WalletService(walletSettings);
            var engine = new BettySlotGameEngine(mockRandom.Object, gameSettings);
            var cts = new CancellationTokenSource();

            var commands = new List<ISlotCommand>
            {
                new BetCommand(engine, wallet, mockConsole.Object, gameSettings, Mock.Of<ILogger<BetCommand>>()),
                new ExitCommand(cts, Mock.Of<ILogger<ExitCommand>>())
            };

            var registry = new CommandRegistry(commands);
            var game = new SlotGameService(engine, wallet, registry, mockConsole.Object, cts);

            game.StartGame();

            var splitInput = inputCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var inputValue = decimal.Parse(splitInput[1]);

            Assert.Equal(startingBalance, wallet.Balance);
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains($"Congrats - you won ${inputValue * 2}! Your current balance is: ${wallet.Balance.ToString("0.##", CultureInfo.InvariantCulture)}"))), Times.Never);
        }


        [Theory]
        [InlineData("deposit 100")]
        [InlineData("DePoSiT 55")]
        [InlineData("DEPOSIT 80")]
        [InlineData("Deposit 3000")]
        public void DepositCommandShouldSuccessIfInputIsValid(string inputCommand)
        {
            var startingBalance = 0M;
            var walletSettings = Options.Create(new WalletSettings { StartingBalance = startingBalance });

            var gameSettings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var mockConsole = new Mock<IConsoleService>();
            var inputQueue = new Queue<string>(new[] { inputCommand, "exit" });

            mockConsole.Setup(c => c.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()));

            var mockRandom = new Mock<IRandomNumberProvider>();
            mockRandom.Setup(r => r.GetRandomNumber()).Returns(0.60m);

            var wallet = new WalletService(walletSettings);
            var engine = new BettySlotGameEngine(mockRandom.Object, gameSettings);
            var cts = new CancellationTokenSource();

            var commands = new List<ISlotCommand>
            {
                new DepositCommand(wallet, mockConsole.Object, Mock.Of<ILogger<DepositCommand>>()),
                new ExitCommand(cts, Mock.Of<ILogger<ExitCommand>>())
            };

            var registry = new CommandRegistry(commands);
            var game = new SlotGameService(engine, wallet, registry, mockConsole.Object, cts);

            game.StartGame();

            var splitInput = inputCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var inputValue = decimal.Parse(splitInput[1]);
            Assert.Equal(startingBalance += inputValue, wallet.Balance);
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains($"Your deposit of ${inputValue} was successful. Your current balance is: ${wallet.Balance.ToString("0.##", CultureInfo.InvariantCulture)}"))), Times.Once);
        }

        [Fact]
        public void DepositCommandShouldFailedIfInputIsInValid()
        {
            var inputCommand = "deposit -10";
            var startingBalance = 0M;
            var walletSettings = Options.Create(new WalletSettings { StartingBalance = startingBalance });

            var gameSettings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var mockConsole = new Mock<IConsoleService>();
            var inputQueue = new Queue<string>(new[] { inputCommand, "exit" });

            mockConsole.Setup(c => c.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()));

            var mockRandom = new Mock<IRandomNumberProvider>();
            mockRandom.Setup(r => r.GetRandomNumber()).Returns(0.60m);

            var wallet = new WalletService(walletSettings);
            var engine = new BettySlotGameEngine(mockRandom.Object, gameSettings);
            var cts = new CancellationTokenSource();

            var commands = new List<ISlotCommand>
            {
                new DepositCommand(wallet, mockConsole.Object, Mock.Of<ILogger<DepositCommand>>()),
                new ExitCommand(cts, Mock.Of<ILogger<ExitCommand>>())
            };

            var registry = new CommandRegistry(commands);
            var game = new SlotGameService(engine, wallet, registry, mockConsole.Object, cts);

            game.StartGame();

            var splitInput = inputCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains($"Invalid deposit"))), Times.Once);
        }

        [Theory]
        [InlineData("withdraw 10")]
        [InlineData("WithDraw 5")]
        [InlineData("withDraw 80")]
        [InlineData("WITHDRAW 30")]
        public void WithdrawCommandShouldSuccessIfInputIsValid(string inputCommand)
        {
            var startingBalance = 100M;
            var walletSettings = Options.Create(new WalletSettings { StartingBalance = startingBalance });

            var gameSettings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var mockConsole = new Mock<IConsoleService>();
            var inputQueue = new Queue<string>(new[] { inputCommand, "exit" });

            mockConsole.Setup(c => c.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()));

            var mockRandom = new Mock<IRandomNumberProvider>();
            mockRandom.Setup(r => r.GetRandomNumber()).Returns(0.60m);

            var wallet = new WalletService(walletSettings);
            var engine = new BettySlotGameEngine(mockRandom.Object, gameSettings);
            var cts = new CancellationTokenSource();

            var commands = new List<ISlotCommand>
            {
                new WithdrawalCommand(wallet, mockConsole.Object, Mock.Of<ILogger<WithdrawalCommand>>()),
                new ExitCommand(cts, Mock.Of<ILogger<ExitCommand>>())
            };

            var registry = new CommandRegistry(commands);
            var game = new SlotGameService(engine, wallet, registry, mockConsole.Object, cts);

            game.StartGame();

            var splitInput = inputCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var inputValue = decimal.Parse(splitInput[1]);

            Assert.Equal(startingBalance -= inputValue, wallet.Balance);
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains($"Your withdrawal of ${inputValue} was successful. Your current balance is: ${wallet.Balance.ToString("0.##")}"))), Times.Once);
        }

        [Theory]        
        [InlineData("WithDraw -5")]
        [InlineData("withDraw 0")]
        public void WithdrawCommandShouldFailedIfInputIsInValid(string inputCommand)
        {
            var startingBalance = 50M;
            var walletSettings = Options.Create(new WalletSettings { StartingBalance = startingBalance });

            var gameSettings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var mockConsole = new Mock<IConsoleService>();
            var inputQueue = new Queue<string>(new[] { inputCommand, "exit" });

            mockConsole.Setup(c => c.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()));

            var mockRandom = new Mock<IRandomNumberProvider>();
            mockRandom.Setup(r => r.GetRandomNumber()).Returns(0.60m);

            var wallet = new WalletService(walletSettings);
            var engine = new BettySlotGameEngine(mockRandom.Object, gameSettings);
            var cts = new CancellationTokenSource();

            var commands = new List<ISlotCommand>
            {
                new WithdrawalCommand(wallet, mockConsole.Object, Mock.Of<ILogger<WithdrawalCommand>>()),
                new ExitCommand(cts, Mock.Of<ILogger<ExitCommand>>())
            };

            var registry = new CommandRegistry(commands);
            var game = new SlotGameService(engine, wallet, registry, mockConsole.Object, cts);

            game.StartGame();

            var splitInput = inputCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var inputValue = decimal.Parse(splitInput[1]);

            Assert.Equal(startingBalance, wallet.Balance);
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("Invalid withdraw"))), Times.Once);
        }

        [Fact]
        public void WithdrawCommandShouldFailedIfInputIsValidButNotEnoughtBalanceInWallet()
        {
            string inputCommand = "withdraw 100";
            var startingBalance = 50M;
            var walletSettings = Options.Create(new WalletSettings { StartingBalance = startingBalance });

            var gameSettings = Options.Create(new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50
            });

            var mockConsole = new Mock<IConsoleService>();
            var inputQueue = new Queue<string>(new[] { inputCommand, "exit" });

            mockConsole.Setup(c => c.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            mockConsole.Setup(c => c.WriteLine(It.IsAny<string>()));

            var mockRandom = new Mock<IRandomNumberProvider>();
            mockRandom.Setup(r => r.GetRandomNumber()).Returns(0.60m);

            var wallet = new WalletService(walletSettings);
            var engine = new BettySlotGameEngine(mockRandom.Object, gameSettings);
            var cts = new CancellationTokenSource();

            var commands = new List<ISlotCommand>
            {
                new WithdrawalCommand(wallet, mockConsole.Object, Mock.Of<ILogger<WithdrawalCommand>>()),
                new ExitCommand(cts, Mock.Of<ILogger<ExitCommand>>())
            };

            var registry = new CommandRegistry(commands);
            var game = new SlotGameService(engine, wallet, registry, mockConsole.Object, cts);

            game.StartGame();

            var splitInput = inputCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var inputValue = decimal.Parse(splitInput[1]);

            Assert.Equal(startingBalance, wallet.Balance);
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("Insufficient balance."))), Times.Once);
        }
    }
}
