using BettySlotGame.Services;
using Moq;

namespace BettySlotGameUnitTests
{
    public class BettySlotGameEngineTests
    {

        [Fact]
        public void BetShouldReturnZeroWhenRollIsBelowOrEqual50()
        {
            // Arrange
            var randomMock = new Mock<Random>();
            randomMock.Setup(r => r.Next(1, 101)).Returns(40); // simulate roll <= 50
            var engine = new BettySlotGameEngine(randomMock.Object);

            // Act
            var result = engine.Bet(10);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void BetShouldReturnx2whenRollIsBeHigherThan50AndLessThan90()
        {
            // Arrange
            var randomMock = new Mock<Random>();
            randomMock.Setup(r => r.Next(1, 101)).Returns(60); // simulate roll <= 60
            var engine = new BettySlotGameEngine(randomMock.Object);

            // Act
            var result = engine.Bet(10);

            // Assert
            Assert.Equal(20, result);
        }
    }
}
