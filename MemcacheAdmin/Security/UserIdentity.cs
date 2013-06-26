using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Xml.Serialization;

namespace MemcacheAdmin.Security
{
    public class UserIdentity : IIdentity
    {
        #region IIdentity Members

        [XmlIgnore]
        public string AuthenticationType { get; internal set; }

        [XmlIgnore]
        public bool IsAuthenticated { get; internal set; }

        [XmlIgnore]
        public string Name { get; internal set; }

        [XmlIgnore]
        public object Properties{ get; internal set; }

        #endregion
    }
}