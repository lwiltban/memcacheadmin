using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using MemcacheAdmin.Interfaces;
using MemcacheAdmin.Models;

namespace MemcacheAdmin.Security
{
    public class MemcacheUser : IUser
    {
        [XmlElement]
        public string IpAddress { get; set; }

        [XmlIgnore]
        public IIdentity Identity { get; private set; }

        public void Authenticate(string userName, object propertyValues)
        {
            Identity = new UserIdentity { IsAuthenticated = userName.Length > 0, Name = userName, Properties = propertyValues };

        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public void Save(bool persist)
        {
            Authentication.SerializeUser(new System.Web.HttpContextWrapper(HttpContext.Current), this, persist);
        }

        public string toXml()
        {
            XmlSerializer s = new XmlSerializer(this.GetType());
            var sb = new StringBuilder();
            using (TextWriter w = new StringWriter(sb))
            {
                s.Serialize(w, this);
                w.Close();
            }
            return sb.ToString();
        }

        public static IUser Load(LoginModel model)
        {
            var user = new MemcacheUser();
            user.Identity = new UserIdentity();

            return user;
        }
    }
}