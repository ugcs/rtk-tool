using System;
using System.IO;
using System.IO.Ports;

namespace MissionPlanner.Comms
{
    public class CommsLog : CommsBase, ICommsSerial
    {
        private TimeSpan timeOffset = TimeSpan.MinValue;
        private int currentLineIndex = -1;
        private DateTime currentLineDateTime = DateTime.MinValue;
        private byte[] currentLine = Array.Empty<byte>();
        private int positionInCurrentLine;
        private StreamReader textStream;
        
        // Properties
        public Stream BaseStream { get; private set; }

        public int BaudRate { get; set; }

        public int BytesToRead
        { get 
            { 
                if (!BaseStream.CanRead) return 0;
                // too early to sen something
                var tillNextLine = (currentLineDateTime + timeOffset - DateTime.Now).TotalMilliseconds;
                if (tillNextLine > 0) return 0;
                return currentLine.Length - positionInCurrentLine;
            }
        }

        public int BytesToWrite { get; set; }

        public int DataBits { get; set; }

        public bool DtrEnable { get; set; }

        public bool IsOpen
        {
            get { return BaseStream != null && BaseStream.CanRead; }
        }

        public Parity Parity { get; set; }

        public string PortName { get; set; }

        public int ReadBufferSize { get; set; }

        public int ReadTimeout { get; set; }

        public bool RtsEnable { get; set; }

        public StopBits StopBits { get; set; }

        public int WriteBufferSize { get; set; }

        public int WriteTimeout { get; set; }

        // Methods
        public void Close()
        { BaseStream.Dispose(); }

        public void DiscardInBuffer()
        { }

        //void DiscardOutBuffer();
        public void Open(string filename)
        {
            PortName = filename;
            textStream = File.OpenText(PortName);
            BaseStream = textStream.BaseStream;
            if (IsOpen)
            {
                if (currentLine != null)
                {
                    var t = GetNextLine();
                    currentLineDateTime = t.Item1;
                    currentLine = t.Item2;
                    timeOffset = DateTime.Now - currentLineDateTime;
                    currentLineIndex = 0;
                }
            }
        }

        private byte[] StringToByteArray(String hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private Tuple<DateTime, byte[]> GetNextLine()
        {
            var len = 0;
            DateTime d = DateTime.MaxValue;
            byte[] s = Array.Empty<byte>();
            
            while (len == 0)
            {
                var l = textStream.ReadLine();

                if (l != null)
                {
                    l = l.Replace("   ", " ");
                    var a = l.Split("   ".ToCharArray());
                    if (a.Length == 2)
                    {
                        var dParsed = DateTime.ParseExact(a[0], "HH:mm:ss.fff", null);
                        d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, dParsed.Hour, dParsed.Minute, dParsed.Second);
                        d = d.AddMilliseconds(dParsed.Millisecond);
                        s = StringToByteArray(a[1]);
                        len = s.Length;
                    }
                }
                else
                {
                    break;
                }
            }

            return new Tuple<DateTime, byte[]>(d, s);
        }

     

        public void Open()
        {
            Open(PortName);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (!IsOpen)
                throw new EndOfStreamException("File not open");
            
            if (currentLineIndex < 0)
                throw new EndOfStreamException("File is corrupt");

            Array.Copy(currentLine, positionInCurrentLine, buffer, 0, count);

            positionInCurrentLine += count;

            if (positionInCurrentLine >= currentLine.Length)
            {
                var t = GetNextLine();
                currentLineDateTime = t.Item1;
                currentLine = t.Item2;
                currentLineIndex++;
                positionInCurrentLine = 0;
            }

            return count;
        }

        //int Read(char[] buffer, int offset, int count);
        public int ReadByte()
        { return BaseStream.ReadByte(); }

        public int ReadChar()
        { return BaseStream.ReadByte(); }

        public string ReadExisting()
        { return ""; }

        public string ReadLine()
        { return ""; }

        public void toggleDTR()
        { }

        //string ReadTo(string value);
        public void Write(string text)
        { }

        public void Write(byte[] buffer, int offset, int count)
        { }

        //void Write(char[] buffer, int offset, int count);
        public void WriteLine(string text)
        { }
    }
}