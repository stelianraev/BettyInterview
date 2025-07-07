using BettySlotGame.Models;
using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BettySlotGameUnitTests
{
    public class BettySlotGameEngineTests
    {
        private readonly BettySlotSettings _bettySlotSettings;
        public BettySlotGameEngineTests()
        {
            _bettySlotSettings = new BettySlotSettings
            {
                MinBet = 1,
                MaxBet = 10,
                BigWinMinMultiplier = 2,
                BigWinMaxMultiplier = 10,
                BigWinsBetPercentage = 10,
                LoseBetPercentage = 50,
                NormalWinBetPercentage = 40,
                NormalWinBetMultiplier = 2
            };
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void BetShouldReturnZeroWhenRollIsBelowOrEqual50(decimal bet)
        {
            var settingsMock = new Mock<IOptions<BettySlotSettings>>();
            settingsMock.SetReturnsDefault(_bettySlotSettings);

            var rngMock = new Mock<IRandomNumberProvider>();
            rngMock.Setup(x => x.GetRandomNumber()).Returns(0.1m);

            var engine = new BettySlotGameEngine(rngMock.Object, settingsMock.Object);

            var (isWin, betAmount) = engine.Bet(bet);

            Assert.False(isWin);
            Assert.Equal(0, betAmount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void BetShouldReturnMultiplierx2WhenRollIsBelow90(decimal bet)
        {
            var settingsMock = new Mock<IOptions<BettySlotSettings>>();
            settingsMock.SetReturnsDefault(_bettySlotSettings);

            var rngMock = new Mock<IRandomNumberProvider>();
            rngMock.Setup(x => x.GetRandomNumber()).Returns(0.7m);

            var engine = new BettySlotGameEngine(rngMock.Object, settingsMock.Object);

            var (isWin, betAmount) = engine.Bet(bet);

            Assert.True(isWin);
            Assert.Equal(bet * _bettySlotSettings.NormalWinBetMultiplier, betAmount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void BetShouldReturnMultiplierBetween2and10WhenRollIsOver90(decimal bet)
        {
            var settingsMock = new Mock<IOptions<BettySlotSettings>>();
            settingsMock.SetReturnsDefault(_bettySlotSettings);

            var rngMock = new Mock<IRandomNumberProvider>();
            rngMock.Setup(x => x.GetRandomNumber()).Returns(0.91m);

            var engine = new BettySlotGameEngine(rngMock.Object, settingsMock.Object);

            var (isWin, betAmount) = engine.Bet(bet);

            var betMultiprier = _bettySlotSettings.BigWinMinMultiplier + (decimal)0.91 * (_bettySlotSettings.BigWinMaxMultiplier - _bettySlotSettings.BigWinMinMultiplier);

            Assert.True(isWin);
            Assert.Equal(betMultiprier * bet, betAmount);
        }
    }
}
