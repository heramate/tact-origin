using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RACTClient.Utilities
{
    public enum E_TunnelErrorType { PoolFull, ServerRefused, Timeout, Unknown }

    public class TunnelException : Exception
    {
        public E_TunnelErrorType ErrorType { get; }
        public TunnelException(E_TunnelErrorType type, string message) : base(message)
        {
            ErrorType = type;
        }
    }
}
