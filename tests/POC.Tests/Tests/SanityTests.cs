using Xunit;

namespace POC.Tests
{
    public class SanityTests
    {
        [Fact]
        public void OnePlusOne_IsTwo()
        {
            Assert.Equal(2, 1 + 1);
        }
    }
}
