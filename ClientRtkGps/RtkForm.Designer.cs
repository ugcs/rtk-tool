namespace ClientRtkGps
{
    partial class RtkForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RtkForm));
            this.sourceSelectorComboBox = new System.Windows.Forms.ComboBox();
            this.sourceBaudRateComboBox = new System.Windows.Forms.ComboBox();
            this.sinkSelectorComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sinkBaudRateComboBox = new System.Windows.Forms.ComboBox();
            this.serialCheckBox = new System.Windows.Forms.CheckBox();
            this.tcpClientTextBox = new System.Windows.Forms.TextBox();
            this.tcpServerTextBox = new System.Windows.Forms.TextBox();
            this.tcpClientCheckBox = new System.Windows.Forms.CheckBox();
            this.tcpServerCheckBox = new System.Windows.Forms.CheckBox();
            this.udpClientCheckBox = new System.Windows.Forms.CheckBox();
            this.udpClientTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelbase = new System.Windows.Forms.Label();
            this.labelgps = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelglonass = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label14BDS = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_status1 = new System.Windows.Forms.Label();
            this.lbl_status2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelmsgseen = new System.Windows.Forms.Label();
            this.lbl_status3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label16Galileo = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbl_svin5 = new System.Windows.Forms.Label();
            this.lbl_svin4 = new System.Windows.Forms.Label();
            this.lbl_svin3 = new System.Windows.Forms.Label();
            this.lbl_svin2 = new System.Windows.Forms.Label();
            this.lbl_svin1 = new System.Windows.Forms.Label();
            this.tcpClientPortTextBox = new System.Windows.Forms.TextBox();
            this.tcpServerPortTextBox = new System.Windows.Forms.TextBox();
            this.udpClientPortTextBox = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.sourceSpecificLabel = new System.Windows.Forms.Label();
            this.sourceSpecificTextBox = new System.Windows.Forms.TextBox();
            this.m8pGroupBox = new System.Windows.Forms.GroupBox();
            this.dg_basepos = new System.Windows.Forms.DataGridView();
            this.Lat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BaseposName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Use = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.savePositionButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.accTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.movingBaseCheckBox = new System.Windows.Forms.CheckBox();
            this.m8pFw130CheckBox = new System.Windows.Forms.CheckBox();
            this.m8pCheckBox = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.radioLinkCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.signalStrengthHeaderLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mavMsgTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.overrideIDTextBox = new System.Windows.Forms.TextBox();
            this.overrideIDCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.m8pGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_basepos)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceSelectorComboBox
            // 
            this.sourceSelectorComboBox.FormattingEnabled = true;
            this.sourceSelectorComboBox.Location = new System.Drawing.Point(18, 32);
            this.sourceSelectorComboBox.Name = "sourceSelectorComboBox";
            this.sourceSelectorComboBox.Size = new System.Drawing.Size(199, 21);
            this.sourceSelectorComboBox.TabIndex = 0;
            this.sourceSelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceSelectorComboBox_SelectedIndexChanged);
            // 
            // sourceBaudRateComboBox
            // 
            this.sourceBaudRateComboBox.Enabled = false;
            this.sourceBaudRateComboBox.FormattingEnabled = true;
            this.sourceBaudRateComboBox.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "57600",
            "115200",
            "500000",
            "1000000"});
            this.sourceBaudRateComboBox.Location = new System.Drawing.Point(18, 59);
            this.sourceBaudRateComboBox.Name = "sourceBaudRateComboBox";
            this.sourceBaudRateComboBox.Size = new System.Drawing.Size(199, 21);
            this.sourceBaudRateComboBox.TabIndex = 1;
            // 
            // sinkSelectorComboBox
            // 
            this.sinkSelectorComboBox.FormattingEnabled = true;
            this.sinkSelectorComboBox.Location = new System.Drawing.Point(240, 32);
            this.sinkSelectorComboBox.Name = "sinkSelectorComboBox";
            this.sinkSelectorComboBox.Size = new System.Drawing.Size(213, 21);
            this.sinkSelectorComboBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Source settings:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sink settings:";
            // 
            // sinkBaudRateComboBox
            // 
            this.sinkBaudRateComboBox.FormattingEnabled = true;
            this.sinkBaudRateComboBox.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "57600",
            "115200",
            "500000",
            "1000000"});
            this.sinkBaudRateComboBox.Location = new System.Drawing.Point(240, 59);
            this.sinkBaudRateComboBox.Name = "sinkBaudRateComboBox";
            this.sinkBaudRateComboBox.Size = new System.Drawing.Size(213, 21);
            this.sinkBaudRateComboBox.TabIndex = 5;
            // 
            // serialCheckBox
            // 
            this.serialCheckBox.AutoSize = true;
            this.serialCheckBox.Location = new System.Drawing.Point(459, 36);
            this.serialCheckBox.Name = "serialCheckBox";
            this.serialCheckBox.Size = new System.Drawing.Size(94, 17);
            this.serialCheckBox.TabIndex = 6;
            this.serialCheckBox.Text = "Use serial sink";
            this.serialCheckBox.UseVisualStyleBackColor = true;
            this.serialCheckBox.CheckedChanged += new System.EventHandler(this.serialCheckBox_CheckedChanged);
            // 
            // tcpClientTextBox
            // 
            this.tcpClientTextBox.Location = new System.Drawing.Point(240, 137);
            this.tcpClientTextBox.Name = "tcpClientTextBox";
            this.tcpClientTextBox.Size = new System.Drawing.Size(156, 20);
            this.tcpClientTextBox.TabIndex = 7;
            // 
            // tcpServerTextBox
            // 
            this.tcpServerTextBox.Location = new System.Drawing.Point(240, 163);
            this.tcpServerTextBox.Name = "tcpServerTextBox";
            this.tcpServerTextBox.Size = new System.Drawing.Size(156, 20);
            this.tcpServerTextBox.TabIndex = 8;
            this.tcpServerTextBox.Visible = false;
            // 
            // tcpClientCheckBox
            // 
            this.tcpClientCheckBox.AutoSize = true;
            this.tcpClientCheckBox.Location = new System.Drawing.Point(459, 139);
            this.tcpClientCheckBox.Name = "tcpClientCheckBox";
            this.tcpClientCheckBox.Size = new System.Drawing.Size(119, 17);
            this.tcpClientCheckBox.TabIndex = 9;
            this.tcpClientCheckBox.Text = "Use TCP client sink";
            this.tcpClientCheckBox.UseVisualStyleBackColor = true;
            this.tcpClientCheckBox.CheckedChanged += new System.EventHandler(this.tcpClientCheckBox_CheckedChanged);
            // 
            // tcpServerCheckBox
            // 
            this.tcpServerCheckBox.AutoSize = true;
            this.tcpServerCheckBox.Location = new System.Drawing.Point(459, 165);
            this.tcpServerCheckBox.Name = "tcpServerCheckBox";
            this.tcpServerCheckBox.Size = new System.Drawing.Size(123, 17);
            this.tcpServerCheckBox.TabIndex = 10;
            this.tcpServerCheckBox.Text = "Use TCP server sink";
            this.tcpServerCheckBox.UseVisualStyleBackColor = true;
            this.tcpServerCheckBox.CheckedChanged += new System.EventHandler(this.tcpServerCheckBox_CheckedChanged);
            // 
            // udpClientCheckBox
            // 
            this.udpClientCheckBox.AutoSize = true;
            this.udpClientCheckBox.Location = new System.Drawing.Point(459, 113);
            this.udpClientCheckBox.Name = "udpClientCheckBox";
            this.udpClientCheckBox.Size = new System.Drawing.Size(121, 17);
            this.udpClientCheckBox.TabIndex = 13;
            this.udpClientCheckBox.Text = "Use UDP client sink";
            this.udpClientCheckBox.UseVisualStyleBackColor = true;
            this.udpClientCheckBox.CheckedChanged += new System.EventHandler(this.udpClientCheckBox_CheckedChanged);
            // 
            // udpClientTextBox
            // 
            this.udpClientTextBox.Location = new System.Drawing.Point(240, 110);
            this.udpClientTextBox.Name = "udpClientTextBox";
            this.udpClientTextBox.Size = new System.Drawing.Size(156, 20);
            this.udpClientTextBox.TabIndex = 11;
            this.udpClientTextBox.Text = "192.168.1.255";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Base";
            // 
            // labelbase
            // 
            this.labelbase.BackColor = System.Drawing.Color.Red;
            this.labelbase.ForeColor = System.Drawing.Color.White;
            this.labelbase.Location = new System.Drawing.Point(46, 26);
            this.labelbase.Name = "labelbase";
            this.labelbase.Size = new System.Drawing.Size(20, 20);
            this.labelbase.TabIndex = 15;
            this.labelbase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelgps
            // 
            this.labelgps.BackColor = System.Drawing.Color.Red;
            this.labelgps.ForeColor = System.Drawing.Color.White;
            this.labelgps.Location = new System.Drawing.Point(104, 26);
            this.labelgps.Name = "labelgps";
            this.labelgps.Size = new System.Drawing.Size(20, 20);
            this.labelgps.TabIndex = 17;
            this.labelgps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(72, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Gps";
            // 
            // labelglonass
            // 
            this.labelglonass.BackColor = System.Drawing.Color.Red;
            this.labelglonass.ForeColor = System.Drawing.Color.White;
            this.labelglonass.Location = new System.Drawing.Point(181, 26);
            this.labelglonass.Name = "labelglonass";
            this.labelglonass.Size = new System.Drawing.Size(20, 20);
            this.labelglonass.TabIndex = 19;
            this.labelglonass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(130, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Glonass";
            // 
            // label14BDS
            // 
            this.label14BDS.BackColor = System.Drawing.Color.Red;
            this.label14BDS.ForeColor = System.Drawing.Color.White;
            this.label14BDS.Location = new System.Drawing.Point(253, 26);
            this.label14BDS.Name = "label14BDS";
            this.label14BDS.Size = new System.Drawing.Size(20, 20);
            this.label14BDS.TabIndex = 21;
            this.label14BDS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(207, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Beidou";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Input data rate";
            // 
            // lbl_status1
            // 
            this.lbl_status1.Location = new System.Drawing.Point(95, 17);
            this.lbl_status1.Name = "lbl_status1";
            this.lbl_status1.Size = new System.Drawing.Size(70, 13);
            this.lbl_status1.TabIndex = 23;
            this.lbl_status1.Text = "label6";
            this.lbl_status1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lbl_status2
            // 
            this.lbl_status2.Location = new System.Drawing.Point(337, 16);
            this.lbl_status2.Name = "lbl_status2";
            this.lbl_status2.Size = new System.Drawing.Size(70, 13);
            this.lbl_status2.TabIndex = 25;
            this.lbl_status2.Text = "label6";
            this.lbl_status2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(182, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Output data rate (per channel)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Messages Seen";
            // 
            // labelmsgseen
            // 
            this.labelmsgseen.Location = new System.Drawing.Point(98, 39);
            this.labelmsgseen.Name = "labelmsgseen";
            this.labelmsgseen.Size = new System.Drawing.Size(522, 34);
            this.labelmsgseen.TabIndex = 27;
            this.labelmsgseen.Text = "label10";
            this.labelmsgseen.Click += new System.EventHandler(this.labelmsgseen_Click);
            // 
            // lbl_status3
            // 
            this.lbl_status3.AutoSize = true;
            this.lbl_status3.Location = new System.Drawing.Point(83, 57);
            this.lbl_status3.Name = "lbl_status3";
            this.lbl_status3.Size = new System.Drawing.Size(241, 13);
            this.lbl_status3.TabIndex = 28;
            this.lbl_status3.Text = "-00.000000000 000.000000000 0000.000000000";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label16Galileo);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label14BDS);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbl_status3);
            this.groupBox1.Controls.Add(this.labelbase);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelgps);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.labelglonass);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(16, 271);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(626, 84);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RTCM";
            // 
            // label16Galileo
            // 
            this.label16Galileo.BackColor = System.Drawing.Color.Red;
            this.label16Galileo.ForeColor = System.Drawing.Color.White;
            this.label16Galileo.Location = new System.Drawing.Point(330, 26);
            this.label16Galileo.Name = "label16Galileo";
            this.label16Galileo.Size = new System.Drawing.Size(20, 20);
            this.label16Galileo.TabIndex = 31;
            this.label16Galileo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(284, 33);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "Galileo";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "RTCM Base";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lbl_status1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.labelmsgseen);
            this.groupBox2.Controls.Add(this.lbl_status2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(15, 189);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(627, 76);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Link Status";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbl_svin5);
            this.groupBox3.Controls.Add(this.lbl_svin4);
            this.groupBox3.Controls.Add(this.lbl_svin3);
            this.groupBox3.Controls.Add(this.lbl_svin2);
            this.groupBox3.Controls.Add(this.lbl_svin1);
            this.groupBox3.Location = new System.Drawing.Point(472, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(147, 172);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Survey In";
            // 
            // lbl_svin5
            // 
            this.lbl_svin5.AutoSize = true;
            this.lbl_svin5.Location = new System.Drawing.Point(6, 68);
            this.lbl_svin5.Name = "lbl_svin5";
            this.lbl_svin5.Size = new System.Drawing.Size(41, 13);
            this.lbl_svin5.TabIndex = 4;
            this.lbl_svin5.Text = "label11";
            this.lbl_svin5.Visible = false;
            // 
            // lbl_svin4
            // 
            this.lbl_svin4.AutoSize = true;
            this.lbl_svin4.Location = new System.Drawing.Point(6, 55);
            this.lbl_svin4.Name = "lbl_svin4";
            this.lbl_svin4.Size = new System.Drawing.Size(41, 13);
            this.lbl_svin4.TabIndex = 3;
            this.lbl_svin4.Text = "label11";
            this.lbl_svin4.Visible = false;
            // 
            // lbl_svin3
            // 
            this.lbl_svin3.AutoSize = true;
            this.lbl_svin3.Location = new System.Drawing.Point(6, 42);
            this.lbl_svin3.Name = "lbl_svin3";
            this.lbl_svin3.Size = new System.Drawing.Size(41, 13);
            this.lbl_svin3.TabIndex = 2;
            this.lbl_svin3.Text = "label11";
            this.lbl_svin3.Visible = false;
            // 
            // lbl_svin2
            // 
            this.lbl_svin2.AutoSize = true;
            this.lbl_svin2.Location = new System.Drawing.Point(6, 29);
            this.lbl_svin2.Name = "lbl_svin2";
            this.lbl_svin2.Size = new System.Drawing.Size(41, 13);
            this.lbl_svin2.TabIndex = 1;
            this.lbl_svin2.Text = "label11";
            this.lbl_svin2.Visible = false;
            // 
            // lbl_svin1
            // 
            this.lbl_svin1.AutoSize = true;
            this.lbl_svin1.Location = new System.Drawing.Point(6, 16);
            this.lbl_svin1.Name = "lbl_svin1";
            this.lbl_svin1.Size = new System.Drawing.Size(41, 13);
            this.lbl_svin1.TabIndex = 0;
            this.lbl_svin1.Text = "label11";
            this.lbl_svin1.Visible = false;
            // 
            // tcpClientPortTextBox
            // 
            this.tcpClientPortTextBox.Location = new System.Drawing.Point(402, 136);
            this.tcpClientPortTextBox.Name = "tcpClientPortTextBox";
            this.tcpClientPortTextBox.Size = new System.Drawing.Size(51, 20);
            this.tcpClientPortTextBox.TabIndex = 32;
            // 
            // tcpServerPortTextBox
            // 
            this.tcpServerPortTextBox.Location = new System.Drawing.Point(402, 162);
            this.tcpServerPortTextBox.Name = "tcpServerPortTextBox";
            this.tcpServerPortTextBox.Size = new System.Drawing.Size(51, 20);
            this.tcpServerPortTextBox.TabIndex = 33;
            // 
            // udpClientPortTextBox
            // 
            this.udpClientPortTextBox.Location = new System.Drawing.Point(402, 110);
            this.udpClientPortTextBox.Name = "udpClientPortTextBox";
            this.udpClientPortTextBox.Size = new System.Drawing.Size(51, 20);
            this.udpClientPortTextBox.TabIndex = 34;
            this.udpClientPortTextBox.Text = "255";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(142, 110);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 35;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // sourceSpecificLabel
            // 
            this.sourceSpecificLabel.AutoSize = true;
            this.sourceSpecificLabel.Location = new System.Drawing.Point(18, 88);
            this.sourceSpecificLabel.Name = "sourceSpecificLabel";
            this.sourceSpecificLabel.Size = new System.Drawing.Size(41, 13);
            this.sourceSpecificLabel.TabIndex = 36;
            this.sourceSpecificLabel.Text = "label11";
            this.sourceSpecificLabel.Visible = false;
            // 
            // sourceSpecificTextBox
            // 
            this.sourceSpecificTextBox.Enabled = false;
            this.sourceSpecificTextBox.Location = new System.Drawing.Point(95, 85);
            this.sourceSpecificTextBox.Name = "sourceSpecificTextBox";
            this.sourceSpecificTextBox.Size = new System.Drawing.Size(122, 20);
            this.sourceSpecificTextBox.TabIndex = 37;
            // 
            // m8pGroupBox
            // 
            this.m8pGroupBox.Controls.Add(this.dg_basepos);
            this.m8pGroupBox.Controls.Add(this.savePositionButton);
            this.m8pGroupBox.Controls.Add(this.restartButton);
            this.m8pGroupBox.Controls.Add(this.timeTextBox);
            this.m8pGroupBox.Controls.Add(this.label12);
            this.m8pGroupBox.Controls.Add(this.accTextBox);
            this.m8pGroupBox.Controls.Add(this.label11);
            this.m8pGroupBox.Controls.Add(this.movingBaseCheckBox);
            this.m8pGroupBox.Controls.Add(this.m8pFw130CheckBox);
            this.m8pGroupBox.Controls.Add(this.groupBox3);
            this.m8pGroupBox.Location = new System.Drawing.Point(18, 390);
            this.m8pGroupBox.Name = "m8pGroupBox";
            this.m8pGroupBox.Size = new System.Drawing.Size(623, 192);
            this.m8pGroupBox.TabIndex = 38;
            this.m8pGroupBox.TabStop = false;
            this.m8pGroupBox.Text = "M8P/F9P";
            this.m8pGroupBox.Visible = false;
            // 
            // dg_basepos
            // 
            this.dg_basepos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_basepos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Lat,
            this.Lng,
            this.Alt,
            this.BaseposName,
            this.Use,
            this.Delete});
            this.dg_basepos.Location = new System.Drawing.Point(6, 69);
            this.dg_basepos.Name = "dg_basepos";
            this.dg_basepos.Size = new System.Drawing.Size(460, 105);
            this.dg_basepos.TabIndex = 40;
            this.dg_basepos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_basepos_CellContentClick);
            this.dg_basepos.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_basepos_CellEndEdit);
            this.dg_basepos.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dg_basepos_DefaultValuesNeeded);
            this.dg_basepos.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dg_basepos_RowsRemoved);
            // 
            // Lat
            // 
            this.Lat.HeaderText = "Lat/ECEFX";
            this.Lat.Name = "Lat";
            this.Lat.Width = 70;
            // 
            // Lng
            // 
            this.Lng.HeaderText = "Lng/ECEFY";
            this.Lng.Name = "Lng";
            this.Lng.Width = 70;
            // 
            // Alt
            // 
            this.Alt.HeaderText = "Alt/ECEFZ";
            this.Alt.Name = "Alt";
            this.Alt.Width = 70;
            // 
            // BaseposName
            // 
            this.BaseposName.HeaderText = "Name";
            this.BaseposName.Name = "BaseposName";
            // 
            // Use
            // 
            this.Use.HeaderText = "Use";
            this.Use.Name = "Use";
            this.Use.Width = 50;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.Width = 50;
            // 
            // savePositionButton
            // 
            this.savePositionButton.Location = new System.Drawing.Point(307, 29);
            this.savePositionButton.Name = "savePositionButton";
            this.savePositionButton.Size = new System.Drawing.Size(88, 34);
            this.savePositionButton.TabIndex = 39;
            this.savePositionButton.Text = "Save Current Position";
            this.savePositionButton.UseVisualStyleBackColor = true;
            this.savePositionButton.Click += new System.EventHandler(this.savePositionButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(226, 29);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(75, 34);
            this.restartButton.TabIndex = 38;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(188, 42);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.Size = new System.Drawing.Size(32, 20);
            this.timeTextBox.TabIndex = 37;
            this.timeTextBox.Text = "60";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(141, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 36;
            this.label12.Text = "Time(s)";
            // 
            // accTextBox
            // 
            this.accTextBox.Location = new System.Drawing.Point(97, 42);
            this.accTextBox.Name = "accTextBox";
            this.accTextBox.Size = new System.Drawing.Size(38, 20);
            this.accTextBox.TabIndex = 35;
            this.accTextBox.Text = "2.00";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "SurveyIn Acc(m)";
            // 
            // movingBaseCheckBox
            // 
            this.movingBaseCheckBox.AutoSize = true;
            this.movingBaseCheckBox.Location = new System.Drawing.Point(124, 19);
            this.movingBaseCheckBox.Name = "movingBaseCheckBox";
            this.movingBaseCheckBox.Size = new System.Drawing.Size(88, 17);
            this.movingBaseCheckBox.TabIndex = 33;
            this.movingBaseCheckBox.Text = "Moving Base";
            this.movingBaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // m8pFw130CheckBox
            // 
            this.m8pFw130CheckBox.AutoSize = true;
            this.m8pFw130CheckBox.Location = new System.Drawing.Point(6, 19);
            this.m8pFw130CheckBox.Name = "m8pFw130CheckBox";
            this.m8pFw130CheckBox.Size = new System.Drawing.Size(113, 17);
            this.m8pFw130CheckBox.TabIndex = 32;
            this.m8pFw130CheckBox.Text = "M8P fw 130+/F9P";
            this.m8pFw130CheckBox.UseVisualStyleBackColor = true;
            // 
            // m8pCheckBox
            // 
            this.m8pCheckBox.AutoSize = true;
            this.m8pCheckBox.Location = new System.Drawing.Point(18, 367);
            this.m8pCheckBox.Name = "m8pCheckBox";
            this.m8pCheckBox.Size = new System.Drawing.Size(125, 17);
            this.m8pCheckBox.TabIndex = 40;
            this.m8pCheckBox.Text = "M8P/F9P autoconfig";
            this.m8pCheckBox.UseVisualStyleBackColor = true;
            this.m8pCheckBox.CheckedChanged += new System.EventHandler(this.m8pCheckBox_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(237, 88);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 13);
            this.label13.TabIndex = 41;
            this.label13.Text = "Host:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(399, 88);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 13);
            this.label14.TabIndex = 42;
            this.label14.Text = "Port:";
            // 
            // radioLinkCheckBox
            // 
            this.radioLinkCheckBox.AutoSize = true;
            this.radioLinkCheckBox.Location = new System.Drawing.Point(140, 367);
            this.radioLinkCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.radioLinkCheckBox.Name = "radioLinkCheckBox";
            this.radioLinkCheckBox.Size = new System.Drawing.Size(98, 17);
            this.radioLinkCheckBox.TabIndex = 43;
            this.radioLinkCheckBox.Text = "3DR Radio link";
            this.radioLinkCheckBox.UseVisualStyleBackColor = true;
            this.radioLinkCheckBox.CheckedChanged += new System.EventHandler(this.radioLinkCheckBox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.signalStrengthHeaderLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 567);
            this.panel1.TabIndex = 45;
            // 
            // signalStrengthHeaderLabel
            // 
            this.signalStrengthHeaderLabel.AutoSize = true;
            this.signalStrengthHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.signalStrengthHeaderLabel.Location = new System.Drawing.Point(13, 10);
            this.signalStrengthHeaderLabel.Name = "signalStrengthHeaderLabel";
            this.signalStrengthHeaderLabel.Size = new System.Drawing.Size(155, 17);
            this.signalStrengthHeaderLabel.TabIndex = 0;
            this.signalStrengthHeaderLabel.Text = "Satellite signal strength";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.mavMsgTypeComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.label15);
            this.splitContainer1.Panel1.Controls.Add(this.overrideIDTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.overrideIDCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.radioLinkCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.m8pCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.sourceSelectorComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.m8pGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.sourceBaudRateComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.label14);
            this.splitContainer1.Panel1.Controls.Add(this.sinkSelectorComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.label13);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.sinkBaudRateComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.serialCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.sourceSpecificTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.tcpClientTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.sourceSpecificLabel);
            this.splitContainer1.Panel1.Controls.Add(this.tcpServerTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.buttonConnect);
            this.splitContainer1.Panel1.Controls.Add(this.tcpClientCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.udpClientPortTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.tcpServerCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.tcpServerPortTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.udpClientTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.tcpClientPortTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.udpClientCheckBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainer1.Size = new System.Drawing.Size(896, 587);
            this.splitContainer1.SplitterDistance = 674;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 46;
            this.splitContainer1.TabStop = false;
            // 
            // mavMsgTypeComboBox
            // 
            this.mavMsgTypeComboBox.FormattingEnabled = true;
            this.mavMsgTypeComboBox.Items.AddRange(new object[] {
            "GPS_RTCM_DATA",
            "GPS_INJECT_DATA",
            "DATA96"});
            this.mavMsgTypeComboBox.Location = new System.Drawing.Point(95, 161);
            this.mavMsgTypeComboBox.Name = "mavMsgTypeComboBox";
            this.mavMsgTypeComboBox.Size = new System.Drawing.Size(123, 21);
            this.mavMsgTypeComboBox.TabIndex = 48;
            this.mavMsgTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.mavMsgTypeComboBox_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 164);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(51, 13);
            this.label15.TabIndex = 47;
            this.label15.Text = "Mav Msg";
            // 
            // overrideIDTextBox
            // 
            this.overrideIDTextBox.Enabled = false;
            this.overrideIDTextBox.Location = new System.Drawing.Point(506, 366);
            this.overrideIDTextBox.Name = "overrideIDTextBox";
            this.overrideIDTextBox.Size = new System.Drawing.Size(54, 20);
            this.overrideIDTextBox.TabIndex = 46;
            this.overrideIDTextBox.Text = "1";
            // 
            // overrideIDCheckBox
            // 
            this.overrideIDCheckBox.AutoSize = true;
            this.overrideIDCheckBox.Location = new System.Drawing.Point(330, 368);
            this.overrideIDCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.overrideIDCheckBox.Name = "overrideIDCheckBox";
            this.overrideIDCheckBox.Size = new System.Drawing.Size(171, 17);
            this.overrideIDCheckBox.TabIndex = 45;
            this.overrideIDCheckBox.Text = "Override Station ID (max 4095)";
            this.overrideIDCheckBox.UseVisualStyleBackColor = true;
            this.overrideIDCheckBox.CheckedChanged += new System.EventHandler(this.overrideIDCheckBox_CheckedChanged);
            // 
            // RtkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 587);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RtkForm";
            this.Text = "RTK Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.m8pGroupBox.ResumeLayout(false);
            this.m8pGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_basepos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox sourceSelectorComboBox;
        private System.Windows.Forms.ComboBox sourceBaudRateComboBox;
        private System.Windows.Forms.ComboBox sinkSelectorComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox sinkBaudRateComboBox;
        private System.Windows.Forms.CheckBox serialCheckBox;
        private System.Windows.Forms.TextBox tcpClientTextBox;
        private System.Windows.Forms.TextBox tcpServerTextBox;
        private System.Windows.Forms.CheckBox tcpClientCheckBox;
        private System.Windows.Forms.CheckBox tcpServerCheckBox;
        private System.Windows.Forms.CheckBox udpClientCheckBox;
        private System.Windows.Forms.TextBox udpClientTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelbase;
        private System.Windows.Forms.Label labelgps;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelglonass;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14BDS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_status1;
        private System.Windows.Forms.Label lbl_status2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelmsgseen;
        private System.Windows.Forms.Label lbl_status3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbl_svin5;
        private System.Windows.Forms.Label lbl_svin4;
        private System.Windows.Forms.Label lbl_svin3;
        private System.Windows.Forms.Label lbl_svin2;
        private System.Windows.Forms.Label lbl_svin1;
        private System.Windows.Forms.TextBox tcpClientPortTextBox;
        private System.Windows.Forms.TextBox tcpServerPortTextBox;
        private System.Windows.Forms.TextBox udpClientPortTextBox;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label sourceSpecificLabel;
        private System.Windows.Forms.TextBox sourceSpecificTextBox;
        private System.Windows.Forms.GroupBox m8pGroupBox;
        private System.Windows.Forms.TextBox accTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox movingBaseCheckBox;
        private System.Windows.Forms.CheckBox m8pFw130CheckBox;
        private System.Windows.Forms.CheckBox m8pCheckBox;
        private System.Windows.Forms.TextBox timeTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dg_basepos;
        private System.Windows.Forms.Button savePositionButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lng;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alt;
        private System.Windows.Forms.DataGridViewTextBoxColumn BaseposName;
        private System.Windows.Forms.DataGridViewButtonColumn Use;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox radioLinkCheckBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label signalStrengthHeaderLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox overrideIDCheckBox;
        private System.Windows.Forms.TextBox overrideIDTextBox;
        private System.Windows.Forms.ComboBox mavMsgTypeComboBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16Galileo;
        private System.Windows.Forms.Label label17;
    }
}

