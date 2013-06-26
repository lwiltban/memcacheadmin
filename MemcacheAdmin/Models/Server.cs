using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinimalisticTelnet;

namespace MemcacheAdmin.Models
{
    public class Server
    {
        public int ServerID { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public Dictionary<string, Slab> Slabs { get; set; }

        public object Get(string key)
        {
            TelnetConnection tc = new TelnetConnection(IPAddress, Port);
            tc.WriteLine("get " + key);
            string line = tc.Read().Trim();
            CacheItem item = new CacheItem();
            return item.Parse(key, line);
        }

        public bool Delete(string key)
        {
            TelnetConnection tc = new TelnetConnection(IPAddress, Port);
            tc.WriteLine("delete " + key);
            string line = tc.Read().Trim();
            return line.CompareTo("DELETED") == 0;
        }

        public bool getSlabs()
        {
            Slabs = new Dictionary<string, Slab>();

            TelnetConnection tc = new TelnetConnection(IPAddress, Port);
            tc.WriteLine("stats items");
            string line = tc.Read();
            string[] parts = line.Split('\n');
            foreach (var part in parts)
            {
                var trim = part.Trim();
                if (trim.CompareTo("END") == 0)
                    break;
                Slab slab = new Slab(trim);
                Slab test;
                if (!Slabs.TryGetValue(slab.Id, out test))
                {
                    Slabs.Add(slab.Id, slab);
                }
                else
                {
                    slab = test;
                }
                slab.Parse(trim);
                slab.Server = this;
            }

            return true;
        }
    }
}