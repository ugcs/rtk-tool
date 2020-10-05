using ClientRtkGps.Properties;
using MissionPlanner.Comms;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flurl.Util;
using log4net;
using log4net.Config;

namespace ClientRtkGps
{
    public partial class RtkForm : Form
    {
        // serialport
        private ICommsSerial comPort;// = new SerialPort();
        // rtcm detection
        private rtcm3 rtcm3 = new rtcm3();
        // sbp detection
        private sbp sbp = new sbp();
        // ubx detection
        private ubx_m8p ubx_m8p = new ubx_m8p();

        private nmea nmea = new nmea();
        private bool threadrun = false;
        // track rtcm msg's seens
        private Dictionary<string, int> msgseen = new Dictionary<string, int>();
        // track bytes seen
        private static int bytes = 0;
        private static int bps = 0;
        private int bpsusefull = 0;

        private int prevRate;
        private int radioLinkRate;

        private MavMsgsPackedType rtcm_msg = MavMsgsPackedType.GPS_RTCM_DATA;

        private bool override_id = false;

        private MyPointLatLngAlt basepos = MyPointLatLngAlt.Zero;
        private MyPointLatLngAlt MovingBase = MyPointLatLngAlt.Zero;

        private string status_line3;

        private string basepostlistfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ClientRtkGps", "baseposlist.xml");

        private RtkTransmitter transmitter;
        private List<NamedPointLatLngAlt> baseposList = new List<NamedPointLatLngAlt>();
        private Dictionary<SourceType, string> sourceSettingsDictionary = new Dictionary<SourceType, string>();

        private SatelliteSignalStrengthAdapter signalAdapter;

        private static ILog log = LogManager.GetLogger(typeof(RtkForm).FullName);

        public RtkForm()
        {
            XmlConfigurator.Configure();

            log.Info("Starting...");
            InitializeComponent();

            signalAdapter = new SatelliteSignalStrengthAdapter(panel1);

            setSerialItems(sourceSelectorComboBox);
            sinkSelectorComboBox.Items.AddRange(SerialPort.GetPortNames());

            transmitter = new RtkTransmitter();

            mavMsgTypeComboBox.SelectedIndex = (int)rtcm_msg;

            loadBasePosList();
            loadBasePOS();

            rtcm3.ObsMessage += Rtcm3_ObsMessage;

            sourceSettingsDictionary.Add(SourceType.FILE, "capture.txt");

            LoadSettings();
        }

