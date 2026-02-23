using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
// Rebex Namespaces
using Rebex.Net;
using Rebex.TerminalEmulation;
// Project Namespaces
using RACTCommon.Data;
using RACTSerialProcess; 
using RACTTerminal;
using RACTClient.SubForm;
using System.Threading;

namespace RACTClient
{
    // Class name to match plan
    public partial class RebexTerminalControl : SenderControl, ISerialEmulator, ITelnetEmulator, ITactTerminal
    {
        private Rebex.TerminalEmulation.TerminalControl _rebexTerminal;
        private Ssh _ssh;
        private Telnet _telnet;
        private Connectivity.SshTunnelManager _tunnelManager = new Connectivity.SshTunnelManager();

        public DeviceInfo DeviceInfo { get; set; }
        public DeviceInfo JumpHost { get; set; }
        
        private E_TerminalStatus _terminalStatus;
        public E_TerminalStatus TerminalStatus 
        { 
            get => _terminalStatus;
            set 
            {
                _terminalStatus = value;
                OnTerminalStatusChange?.Invoke(this, value);
            }
        }

        public string ToolTip 
        { 
            get 
            { 
                if (DeviceInfo != null)
                {
                    // Logic similar to MCTerminalEmulator
                    if (string.IsNullOrEmpty(DeviceInfo.TpoName))
                    {
                        return DeviceInfo.DeviceName;
                    }
                    return string.Format("{0}({1})", DeviceInfo.DeviceName, DeviceInfo.TpoName);
                }
                return string.Empty;
            } 
        }

        public ConnectionTypes ConnectionType
        {
            get
            {
                if (DeviceInfo != null && DeviceInfo.TerminalConnectInfo != null)
                {
                    switch (DeviceInfo.TerminalConnectInfo.ConnectionProtocol)
                    {
                        case E_ConnectionProtocol.TELNET:
                            return ConnectionTypes.RemoteTelnet; // Or decide based on logic
                        case E_ConnectionProtocol.SSH:
                            return ConnectionTypes.RemoteTelnet; // Map appropriately
                        case E_ConnectionProtocol.SERIAL:
                            return ConnectionTypes.Serial;
                    }
                }
                return ConnectionTypes.LocalTelnet;
            }
        }

        public string ComPort { get { return "COM1"; } } // Dummy implementation for now

        public RebexTerminalControl()
        {
            InitializeComponent();
            InitializeRebexTerminal();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "RebexTerminalControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
        }

        private void InitializeRebexTerminal()
        {
            _rebexTerminal = new Rebex.TerminalEmulation.TerminalControl();
            _rebexTerminal.Dock = DockStyle.Fill;
            this.Controls.Add(_rebexTerminal);
        }

        public object ConnectDevice(object aDeviceInfo)
        {
            return ConnectDevice(aDeviceInfo as DeviceInfo, null);
        }

        public void ConnectDevice(DeviceInfo target, DeviceInfo jumpHost)
        {
            if (target == null) return;

            this.DeviceInfo = target;
            this.JumpHost = jumpHost;
            this.TerminalStatus = E_TerminalStatus.TryConnection;

            try
            {
                string connectIp = target.IPAddress;
                int connectPort = target.TerminalConnectInfo.TelnetPort;

                // Jump Host  처리
                if (jumpHost != null)
                {
                    _tunnelManager.OpenTunnel(jumpHost, target);
                    connectIp = "127.0.0.1";
                    connectPort = _tunnelManager.LocalBoundPort;
                }

                if (target.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    ConnectTelnet(connectIp, connectPort);
                }
                else if (target.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSH || target.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    ConnectSsh(connectIp, connectPort, target.TelnetID1, target.TelnetPwd1);
                }
                
                this.TerminalStatus = E_TerminalStatus.Connection;
            }
            catch (Exception ex)
            {
                this.TerminalStatus = E_TerminalStatus.Disconnected;
                AppGlobal.s_FileLogProcessor?.PrintLog(E_FileLogType.Error, $" ӽ  н : {ex.Message}");
            }
        }

