using MavLink;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MissionPlanner.Utilities;
using log4net;
using static MAVLink;
using ClientRtkGps.Properties;

namespace ClientRtkGps
{
    enum MavMsgsPackedType
    {
        GPS_RTCM_DATA,
        GPS_INJECT_DATA,
        DATA96
    }

    abstract class AbstractSink
    {
      
        public bool Enabled { get; set; }
        protected DateTime lastTimeSent;
        protected DateTime lastReconnectAttempt;
        private double RECONNECT_DELAY = 2;
        protected static ILog log = LogManager.GetLogger(typeof(AbstractSink).FullName);
        private object innerlock = new object();

        protected void Connect()
        {
            lock (innerlock)
            {
                DoDisconnect();
                DoConnect();
                Enabled = true;
            }
        }

        public void Disconnect()
        {
            lock (innerlock)
            {
                DoDisconnect();
                Enabled = false;
            }
        }

        public void Send(byte[]  data)
        {
            lock (innerlock)
            {
                try
                {
                    // Do not send data while we trying to reconnect
                    if (DateTime.Now - lastReconnectAttempt < TimeSpan.FromSeconds(RECONNECT_DELAY))
                        return;

                    DoSend(data);
                    lastTimeSent = DateTime.Now;
                    
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (DateTime.Now - lastReconnectAttempt > TimeSpan.FromSeconds(RECONNECT_DELAY))
                    {
                        log.Warn("Reconnecting...");

                        lastReconnectAttempt = DateTime.Now; // Reset stopwatch to avoid endless Reconnection
                        try
                        {
                            DoDisconnect();
                            DoConnect();
                        } catch (Exception exrec)
                        {
                            log.Error("Failed to reconnect.", exrec);
                        }
                    }
                }
            }
        }

        protected abstract void DoConnect();
        protected abstract void DoSend(byte[] data);
        protected abstract void DoDisconnect();              
    }

    class SerialSink : AbstractSink
    {
        private SerialPort serialSink;
        private string port;
        private int baudRate;

        public void Enable(string port, int baudRate)
        {
            this.port = port;
            this.baudRate = baudRate;
            Connect();
        }

        protected override void DoConnect()
        {
            serialSink = new SerialPort(port);
            serialSink.BaudRate = baudRate;
            serialSink.Open();
            log.Info("Connected to Serial " + port + ":" + baudRate);
        }

        protected override  void DoDisconnect()
        {
            if (serialSink != null)
            {                
                serialSink.Close();
                serialSink = null;
            }
        }

        protected override void DoSend(byte[] data)
        {
            if (serialSink != null)
            {
                serialSink.Write(data, 0, data.Length);             
            }
        }
    }

    class UdpSink : AbstractSink
    {
        private Socket udpSink;
        private IPEndPoint endPoint;
        private IPEndPoint localEndPoint;
        private string host;
        private int port;
        private int localPort;

        public void Enable(string host, int port)
        {
            this.host = host;
            this.port = port;
            this.localPort = Settings.Default.UdpLocalPort; 
            Connect();            
        }

        protected override void DoConnect()
        {
            udpSink = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endPoint = new IPEndPoint(IPAddress.Parse(host), port);
            localEndPoint = new IPEndPoint(IPAddress.Any, localPort);
            
            udpSink.Bind(localEndPoint);
            
            log.Info("Connected to Udp " + host + ":" + port);
        }

        protected override void DoDisconnect()
        {
            if (udpSink != null)
            {
                udpSink.Close();
                udpSink = null;
            }
        }

        protected override void DoSend(byte[] data)
        {
            if (udpSink != null)
            {
                udpSink.SendTo(data, endPoint);
            }
        }
    }

    class TcpClientSink : AbstractSink
    {
        private Socket tcpClientSink;
        private string host;
        private int port;

        public void Enable(string host, int port)
        {
            this.host = host;
            this.port = port;
            Connect();
        }

        protected override void DoDisconnect()
        {
            if (tcpClientSink != null)
            {
                tcpClientSink.Close();
                tcpClientSink = null;
            }
        }

