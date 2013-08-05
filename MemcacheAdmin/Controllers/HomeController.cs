using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using MemcacheAdmin.DAL;
using MemcacheAdmin.Models;
using MemcacheAdmin.Services;
using MinimalisticTelnet;
using Wise.ServiceRepository;

namespace MemcacheAdmin.Controllers
{
    public class HomeController : Controller
    {
        Dictionary<string, Server> _Servers;
        private ServerContext db = new ServerContext();

        private void InitServers()
        {
            if (HttpContext.Cache.Get("Servers") == null)
            {
                _Servers = new Dictionary<string, Models.Server>();
                var serverLine = ConfigurationManager.AppSettings["MemcacheServers"];
                string[] servers = serverLine.Split(',');
                int index = 0;
                foreach (string server in servers)
                {
                    string[] address = server.Split(':');
                    var name = string.Format("{0}", address[0]);
                    var serverObj = new Server { ServerID = index++, Name = name, IPAddress = address[1], Port = Int32.Parse(address[2]) };
                    _Servers.Add(name, serverObj);
                }
                HttpContext.Cache.Insert("Servers", _Servers, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 0), CacheItemPriority.Default, null);
            }
            else
            {
                _Servers = HttpContext.Cache.Get("Servers") as Dictionary<string, Server>;
            }
        }
        
        public ActionResult Index()
        {
            HttpContext.Response.Redirect("/Home/Servers");
            ViewBag.Title = "MemcacheAdmin";
            ViewBag.Message = "Memcached Servers"; 

            InitServers();
            ViewBag.Items = new List<Server>();
            foreach (KeyValuePair<string, Server> server in _Servers)
            {
                ViewBag.Items.Add(server.Value);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "MemcacheAdmin";

            return View();
        }

        [HttpPost]
        public JsonResult AjaxLookup(string serverId, string slabId, string key)
        {
            InitServers();
            Server current = _Servers[serverId];
            if (current != null)
            {
                var line = current.Get(key);
                JsonResult result = new JsonResult()
                {
                    Data = new
                    {
                        id = key,
                        value = line,
                        code = line != null
                    }
                };
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return result;
            }

            return null;

        }

        [HttpDelete]
        public JsonResult AjaxDelete(string serverId, string slabId, string key)
        {
            InitServers();
            Server current = _Servers[serverId];
            if (current != null)
            {
                bool rc = current.Delete(key);
                return new JsonResult()
                {
                    Data = new
                    {
                        id = key,
                        code = rc
                    }
                };
            }
            return null;
        }

        public ActionResult Servers()
        {
            ViewBag.Title = "MemcacheAdmin";
            ViewBag.Message = "Memcached Servers";

            InitServers();
            ViewBag.Items = new List<Server>();
            foreach (KeyValuePair<string, Server> server in _Servers)
            {
                ViewBag.Items.Add(server.Value);
            }
            return View();
        }

        public ActionResult Server(string id)
        {
            ViewBag.Title = "MemcacheAdmin";
            ViewBag.Message = "Server: " + id;
            Server current = null;
            try
            {
                InitServers();
                current = _Servers[id];
                if (current != null && current.getSlabs())
                {
                    ViewBag.Items = current.Slabs;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Exception " + ex.Message;
            }
            return View(current);
        }

        public ActionResult Slab(string serverId, string slabId)
        {
            ViewBag.Title = "MemcacheAdmin";
            ViewBag.Message = "Slab: " + slabId;
            Slab slab = null;
            try
            {
                InitServers();
                Server current = _Servers[serverId];
                if (current != null)
                {
                    slab = current.Slabs[slabId];
                    slab.getItems();
                    ViewBag.Items = slab.Items;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Exception " + ex.Message;
            }
            return View(slab);
        }

    }
}
