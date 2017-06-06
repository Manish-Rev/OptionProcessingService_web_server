using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using ddfplus;
using ddfplus.Net;
using ddfplus.Parser;
using OptionProcessingService;
using System.Threading.Tasks;
using CommonObjects.Unitity;
using OptionProcessingService.Enums;
using CommonObjects.Logger;

namespace ddfClient
{
    public partial class Form1 : Form
    {
        #region Delegates

        delegate void UpdateStatusHandler(Status status);
        delegate void NewMessageHandler(byte[] message);
        delegate void NewQuoteHandler(Quote quote);
        delegate void NewBookQuoteHandler(BookQuote quote);
        delegate void NewOHLCQuoteHandler(OHLCQuote quote);

        #endregion Delegates

        #region Fields

        Client _client = null;
        Status _status = Status.Disconnected;

        #endregion Fields

        #region Constructors

        public Form1()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Form and Control Events

        private void Form1_Load(object sender, EventArgs e)
        {
            TraceListener traceListener = new RtbTraceListener(rtbTrace);
            Trace.Listeners.Add(traceListener);
            Trace.AutoFlush = true;

            InitializeDdf();

            comboProtocol.Items.Clear();
            comboProtocol.Items.Add("TCP");
            comboProtocol.Items.Add("UDP");
            comboProtocol.SelectedIndex = 0;

            comboClientFilter.Items.Clear();
            comboClientFilter.Items.Add("Filtered");
            comboClientFilter.Items.Add("Unfiltered");
            comboClientFilter.Items.Add("Raw");
            comboClientFilter.SelectedIndex = 0;

            comboView.Items.Clear();
            comboView.Items.Add("Quotes Updates");
            comboView.Items.Add("Book Quote Updates");
            comboView.Items.Add("OHLC Minute Bar Updates");
            comboView.Items.Add("Raw Messages");
            comboView.Items.Add("Trace Output");
            comboView.SelectedIndex = 0;

            UpdateClientGroup();

            ClearSymbols();

            UpdateSymbolOptions("");

            textSymbol.KeyPress += new KeyPressEventHandler(textSymbol_KeyPress);
            checkAutoResolve.Checked = true;

            UpdateControlsFromConnectionDiagnostics();

            checkTraceErrors.CheckedChanged += new EventHandler(checkDiagnostics_CheckedChanged);
            checkTraceWarnings.CheckedChanged += new EventHandler(checkDiagnostics_CheckedChanged);
            checkTraceInfo.CheckedChanged += new EventHandler(checkDiagnostics_CheckedChanged);
            checkTraceMessages.CheckedChanged += new EventHandler(checkDiagnostics_CheckedChanged);

            Connection.StatusChange += new Connection.StatusChangeEventHandler(Connection_StatusChange);
            //shashi
            this.textUsername.Text = "akaushal";
            this.textPassword.Text = "barchart";
            textSymbol.Text = "GOOG.BZ";
            this.checkInitialRefresh.Checked = true;
            this.checkStreamingData.Checked = true;
            //UpdateSymbols();
            //client = new OPClient();
        }

        private void checkDiagnostics_CheckedChanged(object sender, EventArgs e)
        {
            UpdateConnectionDiagnosticsFromControls();
        }

        private void checkAutoResolve_CheckedChanged(object sender, EventArgs e)
        {
            groupServerSettings.Enabled = !checkAutoResolve.Checked;
        }

        private void textServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonClearCurrentView_Click(object sender, EventArgs e)
        {
            ClearCurrentView();
        }

        private void buttonClearAllViews_Click(object sender, EventArgs e)
        {
            ClearAllViews();
        }

