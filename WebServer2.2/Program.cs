using System;
using System.Threading.Tasks;
using ConsoleDI.DI;
using WebServer2._2.DI;
using WebServer2._2.Services;

namespace WebServer2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dp = Create())
            {
                dp.Start();

                var server = dp.GetService<IMailServer>();
                server.IsListening = true;
                
                Task.Delay(-1).Wait();
            }

            Console.ReadKey();
        }

        static DependencyInjectionBase Create()
        {
            return new MailApp();
        }
    }
}