using System;
using System.Configuration;
using System.Diagnostics;
using Elvencurse2.Engine;
using Microsoft.Owin.Hosting;

namespace Elvencurse2.Server
{
    class Program
    {
        public static ElvenGame Game = new ElvenGame();

        static void Main(string[] args)
        {

            Trace.Listeners.RemoveAt(0);

            //netsh http add urlacl http://+:1234/ user=Everyone
            using (WebApp.Start(ConfigurationManager.AppSettings["realm"]))
            {
                Console.WriteLine("Kører {0}", ConfigurationManager.AppSettings["realm"]);
                Console.ReadLine();
            }
        }
    }
}
