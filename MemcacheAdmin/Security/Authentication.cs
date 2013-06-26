using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MemcacheAdmin.Interfaces;

namespace MemcacheAdmin.Security
{
    public class Authentication
    {
        public static void SerializeUser(HttpContextBase context, IUser user, bool createPersistentCookie)
        {


            var userData = UserCookie.Serialize(user);

            var ticket = new FormsAuthenticationTicket(
                        2,                                  /* Version */
                        user.Identity.Name,                 /* Name */
                        DateTime.Now,                       /* IssueDate */
                        DateTime.Now.AddMinutes(60),      /* Expiration */
                        createPersistentCookie,             /* Persistent */
                        userData,                           /* UserData */
                        FormsAuthentication.FormsCookiePath /* Cookie Path */
                    );

            context.Response.Cookies[FormsAuthentication.FormsCookieName].Value =
                FormsAuthentication.Encrypt(ticket);
        }

        public static IUser DeserializeUser(HttpContextBase context)
        {
            var u = new MemcacheUser();

            var authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (FormsAuthentication.IsEnabled && authCookie != null)
            {
                FormsAuthenticationTicket authTicket = null;
                try
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch //(Exception ex)
                {
                    // should log exception...
                    SerializeUser(context, u, false);
                    return u;
                }
                if (authTicket == null)
                {
                    SerializeUser(context, u, false);
                    return u;
                }
                else
                {
                    try
                    {
                        context.User = u = UserCookie.Deserialize(authTicket.UserData);

                        // get ip address
                        string ip = context.Request["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(ip))
                        {
                            ip = context.Request["REMOTE_ADDR"];
                        }
                        u.IpAddress = ip;

                        context.User = u;
                    }
                    catch (Exception error)
                    {
                        var msg = error.Message;
                        SerializeUser(context, u, false);
                    }
                }
            }
            else
            {
                SerializeUser(context, u, false);
            }

            return u;
        }


    }
}