        private void comboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboView.SelectedIndex)
            {
                case 0:
                    rtbTrace.Visible = false;
                    listRaw.Visible = false;
                    listOHLC.Visible = false;
                    rtbBookQuote.Visible = false;
                    rtbQuote.Visible = true;
                    break;
                case 1:
                    rtbQuote.Visible = false;
                    rtbTrace.Visible = false;
                    listRaw.Visible = false;
                    listOHLC.Visible = false;
                    rtbBookQuote.Visible = true;
                    break;
                case 2:
                    rtbQuote.Visible = false;
                    rtbBookQuote.Visible = false;
                    rtbTrace.Visible = false;
                    listRaw.Visible = false;
                    listOHLC.Visible = true;
                    break;
                case 3:
                    rtbQuote.Visible = false;
                    rtbBookQuote.Visible = false;
                    listOHLC.Visible = false;
                    rtbTrace.Visible = false;
                    listRaw.Visible = true;
                    break;
                case 4:
                    rtbQuote.Visible = false;
                    rtbBookQuote.Visible = false;
                    listOHLC.Visible = false;
                    listRaw.Visible = false;
                    rtbTrace.Visible = true;
                    break;
                default:
                    rtbQuote.Visible = false;
                    rtbBookQuote.Visible = false;
                    listOHLC.Visible = false;
                    listRaw.Visible = false;
                    rtbTrace.Visible = false;
                    break;
            }
        }

        private void textSymbol_TextChanged(object sender, EventArgs e)
        {
            UpdateSymbolOptions(textSymbol.Text);
        }

        void textSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);

            if (char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void comboClientFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateClientFilter(comboClientFilter.Text);
        }

        private void textUsername_TextChanged(object sender, EventArgs e)
        {
            UpdateClientGroup();
        }

        void textUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void buttonUpdateSymbols_Click(object sender, EventArgs e)
        {
            UpdateSymbols();
        }

        private void gridSymbolList_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
        }

        private void gridSymbolList_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSymbolSelection();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _client.Symbols = "";
            Connection.Close();
        }

        private void buttonRemoveSymbol_Click(object sender, EventArgs e)
        {
            RemoveSymbol();
        }

        private void buttonClearSymbols_Click(object sender, EventArgs e)
        {
            ClearSymbols();
        }

        #endregion Form and Control Events

        #region Control Logic

        private void UpdateConnectionSettings()
        {
            Connection.Username = textUsername.Text;
            Connection.Password = textPassword.Text;
            Connection.Mode = comboProtocol.Text == "UDP" ? ConnectionMode.UDPClient : ConnectionMode.TCPClient;
            if (checkAutoResolve.Checked == false)
            {
                UpdateServerSettingsFromControls();
            }
            else
            {
                Connection.Server = "";
                Connection.Port = 0;
            }
        }

        private void UpdateControlsFromConnectionDiagnostics()
        {
            checkTraceErrors.Checked = (bool)Connection.Properties["traceerrors"];
            checkTraceWarnings.Checked = (bool)Connection.Properties["tracewarnings"];
            checkTraceInfo.Checked = (bool)Connection.Properties["traceinfo"];
            checkTraceMessages.Checked = !string.IsNullOrEmpty((string)Connection.Properties["messagetracefilter"]);
        }

        private void UpdateConnectionDiagnosticsFromControls()
        {
            Connection.Properties["traceerrors"] = checkTraceErrors.Checked;
            Connection.Properties["tracewarnings"] = checkTraceWarnings.Checked;
            Connection.Properties["traceinfo"] = checkTraceInfo.Checked;
            Connection.Properties["messagetracefilter"] = checkTraceMessages.Checked ? "*" : "";
        }

        private void UpdateServerSettingsFromControls()
        {
            Connection.Server = textServer.Text;
            try
            {
                Connection.Port = textPort.Text.Length > 0 ? ushort.Parse(textPort.Text) : (ushort)0;
            }
            catch
            {
                MessageBox.Show("Invalid port specified");
            }
        }

        private void UpdateControlsFromServerSettings()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(UpdateControlsFromServerSettings));
            }
            else
            {
                textServer.Text = Connection.Server;
                textPort.Text = Connection.Port.ToString();
            }
        }

        private void ClearSymbols()
        {
            gridSymbolList.Rows.Clear();
            _client.Symbols = "";
        }

        private void ClearCurrentView()
        {
            switch (comboView.SelectedIndex)
            {
                case 0:
                    rtbQuote.Clear();
                    break;
                case 1:
                    rtbBookQuote.Clear();
                    break;
                case 2:
                    listOHLC.Items.Clear();
                    break;
                case 3:
                    listRaw.Items.Clear();
                    break;
                case 4:
                    rtbTrace.Clear();
                    break;
            }
        }

        private void ClearAllViews()
        {
            listOHLC.Items.Clear();
            listRaw.Items.Clear();
            rtbTrace.Clear();
            rtbQuote.Clear();
            rtbBookQuote.Clear();
        }

        private void UpdateSymbolOptions(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                checkInitialRefresh.Enabled = false;
                checkStreamingData.Enabled = false;
                checkBookRefresh.Enabled = false;
                checkBookUpdates.Enabled = false;
                checkOHLCMinuteBars.Enabled = false;
                buttonUpdateSymbols.Enabled = false;
            }
            else
            {
                Symbol sym = new Symbol(symbol);
                checkBookUpdates.Enabled = sym.Type == Symbol.TypeConstants.Future;
                checkBookRefresh.Enabled = sym.Type == Symbol.TypeConstants.Future;
                checkStreamingData.Enabled = sym.Type != Symbol.TypeConstants.Invalid;
                checkInitialRefresh.Enabled = sym.Type != Symbol.TypeConstants.Invalid;
                checkOHLCMinuteBars.Enabled = sym.Type != Symbol.TypeConstants.Invalid;
                textSymbol.ForeColor = sym.Type == Symbol.TypeConstants.Invalid ? Color.Red : Color.Black;
                buttonUpdateSymbols.Enabled = sym.Type != Symbol.TypeConstants.Invalid;
            }
        }

        private void UpdateClientGroup()
        {
            if ((textUsername.Text.Length > 0) && (textPassword.Text.Length > 0))
            {
                groupClient.Enabled = true;
            }
            else
            {
                groupClient.Enabled = false;
            }
        }

        private void UpdateSymbols()
        {
            try
            {
                ddfplus.Symbol symbol = new ddfplus.Symbol(textSymbol.Text);
                bool doubleDigitYear = (symbol.Type == Symbol.TypeConstants.Future) && char.IsDigit(symbol.Name[symbol.Name.Length - 2]);
                if (!doubleDigitYear || (MessageBox.Show("Only single-digit years are supported in the feed for futures contracts.\n"
                    + "Would you still like to use this symbol?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                    == DialogResult.OK))
                {
                    AddSymbol(textSymbol.Text,
                        checkInitialRefresh.Enabled && checkInitialRefresh.Checked,
                        checkStreamingData.Enabled && checkStreamingData.Checked,
                        checkBookRefresh.Enabled && checkBookRefresh.Checked,
                        checkBookUpdates.Enabled && checkBookUpdates.Checked,
                        checkOHLCMinuteBars.Enabled && checkOHLCMinuteBars.Checked);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error updating symbols: {0}", ex.Message), 
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateSymbolsFromDB()
        {

            IList<OptionProcessingService.Types.SymbolNew> symbollist = DB.GetSymbolsNew();
            foreach (var dbsymbol in symbollist)
            {
                textSymbol.Text = dbsymbol.ShortName;
                    //TimestampUtility.BuildSymbolName(dbsymbol.ShortName, dbsymbol.Type == SecurityType.Forex ?
                    //CommonObjects.Unitity.Instrument.Forex : CommonObjects.Unitity.Instrument.Equity);
                ddfplus.Symbol symbol = new ddfplus.Symbol(textSymbol.Text);
                bool doubleDigitYear = (symbol.Type == ddfplus.Symbol.TypeConstants.Future) && char.IsDigit(symbol.Name[symbol.Name.Length - 2]);
                if (!doubleDigitYear || (MessageBox.Show("Only single-digit years are supported in the feed for futures contracts.\n"
                    + "Would you still like to use this symbol?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                    == DialogResult.OK))
                {
                    AddSymbol(textSymbol.Text,
                    checkInitialRefresh.Enabled && checkInitialRefresh.Checked,
                        checkStreamingData.Enabled && checkStreamingData.Checked,
                        checkBookRefresh.Enabled && checkBookRefresh.Checked,
                        checkBookUpdates.Enabled && checkBookUpdates.Checked,
                        checkOHLCMinuteBars.Enabled && checkOHLCMinuteBars.Checked);
                }
            }
        }
        private void AddSymbol(string symbol, bool initialRefresh, bool streamingData, bool bookRefresh, bool bookUpdates, bool ohlcBars)
        {
            string requests = "";
            if (checkInitialRefresh.Enabled && checkInitialRefresh.Checked)
                requests += "s";
            if (checkStreamingData.Enabled && checkStreamingData.Checked)
                requests += "S";
            if (checkBookRefresh.Enabled && checkBookRefresh.Checked)
                requests += "b";
            if (checkBookUpdates.Enabled && checkBookUpdates.Checked)
                requests += "B";
            if (checkOHLCMinuteBars.Enabled && checkOHLCMinuteBars.Checked)
                requests += "O";

            bool listUpdated = false;

            if (!initialRefresh && !streamingData && !bookRefresh && !bookUpdates && !ohlcBars)
            {
                DataGridViewRow row = FindRow(symbol);
                if (row != null)
                {
                    gridSymbolList.Rows.Remove(row);
                    listUpdated = true;
                }
            }
            else
            {
                DataGridViewRow row = FindRow(symbol);

                if (row == null)
                {
                    int newRow = gridSymbolList.Rows.Add();
                    row = gridSymbolList.Rows[newRow];
                }

                UpdateRowFromSymbol(row, symbol, initialRefresh, streamingData, bookRefresh, bookUpdates, ohlcBars);
                listUpdated = true;
            }

            if (listUpdated)
                UpdateSymbolList();
        }

        private DataGridViewRow FindRow(string symbol)
        {
            DataGridViewRow symbolRow = null;

            // Look for existing symbol in list
            foreach (DataGridViewRow row in gridSymbolList.Rows)
            {
                if ((string)row.Cells["colSymbol"].Value == symbol)
                {
                    symbolRow = row;
                    break;
                }
            }

            return symbolRow;
        }

        private void UpdateSymbolSelection()
        {
            bool rowSelected = false;

            foreach (DataGridViewRow row in gridSymbolList.Rows)
            {
                if (row.Selected && !string.IsNullOrEmpty((string)row.Cells["colSymbol"].Value))
                {
                    rowSelected = true;
                    // Update symbol entry with selected row
                    UpdateSymbolFromRow(row);
                    break;
                }
            }

            buttonRemoveSymbol.Enabled = rowSelected;
        }

        private void UpdateSymbolFromRow(DataGridViewRow row)
        {
            textSymbol.Text = (string)row.Cells["colSymbol"].Value;
            checkInitialRefresh.Checked = (bool)row.Cells["colRefresh"].Value;
            checkStreamingData.Checked = (bool)row.Cells["colStreaming"].Value;
            checkBookRefresh.Checked = (bool)row.Cells["colBookRefresh"].Value;
            checkBookUpdates.Checked = (bool)row.Cells["colBookUpdates"].Value;
            checkOHLCMinuteBars.Checked = (bool)row.Cells["colOHLCBars"].Value;
        }

        private void UpdateRowFromSymbol(DataGridViewRow row, string symbol, bool initialRefresh, bool streamingData, bool bookRefresh, bool bookUpdates, bool ohlcBars)
        {
            row.Cells["colSymbol"].Value = symbol;
            row.Cells["colRefresh"].Value = initialRefresh;
            row.Cells["colStreaming"].Value = streamingData;
            row.Cells["colBookRefresh"].Value = bookRefresh;
            row.Cells["colBookUpdates"].Value = bookUpdates;
            row.Cells["colOHLCBars"].Value = ohlcBars;
        }

        private void UpdateSymbolList()
        {
            string symbolList = "";

            foreach (DataGridViewRow row in gridSymbolList.Rows)
            {
                string symbol = SymbolFromRow(row);
                if (!string.IsNullOrEmpty(symbol))
                {
                    if (symbolList.Length > 0)
                        symbolList += ",";
                    symbolList += symbol;
                }
            }

            if (_status == Status.Disconnected)
            {
                // Set connection properties prior to connecting
                UpdateConnectionSettings();
            }

            _client.Symbols = symbolList;
        }

        private string SymbolFromRow(DataGridViewRow row)
        {
            string symbol = (string)row.Cells["colSymbol"].Value;

            if (!string.IsNullOrEmpty(symbol))
            {
                string requests = "";
                if ((bool)row.Cells["colRefresh"].Value)
                    requests += "s";
                if ((bool)row.Cells["colStreaming"].Value)
                    requests += "S";
                if ((bool)row.Cells["colBookRefresh"].Value)
                    requests += "b";
                if ((bool)row.Cells["colBookUpdates"].Value)
                    requests += "B";
                if ((bool)row.Cells["colOHLCBars"].Value)
                    requests += "O";

                if (requests.Length > 0)
                    symbol = symbol + "=" + requests;
            }

            return symbol;
        }

        private void RemoveSymbol()
        {
            bool rowSelected = false;

            foreach (DataGridViewRow row in gridSymbolList.Rows)
            {
                if (row.Selected && !string.IsNullOrEmpty((string)row.Cells["colSymbol"].Value))
                {
                    rowSelected = true;
                    gridSymbolList.Rows.Remove(row);
                    break;
                }
            }

            if (rowSelected)
                UpdateSymbolList();
        }

        private void UpdateClientFilter(string filter)
        {
            _client.Filter = (Filter)Enum.Parse(typeof(Filter), filter, true);
        }

        #endregion Control Logic

        #region ddf Event to UI Handlers

        void NewMessage(byte[] message)
        {
            ddfplus.Parser.Message ddfMessage = ddfplus.Parser.MessageParser.ParseMessage(new Stream(message));

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new NewMessageHandler(NewMessage), new object[] { message });
            }
            else
            {
                string messageType = "";

                if (ddfMessage is MessageTimestamp)
                    messageType = "Timestamp";
                else
                    messageType = "ddf Message";

                string messageContents = Encoding.ASCII.GetString(message);

                listRaw.Items.Add(string.Format("{0}: {1}", messageType, messageContents));
            }
        }

        void NewOHLCQuote(OHLCQuote quote)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new NewOHLCQuoteHandler(NewOHLCQuote), new object[] { quote });
            }
            else
            {
                listOHLC.Items.Add(OHLCQuoteToString(quote));
            }
        }
        
        void NewQuote(Quote quote)
        {
            try
            {
                if (DataFeedProcessor.DataServerClient != null)
                {
                    DataFeedProcessor.DataServerClient.NewTick(quote);
                }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new NewQuoteHandler(NewQuote), new object[] { quote });
                }
                else
                {
                    rtbQuote.Text = QuoteToString(quote);
                }
            }catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }

        void NewBookQuote(BookQuote quote)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new NewBookQuoteHandler(NewBookQuote), new object[] { quote });
            }
            else
            {
                rtbBookQuote.Text = BookQuoteToString(quote);
            }
        }

        private void UpdateConnectionStatus(Status status)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateStatusHandler(UpdateConnectionStatus), new object[] { status });
            }
            else
            {
                labelStatus.Text = status.ToString();
                groupConnection.Enabled = status == Status.Disconnected;
                //comboClientFilter.Enabled = status == Status.Disconnected;
                textServer.Text = Connection.Server;
                textPort.Text = Connection.Port.ToString();
            }

        }

        #endregion ddf Event to UI Handlers

        #region ddf Client Event Handlers

        void _client_NewMessage(object sender, Client.NewMessageEventArgs e)
        {
            NewMessage(e.Message);
        }

        void _client_NewOHLCQuote(object sender, Client.NewOHLCQuoteEventArgs e)
        {
            NewOHLCQuote(e.OHLCQuote);
        }

        void _client_NewTimestamp(object sender, Client.NewTimestampEventArgs e)
        {
            // 
        }

        void _client_NewBookQuote(object sender, Client.NewBookQuoteEventArgs e)
        {
            NewBookQuote(e.BookQuote);
        }

        void _client_NewQuote(object sender, Client.NewQuoteEventArgs e)
        {
            NewQuote(e.Quote);
        }

        #endregion ddf Client Event Handlers

        #region ddf Connection Event Handlers

        void Connection_StatusChange(object sender, ddfplus.Net.StatusChangeEventArgs e)
        {
            UpdateConnectionStatus(e.NewStatus);
        }

        #endregion ddf Connection Event Handlers

        #region ddf Initialization

        private void InitializeDdf()
        {
            // The streaming version must be set prior to creating any clients expected to work for that version
            Connection.Properties["streamingversion"] = "3";

            _client = new Client();
            _client.NewQuote += new Client.NewQuoteEventHandler(_client_NewQuote);
            _client.NewBookQuote += new Client.NewBookQuoteEventHandler(_client_NewBookQuote);
            _client.NewTimestamp += new Client.NewTimestampEventHandler(_client_NewTimestamp);
            _client.NewOHLCQuote += new Client.NewOHLCQuoteEventHandler(_client_NewOHLCQuote);
            _client.NewMessage += new Client.NewMessageEventHandler(_client_NewMessage);
        }

        #endregion ddf Initialization

        #region ddf Object Serialization

        private string QuoteToString(Quote q)
        {
            string res = "";

            try
            {
                string quote = string.Format("Symbol={0}\nUpdate Source={1}\nTimestamp={2}\nAsk={3}\nAskSize={4}\nBid={5}\nBidSize={6}\nChange={7}\nExchange={8}\nPermission={9}\nRecord={10}\nSubrecord={11}\nElement={12}\nModifier={13}\n-Source:{14}\n",
                    q.Symbol,
                    QuoteUpdateSource(q),
                    q.Timestamp.ToString("MM/dd/yyyy HH:mm:ss.fff"),
                    q.FormatValue(q.Ask, NumericFormat.Default),
                    q.AskSize.ToString(),
                    q.FormatValue(q.Bid, NumericFormat.Default),
                    q.FormatValue(q.BidSize, NumericFormat.Default),
                    q.FormatValue(q.Change, NumericFormat.Default),
                    q.Exchange != '\0' ? q.Exchange.ToString() : "",
                    q.Permission.ToString(),
                    q.Record != '\0' ? q.Record.ToString() : "",
                    q.Subrecord != '\0' ? q.Subrecord.ToString() : "",
                    q.Element != '\0' ? q.Element.ToString() : "",
                    q.Modifier != '\0' ? q.Modifier.ToString() : "",
                    Encoding.ASCII.GetString(q.Source));

                Session c = q != null ? q.Sessions["combined"] : null;

                string combined = "Combined session:\n"
                    + (null == c ? "No data\n" : SessionToString(c, q.Unitcode));

                Session p = q != null ? q.Sessions["previous"] : null;

                string previous = "Previous session:\n"
                    + (null == p ? "No data\n" : SessionToString(p, q.Unitcode));

                res = quote + combined + previous;
            }
            catch (Exception ex)
            {
                res = "Error converting data from quote to string: " + ex.Message;
            }

            return res;
        }

        private string SessionToString(Session s, char unitcode)
        {
            string res = "";

            try
            {
                if (s != null)
                {
                    res = string.Format("Timestamp={0}\nLast={1}\nLastSize={2}\nOpen1={3}\nOpen2={4}\nHigh={5}\nLow={6}\nClose1={7}\nClose2={8}\nVolume={9}\nOpenInterest={10}\nTickTrend={11}\nTradeSession={12}\nDay={13}\n",
                        s.Timestamp.ToString("MM/dd/yyyy HH:mm:ss.fff"),
                        Quote.FormatValue(s.Last, NumericFormat.Default, unitcode),
                        s.LastSize,
                        Quote.FormatValue(s.Open1, NumericFormat.Default, unitcode),
                        Quote.FormatValue(s.Open2, NumericFormat.Default, unitcode),
                        Quote.FormatValue(s.High, NumericFormat.Default, unitcode),
                        Quote.FormatValue(s.Low, NumericFormat.Default, unitcode),
                        Quote.FormatValue(s.Close1, NumericFormat.Default, unitcode),
                        Quote.FormatValue(s.Close2, NumericFormat.Default, unitcode),
                        s.Volume,
                        s.OpenInterest,
                        s.TickTrend,
                        s.TradeSession,
                        s.Day.ToString());
                }
            }
            catch (Exception ex)
            {
                res = "Error converting data from session to string: " + ex.Message;
            }

            return res;
        }

        private string BookQuoteToString(BookQuote q)
        {
            string res = "";

            try
            {
                int askCount = Math.Min(q.AskPrices.Length, q.AskPrices.Length);
                string askItems = "";
                for (int i = 0; i < askCount; i++)
                {
                    if (askItems.Length > 0)
                        askItems += ",";
                    askItems += string.Format("{0}x{1}", q.FormatValue(q.AskPrices[i], NumericFormat.Default), q.AskSizes[i]);
                }

                int bidCount = Math.Min(q.BidPrices.Length, q.BidPrices.Length);
                string bidItems = "";
                for (int i = 0; i < bidCount; i++)
                {
                    if (bidItems.Length > 0)
                        bidItems += ",";
                    bidItems += string.Format("{0}x{1}", q.FormatValue(q.BidPrices[i], NumericFormat.Default), q.BidSizes[i]);
                }

                res = string.Format("Symbol={0}\nTimestamp={1}\nAsks={2}\nBids={3}\nSource={4}\n",
                    q.Symbol, q.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), askItems, bidItems, Encoding.ASCII.GetString(q.Source));
            }
            catch (Exception ex)
            {
                res = "Error converting data from book quote to string: " + ex.Message;
            }

            return res;
        }

        private string OHLCQuoteToString(OHLCQuote q)
        {
            string res = "";

            try
            {
                res = string.Format("{0} @ {1}: Day={2},Open={3},High={4},Low={5},Close={6},Volume={7}",
                                    q.Symbol, q.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                                    q.Day,
                                    q.FormatValue(q.Open, NumericFormat.Default),
                                    q.FormatValue(q.High, NumericFormat.Default),
                                    q.FormatValue(q.Low, NumericFormat.Default),
                                    q.FormatValue(q.Close, NumericFormat.Default),
                                    q.Volume);
            }
            catch (Exception ex)
            {
                res = "Error converting data from OHLC quote to string: " + ex.Message;
            }

            return res;
        }

        private string QuoteUpdateSource(Quote q)
        {
            string source = "";

            if (q.IsRefresh && q.IsTrade)
                source = "trade/refresh message";
            else if (q.IsRefresh)
                source = "refresh message";
            else if (q.IsTrade)
                source = "trade message";
            else if ((q.Record == '2') && (q.Subrecord == '8'))
                source = "bid/ask message";
            else
                source = string.Format("{0}.{1} type message", q.Record, q.Subrecord);

            return source;
        }

        #endregion ddf Object Serialization

        public OPClient client;
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                client = new OPClient();
                
                UpdateSymbolsFromDB();
                MessageBox.Show("Server Started Successfully");
                Logger.Info("Server Started Successfully");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.Error(ex);
            }
        }
    }
}
