using System;
using RACTCommon.Data;
using RACTSerialProcess;

namespace RACTClient
{
    public interface ITerminal : ISerialEmulator
    {
        bool IsConnected { get; }
        DeviceInfo DeviceInfo { get; set; }
        string ToolTip { get; }
        ConnectionTypes ConnectionType { get; }
        object ConnectDevice(object aDeviceInfo);
        void ConnectDevice(DeviceInfo target, DeviceInfo jumpHost);
        void Disconnect();
        // void DisplayResult(SerialCommandResultInfo aResult); // Inherited from ISerialEmulator
        void DisplayResult(int aSessionID, string aResult);
        void ScriptWork(E_ScriptWorkType aType, object aScript);
        
        // Members added for TerminalPanel compatibility
        DeviceInfo JumpHost { get; set; }
        E_TerminalStatus TerminalStatus { get; set; }
        int ConnectedSessionID { get; }
        bool IsQuickConnection { get; }
        E_TerminalMode TerminalMode { get; set; }

        void RunScript(Script aScript);
        void ExecTerminalScreen(E_TerminalScreenTextEditType aEditType);
        void ChangeClientMode();
        void WriteTerminalLog();
        void ApplyOption();
        void FindForm_Close();
        void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aArgs);

        event HandlerArgument2<object, E_TerminalStatus> OnTerminalStatusChange;
        event DefaultHandler OnTelnetFindString;
        event DefaultHandler CallOptionHandlerEvent;
        event HandlerArgument3<String, eProgressItemType, bool> ProgreBarHandlerEvent;
        
        void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aArgs);
        void IsLimitCmdForShortenCommand(object aSender, string aText);
        
        // Needed for compatibility with TerminalPanel usage (as Control)
        string Name { get; set; }
        System.Windows.Forms.Control Parent { get; set; }
        void BringToFront();
        void Show();
        void Dispose();
    }
}
