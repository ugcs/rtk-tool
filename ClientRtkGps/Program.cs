using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bluegrams.Application;
using ClientRtkGps.Properties;

namespace ClientRtkGps
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            settings();

            Application.Run(new RtkForm());
        }

        private static void settings()
        {
            if (!Directory.Exists(ApplicationPaths.UserConfigDir))
            {
                Directory.CreateDirectory(ApplicationPaths.UserConfigDir);
            }

            PortableSettingsProvider.SettingsFileName = ApplicationPaths.ClientRtkGpsConfig;
            PortableSettingsProvider.SettingsDirectory = ApplicationPaths.UserConfigDir;
            PortableSettingsProvider.ApplyProvider(Settings.Default);
        }

    }
}
