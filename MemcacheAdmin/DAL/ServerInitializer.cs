using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MemcacheAdmin.Models;

namespace MemcacheAdmin.DAL
{
    public class ServerInitializer : DropCreateDatabaseIfModelChanges<ServerContext>
    {
        protected override void Seed(ServerContext context)
        {
            List<Server> Servers = new List<Server>();
            var serverLine = ConfigurationManager.AppSettings["MemcacheServers"];
            string[] servers = serverLine.Split(',');
            int index = 0;
            foreach (string server in servers)
            {
                string[] address = server.Split(':');
                var name = string.Format("{0}", address[0]);
                var serverObj = new Server { ServerID = index++, Name = name, IPAddress = address[1], Port = Int32.Parse(address[2]) };
                Servers.Add(serverObj);
            }

            Servers.ForEach(s => context.Servers.Add(s));
            context.SaveChanges();
        }
    }
}