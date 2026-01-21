using System;
using System.Collections.Generic;
using System.Text;

namespace RACTDaemonLauncher
{
    [Serializable]
    public class HealthCheckProcessCollection : Dictionary<string, HealthCheckProcess>
    {
        public HealthCheckProcessCollection() { }
    }
}
