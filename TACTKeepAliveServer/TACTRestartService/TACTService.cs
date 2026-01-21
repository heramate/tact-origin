using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.ComponentModel;

namespace TACTRestartService
{
    public class TACTService
    {
        [DefaultValue("")]
        public string ServiceName { get; set; }

        [DefaultValue("")]
        public string ProcessName { get; set; }

        [DefaultValue("")]
        public string Description { get; set; }

        public TACTService() { }
        public TACTService(string aServiceName, string aProcessName, string aDescription)
        {
            ServiceName = aServiceName;
            ProcessName = aProcessName;
            Description = aDescription;
        }

        public string _ToString()
        {
            return string.Format("ServiceName={0}, ProcessName={1}, Description={2}", ServiceName, ProcessName, Description);
        }

    }

    public class TACTServiceList : List<TACTService>
    {
    }
}
