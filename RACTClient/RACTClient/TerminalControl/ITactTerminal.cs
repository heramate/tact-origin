using DevComponents.DotNetBar;
using RACTCommonClass;
using RACTSerialProcess;
using RACTTerminal;
using System;
using System.Windows.Forms;

namespace RACTClient
{
    public interface ITactTerminal : ISerialEmulator
    {
        Control UIControl { get; } // 컨트롤 객체를 가져오는 속성
        bool IsConnected { get;  }
        DeviceInfo DeviceInfo { get; set; }
        string ToolTip { get; }
        ConnectionTypes ConnectionType { get; set; }
        object ConnectDevice(object aDeviceInfo);
        void Disconnect();
        // void DisplayResult(SerialCommandResultInfo aResult); // Inherited from ISerialEmulator
        void DisplayResult(int aSessionID, string aResult);
        
        // Members added for TerminalPanel compatibility
        int ConnectedSessionID { get; }
        bool IsQuickConnection { get; }
        E_TerminalMode TerminalMode { get; set; }

        void RunScript(Script aScript);
        void WriteTerminalLog();
        void ApplyOption();
        void FindForm_Close();
        void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aArgs);

        event HandlerArgument2<ITactTerminal, E_TerminalStatus> OnTerminalStatusChange;
        event DefaultHandler OnTelnetFindString;
        event DefaultHandler CallOptionHandlerEvent;
        event HandlerArgument3<String, eProgressItemType, bool> ProgreBarHandlerEvent;
        void IsLimitCmdForShortenCommand(object aSender, string aText);
        
        // Needed for compatibility with TerminalPanel usage (as Control)
        string Name { get; set; }
        System.Windows.Forms.Control Parent { get; set; }
        E_TerminalStatus TerminalStatus { get; set; }
        Mode Modes { get; set; }
        DaemonProcessInfo DaemonProcessInfo { get; set; }

        void BringToFront();
        void Show();
        void Dispose();
        void ScriptWork(E_ScriptWorkType aScriptWorkType);
        void ChangeClientMode();
        void ExecTerminalScreen(E_TerminalScreenTextEditType aEditType);
    }
}
