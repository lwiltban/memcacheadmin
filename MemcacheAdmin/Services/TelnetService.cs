using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MemcacheAdmin.Interfaces;
using MinimalisticTelnet;

namespace MemcacheAdmin.Services
{
    public class TelnetService : ITelnet, IDisposable
    {
        TelnetConnection Connection { get; set; }

        public void Dispose()
        {

        }

        public TelnetService(string serverName, int serverPort)
        {
            Connection = new TelnetConnection(serverName, serverPort);            
        }

        public ITelnet Connect(string serverName, int serverPort)
        {
            Connection = new TelnetConnection(serverName, serverPort);
            return this;
        }

        public void Disconnect(ITelnet telnet)
        {            
        }

        public string Read()
        {
            return Connection.Read();
        }

        public void Write(string cmd)
        {
            Connection.Write(cmd);
        }

        public void WriteLine(string cmd)
        {
            Connection.WriteLine(cmd);
        }

    }
}