        public void ConnectTelnet(string server, int port)
        {
            try 
            {
                Disconnect();

                _telnet = new Telnet(server, port);
                _rebexTerminal.Bind(_telnet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConnectSsh(string server, int port, string user, string password)
        {
            try
            {
                Disconnect();

                _ssh = new Ssh();
                _ssh.Connect(server, port);
                if (!string.IsNullOrEmpty(user))
                {
                    _ssh.Login(user, password);
                }
                _rebexTerminal.Bind(_ssh);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ISerialEmulator / ITerminal Implementation
        public void DisplayResult(SerialCommandResultInfo aResult)
        {
            if (aResult != null && !string.IsNullOrEmpty(aResult.ReceivedData))
            {
                _rebexTerminal.Writer.Write(aResult.ReceivedData);
            }
        }

        // ITelnetEmulator Implementation
        public void DisplayResult(TelnetCommandResultInfo tResult)
        {
            if (tResult != null && tResult.ReslutType == E_TelnetReslutType.Data)
            {
                 if (tResult.CommandResult != null)
                 {
                     _rebexTerminal.Writer.Write(tResult.CommandResult.ToString());
                 }
            }
        }
        
         public void DisplayResult(int aSessionID, string aResult)
        {
            _rebexTerminal.Writer.Write(aResult);
        }


        public override void ResultReceiver(ResultCommunicationData vResult)
        {
            base.ResultReceiver(vResult);
        }

        public bool IsConnected
        {
            get
            {
                if (_telnet != null) return _telnet.IsConnected;
                if (_ssh != null) return _ssh.IsConnected;
                return false;
            }
        }

        public void Disconnect()
        {
             if (_telnet != null)
             {
                if (_telnet.IsConnected) _telnet.Disconnect();
                _telnet = null;
             }
             if (_ssh != null)
             {
                 if (_ssh.IsConnected) _ssh.Disconnect();
                 _ssh = null;
             }
        }

        public event HandlerArgument2<object, E_TerminalStatus> OnTerminalStatusChange;
        public event DefaultHandler OnTelnetFindString;
        public event DefaultHandler CallOptionHandlerEvent;
        public event HandlerArgument3<String, eProgressItemType, bool> ProgreBarHandlerEvent;

        public void ScriptWork(E_ScriptWorkType aType, object aScript)
        {
             // Placeholder
        }

        public int ConnectedSessionID { get { return -1; } }
        public bool IsQuickConnection { get; set; }
        public E_TerminalMode TerminalMode { get; set; }

        public void RunScript(Script aScript) 
        { 
             // Placeholder for script execution
        }

        public void ExecTerminalScreen(E_TerminalScreenTextEditType aEditType) 
        { 
             // Placeholder for terminal screen commands (Copy/Paste etc can be mapped to Rebex)
             switch(aEditType)
             {
                 case E_TerminalScreenTextEditType.Copy:
                     _rebexTerminal.Copy();
                     break;
                 case E_TerminalScreenTextEditType.Paste:
                     _rebexTerminal.Paste();
                     break;
                 case E_TerminalScreenTextEditType.SelectAll:
                     _rebexTerminal.SelectAll();
                     break;
             }
        }

        public void ChangeClientMode() 
        { 
             // Placeholder
        }

        public void WriteTerminalLog() 
        { 
             // Placeholder
        }

        public void ApplyOption() 
        { 
             // Placeholder for applying settings (font, colors)
        }

        public void FindForm_Close() 
        { 
             // Placeholder
        }

        public void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aArgs) 
        { 
             // Placeholder
        }

        public void IsLimitCmdForShortenCommand(object aSender, string aText)
        {
             // Placeholder for now, can implement logic to send command if needed
             // Similar to MCTerminalEmulator logic
             if (IsConnected)
             {
                 // Send command
                 if (_telnet != null && _telnet.IsConnected) _telnet.Send(aText);
                 if (_ssh != null && _ssh.IsConnected) _ssh.Send(aText);
             }
        }
    }
}
