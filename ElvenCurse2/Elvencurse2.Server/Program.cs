using System;
using System.Configuration;
using System.Diagnostics;
using Elvencurse2.Engine;
using Microsoft.Owin.Hosting;

namespace Elvencurse2.Server
{
    class Program
    {
        public static ElvenGame Game;

        static void Main(string[] args)
        {
            Program.Game = new ElvenGame();

            Program.Game.FpsUpdate += Game_FpsUpdate;

            Trace.Listeners.RemoveAt(0);

            //netsh http add urlacl http://+:1234/ user=Everyone
            using (WebApp.Start(ConfigurationManager.AppSettings["realm"]))
            {
                Console.WriteLine("Kører {0}", ConfigurationManager.AppSettings["realm"]);

                var cmd = "";
                do
                {
                    cmd = Console.ReadLine();

                    if (cmd == "show npcs")
                    {
                        foreach (var o in Program.Game.Gameobjects)
                        {
                            Console.WriteLine("{0} Zone {1} X {2} Y {3}",o.Name, o.Location.Zone, o.Location.X, o.Location.Y);
                        }
                    }
                } while (cmd != "exit");
            }
        }

        private static void Game_FpsUpdate(object sender, LongEventArgs e)
        {
            Console.Title = string.Format("Elvencurse2 {0}fps, Queuelength {1}", e.Value, Game.GameChanges.Count);
        }
    }
}
