namespace ddfClient
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupConnection = new System.Windows.Forms.GroupBox();
            this.groupDiagnostics = new System.Windows.Forms.GroupBox();
            this.checkTraceMessages = new System.Windows.Forms.CheckBox();
            this.checkTraceInfo = new System.Windows.Forms.CheckBox();
            this.checkTraceWarnings = new System.Windows.Forms.CheckBox();
            this.checkTraceErrors = new System.Windows.Forms.CheckBox();
            this.comboProtocol = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupServerSettings = new System.Windows.Forms.GroupBox();
            this.textPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkAutoResolve = new System.Windows.Forms.CheckBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.textUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupClient = new System.Windows.Forms.GroupBox();
            this.buttonClearSymbols = new System.Windows.Forms.Button();
            this.buttonRemoveSymbol = new System.Windows.Forms.Button();
            this.buttonUpdateSymbols = new System.Windows.Forms.Button();
            this.gridSymbolList = new System.Windows.Forms.DataGridView();
            this.colSymbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRefresh = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colStreaming = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colBookRefresh = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colBookUpdates = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colOHLCBars = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkOHLCMinuteBars = new System.Windows.Forms.CheckBox();
            this.checkBookUpdates = new System.Windows.Forms.CheckBox();
            this.checkBookRefresh = new System.Windows.Forms.CheckBox();
            this.checkStreamingData = new System.Windows.Forms.CheckBox();
            this.checkInitialRefresh = new System.Windows.Forms.CheckBox();
            this.textSymbol = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboClientFilter = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupOutput = new System.Windows.Forms.GroupBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.rtbBookQuote = new System.Windows.Forms.RichTextBox();
            this.rtbQuote = new System.Windows.Forms.RichTextBox();
            this.buttonClearAllViews = new System.Windows.Forms.Button();
            this.buttonClearCurrentView = new System.Windows.Forms.Button();
            this.comboView = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rtbTrace = new System.Windows.Forms.RichTextBox();
            this.listOHLC = new System.Windows.Forms.ListBox();
            this.listRaw = new System.Windows.Forms.ListBox();
            this.groupConnection.SuspendLayout();
            this.groupDiagnostics.SuspendLayout();
            this.groupServerSettings.SuspendLayout();
            this.groupClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSymbolList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupConnection
            // 
            this.groupConnection.Controls.Add(this.groupDiagnostics);
            this.groupConnection.Controls.Add(this.comboProtocol);
            this.groupConnection.Controls.Add(this.label6);
            this.groupConnection.Controls.Add(this.groupServerSettings);
            this.groupConnection.Controls.Add(this.checkAutoResolve);
            this.groupConnection.Controls.Add(this.labelStatus);
            this.groupConnection.Controls.Add(this.label3);
            this.groupConnection.Controls.Add(this.textPassword);
            this.groupConnection.Controls.Add(this.textUsername);
            this.groupConnection.Controls.Add(this.label2);
            this.groupConnection.Controls.Add(this.label1);
            this.groupConnection.Location = new System.Drawing.Point(12, 12);
            this.groupConnection.Name = "groupConnection";
            this.groupConnection.Size = new System.Drawing.Size(737, 162);
            this.groupConnection.TabIndex = 0;
            this.groupConnection.TabStop = false;
            this.groupConnection.Text = "Connection";
            // 
            // groupDiagnostics
            // 
            this.groupDiagnostics.Controls.Add(this.checkTraceMessages);
            this.groupDiagnostics.Controls.Add(this.checkTraceInfo);
            this.groupDiagnostics.Controls.Add(this.checkTraceWarnings);
            this.groupDiagnostics.Controls.Add(this.checkTraceErrors);
            this.groupDiagnostics.Location = new System.Drawing.Point(463, 19);
            this.groupDiagnostics.Name = "groupDiagnostics";
            this.groupDiagnostics.Size = new System.Drawing.Size(255, 125);
            this.groupDiagnostics.TabIndex = 10;
            this.groupDiagnostics.TabStop = false;
            this.groupDiagnostics.Text = "Diagnostics";
            // 
            // checkTraceMessages
            // 
            this.checkTraceMessages.AutoSize = true;
            this.checkTraceMessages.Location = new System.Drawing.Point(15, 94);
            this.checkTraceMessages.Name = "checkTraceMessages";
            this.checkTraceMessages.Size = new System.Drawing.Size(161, 17);
            this.checkTraceMessages.TabIndex = 11;
            this.checkTraceMessages.Text = "Dump ddf messages to trace";
            this.checkTraceMessages.UseVisualStyleBackColor = true;
            // 
            // checkTraceInfo
            // 
            this.checkTraceInfo.AutoSize = true;
            this.checkTraceInfo.Location = new System.Drawing.Point(15, 71);
            this.checkTraceInfo.Name = "checkTraceInfo";
            this.checkTraceInfo.Size = new System.Drawing.Size(140, 17);
            this.checkTraceInfo.TabIndex = 10;
            this.checkTraceInfo.Text = "Enable trace information";
            this.checkTraceInfo.UseVisualStyleBackColor = true;
            // 
            // checkTraceWarnings
            // 
            this.checkTraceWarnings.AutoSize = true;
            this.checkTraceWarnings.Location = new System.Drawing.Point(15, 48);
            this.checkTraceWarnings.Name = "checkTraceWarnings";
            this.checkTraceWarnings.Size = new System.Drawing.Size(131, 17);
            this.checkTraceWarnings.TabIndex = 9;
            this.checkTraceWarnings.Text = "Enable trace warnings";
            this.checkTraceWarnings.UseVisualStyleBackColor = true;
            // 
            // checkTraceErrors
            // 
            this.checkTraceErrors.AutoSize = true;
            this.checkTraceErrors.Location = new System.Drawing.Point(15, 25);
            this.checkTraceErrors.Name = "checkTraceErrors";
            this.checkTraceErrors.Size = new System.Drawing.Size(115, 17);
            this.checkTraceErrors.TabIndex = 8;
            this.checkTraceErrors.Text = "Enable trace errors";
            this.checkTraceErrors.UseVisualStyleBackColor = true;
            // 
            // comboProtocol
            // 
            this.comboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProtocol.FormattingEnabled = true;
            this.comboProtocol.Location = new System.Drawing.Point(80, 83);
            this.comboProtocol.Name = "comboProtocol";
            this.comboProtocol.Size = new System.Drawing.Size(121, 21);
            this.comboProtocol.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Protocol:";
            // 
            // groupServerSettings
            // 
            this.groupServerSettings.Controls.Add(this.textPort);
            this.groupServerSettings.Controls.Add(this.label5);
            this.groupServerSettings.Controls.Add(this.textServer);
            this.groupServerSettings.Controls.Add(this.label4);
            this.groupServerSettings.Location = new System.Drawing.Point(235, 56);
            this.groupServerSettings.Name = "groupServerSettings";
            this.groupServerSettings.Size = new System.Drawing.Size(200, 88);
            this.groupServerSettings.TabIndex = 7;
            this.groupServerSettings.TabStop = false;
            this.groupServerSettings.Text = "Server settings";
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(61, 55);
            this.textPort.MaxLength = 5;
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(123, 20);
            this.textPort.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Port:";
            // 
            // textServer
            // 
            this.textServer.Location = new System.Drawing.Point(61, 26);
            this.textServer.Name = "textServer";
            this.textServer.Size = new System.Drawing.Size(123, 20);
            this.textServer.TabIndex = 4;
            this.textServer.TextChanged += new System.EventHandler(this.textServer_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Server:";
            // 
            // checkAutoResolve
            // 
            this.checkAutoResolve.AutoSize = true;
            this.checkAutoResolve.Location = new System.Drawing.Point(235, 27);
            this.checkAutoResolve.Name = "checkAutoResolve";
            this.checkAutoResolve.Size = new System.Drawing.Size(193, 17);
            this.checkAutoResolve.TabIndex = 6;
            this.checkAutoResolve.Text = "Resolve server settings dynamically";
            this.checkAutoResolve.UseVisualStyleBackColor = true;
            this.checkAutoResolve.CheckedChanged += new System.EventHandler(this.checkAutoResolve_CheckedChanged);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(77, 116);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(28, 13);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "Idle";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Status:";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(80, 53);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(123, 20);
            this.textPassword.TabIndex = 3;
            this.textPassword.TextChanged += new System.EventHandler(this.textUsername_TextChanged);
            this.textPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textUsername_KeyPress);
            // 
            // textUsername
            // 
            this.textUsername.Location = new System.Drawing.Point(80, 25);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(123, 20);
            this.textUsername.TabIndex = 2;
            this.textUsername.TextChanged += new System.EventHandler(this.textUsername_TextChanged);
            this.textUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textUsername_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // groupClient
            // 
            this.groupClient.Controls.Add(this.buttonClearSymbols);
            this.groupClient.Controls.Add(this.buttonRemoveSymbol);
            this.groupClient.Controls.Add(this.buttonUpdateSymbols);
            this.groupClient.Controls.Add(this.gridSymbolList);
            this.groupClient.Controls.Add(this.groupBox1);
            this.groupClient.Controls.Add(this.comboClientFilter);
            this.groupClient.Controls.Add(this.label7);
            this.groupClient.Location = new System.Drawing.Point(13, 190);
            this.groupClient.Name = "groupClient";
            this.groupClient.Size = new System.Drawing.Size(736, 261);
            this.groupClient.TabIndex = 1;
            this.groupClient.TabStop = false;
            this.groupClient.Text = "Client";
            // 
            // buttonClearSymbols
            // 
            this.buttonClearSymbols.Location = new System.Drawing.Point(545, 215);
            this.buttonClearSymbols.Name = "buttonClearSymbols";
            this.buttonClearSymbols.Size = new System.Drawing.Size(109, 23);
            this.buttonClearSymbols.TabIndex = 16;
            this.buttonClearSymbols.Text = "Clear Symbols";
            this.buttonClearSymbols.UseVisualStyleBackColor = true;
            this.buttonClearSymbols.Click += new System.EventHandler(this.buttonClearSymbols_Click);
            // 
            // buttonRemoveSymbol
            // 
            this.buttonRemoveSymbol.Location = new System.Drawing.Point(415, 215);
            this.buttonRemoveSymbol.Name = "buttonRemoveSymbol";
            this.buttonRemoveSymbol.Size = new System.Drawing.Size(109, 23);
            this.buttonRemoveSymbol.TabIndex = 15;
            this.buttonRemoveSymbol.Text = "Remove Symbol";
            this.buttonRemoveSymbol.UseVisualStyleBackColor = true;
            this.buttonRemoveSymbol.Click += new System.EventHandler(this.buttonRemoveSymbol_Click);
            // 
            // buttonUpdateSymbols
            // 
            this.buttonUpdateSymbols.Location = new System.Drawing.Point(284, 215);
            this.buttonUpdateSymbols.Name = "buttonUpdateSymbols";
            this.buttonUpdateSymbols.Size = new System.Drawing.Size(109, 23);
            this.buttonUpdateSymbols.TabIndex = 14;
            this.buttonUpdateSymbols.Text = "Update Symbols";
            this.buttonUpdateSymbols.UseVisualStyleBackColor = true;
            this.buttonUpdateSymbols.Click += new System.EventHandler(this.buttonUpdateSymbols_Click);
            // 
            // gridSymbolList
            // 
            this.gridSymbolList.AllowUserToAddRows = false;
            this.gridSymbolList.AllowUserToDeleteRows = false;
            this.gridSymbolList.AllowUserToResizeColumns = false;
            this.gridSymbolList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSymbolList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSymbolList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSymbolList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSymbol,
            this.colRefresh,
            this.colStreaming,
            this.colBookRefresh,
            this.colBookUpdates,
            this.colOHLCBars});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSymbolList.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridSymbolList.Location = new System.Drawing.Point(234, 22);
            this.gridSymbolList.MultiSelect = false;
            this.gridSymbolList.Name = "gridSymbolList";
            this.gridSymbolList.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSymbolList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridSymbolList.RowHeadersVisible = false;
            this.gridSymbolList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridSymbolList.RowTemplate.ReadOnly = true;
            this.gridSymbolList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSymbolList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSymbolList.Size = new System.Drawing.Size(483, 179);
            this.gridSymbolList.TabIndex = 13;
            this.gridSymbolList.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.gridSymbolList_RowStateChanged);
            this.gridSymbolList.SelectionChanged += new System.EventHandler(this.gridSymbolList_SelectionChanged);
            // 
            // colSymbol
            // 
            this.colSymbol.Frozen = true;
            this.colSymbol.HeaderText = "Symbol";
            this.colSymbol.Name = "colSymbol";
            this.colSymbol.ReadOnly = true;
            this.colSymbol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSymbol.Width = 80;
            // 
            // colRefresh
            // 
            this.colRefresh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colRefresh.Frozen = true;
            this.colRefresh.HeaderText = "Refresh";
            this.colRefresh.Name = "colRefresh";
            this.colRefresh.ReadOnly = true;
            this.colRefresh.Width = 50;
            // 
            // colStreaming
            // 
            this.colStreaming.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colStreaming.Frozen = true;
            this.colStreaming.HeaderText = "Streaming";
            this.colStreaming.Name = "colStreaming";
            this.colStreaming.ReadOnly = true;
            this.colStreaming.Width = 60;
            // 
            // colBookRefresh
            // 
            this.colBookRefresh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colBookRefresh.Frozen = true;
            this.colBookRefresh.HeaderText = "Book Refresh";
            this.colBookRefresh.Name = "colBookRefresh";
            this.colBookRefresh.ReadOnly = true;
            this.colBookRefresh.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colBookRefresh.Width = 78;
            // 
            // colBookUpdates
            // 
            this.colBookUpdates.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colBookUpdates.Frozen = true;
            this.colBookUpdates.HeaderText = "Book Updates";
            this.colBookUpdates.Name = "colBookUpdates";
            this.colBookUpdates.ReadOnly = true;
            this.colBookUpdates.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colBookUpdates.Width = 81;
            // 
            // colOHLCBars
            // 
            this.colOHLCBars.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colOHLCBars.Frozen = true;
            this.colOHLCBars.HeaderText = "OHLC Minute Bars";
            this.colOHLCBars.Name = "colOHLCBars";
            this.colOHLCBars.ReadOnly = true;
            this.colOHLCBars.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colOHLCBars.Width = 101;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkOHLCMinuteBars);
            this.groupBox1.Controls.Add(this.checkBookUpdates);
            this.groupBox1.Controls.Add(this.checkBookRefresh);
            this.groupBox1.Controls.Add(this.checkStreamingData);
            this.groupBox1.Controls.Add(this.checkInitialRefresh);
            this.groupBox1.Controls.Add(this.textSymbol);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(18, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 179);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Symbol";
            // 
            // checkOHLCMinuteBars
            // 
            this.checkOHLCMinuteBars.AutoSize = true;
            this.checkOHLCMinuteBars.Location = new System.Drawing.Point(16, 148);
            this.checkOHLCMinuteBars.Name = "checkOHLCMinuteBars";
            this.checkOHLCMinuteBars.Size = new System.Drawing.Size(114, 17);
            this.checkOHLCMinuteBars.TabIndex = 9;
            this.checkOHLCMinuteBars.Text = "OHLC Minute Bars";
            this.checkOHLCMinuteBars.UseVisualStyleBackColor = true;
            // 
            // checkBookUpdates
            // 
            this.checkBookUpdates.AutoSize = true;
            this.checkBookUpdates.Location = new System.Drawing.Point(16, 125);
            this.checkBookUpdates.Name = "checkBookUpdates";
            this.checkBookUpdates.Size = new System.Drawing.Size(94, 17);
            this.checkBookUpdates.TabIndex = 8;
            this.checkBookUpdates.Text = "Book Updates";
            this.checkBookUpdates.UseVisualStyleBackColor = true;
            // 
            // checkBookRefresh
            // 
            this.checkBookRefresh.AutoSize = true;
            this.checkBookRefresh.Location = new System.Drawing.Point(16, 102);
            this.checkBookRefresh.Name = "checkBookRefresh";
            this.checkBookRefresh.Size = new System.Drawing.Size(91, 17);
            this.checkBookRefresh.TabIndex = 7;
            this.checkBookRefresh.Text = "Book Refresh";
            this.checkBookRefresh.UseVisualStyleBackColor = true;
            // 
            // checkStreamingData
            // 
            this.checkStreamingData.AutoSize = true;
            this.checkStreamingData.Location = new System.Drawing.Point(16, 79);
            this.checkStreamingData.Name = "checkStreamingData";
            this.checkStreamingData.Size = new System.Drawing.Size(116, 17);
            this.checkStreamingData.TabIndex = 6;
            this.checkStreamingData.Text = "Streaming Updates";
            this.checkStreamingData.UseVisualStyleBackColor = true;
            // 
            // checkInitialRefresh
            // 
            this.checkInitialRefresh.AutoSize = true;
            this.checkInitialRefresh.Location = new System.Drawing.Point(16, 56);
            this.checkInitialRefresh.Name = "checkInitialRefresh";
            this.checkInitialRefresh.Size = new System.Drawing.Size(90, 17);
            this.checkInitialRefresh.TabIndex = 5;
            this.checkInitialRefresh.Text = "Initial Refresh";
            this.checkInitialRefresh.UseVisualStyleBackColor = true;
            // 
            // textSymbol
            // 
            this.textSymbol.Location = new System.Drawing.Point(63, 24);
            this.textSymbol.MaxLength = 10;
            this.textSymbol.Name = "textSymbol";
            this.textSymbol.Size = new System.Drawing.Size(111, 20);
            this.textSymbol.TabIndex = 4;
            this.textSymbol.TextChanged += new System.EventHandler(this.textSymbol_TextChanged);
            this.textSymbol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textSymbol_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Symbol:";
            // 
            // comboClientFilter
            // 
            this.comboClientFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClientFilter.FormattingEnabled = true;
            this.comboClientFilter.Location = new System.Drawing.Point(79, 22);
            this.comboClientFilter.Name = "comboClientFilter";
            this.comboClientFilter.Size = new System.Drawing.Size(121, 21);
            this.comboClientFilter.TabIndex = 11;
            this.comboClientFilter.SelectedIndexChanged += new System.EventHandler(this.comboClientFilter_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Filter:";
            // 
            // groupOutput
            // 
            this.groupOutput.Controls.Add(this.btnStartServer);
            this.groupOutput.Controls.Add(this.rtbBookQuote);
            this.groupOutput.Controls.Add(this.rtbQuote);
            this.groupOutput.Controls.Add(this.buttonClearAllViews);
            this.groupOutput.Controls.Add(this.buttonClearCurrentView);
            this.groupOutput.Controls.Add(this.comboView);
            this.groupOutput.Controls.Add(this.label9);
            this.groupOutput.Controls.Add(this.rtbTrace);
            this.groupOutput.Controls.Add(this.listOHLC);
            this.groupOutput.Controls.Add(this.listRaw);
            this.groupOutput.Location = new System.Drawing.Point(13, 471);
            this.groupOutput.Name = "groupOutput";
            this.groupOutput.Size = new System.Drawing.Size(736, 256);
            this.groupOutput.TabIndex = 2;
            this.groupOutput.TabStop = false;
            this.groupOutput.Text = "Output";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(579, 28);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartServer.TabIndex = 17;
            this.btnStartServer.Text = "StartServer";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // rtbBookQuote
            // 
            this.rtbBookQuote.Location = new System.Drawing.Point(20, 73);
            this.rtbBookQuote.Name = "rtbBookQuote";
            this.rtbBookQuote.ReadOnly = true;
            this.rtbBookQuote.Size = new System.Drawing.Size(697, 160);
            this.rtbBookQuote.TabIndex = 21;
            this.rtbBookQuote.Text = "";
            // 
            // rtbQuote
            // 
            this.rtbQuote.Location = new System.Drawing.Point(20, 73);
            this.rtbQuote.Name = "rtbQuote";
            this.rtbQuote.ReadOnly = true;
            this.rtbQuote.Size = new System.Drawing.Size(697, 160);
            this.rtbQuote.TabIndex = 20;
            this.rtbQuote.Text = "";
            // 
            // buttonClearAllViews
            // 
            this.buttonClearAllViews.Location = new System.Drawing.Point(415, 28);
            this.buttonClearAllViews.Name = "buttonClearAllViews";
            this.buttonClearAllViews.Size = new System.Drawing.Size(109, 23);
            this.buttonClearAllViews.TabIndex = 16;
            this.buttonClearAllViews.Text = "Clear All Views";
            this.buttonClearAllViews.UseVisualStyleBackColor = true;
            this.buttonClearAllViews.Click += new System.EventHandler(this.buttonClearAllViews_Click);
            // 
            // buttonClearCurrentView
            // 
            this.buttonClearCurrentView.Location = new System.Drawing.Point(284, 28);
            this.buttonClearCurrentView.Name = "buttonClearCurrentView";
            this.buttonClearCurrentView.Size = new System.Drawing.Size(109, 23);
            this.buttonClearCurrentView.TabIndex = 15;
            this.buttonClearCurrentView.Text = "Clear Current View";
            this.buttonClearCurrentView.UseVisualStyleBackColor = true;
            this.buttonClearCurrentView.Click += new System.EventHandler(this.buttonClearCurrentView_Click);
            // 
            // comboView
            // 
            this.comboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboView.FormattingEnabled = true;
            this.comboView.Location = new System.Drawing.Point(56, 30);
            this.comboView.Name = "comboView";
            this.comboView.Size = new System.Drawing.Size(201, 21);
            this.comboView.TabIndex = 13;
            this.comboView.SelectedIndexChanged += new System.EventHandler(this.comboView_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "View:";
            // 
            // rtbTrace
            // 
            this.rtbTrace.Location = new System.Drawing.Point(20, 73);
            this.rtbTrace.Name = "rtbTrace";
            this.rtbTrace.ReadOnly = true;
            this.rtbTrace.Size = new System.Drawing.Size(697, 160);
            this.rtbTrace.TabIndex = 18;
            this.rtbTrace.Text = "";
            // 
            // listOHLC
            // 
            this.listOHLC.FormattingEnabled = true;
            this.listOHLC.Location = new System.Drawing.Point(20, 73);
            this.listOHLC.Name = "listOHLC";
            this.listOHLC.Size = new System.Drawing.Size(697, 160);
            this.listOHLC.TabIndex = 19;
            // 
            // listRaw
            // 
            this.listRaw.FormattingEnabled = true;
            this.listRaw.Location = new System.Drawing.Point(20, 73);
            this.listRaw.Name = "listRaw";
            this.listRaw.Size = new System.Drawing.Size(697, 160);
            this.listRaw.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 739);
            this.Controls.Add(this.groupOutput);
            this.Controls.Add(this.groupClient);
            this.Controls.Add(this.groupConnection);
            this.Name = "Form1";
            this.Text = "ddf Client Sample";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupConnection.ResumeLayout(false);
            this.groupConnection.PerformLayout();
            this.groupDiagnostics.ResumeLayout(false);
            this.groupDiagnostics.PerformLayout();
            this.groupServerSettings.ResumeLayout(false);
            this.groupServerSettings.PerformLayout();
            this.groupClient.ResumeLayout(false);
            this.groupClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSymbolList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupOutput.ResumeLayout(false);
            this.groupOutput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupConnection;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.TextBox textUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupServerSettings;
        private System.Windows.Forms.TextBox textServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkAutoResolve;
        private System.Windows.Forms.ComboBox comboProtocol;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupClient;
        private System.Windows.Forms.ComboBox comboClientFilter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkOHLCMinuteBars;
        private System.Windows.Forms.CheckBox checkBookUpdates;
        private System.Windows.Forms.CheckBox checkBookRefresh;
        private System.Windows.Forms.CheckBox checkStreamingData;
        private System.Windows.Forms.CheckBox checkInitialRefresh;
        private System.Windows.Forms.TextBox textSymbol;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonClearSymbols;
        private System.Windows.Forms.Button buttonRemoveSymbol;
        private System.Windows.Forms.Button buttonUpdateSymbols;
        private System.Windows.Forms.GroupBox groupOutput;
        private System.Windows.Forms.ListBox listRaw;
        private System.Windows.Forms.Button buttonClearAllViews;
        private System.Windows.Forms.Button buttonClearCurrentView;
        private System.Windows.Forms.ComboBox comboView;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupDiagnostics;
        private System.Windows.Forms.CheckBox checkTraceMessages;
        private System.Windows.Forms.CheckBox checkTraceInfo;
        private System.Windows.Forms.CheckBox checkTraceWarnings;
        private System.Windows.Forms.CheckBox checkTraceErrors;
        private System.Windows.Forms.RichTextBox rtbTrace;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSymbol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colRefresh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colStreaming;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBookRefresh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBookUpdates;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colOHLCBars;
        private System.Windows.Forms.ListBox listOHLC;
        private System.Windows.Forms.DataGridView gridSymbolList;
        private System.Windows.Forms.RichTextBox rtbBookQuote;
        private System.Windows.Forms.RichTextBox rtbQuote;
        private System.Windows.Forms.Button btnStartServer;
    }
}

