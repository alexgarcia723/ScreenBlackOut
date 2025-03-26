using ScreenBlackOut;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ScreenBlackoutTool
{
    static class Program
    {
        // Unique name for your app
        private static Mutex? mutex;

        [STAThread]
        static void Main()
        {
            bool isNewInstance;

            mutex = new Mutex(true, "ScreenBlackoutTool_Mutex", out isNewInstance);

            if (!isNewInstance)
            {
                // Another instance is already running — exit this one
                MessageBox.Show("App is already running.", "Already Running", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());

            // Release the mutex when the app closes
            mutex.ReleaseMutex();
        }
    }
}