        private void SetComboStringItem(ComboBox comboBox, string name)
        {
            for (int i = 0; i < comboBox.Items.Count; ++i)
            {
                var item = comboBox.Items[i]?.ToString();
                if (item != null && item.Equals(name))
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void LoadSettings()
        {
            var settings = Settings.Default;
            settings.Reload();

            radioLinkCheckBox.Checked = settings.RadioLink;

            SetComboStringItem(sourceSelectorComboBox, settings.SourceType);
            SetComboStringItem(sourceBaudRateComboBox, settings.SourceBaudRate.ToString());
            sourceSpecificTextBox.Text = settings.SourceSpecificText;
            mavMsgTypeComboBox.SelectedIndex = settings.InjectMsgType;


            SetComboStringItem(sinkSelectorComboBox, settings.SerialSinkName);
            SetComboStringItem(sinkBaudRateComboBox, settings.SinkBaudRate.ToString());
            serialCheckBox.Checked = settings.UseSerialSink;


            udpClientTextBox.Text = settings.UdpHost;
            udpClientPortTextBox.Text = settings.UdpPort.ToString();
            udpClientCheckBox.Checked = settings.UseUdpSink;

            tcpClientTextBox.Text = settings.TcpClientHost;
            tcpClientPortTextBox.Text = settings.TcpClientPort.ToString();
            tcpClientCheckBox.Checked = settings.UseTcpClientSink;

            tcpServerTextBox.Text = settings.TcpServerHost;
            tcpServerPortTextBox.Text = settings.TcpServerPort.ToString();
            tcpServerCheckBox.Checked = settings.UseTcpServerSink;

            radioLinkRate = settings.RadioLinkBaud;

           
            m8pCheckBox.Checked = settings.M8pAutoconfig;
            m8pFw130CheckBox.Checked = settings.M8pFW130;
            movingBaseCheckBox.Checked = settings.MovingBase;
            accTextBox.Text = settings.SurveyInAcc.ToString();
            timeTextBox.Text = settings.M8pTime.ToString();

            SaveSettings();
        }

        private void SaveSettings()
        {
            log.Info("Loaded settings...");
            var settings = Settings.Default;
            int intValue;
            double doubleValue;

            settings.SourceType = sourceSelectorComboBox.SelectedItem?.ToString();
            intValue = 0;
            int.TryParse(sourceBaudRateComboBox.SelectedItem?.ToString(), out intValue);
            settings.SourceBaudRate = intValue;
            settings.SourceSpecificText = sourceSpecificTextBox.Text;
            settings.InjectMsgType = mavMsgTypeComboBox.SelectedIndex;

            settings.SerialSinkName = sinkSelectorComboBox.SelectedItem?.ToString();
            intValue = 0;
            int.TryParse(sinkBaudRateComboBox.SelectedItem?.ToString(), out intValue);
            settings.SinkBaudRate = intValue;
            settings.UseSerialSink = serialCheckBox.Checked;

            settings.UdpHost = udpClientTextBox.Text;
            intValue = 0;
            int.TryParse(udpClientPortTextBox.Text, out intValue);
            settings.UdpPort = intValue;
            settings.UseUdpSink = udpClientCheckBox.Checked;

            settings.TcpClientHost = tcpClientTextBox.Text;
            intValue = 0;
            int.TryParse(tcpClientPortTextBox.Text, out intValue);
            settings.TcpClientPort = intValue;
            settings.UseTcpClientSink = tcpClientCheckBox.Checked;

            settings.TcpServerHost = tcpServerTextBox.Text;
            intValue = 0;
            int.TryParse(tcpServerPortTextBox.Text, out intValue);
            settings.TcpServerPort = intValue;
            settings.UseTcpServerSink = tcpServerCheckBox.Checked;

            settings.M8pAutoconfig = m8pCheckBox.Checked;
            settings.RadioLink = radioLinkCheckBox.Checked;
            settings.M8pFW130 = m8pFw130CheckBox.Checked;
            settings.MovingBase = movingBaseCheckBox.Checked;
            doubleValue = 0;
            double.TryParse(accTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out doubleValue);
            settings.SurveyInAcc = doubleValue;
            intValue = 0;
            int.TryParse(timeTextBox.Text, out intValue);
            settings.M8pTime = intValue;

            if (radioLinkCheckBox.Checked)
            {
                intValue = 0;
                int.TryParse(sourceBaudRateComboBox.Text, out intValue);
                settings.RadioLinkBaud = intValue;
            }

            settings.Save();
        }

        private void Rtcm3_ObsMessage(object sender, EventArgs e)
        {
            if (IsDisposed)
                threadrun = false;

            BeginInvoke((MethodInvoker)delegate
            {
                List<rtcm3.ob> obs = sender as List<rtcm3.ob>;

                if (obs.Count == 0) return;
                    
                int gpsCount = 0;
                int glonassCount = 0;
                int beidouCount = 0;
                int galileoCount = 0;
                

                foreach (var ob in obs)
                {
                    ProgressBar progressBar = null;

                    switch (ob.sys)
                    {
                        case 'G':
                            ++gpsCount;
                            break;
                        case 'R':
                            ++glonassCount;
                            break;
                        case 'B':
                            ++beidouCount;
                            break;
                        case 'E':
                            ++galileoCount;
                            break;
                    }

                    if (progressBar != null)
                    {
                        progressBar.Value = ob.snr;
                    }
                }

                SatelliteType type;
                switch (obs[0].sys)
                {
                    case 'G':
                    default:
                        type = SatelliteType.GPS;
                        labelgps.Text = gpsCount.ToString();
                        break;
                    case 'R':
                        type = SatelliteType.Glonass;
                        labelglonass.Text = glonassCount.ToString();
                        break;
                    case 'B':
                        type = SatelliteType.Beidou;
                        label14BDS.Text = beidouCount.ToString();
                        break;
                    case 'E':
                        type = SatelliteType.Galileo;
                        label16Galileo.Text = galileoCount.ToString();
                        break;
                }

                var signals = obs.Select((ob) => new SatelliteSignal()
                {
                    prn = ob.prn,
                    value = ob.snr
                });

                signalAdapter.SetSignals(type, signals.ToList());
            }
            );
        }

        void loadBasePosList()
        {
            if (File.Exists(basepostlistfile))
            {
                //load config
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(List<NamedPointLatLngAlt>), new Type[] { typeof(Color) });

                using (StreamReader sr = new StreamReader(basepostlistfile))
                {
                    try
                    {
                        baseposList = (List<NamedPointLatLngAlt>)reader.Deserialize(sr);
                    }
                    catch (Exception ex)
                    {
                        // TODO: log
                        log.Error(ex);
                        MessageBox.Show("Failed to load Base Position List\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            updateBasePosDG();
        }

        void updateBasePosDG()
        {
            if (baseposList.Count == 0)
                return;

            //dont trigger on clear
            dg_basepos.RowsRemoved -= dg_basepos_RowsRemoved;
            dg_basepos.Rows.Clear();
            dg_basepos.RowsRemoved += dg_basepos_RowsRemoved;

            foreach (var pointLatLngAlt in baseposList)
            {
                dg_basepos.Rows.Add(pointLatLngAlt.Lat.ToString(CultureInfo.InvariantCulture), 
                    pointLatLngAlt.Lng.ToString(CultureInfo.InvariantCulture), 
                    pointLatLngAlt.Alt.ToString(CultureInfo.InvariantCulture), 
                    pointLatLngAlt.Name, 
                    "Use", "Delete");
            }

            saveBasePosList();
        }

        private delegate void TimeoutAction<T>(T obj);
        private Dictionary<object, System.Windows.Forms.Timer> Timeouts = new Dictionary<object, System.Windows.Forms.Timer>();

        private void SetTimeout<T>(T obj, int timeoutMs, TimeoutAction<T> action)
        {
            if (Timeouts.ContainsKey(obj))
            {
                Timeouts[obj].Stop();
            }
            else
            {
                Timeouts.Add(obj, new System.Windows.Forms.Timer());
            }

            var timer = Timeouts[obj];
            timer.Interval = timeoutMs;
            timer.Tick += (sender, e) =>
            {
                action(obj);
                timer.Stop();
                Timeouts.Remove(obj);
            };
            timer.Start();
        }

        private enum SourceType
        {
            SERIAL,
            UDP_HOST,
            UDP_CLIENT,
            TCP_CLIENT,
            NTRIP,
            FILE
        }

        private class SourceDescriptor
        {
            public string Name { get; set; }
            public SourceType Type { get; set; }

            public SourceDescriptor(string name, SourceType type)
            {
                Name = name;
                Type = type;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private void setSerialItems(ComboBox comboBox)
        {
            comboBox.Items.AddRange(SerialPort.GetPortNames().Select((port) => new SourceDescriptor(port, SourceType.SERIAL)).ToArray());
            comboBox.Items.Add(new SourceDescriptor("UDP Host", SourceType.UDP_HOST));
            comboBox.Items.Add(new SourceDescriptor("UDP Client", SourceType.UDP_CLIENT));
            comboBox.Items.Add(new SourceDescriptor("TCP Client", SourceType.TCP_CLIENT));
            comboBox.Items.Add(new SourceDescriptor("NTRIP", SourceType.NTRIP));
            comboBox.Items.Add(new SourceDescriptor("Dump File", SourceType.FILE));
        }

        private void RunMainLoop()
        {
            new Thread(() =>
            {
                DateTime lastrecv = DateTime.MinValue;
                threadrun = true;

                bool isrtcm = false;
                bool issbp = false;

                int reconnecttimeout = 10;

                while (threadrun)
                {
                    try
                    {
                        // reconnect logic - 10 seconds with no data, or comport is closed
                        try
                        {
                            if ((DateTime.Now - lastrecv).TotalSeconds > reconnecttimeout || !comPort.IsOpen)
                            {
                                if (comPort is CommsNTRIP || comPort is UdpSerialConnect || comPort is UdpSerial)
                                {

                                }
                                else
                                {
                                    log.Warn("Reconnecting");
                                    // close existing
                                    comPort.Close();
                                    // reopen
                                    comPort.Open();
                                }
                                // reset timer
                                lastrecv = DateTime.Now;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Failed to reconnect", ex);
                            // sleep for 10 seconds on error
                            System.Threading.Thread.Sleep(10000);
                        }

                        byte[] buffer = new byte[1];

                        // limit to 180 byte packet if using rtcm_data msg
                        if (rtcm_msg == MavMsgsPackedType.GPS_RTCM_DATA)
                        {
                            buffer = new byte[180];
                        }
                        // limit to 110 byte packet if using inject_data msg
                        else if (rtcm_msg == MavMsgsPackedType.GPS_INJECT_DATA)
                        {
                            buffer = new byte[110];
                        }
                        // limit to 90 byte packet if using inject_data msg
                        else if (rtcm_msg == MavMsgsPackedType.DATA96)
                        {
                            buffer = new byte[90];
                        }

                        while (comPort.BytesToRead > 0)
                        {
                            int read = comPort.Read(buffer, 0, Math.Min(buffer.Length, comPort.BytesToRead));

                            if (read > 0)
                                lastrecv = DateTime.Now;

                            bytes += read;
                            bps += read;

                            // if this is raw data transport of unknown packet types
                            //if (!(isrtcm || issbp))
                            //    sendData(buffer, (byte)read);

                            // check for valid rtcm/sbp/ubx packets
                            for (int a = 0; a < read; a++)
                            {
                                int seenmsg = -1;
                                // rtcm
                                if ((seenmsg = rtcm3.Read(buffer[a])) > 0)
                                {
                                    sbp.resetParser();
                                    ubx_m8p.resetParser();
                                    nmea.resetParser();
                                    isrtcm = true;

                                    // if change RTK Station ID 
                                    if (override_id)
                                    {
                                        int new_id = 0;
                                        if (int.TryParse(overrideIDTextBox.Text, out new_id))
                                        {
                                            if (new_id >= 0 && new_id < 4096)
                                            {
                                                // tmp
                                                //uint iddd = (uint)(rtcm3.packet[3] << 4) + (uint)(rtcm3.packet[4] >> 4);
                                                //var crc = rtcm3.crc24.crc24q(rtcm3.packet, (uint)rtcm3.length - 3, 0);
                                                //var crcpacket = rtcm3.getbitu(rtcm3.packet, (uint)(rtcm3.length - 3) * 8, 24);
                                                // --tmp
                                                   
                                                // set new id                                            
                                                rtcm3.setbitu(rtcm3.packet, 36, 12, (uint)new_id);

                                                // calculate and update new CRC
                                                uint crc = rtcm3.crc24.crc24q(rtcm3.packet, (uint)rtcm3.length - 3, 0);
                                                rtcm3.setbitu(rtcm3.packet, (uint)(rtcm3.length - 3) * 8, 24, crc);
                                            }
                                        }
                                    }

                                    sendData(rtcm3.packet, (byte)rtcm3.length);
                                    bpsusefull += rtcm3.length;
                                    string msgname = "Rtcm" + seenmsg;
                                    if (!msgseen.ContainsKey(msgname))
                                        msgseen[msgname] = 0;
                                    msgseen[msgname] = (int)msgseen[msgname] + 1;

                                    ExtractBasePos(seenmsg);

                                        seenRTCM(seenmsg);
                                }

                                // sbp
                                if ((seenmsg = sbp.read(buffer[a])) > 0)
                                {
                                    rtcm3.resetParser();
                                    ubx_m8p.resetParser();
                                    nmea.resetParser();
                                    issbp = true;
                                    //sendData(sbp.packet, (byte)sbp.length);
                                    bpsusefull += sbp.length;
                                    string msgname = "Sbp" + seenmsg.ToString("X4");
                                    if (!msgseen.ContainsKey(msgname))
                                        msgseen[msgname] = 0;
                                    msgseen[msgname] = (int)msgseen[msgname] + 1;
                                }

                                // ubx
                                if ((seenmsg = ubx_m8p.Read(buffer[a])) > 0)
                                {
                                    rtcm3.resetParser();
                                    sbp.resetParser();
                                    nmea.resetParser();
                                    ProcessUBXMessage();
                                    string msgname = "Ubx" + seenmsg.ToString("X4");
                                    if (!msgseen.ContainsKey(msgname))
                                        msgseen[msgname] = 0;
                                    msgseen[msgname] = (int)msgseen[msgname] + 1;
                                }

                                // nmea
                                if ((seenmsg = nmea.Read(buffer[a])) > 0)
                                {
                                    rtcm3.resetParser();
                                    sbp.resetParser();
                                    ubx_m8p.resetParser();
                                    string msgname = "NMEA";
                                    if (!msgseen.ContainsKey(msgname))
                                        msgseen[msgname] = 0;
                                    msgseen[msgname] = (int)msgseen[msgname] + 1;
                                }
                            }
                        }
                        // for some RTK Bases (North) this original Sleep() missed messages 
                        //System.Threading.Thread.Sleep(10);

                        System.Threading.Thread.Sleep(1);
                    }
                    catch (Exception ex)
                    {
                        // TODO: log, handle
                        log.Error(ex);             
                    }
                }
            }).Start();
        }

        private void sendData(byte[] data, byte length)
        {
            transmitter.Send(data, length, rtcm_msg);
        }

        private void seenRTCM(int seenmsg)
        {
            var packet = new byte[4096];
            Array.Copy(rtcm3.packet, packet, packet.Length);

            BeginInvoke((Action)delegate ()
            {
                switch (seenmsg)
                {
                    case 1001:
                    case 1002:
                    case 1003:
                    case 1004:
                    case 1071:
                    case 1072:
                    case 1073:
                    case 1074:
                    case 1075:
                    case 1076:
                    case 1077:
                        labelgps.BackColor = Color.Green;
                        SetTimeout(labelgps, 5000, (Label obj) =>
                        {
                            obj.BackColor = Color.Red;
                            obj.Text = null;
                            signalAdapter.SetSignals(SatelliteType.GPS, null);
                        });
                        break;
                    case 1005:
                    case 1006:
                    case 4072: // ublox moving base
                        labelbase.BackColor = Color.Green;
                        SetTimeout(labelbase, 20000, (Label obj) =>
                        {
                            obj.BackColor = Color.Red;
                            obj.Text = null;
                        });
                        break;
                    case 1009:
                    case 1010:
                    case 1011:
                    case 1012:
                    case 1081:
                    case 1082:
                    case 1083:
                    case 1084:
                    case 1085:
                    case 1086:
                    case 1087:
                        labelglonass.BackColor = Color.Green;
                        SetTimeout(labelglonass, 5000, (Label obj) =>
                        {
                            obj.BackColor = Color.Red;
                            obj.Text = null;
                            signalAdapter.SetSignals(SatelliteType.Glonass, null);
                        });
                        break;
                    case 1092:
                    case 1093:
                    case 1094:
                    case 1095:
                    case 1096:
                    case 1097:
                        label16Galileo.BackColor = Color.Green;
                        SetTimeout(label16Galileo, 5000, (Label obj) =>
                        {
                            obj.BackColor = Color.Red;
                            obj.Text = null;
                            signalAdapter.SetSignals(SatelliteType.Galileo, null);
                        });
                        break;
                    case 1121:
                    case 1122:
                    case 1123:
                    case 1124:
                    case 1125:
                    case 1126:
                    case 1127:
                        // TODO: 1301-1307 ??? https://www.use-snip.com/kb/knowledge-base/an-rtcm-message-cheat-sheet/
                        label14BDS.BackColor = Color.Green;
                        SetTimeout(label14BDS, 5000, (Label obj) =>
                        {
                            obj.BackColor = Color.Red;
                            obj.Text = null;
                            signalAdapter.SetSignals(SatelliteType.Beidou, null);
                        });
                        /*
                         * Seems like there are MSM messages only for Beidou...
                         * See: https://github.com/tomojitakasu/RTKLIB/blob/master/src/rtcm.c
                        */
                        break;
                    default:
                        break;
                }
            }
            );
        }

        private void ExtractBasePos(int seen)
        {
            try
            {
                double? lat = null, lng = null, alt = null;

                if (seen == 1005)
                {
                    var basepos = new rtcm3.type1005();
                    basepos.Read(rtcm3.packet);

                    var pos = basepos.ecefposition;

                    double[] baseposllh = new double[3];

                    rtcm3.ecef2pos(pos, ref baseposllh);

                    lat = baseposllh[0] * rtcm3.R2D;
                    lng = baseposllh[1] * rtcm3.R2D;
                    alt = baseposllh[2];
                }
                else if (seen == 1006)
                {
                    var basepos = new rtcm3.type1006();
                    basepos.Read(rtcm3.packet);

                    var pos = basepos.ecefposition;

                    double[] baseposllh = new double[3];

                    rtcm3.ecef2pos(pos, ref baseposllh);

                    lat = baseposllh[0];
                    lng = baseposllh[1];
                    alt = baseposllh[2];
                }

                if (lat.HasValue && lng.HasValue && alt.HasValue)
                {
                    status_line3 =
                       (String.Format("{0} {1} {2} - {3}", lat, lng, alt, DateTime.Now.ToString("HH:mm:ss")));

                    savePositionButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);                
            }
        }

        /*
        public static TMavlinkPacket ByteArrayToStructure<TMavlinkPacket>(this byte[] bytearray, int startoffset = 6)
        where TMavlinkPacket : struct
        {
            return ReadUsingPointer<TMavlinkPacket>(bytearray, startoffset);
        }
        public static T ReadUsingPointer<T>(byte[] data, int startoffset) where T : struct
        {
            unsafe
            {
                fixed (byte* p = &data[startoffset])
                {
                    return (T)Marshal.PtrToStructure(new IntPtr(p), typeof(T));
                }
            }
        }
        */

        static DateTime pollTMODE = DateTime.MinValue;
        static ubx_m8p.ubx_cfg_tmode3 ubxmode;
        static ubx_m8p.ubx_nav_svin ubxsvin;
        internal static ubx_m8p.ubx_nav_velned ubxvelned;
        internal static ubx_m8p.ubx_nav_pvt ubxpvt;

        private void ProcessUBXMessage()
        {
            try
            {
                // survey in
                if (ubx_m8p.@class == 0x1 && ubx_m8p.subclass == 0x3b)
                {
                    var svin = ubx_m8p.packet.ByteArrayToStructure<ubx_m8p.ubx_nav_svin>(6);

                    ubxsvin = svin;

                    updateSVINLabel((svin.valid == 1), (svin.active == 1), svin.dur, svin.obs, svin.meanAcc / 10000.0);

                    var pos = svin.getECEF();

                    double[] baseposllh = new double[3];

                    rtcm3.ecef2pos(pos, ref baseposllh);

                    if (svin.valid == 1)
                    {
                        // TODO: examine why MP removed it
                        //MainV2.comPort.MAV.cs.MovingBase = new Utilities.PointLatLngAlt(baseposllh[0]*Utilities.rtcm3.R2D,
                        //baseposllh[1]*Utilities.rtcm3.R2D, baseposllh[2]);
                    }

                    // TODO: examine why MP removed it
                    //if (svin.valid == 1)
                    //ubx_m8p.turnon_off(comPort, 0x1, 0x3b, 0);
                }
                else if (ubx_m8p.@class == 0x1 && ubx_m8p.subclass == 0x7)
                {
                    var pvt = ubx_m8p.packet.ByteArrayToStructure<ubx_m8p.ubx_nav_pvt>(6);
                    if (pvt.fix_type >= 0x3 && (pvt.flags & 1) > 0)
                    {
                        MovingBase = new MyPointLatLngAlt(pvt.lat / 1e7, pvt.lon / 1e7, pvt.height / 1000.0);
                    }
                    ubxpvt = pvt;
                }
                else if (ubx_m8p.@class == 0x5 && ubx_m8p.subclass == 0x1)
                {
                    // TODO: log
                    //log.InfoFormat("ubx ack {0} {1}", ubx_m8p.packet[6], ubx_m8p.packet[7]);
                }
                else if (ubx_m8p.@class == 0x5 && ubx_m8p.subclass == 0x0)
                {
                    // TODO: log
                    //log.InfoFormat("ubx Nack {0} {1}", ubx_m8p.packet[6], ubx_m8p.packet[7]);
                }
                else if (ubx_m8p.@class == 0xa && ubx_m8p.subclass == 0x4)
                {
                    var ver = ubx_m8p.packet.ByteArrayToStructure<ubx_m8p.ubx_mon_ver>(6);//, ubx_m8p.length - 8);

                    Console.WriteLine("ubx mon-ver {0} {1}", ASCIIEncoding.ASCII.GetString(ver.hwVersion),
                        ASCIIEncoding.ASCII.GetString(ver.swVersion));

                    for (int a = 40 + 6; a < ubx_m8p.length - 2; a += 30)
                    {
                        var extension = ASCIIEncoding.ASCII.GetString(ubx_m8p.buffer, a, 30);
                        Console.WriteLine("ubx mon-ver {0}", extension);
                    }
                }
                else if (ubx_m8p.@class == 0xa && ubx_m8p.subclass == 0x9)
                {
                    var hw = ubx_m8p.packet.ByteArrayToStructure<ubx_m8p.ubx_mon_hw>(6);

                    Console.WriteLine("ubx mon-hw noise {0} agc% {1} jam% {2} jamstate {3}", hw.noisePerMS, (hw.agcCnt / 8191.0) * 100.0, (hw.jamInd / 256.0) * 100, hw.flags & 0xc);
                }
                else if (ubx_m8p.@class == 0x1 && ubx_m8p.subclass == 0x12)
                {
                    var velned = ubx_m8p.packet.ByteArrayToStructure<ubx_m8p.ubx_nav_velned>(6);

                    var time = (velned.iTOW - ubxvelned.iTOW) / 1000.0;

                    ubxvelned = velned;
                }
                else if (ubx_m8p.@class == 0xf5)
                {
                    // rtcm
                }
                else if (ubx_m8p.@class == 0x02)
                {
                    // rxm-raw
                }
                else if (ubx_m8p.@class == 0x06 && ubx_m8p.subclass == 0x71)
                {
                    // TMODE3
                    var tmode = ubx_m8p.packet.ByteArrayToStructure<ubx_m8p.ubx_cfg_tmode3>(6);

                    ubxmode = tmode;

                    // TODO: log
                    //log.InfoFormat("ubx TMODE3 {0} {1}", (ubx_m8p.ubx_cfg_tmode3.modeflags)tmode.flags, "");
                }
                else
                {
                    ubx_m8p.turnon_off(comPort, ubx_m8p.@class, ubx_m8p.subclass, 0);
                }

                if (pollTMODE < DateTime.Now)
                {
                    ubx_m8p.poll_msg(comPort, 0x06, 0x71);
                    pollTMODE = DateTime.Now.AddSeconds(30);

                    ubx_m8p.poll_msg(comPort, 0x0a, 0x4);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to process UBX message", ex);
            }
        }

        public class NamedPointLatLngAlt : MyPointLatLngAlt
        {
            public string Name;

            public NamedPointLatLngAlt()
            {

            }

            public NamedPointLatLngAlt(MyPointLatLngAlt point) : base(point)
            {
                
            }
        }

        private static MyPointLatLngAlt getMyPointLatLngAlt(ubx_m8p.ubx_cfg_tmode3 data)
        {
            if (data.flags == 2)
            {
                return new MyPointLatLngAlt
                {
                    Lat = data.ecefXorLat / 100.0 + data.ecefXOrLatHP * 0.0001,
                    Lng = data.ecefYorLon / 100.0 + data.ecefYOrLonHP * 0.0001,
                    Alt = data.ecefZorAlt / 100.0 + data.ecefZOrAltHP * 0.0001
                };
            }
            else if (data.flags == 258)
            {
                return new MyPointLatLngAlt
                {
                    Lat = data.ecefXorLat / 1e7 + data.ecefXOrLatHP / 1e9,
                    Lng = data.ecefYorLon / 1e7 + data.ecefYOrLonHP / 1e9,
                    Alt = data.ecefZorAlt / 100.0 + data.ecefZOrAltHP * 0.0001
                };
            }

            return null;
        }

        private void updateSVINLabel(bool valid, bool active, uint dur, uint obs, double acc)
        {
            BeginInvoke((MethodInvoker)delegate
           {
               if (basepos == MyPointLatLngAlt.Zero)
               {
                   lbl_svin1.Visible = true;
                   lbl_svin2.Visible = true;
                   lbl_svin3.Visible = true;
                   lbl_svin4.Visible = true;
                   lbl_svin5.Visible = true;

                   lbl_svin1.Text = valid ? "Postion is valid" : "Position is invalid";
                   if (valid)
                       lbl_svin1.BackColor = Color.Green;
                   else
                       lbl_svin1.BackColor = Color.Red;

                   if (!valid)
                   {
                       lbl_svin2.Text = active
                           ? "In Progress"
                           : "Complete";
                       lbl_svin3.Text = "Duration: " + dur;
                       lbl_svin4.Text = "Observations: " + obs;
                   }
                   else
                   {
                       double[] posllh = new double[3];

                       rtcm3.ecef2pos(ubxsvin.getECEF(), ref posllh);

                       lbl_svin2.Text = "Lat/X: " + posllh[0] * (180 / Math.PI);
                       lbl_svin3.Text = "Lng/Y: " + posllh[1] * (180 / Math.PI);
                       lbl_svin4.Text = "Alt/Z: " + posllh[2];
                       lbl_svin2.Visible = true;
                       lbl_svin3.Visible = true;
                       lbl_svin4.Visible = true;
                   }
                   lbl_svin5.Text = "Current Acc: " + acc;
               }
               else
               {
                   lbl_svin1.Visible = true;
                   lbl_svin1.Text = "Using " + (ubx_m8p.ubx_cfg_tmode3.modeflags)ubxmode.flags;
                   lbl_svin1.BackColor = Color.Green;
                   lbl_svin2.Visible = false;
                   lbl_svin3.Visible = false;
                   lbl_svin4.Visible = false;
                   var pnt = getMyPointLatLngAlt(ubxmode);
                   if (pnt != null)
                   {
                       lbl_svin2.Text = "Lat/X: " + pnt.Lat;
                       lbl_svin3.Text = "Lng/Y: " + pnt.Lng;
                       lbl_svin4.Text = "Alt/Z: " + pnt.Alt;
                       lbl_svin2.Visible = true;
                       lbl_svin3.Visible = true;
                       lbl_svin4.Visible = true;
                   }

                   lbl_svin5.Visible = false;
               }
           }
            );
        }

        private void ManageInterfaceInputsVisibility(SourceType type)
        {
            sourceBaudRateComboBox.Enabled = type == SourceType.SERIAL;
            sourceSpecificLabel.Visible = type != SourceType.SERIAL;
            sourceSpecificTextBox.Enabled = type != SourceType.SERIAL;
        }

        private void sourceSelectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceSelectorComboBox.SelectedIndex == -1 || sourceSelectorComboBox.SelectedItem == null)
                return;

            var descriptor = (SourceDescriptor)sourceSelectorComboBox.SelectedItem;

            ManageInterfaceInputsVisibility(descriptor.Type);

            if (sourceSettingsDictionary.TryGetValue(descriptor.Type, out string setting))
            {
                sourceSpecificTextBox.Text = setting;
            }
            else
            {
                sourceSpecificTextBox.Clear();
            }

            switch (descriptor.Type)
            {
                case SourceType.SERIAL:
                    break;
                case SourceType.FILE:
                    sourceSpecificLabel.Text = "Filename:";
                    break;
                case SourceType.NTRIP:
                    sourceSpecificLabel.Text = "NTRIP address:";
                    break;
                case SourceType.UDP_CLIENT:
                    sourceSpecificLabel.Text = "UDP address:";
                    break;
                case SourceType.UDP_HOST:
                    sourceSpecificLabel.Text = "UDP port:";
                    break;
                case SourceType.TCP_CLIENT:
                    sourceSpecificLabel.Text = "TCP address:";
                    break;
                default:
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateInormation();
        }

        private void updateInormation()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (var item in msgseen.Keys)
                {
                    sb.Append(item + "=" + msgseen[item] + " ");
                }
            }
            catch
            {
            }

            updateLabel(String.Format("{0,10} bps", bps),
                String.Format("{0,10} bps sent", bpsusefull), status_line3,
                sb.ToString());
            bps = 0;
            bpsusefull = 0;
        }

        private void updateLabel(string line1, string line2, string line3, string line4)
        {
            if (!this.IsDisposed)
            {
                this.BeginInvoke(
                    (MethodInvoker)
                        delegate
                        {
                            this.lbl_status1.Text = line1;
                            this.lbl_status2.Text = line2;
                            this.lbl_status3.Text = line3;
                            this.labelmsgseen.Text = line4;
                        }
                    );
            }
        }

        private void udpClientCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (udpClientCheckBox.Checked && Int32.TryParse(udpClientPortTextBox.Text, out int port))
            {
               
                    try
                    {
                        transmitter.SetUdpClientSink(udpClientTextBox.Text, port);
                        udpClientTextBox.Enabled = false;
                        udpClientPortTextBox.Enabled = false;

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show("Failed to start UDP sink\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        udpClientCheckBox.Checked = false;
                    }
            }
            else
            {
                udpClientTextBox.Enabled = true;
                udpClientPortTextBox.Enabled = true;
                if (udpClientCheckBox.Checked)
                {
                    MessageBox.Show("Failed to start UDP sink\nPort is invalid" , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                udpClientCheckBox.Checked = false;

                transmitter.DisableUdpClientSink();
            }
        }

        private void serialCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (serialCheckBox.Checked && Int32.TryParse(sinkBaudRateComboBox.Text, out int baudrate))
            {        
                     try
                    {
                        transmitter.SetSerialSink(sinkSelectorComboBox.Text, baudrate);
                        sinkSelectorComboBox.Enabled = false;
                        sinkBaudRateComboBox.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show("Failed to start Serial sink\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        serialCheckBox.Checked = false;
                    }             
            }
            else
            {
                sinkSelectorComboBox.Enabled = true;
                sinkBaudRateComboBox.Enabled = true;
                if (serialCheckBox.Checked)
                {
                    MessageBox.Show("Failed to start Serial sink\nBaud rate is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                serialCheckBox.Checked = false;

                transmitter.DisableSerialSink();
            }
        }

        private void tcpClientCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (tcpClientCheckBox.Checked && Int32.TryParse(tcpClientPortTextBox.Text, out int port))
            {                     
                    try
                    {
                        transmitter.SetTcpClientSink(tcpClientTextBox.Text, port);
                        tcpClientTextBox.Enabled = false;
                        tcpClientPortTextBox.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show("Failed to start TCP client sink\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tcpClientCheckBox.Checked = false;
                    }
            }
            else
            {
                tcpClientTextBox.Enabled = true;
                tcpClientPortTextBox.Enabled = true;
                if (tcpClientCheckBox.Checked)
                {
                    MessageBox.Show("Failed to start TCP client sink\nPort is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                tcpClientCheckBox.Checked = false;

                transmitter.DisableTcpClientSink();
            }
        }

        private void tcpServerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (tcpServerCheckBox.Checked && Int32.TryParse(tcpServerPortTextBox.Text, out int port))
            {           
                    try
                    {
                        transmitter.SetTcpServerSink(port);
                        tcpServerTextBox.Enabled = false;
                        tcpServerPortTextBox.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show("Failed to start TCP server sink\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tcpServerCheckBox.Checked = false;
                    }
            }
            else
            {
                tcpServerTextBox.Enabled = true;
                tcpServerPortTextBox.Enabled = true;
                if (tcpServerCheckBox.Checked)
                {
                    MessageBox.Show("Failed to start TCP server sink\nPort is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                tcpServerCheckBox.Checked = false;

                transmitter.DisableTcpServerSink();
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            threadrun = false;
            if (comPort?.IsOpen ?? false)
            {
                threadrun = false;
                comPort.Close();
                buttonConnect.Text = "Connect";

                sourceSelectorComboBox.Enabled = true;
                if (sourceSelectorComboBox.SelectedIndex != -1)
                {
                    var selected = (SourceDescriptor)sourceSelectorComboBox.SelectedItem;
                    if (selected.Type == SourceType.SERIAL)
                    {
                        sourceBaudRateComboBox.Enabled = true;
                    }
                    else
                    {
                        sourceSpecificTextBox.Enabled = true;
                    }
                }
            }
            else
            {
                // TODO: trycatches

                status_line3 = null;
                var item = (SourceDescriptor)sourceSelectorComboBox.SelectedItem;

                try
                {

                    // TODO: handle baudratenull value
                    if (item != null)
                    {
                        switch (item.Type)
                        {
                            case SourceType.SERIAL:
                                comPort = new SerialPort();
                                comPort.PortName = item.Name;
                                comPort.BaudRate = Int32.Parse(sourceBaudRateComboBox.Text);
                                break;
                            case SourceType.FILE:
                                comPort = new CommsFile();
                                comPort.PortName = sourceSpecificTextBox.Text;
                                break;
                            case SourceType.UDP_CLIENT:
                                var udpClient = new UdpSerialConnect();
                                udpClient.Port = sourceSpecificTextBox.Text.Substring(sourceSpecificTextBox.Text.IndexOf(':') + 1);
                                udpClient.Host = sourceSpecificTextBox.Text.Substring(0, sourceSpecificTextBox.Text.IndexOf(':'));
                                comPort = udpClient;
                                break;
                            case SourceType.UDP_HOST:
                                var udpHost = new UdpSerial();
                                udpHost.Port = sourceSpecificTextBox.Text;
                                comPort = udpHost;
                                break;
                            case SourceType.TCP_CLIENT:
                                var tcpClient = new TcpSerial();
                                tcpClient.Port = sourceSpecificTextBox.Text.Substring(sourceSpecificTextBox.Text.IndexOf(':') + 1);
                                tcpClient.Host = sourceSpecificTextBox.Text.Substring(0, sourceSpecificTextBox.Text.IndexOf(':'));
                                comPort = tcpClient;
                                break;
                            case SourceType.NTRIP:
                                var ntripClient = new CommsNTRIP();
                                ntripClient.URL = sourceSpecificTextBox.Text;
                                comPort = ntripClient;
                                break;
                        }

                        try
                        {
                            comPort.ReadBufferSize = 1024 * 64;

                            try
                            {
                                comPort.Open();
                            }
                            catch (ArgumentException ex)
                            {
                                log.Error(ex);                               

                                // try pipe method
                                comPort = new CommsSerialPipe();
                                comPort.PortName = item.Name;
                                comPort.BaudRate = Int32.Parse(sourceBaudRateComboBox.Text);

                                try
                                {
                                    comPort.Open();
                                }
                                catch
                                {
                                    comPort.Close();
                                    throw;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // TODO: catch???
                            log.Error(ex);
                            throw;
                        }

                        // inject init strings - m8p
                        if (m8pCheckBox.Checked)
                        {
                            //this.LogInfo("Setup M8P");

                            ubx_m8p.SetupM8P(comPort, m8pFw130CheckBox.Checked, movingBaseCheckBox.Checked, radioLinkCheckBox.Checked);

                            if (basepos != MyPointLatLngAlt.Zero)
                                ubx_m8p.SetupBasePos(comPort, basepos, 0, 0, false, movingBaseCheckBox.Checked);

                            /**if (radioLinkCheckBox.Checked)
                            {
                               sourceBaudRateComboBox.Text = "9600";
                            }
                            else
                            {
                                sourceBaudRateComboBox.Text = "115200";
                            }**/

                            //this.LogInfo("Setup M8P done");
                        }

                        RunMainLoop();

                        buttonConnect.Text = "Stop";
                        sourceSelectorComboBox.Enabled = false;
                        sourceBaudRateComboBox.Enabled = false;
                        sourceSpecificTextBox.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show("Could not connect to serial source", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dg_basepos_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (baseposList.Count == 0)
                return;

            baseposList.RemoveAt(e.RowIndex);

            saveBasePosList();
        }

        void saveBasePosList()
        {
            // save config
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<NamedPointLatLngAlt>), new Type[] { typeof(Color) });

            Directory.CreateDirectory(Path.GetDirectoryName(basepostlistfile));
            using (StreamWriter sw = new StreamWriter(basepostlistfile))
            {
                writer.Serialize(sw, baseposList);
            }
        }

        private void dg_basepos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == Use.Index)
            {
                double lat, lng, alt;
                try
                {
                    lat = double.Parse(dg_basepos[Lat.Index, e.RowIndex].Value.ToInvariantString(), CultureInfo.InvariantCulture);
                    lng = double.Parse(dg_basepos[Lng.Index, e.RowIndex].Value.ToInvariantString(), CultureInfo.InvariantCulture);
                    alt = double.Parse(dg_basepos[Alt.Index, e.RowIndex].Value.ToInvariantString(), CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("Cound not parse values to double. Line 1268.");
                    return;
                }
                loadCustomPOS(
                    lat,
                    lng,
                    alt
                    );
                if (comPort != null && comPort.IsOpen)
                {
                    if (int.TryParse(timeTextBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int time) &&
                        double.TryParse(accTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double acc))
                    {
                        ubx_m8p.SetupBasePos(comPort, basepos, time, acc, false, movingBaseCheckBox.Checked);
                        ubx_m8p.poll_msg(comPort, 0x06, 0x71);
                    }
                    else
                    {
                        MessageBox.Show("Cound not parse values to double or int. Line 1286.");
                    }
                }
            }
            if (e.ColumnIndex == Delete.Index)
            {
                if (e.RowIndex != dg_basepos.NewRowIndex)
                    dg_basepos.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void loadCustomPOS(double lat, double lng, double alt)
        {
            try
            {
                basepos = new MyPointLatLngAlt(lat, lng, alt);
            }
            catch
            {
                basepos = MyPointLatLngAlt.Zero;
            }
        }

        private void loadBasePOS()
        {
            basepos = MyPointLatLngAlt.Zero;
        }

        private void dg_basepos_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[Use.Index].Value = "Use";
            e.Row.Cells[Delete.Index].Value = "Delete";
        }

        private void labelmsgseen_Click(object sender, EventArgs e)
        {
            msgseen.Clear();
        }

        private void dg_basepos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            while (baseposList.Count <= e.RowIndex)
                baseposList.Add(new NamedPointLatLngAlt());

            if (e.ColumnIndex == Lat.Index)
            {
                if (double.TryParse(dg_basepos[e.ColumnIndex, e.RowIndex].Value?.ToString(), out double lat))
                {
                    baseposList[e.RowIndex].Lat = lat;
                }
            }
            if (e.ColumnIndex == Lng.Index)
            {
                if (double.TryParse(dg_basepos[e.ColumnIndex, e.RowIndex].Value?.ToString(), out double lng))
                {
                    baseposList[e.RowIndex].Lng = lng;
                }
            }
            if (e.ColumnIndex == Alt.Index)
            {
                if (double.TryParse(dg_basepos[e.ColumnIndex, e.RowIndex].Value?.ToString(), out double alt))
                {
                    baseposList[e.RowIndex].Alt = alt;
                }
            }
            if (e.ColumnIndex == BaseposName.Index)
            {
                baseposList[e.RowIndex].Name = dg_basepos[e.ColumnIndex, e.RowIndex].Value?.ToString();
            }

            saveBasePosList();
        }

        private void savePositionButton_Click(object sender, EventArgs e)
        {
            if (MovingBase == null)
            {
                // TODO: some error message (?)
                return;
            }

            string location = "";
            // TODO: enter location name somehow
            //if (InputBox.Show("Enter Location", "Enter a friendly name for this location.", ref location) ==
            //    DialogResult.OK)
            {
                var basepos = MovingBase;
                // TODO: save
                // Settings.Instance["base_pos"] = String.Format("{0},{1},{2},{3}", basepos.Lat.ToString(CultureInfo.InvariantCulture), basepos.Lng.ToString(CultureInfo.InvariantCulture), basepos.Alt.ToString(CultureInfo.InvariantCulture), location);

                baseposList.Add(new NamedPointLatLngAlt(basepos) { Name = location });

                updateBasePosDG();
            }
        }

        private void m8pCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m8pGroupBox.Visible = m8pCheckBox.Checked;
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            basepos = MyPointLatLngAlt.Zero;

            msgseen.Clear();

            if (comPort != null && comPort.IsOpen)
            {
                if (int.TryParse(timeTextBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int time) && 
                    double.TryParse(accTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double acc))
                {
                    ubx_m8p.SetupBasePos(comPort, basepos, 0, 0, true, movingBaseCheckBox.Checked);
                    ubx_m8p.SetupBasePos(comPort, basepos, time, acc, false, movingBaseCheckBox.Checked);
                } 
                else
                {
                    // TODO: some message
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            updateInormation();

            Text = "RTK Client " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(
                    "Application will be closed.\nAre you sure?",
                    "Application exit confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            threadrun = false;
            SaveSettings();
        }
              
        private void overrideIDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            override_id = overrideIDCheckBox.Checked;
            overrideIDTextBox.Enabled = override_id;
        }

        private void mavMsgTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtcm_msg = (MavMsgsPackedType)mavMsgTypeComboBox.SelectedIndex;
        }

        private void radioLinkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLinkCheckBox.Checked)
            {
                this.sourceBaudRateComboBox.Text = "9600";
            }
        }
    }
}