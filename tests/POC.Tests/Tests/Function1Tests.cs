using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using Xunit;

namespace POC.Tests
{
    public class Function1Tests
    {
        [Fact]
        public void Run_ReturnsHelloWorldResponse()
        {
            var logger = new NullLogger<Function1>();
            var function = new Function1(logger);

            var req = new TestHttpRequestData();

            var response = function.Run(req);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Try to read body if our TestHttpResponseData supports it
            if (response is TestHttpResponseData tr)
            {
                var body = tr.ReadBody();
                Assert.Contains("Hola Mundo", body);
            }
        }
    }
}
