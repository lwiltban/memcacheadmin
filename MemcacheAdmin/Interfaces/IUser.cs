using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MemcacheAdmin.Interfaces
{
    public interface IUser : IPrincipal
    {
        void Authenticate(string userName, object propertyValues);
    }
}
