using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinimalisticTelnet;

namespace MemcacheAdmin.Models
{
    public class Slab
    {
        // STAT items:1:number 1
        public Server Server { get; set; }
        public string Id { get; set; }
        public int Count { get; set; }
        public int Age { get; set; }
        public int Evicted { get; set; }
        public int OutOfMemory { get; set; }
        public Dictionary<string, Item> Items;
        public Slab(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] parts = line.Split(' ');
                if (parts.Length == 3)
                {
                    string[] items = parts[1].Split(':');
                    Id = items[1];
                }
            }
        }

        public void Parse(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] parts = line.Split(' ');
                if (parts.Length == 3)
                {
                    string[] items = parts[1].Split(':');
                    if (Id.CompareTo(items[1]) != 0)
                        throw new Exception("Bad slab parse");

                    switch (items[2])
                    {
                        case "number":
                            Count = Int32.Parse(parts[2]);
                            break;
                        case "age":
                            Age = Int32.Parse(parts[2]);
                            break;
                        case "evicted":
                            Evicted = Int32.Parse(parts[2]);
                            break;
                        case "outofmemory":
                            OutOfMemory = Int32.Parse(parts[2]);
                            break;
                    }
                }
            }
        }

        public bool getItems()
        {
            Items = new Dictionary<string, Item>();
            TelnetConnection tc = new TelnetConnection(Server.IPAddress, Server.Port);

            tc.WriteLine(string.Format("stats cachedump {0} {1}", Id, Count));
            string line = tc.Read();
            string[] parts = line.Split('\n');
            foreach (var part in parts)
            {
                var trim = part.Trim();
                if (trim.CompareTo("END") == 0)
                    break;
                Item item = new Item(trim);
                Items.Add(item.Name, item);
            }
            return true;
        }
    }
}