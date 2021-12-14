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
            
            PortableSettingsProvider.SettingsFileName = "ClientRtkGps.config";
            PortableSettingsProvider.SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"DSS\configuration\");
            PortableSettingsProvider.ApplyProvider(Settings.Default);

            Application.Run(new RtkForm());
        }
    }
}
