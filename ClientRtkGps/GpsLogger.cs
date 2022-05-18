using System;
using System.IO;
using System.Text;

public class GpsLogger
{
    private string logDir;
    private StreamWriter rtcmStream;

    public GpsLogger(string logDir, string fileName)
	{
        if (logDir.Length == 0)
        {
            this.logDir = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%/DSS/logs/ClientRtkGps");
        } else
        {
            this.logDir = Environment.ExpandEnvironmentVariables(logDir);
        }
        this.rtcmStream = new StreamWriter(new FileStream(getFileName(this.logDir, fileName), FileMode.Append));
    }


    public void Write(byte[]  data, int size, bool result)
    {
        string dateTime = DateTime.UtcNow.ToString("HH:mm:ss.fff") + "   ";
        if (!result)
        {
            dateTime += "ERROR   ";
        }

        rtcmStream.Write(dateTime);
        rtcmStream.WriteLine(BitConverter.ToString(data, 0, size).Replace("-",""));

        rtcmStream.Flush();
    }

    private string getFileName(string logdir, string filename)
    {        
        if (!Directory.Exists(logdir))
        {
            Directory.CreateDirectory(logdir);            
        }
        return logdir + "/" + filename;
    }
}
