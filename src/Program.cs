using System;
using System.Threading;
using System.Windows.Forms;

namespace MonoGame_Learning_Livestream {
//#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {

        public static Engine Game;
        public static Thread t_WinForms;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // This will be our Game "Editor" if you will.
            // Create a new thread (WinForms Thread)
            t_WinForms = new Thread(new ThreadStart(t_WinForms_Start));
            t_WinForms.Start();

            // Create the game instance
            // Run the game
            Game = new Engine();
            Game.Run();
        }

        static void t_WinForms_Start() {
            // Should open a gui and the game 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new Forms.SampleOverview());
        }
    }

}
