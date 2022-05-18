using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRtkGps
{
    public static class ApplicationPaths
    {
        public static string AppFolderName => "DSS";
        public static string AppLocalDataFolder => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppFolderName);

        public static string UserConfigDir => Path.Combine(AppLocalDataFolder, "configuration", "ClientRtkGps");

        public static string ClientRtkGpsConfig = "ClientRtkGps.config";
        public static string BasepostlistFile = "baseposlist.xml";

    }
}