        protected override void DoConnect()
        {
            tcpClientSink = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpClientSink.Connect(host, port);
            log.Info("Connected to Tcp " + host + ":" + port);
        }

        protected override void DoSend(byte[] data)
        {
            if (tcpClientSink != null)
            {
                tcpClientSink.Send(data);
            }
        }
    }

    class TcpServerSink : AbstractSink
    {
        private CancellationTokenSource tcpCts;
        private Task tcpServerTask;
        private LinkedList<TcpClient> tcpServerClients = new LinkedList<TcpClient>();
        private int port;
        private TcpListener tcpServerSink;
        private object tcpServerLocker = new object();

        public void Enable(int port)
        {
            this.port = port;
            Connect();
        }


        protected override void DoConnect()
        {
            tcpServerSink = new TcpListener(IPAddress.Any, port);
            tcpServerSink.Start();

            tcpCts = new CancellationTokenSource();
            var token = tcpCts.Token;
            tcpServerTask = Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        TcpClient client = tcpServerSink.AcceptTcpClient();
                        lock (tcpServerLocker)
                        {
                            tcpServerClients.AddLast(client);

                            var current = tcpServerClients.First;
                            while (current != null)
                            {
                                var next = current.Next;
                                try
                                {
                                    if (!current.Value.Connected)
                                    {
                                        tcpServerClients.Remove(client);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                }

                                current = next;
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.Interrupted)
                        { }
                        else
                        {
                            log.Error(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }, token);
            log.Info("Connected as Server on " + port);
        }

        protected override void DoDisconnect()
        {
            if (tcpServerSink != null)
            {
                foreach (var client in tcpServerClients)
                {
                    try
                    {
                        client.Close();
                        client.Dispose();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                tcpServerClients.Clear();
                tcpServerSink.Stop();

                if (tcpServerTask != null)
                {
                    tcpCts.Cancel();
                    try
                    {
                        tcpServerTask.Wait(5000);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    finally
                    {
                        tcpCts.Dispose();
                    }
                }

                tcpCts = null;
                tcpServerTask = null;
            }
        }

        protected override void DoSend(byte[] data)
        {
            if (tcpServerSink != null)
            {
                var client = tcpServerClients.First;
                while (client != null)
                {
                    var next = client.Next;
                    try
                    {
                        if (!client.Value.Connected)
                        {
                            tcpServerClients.Remove(client);
                        }
                        else
                        {
                            client.Value.GetStream().Write(data, 0, data.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    client = next;
                }
            }
        }
    }

    class RtkTransmitter
    {
        private SerialSink serialSink = new SerialSink();
        private UdpSink udpSink = new UdpSink();
        private TcpClientSink tcpClientSink = new TcpClientSink();
        private TcpServerSink tcpServerSink = new TcpServerSink();
        private List<AbstractSink> sinks;

        // we will put this copmonents id to mavlink messages to
        // separate channels onboard
        private byte wifiCompIdSettings = 25;
        private byte serialCompIdSettings = 26;

        private static ILog log = LogManager.GetLogger(typeof(RtkTransmitter).FullName);

        public void SetCompIdSettings(byte wifiCompId, byte serialCompId)
        {
            wifiCompIdSettings = wifiCompId;
            serialCompIdSettings = serialCompId;
        }
        
        public RtkTransmitter()
        {
            sinks = new List<AbstractSink> { serialSink, udpSink, tcpClientSink, tcpServerSink };
        }

        public void SetTcpServerSink(int port)
        {          
            tcpServerSink.Enable(port);           
        }      

        public void DisableTcpServerSink()
        {
            tcpServerSink.Disconnect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception">Various serial port exceptions</exception>
        /// <param name="serialPortName"></param>
        public void SetSerialSink(string serialPortName, int baudRate)
        {
            serialSink.Enable(serialPortName, baudRate);
        }

        public void DisableSerialSink()
        {
            serialSink.Disconnect();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception">Various network/parsing exceptions</exception>
        /// <param name="host"></param>
        public void SetTcpClientSink(string host, int port)
        {
            tcpClientSink.Enable(host, port);
        }

        public void DisableTcpClientSink()
        {
            tcpClientSink.Disconnect();           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception">Various network/parsing exceptions</exception>
        /// <param name="host"></param>
        public void SetUdpClientSink(string host, int port)
        {
            udpSink.Enable(host, port);
        }

        public void DisableUdpClientSink()
        {
            udpSink.Disconnect();
        }

        private const int SYSTEM_ID = 255;

        private Mavlink mav = new Mavlink();
        private byte mavMsg = 0;

        public DateTime lastTimeSent;
        public DateTime LastTimeSet {
             get { return this.lastTimeSent; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception">Various IO exceptions</exception>
        /// <param name="data"></param>
        /// <param name="length"></param>
        private void Transmit(MavlinkMessage message)
        {
            MavlinkPacket packet = new MavlinkPacket();
            packet.Message = message;
            packet.SequenceNumber = mavMsg++;
            packet.SystemId = SYSTEM_ID;
            packet.ComponentId = wifiCompIdSettings;
            packet.TimeStamp = DateTime.Now;
            // TODO: systemid, componentid (?)
            byte[] serializedMessageWiFi = mav.Send(packet); // mav.Serialize(message, 0, 0);

            packet.ComponentId = serialCompIdSettings;
            byte[] serializedMessageSerial = mav.Send(packet);

            Parallel.ForEach(sinks, sink => { 
                lock(sink)
                {
                    if (sink.Enabled)
                    {
                        if (sink.GetType() == typeof(SerialSink))
                        {
                            sink.Send(serializedMessageSerial);
                        }
                        else
                        {
                            sink.Send(serializedMessageWiFi);
                        }
                        lastTimeSent = DateTime.Now;
                    }
                }
            });          
        }

        int rtcmSequenceNumber = 0;

        /*
        public void InjectGpsData(byte sysid, byte compid, byte[] data, ushort length, bool rtcm_message = true)
        {
            // new message
            if (rtcm_message)
            {
                mavlink_gps_rtcm_data_t gps = new mavlink_gps_rtcm_data_t();
                var msglen = 180;

                // TODO: if (length > msglen * 4)
                //log.Error("Message too large " + length);

                // number of packets we need, including a termination packet if needed
                var nopackets = (length % msglen) == 0 ? length / msglen + 1 : (length / msglen) + 1;

                if (nopackets >= 4)
                    nopackets = 4;

                // flags = isfrag(1)/frag(2)/seq(5)

                for (int a = 0; a < nopackets; a++)
                {
                    // check if its a fragment
                    if (nopackets > 1)
                        gps.flags = 1;
                    else
                        gps.flags = 0;

                    // add fragment number
                    gps.flags += (byte)((a & 0x3) << 1);

                    // add seq number
                    gps.flags += (byte)((rtcmSequenceNumber & 0x1f) << 3);

                    // create the empty buffer
                    gps.data = new byte[msglen];

                    // calc how much data we are copying
                    int copy = Math.Min(length - a * msglen, msglen);

                    // copy the data
                    Array.Copy(data, a * msglen, gps.data, 0, copy);

                    // set the length
                    gps.len = (byte)copy;

                    ...
                    //generatePacket((byte)MAVLINK_MSG_ID.GPS_RTCM_DATA, gps, sysid, compid);
                }

                rtcmSequenceNumber++;
            }
            else
            {
                mavlink_gps_inject_data_t gps = new mavlink_gps_inject_data_t();
                var msglen = 110;

                var len = (length % msglen) == 0 ? length / msglen : (length / msglen) + 1;

                for (int a = 0; a < len; a++)
                {
                    gps.data = new byte[msglen];

                    int copy = Math.Min(length - a * msglen, msglen);

                    Array.Copy(data, a * msglen, gps.data, 0, copy);
                    gps.len = (byte)copy;
                    gps.target_component = compid;
                    gps.target_system = sysid;

                    ...
                    //generatePacket((byte)MAVLINK_MSG_ID.GPS_INJECT_DATA, gps, sysid, compid);
                }
            }
        }
        */

        /*
        void generatePacket(int messageType, object indata, int sysid, int compid, bool forcemavlink2 = false, bool forcesigning = false)
        {
            
                byte[] data = MavlinkUtil.StructureToByteArray(indata);
                byte[] packet = new byte[0];
                int i = 0;
            
                    // trim packet for mavlink2
                    MavlinkUtil.trim_payload(ref data);

                    packet = new byte[data.Length + MAVLINK_NUM_HEADER_BYTES + MAVLINK_NUM_CHECKSUM_BYTES + MAVLINK_SIGNATURE_BLOCK_LEN];

                    packet[0] = MAVLINK_STX;
                    packet[1] = (byte)data.Length;
                    packet[2] = 0; // incompat
                    if (MAVlist[sysid, compid].signing || forcesigning) // current mav
                        packet[2] |= MAVLINK_IFLAG_SIGNED;
                    packet[3] = 0; // compat
                    packet[4] = (byte)packetcount;

                    packetcount++;

                    packet[5] = gcssysid;
                    packet[6] = (byte)MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER;
                    packet[7] = (byte)(messageType & 0xff);
                    packet[8] = (byte)((messageType >> 8) & 0xff);
                    packet[9] = (byte)((messageType >> 16) & 0xff);

                    i = 10;
                    foreach (byte b in data)
                    {
                        packet[i] = b;
                        i++;
                    }

                    ushort checksum = MavlinkCRC.crc_calculate(packet, packet[1] + MAVLINK_NUM_HEADER_BYTES);

                    checksum = MavlinkCRC.crc_accumulate(MAVLINK_MESSAGE_INFOS.GetMessageInfo((uint)messageType).crc, checksum);

                    byte ck_a = (byte)(checksum & 0xFF); ///< High byte
                    byte ck_b = (byte)(checksum >> 8); ///< Low byte

                    packet[i] = ck_a;
                    i += 1;
                    packet[i] = ck_b;
                    i += 1;

                    if (MAVlist[sysid, compid].signing || forcesigning)
                    {
                        //https://docs.google.com/document/d/1ETle6qQRcaNWAmpG2wz0oOpFKSF_bcTmYMQvtTGI8ns/edit

                        
                        // 8 bits of link ID
                        // 48 bits of timestamp
                        // 48 bits of signature
                        

                        // signature = sha256_48(secret_key + header + payload + CRC + link-ID + timestamp)

                        var timestamp = (UInt64)((DateTime.UtcNow - new DateTime(2015, 1, 1)).TotalMilliseconds * 100);

                        if (timestamp == MAVlist[sysid, compid].timestamp)
                            timestamp++;

                        MAVlist[sysid, compid].timestamp = timestamp;

                        var timebytes = BitConverter.GetBytes(timestamp);

                        var sig = new byte[7]; // 13 includes the outgoing hash
                        sig[0] = MAVlist[sysid, compid].sendlinkid;
                        Array.Copy(timebytes, 0, sig, 1, 6); // timestamp

                        //Console.WriteLine("gen linkid {0}, time {1} {2} {3} {4} {5} {6} {7}", sig[0], sig[1], sig[2], sig[3], sig[4], sig[5], sig[6], timestamp);

                        var signingKey = MAVlist[sysid, compid].signingKey;

                        if (signingKey == null || signingKey.Length != 32)
                        {
                            signingKey = new byte[32];
                        }

                        using (SHA256Managed signit = new SHA256Managed())
                        {
                            signit.TransformBlock(signingKey, 0, signingKey.Length, null, 0);
                            signit.TransformBlock(packet, 0, i, null, 0);
                            signit.TransformFinalBlock(sig, 0, sig.Length);
                            var ctx = signit.Hash;
                            // trim to 48
                            Array.Resize(ref ctx, 6);

                            foreach (byte b in sig)
                            {
                                packet[i] = b;
                                i++;
                            }

                            foreach (byte b in ctx)
                            {
                                packet[i] = b;
                                i++;
                            }
                        }
                    }
        }
        */

        public void Send(byte[] data, int length, MavMsgsPackedType mavType)
        {
            if (mavType == MavMsgsPackedType.GPS_RTCM_DATA) {

                Msg_gps_rtcm_data message = new Msg_gps_rtcm_data();
                const byte RTCM_MSGLEN = 180;

                // TODO: handle ... log.Error("Message too large " + length);
                //if (length > msglen * 4)
                //{
                //    Console.Write("Message too large " + length);
                //}

                // number of packets we need, including a termination packet if needed
                var nopackets = (length % RTCM_MSGLEN) == 0 ? length / RTCM_MSGLEN + 1 : (length / RTCM_MSGLEN) + 1;

                if (nopackets >= 4)
                    nopackets = 4;

                for (int a = 0; a < nopackets; a++)
                {
                    message.flags = 0;
                    message.flags |= (byte)((nopackets > 1) ? 1 : 0);
                    message.flags |= (byte)((a & 0x3) << 1);
                    message.flags |= (byte)((rtcmSequenceNumber & 0x1f) << 3);
                    
                    // calc how much data we are copying
                    var copy = (byte)Math.Min(length - a * RTCM_MSGLEN, RTCM_MSGLEN);
                    message.len = copy;

                    message.data = new byte[RTCM_MSGLEN];
                    // copy the data
                    Array.Copy(data, a * RTCM_MSGLEN, message.data, 0, copy);
                    
                    Transmit(message);
                }

                rtcmSequenceNumber++;
            }
            else if (mavType == MavMsgsPackedType.GPS_INJECT_DATA)
            {
                Msg_gps_inject_data message = new Msg_gps_inject_data();
                const byte GPS_MSGLEN = 110;

                var len = (length % GPS_MSGLEN) == 0 ? length / GPS_MSGLEN : (length / GPS_MSGLEN) + 1;

                for (int a = 0; a < len; a++)
                {
                    message.data = new byte[GPS_MSGLEN];

                    int copy = Math.Min(length - a * GPS_MSGLEN, GPS_MSGLEN);

                    Array.Copy(data, a * GPS_MSGLEN, message.data, 0, copy);
                    message.len = (byte)copy;
                    message.target_component = 0; // TODO: compid;
                    message.target_system = 0; // TODO: sysid;

                    Transmit(message);
                }
            } else if (mavType == MavMsgsPackedType.DATA96)
            {
                Msg_data96 message = new Msg_data96();
                const byte DATA96_MSGLEN = 90;

                // get station id
                uint stationId = rtcm3.getbitu(data, 36, 12); 

                // number of packets we need, including a termination packet if needed
                var nopackets = (length % DATA96_MSGLEN) == 0 ? length / DATA96_MSGLEN: (length / DATA96_MSGLEN) + 1;

                for (int a = 0; a < nopackets; a++)
                {
                    message.type = 34;
                    message.len = 96;

                    // calc how much data we are copying
                    var copy = (byte)Math.Min(length - a * DATA96_MSGLEN, DATA96_MSGLEN);
                    message.len = copy;

                    message.data = new byte[DATA96_MSGLEN + 6];
                    // copy the data
                    Array.Copy(data, a * DATA96_MSGLEN, message.data, 6, copy);

                    // add rtcm station id (first three bytes)
                    rtcm3.setbitu(message.data, 0, 24, stationId);

                    // add rtcm seq (fourth byte)
                    rtcm3.setbitu(message.data, 24, 8, ((uint)rtcmSequenceNumber & 0xff));

                    // add current chucnk number (fifth byte)
                    rtcm3.setbitu(message.data, 32, 8, ((uint)a & 0xff));

                    // add chuncks count (six byte)
                    rtcm3.setbitu(message.data, 40, 8, ((uint)nopackets & 0xff));

                    Transmit(message);
                }

                rtcmSequenceNumber++;

            }
        }
    }
}
