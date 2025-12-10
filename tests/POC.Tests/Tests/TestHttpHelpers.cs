using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace POC.Tests
{
    internal class TestFunctionContext : FunctionContext
    {
        private readonly IDictionary<object, object?> _items = new Dictionary<object, object?>();

        public override IServiceProvider InstanceServices { get; } = new DefaultServiceProvider();
        public override string InvocationId { get; } = Guid.NewGuid().ToString();
        public override FunctionDefinition? FunctionDefinition => null;
        public override TraceContext TraceContext => throw new NotImplementedException();
        public override BindingContext BindingContext => throw new NotImplementedException();
        public override IDictionary<object, object?> Items => _items;

        private class DefaultServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType) => null;
        }
    }

    internal class TestHttpRequestData : HttpRequestData
    {
        private readonly MemoryStream _body = new();
        public TestHttpRequestData(string method = "GET", string url = "http://localhost") : base(new TestFunctionContext())
        {
            Method = method;
            Url = new Uri(url);
            Headers = new HttpHeadersCollection();
        }

        public override Stream Body => _body;
        public override HttpHeadersCollection Headers { get; }
        public override IReadOnlyCollection<Cookie> Cookies => Array.Empty<Cookie>();
        public override Uri Url { get; }
        public override string Method { get; }

        public void SetBody(string content)
        {
            _body.SetLength(0);
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            _body.Write(bytes, 0, bytes.Length);
            _body.Position = 0;
        }

        public override HttpResponseData CreateResponse()
        {
            return new TestHttpResponseData(this);
        }
    }

    internal class TestHttpResponseData : HttpResponseData
    {
        private readonly MemoryStream _body = new();
        public TestHttpResponseData(HttpRequestData request) : base(request.FunctionContext!)
        {
            StatusCode = HttpStatusCode.OK;
            Headers = new HttpHeadersCollection();
            Body = _body;
        }

        public override HttpHeadersCollection Headers { get; }
        public override Stream Body { get; }
        public override HttpStatusCode StatusCode { get; set; }
        public override IReadOnlyCollection<Cookie> Cookies => Array.Empty<Cookie>();

        public string ReadBody()
        {
            Body.Position = 0;
            using var reader = new StreamReader(Body);
            return reader.ReadToEnd();
        }
    }
}
