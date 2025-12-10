using Xunit;
using Microsoft.Extensions.Logging.Abstractions;

namespace POC.Tests
{
    public class Function1SimpleTests
    {
        [Fact]
        public void Function1_Constructor_CreatesInstance()
        {
            // Arrange
            var logger = NullLogger<Function1>.Instance;

            // Act
            var function = new Function1(logger);

            // Assert
            Assert.NotNull(function);
        }
    }
}
