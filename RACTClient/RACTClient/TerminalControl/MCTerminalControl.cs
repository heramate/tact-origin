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

namespace RACTClient
{
    // Class name must match existing code expectations
    public partial class MCTerminalEmulator : SenderControl, ISerialEmulator, ITelnetEmulator
    {
        private Rebex.TerminalEmulation.TerminalControl _rebexTerminal;
        private Ssh _ssh;
        private Telnet _telnet;

        // Interface implementation for ISerialEmulator
        public string ComPort { get { return "COM1"; } } // Dummy implementation for now

        public MCTerminalEmulator()
        {
            InitializeComponent(); // Ensure InitializeComponent exists or remove if not needed (Designer might be missing)
            InitializeRebexTerminal();
        }

        // IMPORTANT: If 'MCTerminalControl.Designer.cs' does not exist, InitializeComponent might be missing.
        // We will define a dummy one if needed, or rely on manual setup.
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "MCTerminalEmulator";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
        }

        private void InitializeRebexTerminal()
        {
            _rebexTerminal = new Rebex.TerminalEmulation.TerminalControl();
            _rebexTerminal.Dock = DockStyle.Fill;
            this.Controls.Add(_rebexTerminal);
        }

        public void ConnectTelnet(string server, int port)
        {
            try 
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

                _telnet = new Telnet(server, port);
                // Basic terminal options
                _rebexTerminal.Bind(_telnet);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Telnet Connection Error: " + ex.Message);
            }
        }

        public void ConnectSsh(string server, int port, string user, string password)
        {
            try
            {
                if (_ssh != null) 
                {
                    if (_ssh.IsConnected) _ssh.Disconnect();
                    _ssh = null;    
                }
                if (_telnet != null)
                {
                   if (_telnet.IsConnected) _telnet.Disconnect();
                   _telnet = null;
                }

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
                MessageBox.Show("SSH Connection Error: " + ex.Message);
            }
        }

        // ISerialEmulator Implementation
        public void DisplayResult(SerialCommandResultInfo aResult)
        {
            if (aResult != null && !string.IsNullOrEmpty(aResult.ReceivedData))
            {
                // Write received data to the Rebex terminal
                _rebexTerminal.Writer.Write(aResult.ReceivedData);
            }
        }

        // ITelnetEmulator Implementation
        public void DisplayResult(TelnetCommandResultInfo tResult)
        {
            if (tResult != null && tResult.ReslutType == E_TelnetReslutType.Data)
            {
                 // Check base class for data property if specific one is missing in TelnetCommandResultInfo
                 // Assuming CommandResult property from base class CommandResultItem
                 if (tResult.CommandResult != null)
                 {
                     _rebexTerminal.Writer.Write(tResult.CommandResult.ToString());
                 }
            }
        }

        // SenderControl Overrides (if any required)
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

        public void RunScript(object scriptInfo)
        {
            // Placeholder for script execution logic
            // Rebex supports scripting via Rebex.Net.Scripting
            // Ensure scriptInfo is of expected type before usage
            MessageBox.Show("Script execution not yet fully implemented for Rebex.");
        }
    }
}
