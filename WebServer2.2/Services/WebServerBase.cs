using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebServer2._2.Services
{
    public abstract class WebServerBase : IMailServer
    {
        private readonly IConfigurationRoot _root;
        
        private readonly HttpListener _listener;

        protected string Folder
        {
            get
            {
                _root.Reload();
                return _root.GetSection("Folder").Value;
            }
        }

        public bool IsListening
        {
            get => _listener.IsListening;
            set
            {
                if (IsListening == value)
                    return;

                if (value)
                {
                    _listener.Start();
                    RunLoop();
                }
                else
                {
                    _listener.Stop();
                }
            }
        }

        protected WebServerBase(IConfigurationRoot root)
        {
            _root = root;
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:8080/");
        }

        public abstract void Handle(HttpListenerContext context);

        private async void RunLoop()
        {
            while (IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    Task.Run(() =>
                    {
                        try
                        {
                            Handle(context);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Dispose()
        {
            ((IDisposable) _listener)?.Dispose();
        }
    }
}