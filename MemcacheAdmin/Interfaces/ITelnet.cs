using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemcacheAdmin.Interfaces
{
    public interface ITelnet
    {
        ITelnet Connect(string serverName, int serverPort);
        void Disconnect(ITelnet telnet);

        string Read();
        void Write(string cmd);
        void WriteLine(string cmd);
    }
}
