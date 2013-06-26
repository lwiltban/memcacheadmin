using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinimalisticTelnet;

namespace MemcacheAdmin.Models
{
    public class ServersModel
    {
        public virtual ICollection<Server> Servers { get; set; }
    }
}