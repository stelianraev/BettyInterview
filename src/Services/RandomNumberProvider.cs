using BettySlotGame.Services.Abtractions;
using System.Security.Cryptography;

namespace BettySlotGame.Services
{
    public class RandomNumberProvider : IRandomNumberProvider
    {
        public decimal GetRandomNumber()
        {
            byte[] bytes = new byte[8];
            RandomNumberGenerator.Fill(bytes);
            ulong ulongRand = BitConverter.ToUInt64(bytes, 0);
            return (decimal)(ulongRand / (double)ulong.MaxValue);
        }
    }
}
