using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using BettySlotGame.Services.Validators;
using FluentValidation;
using Moq;

namespace BettySlotGameUnitTests
{
    public class InputValidationTests
    {
        private readonly IValidator<Command> _validator;
        private readonly Mock<IWalletService> _walletMock = new Mock<IWalletService>();

        public InputValidationTests()
        {
            _walletMock.Setup(wallet => wallet.Balance);

            _validator = new CommandValidator(_walletMock.Object);

        }

        [Theory]
        [InlineData("bet", -10)]
        [InlineData("bet", 0)]
        [InlineData("bet", 12)]
        [InlineData(" ", 10)]
        [InlineData("wrongCommand", 10)]
        [InlineData("deposit", -100)]
        [InlineData("withdraw", -50)]
        public void InvalidInputValidation(string commandName, decimal value)
        {
            var command = new Command { CommandName = commandName, Value = value };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("bet", 2)]
        [InlineData("deposit", 100)]
        [InlineData("withdraw", 20)]
        public void ValidInputValidation(string commandName, decimal value)
        {
            var command = new Command { CommandName = commandName, Value = value };

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }
    }
}
