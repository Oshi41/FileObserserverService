using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebServer2._2.Services
{
    public class WebServer : WebServerBase
    {
        public WebServer(IConfigurationRoot root)
            : base(root)
        {
        }

        public override void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod == "GET")
            {
                SendFiles(context, context.Request.RawUrl);
            }
        }

        private void SendFiles(HttpListenerContext context, string requestRawUrl)
        {
            try
            {
                var info = new FileInfo(Folder + requestRawUrl);
                var response = context.Response;
                
                using (var stream = File.OpenRead(info.FullName))
                {
                    response.ContentLength64 = stream.Length;
                    response.SendChunked = true;
                    
                    WriteToStream(stream, response.OutputStream);
                    
                    response.StatusDescription = HttpStatusCode.OK.ToString();
                    response.StatusCode = (int) HttpStatusCode.OK;
                    
                    stream.Close();
                }
                
                response.ContentType = MimeMapping.MimeUtility.GetMimeMapping(info.FullName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(e.Message));
                context.Response.Close();
            }
        }

        private void WriteToStream(Stream @from, Stream to)
        {
            var buffer = new byte[60 * 1024];
            int position;

            using (var writer = new BinaryWriter(to))
            {
                while ((position = @from.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer.Write(buffer, 0, position);
                    writer.Flush();
                }
            }
        }
    }
}