using System;

namespace Elvencurse2.Client
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new ElvenCurse2())
                game.Run();
        }
    }
}
