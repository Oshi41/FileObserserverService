using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebServer2._2.Services
{
    public class TestWebServer : WebServerBase
    {
        public TestWebServer(IConfigurationRoot root) : base(root)
        {
        }

        public override async void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod == "GET")
            {
                var bytes = Encoding.UTF8.GetBytes("Hello World!");

                var response = context.Response;
                response.ContentLength64 = bytes.Length;
                await response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
        }
    }
}