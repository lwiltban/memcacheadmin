using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using MemcacheAdmin.Interfaces;

namespace MemcacheAdmin.Security
{
    public class UserCookie
    {
        public static string Serialize(IUser user)
        {
            return Serialize(user as MemcacheUser);
        }

        public static string Serialize(MemcacheUser user)
        {
            #region Old Serialization
            /*------------------------------------------------------------*/

            //return String.Format( "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
            //                          user.PersonId,
            //                          user.Identity.Name,
            //                          user.ProgramNumber,
            //                          user.Role,
            //                          user.Type,
            //                          user.EmployeeId,
            //                          user.AreaId,
            //                          user.YearId,
            //                          user.TermId,
            //                          user.ClassId,
            //                          user.Remember,
            //                          user.Culture,
            //                          user.LanguageId );

            /*------------------------------------------------------------*/
            #endregion

            #region New Serialization
            /*------------------------------------------------------------*/

            var s = new XmlSerializer(typeof(MemcacheUser));
            using (var sw = new StringWriter())
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented; // indent the Xml so it's human readable
                    s.Serialize(writer, user);
                    writer.Flush();
                    return sw.ToString();
                }
            }


            /*------------------------------------------------------------*/
            #endregion
        }

        public static MemcacheUser Deserialize(string data)
        {
            #region Old Deserialization
            /*------------------------------------------------------------*/

            //var u = new WiseUser();
            //string[] dat = data.Split( '|' );
            //u.PersonId = decimal.Parse( dat[ 0 ] );
            //string name = dat[ 1 ];
            //u.ProgramNumber = dat[ 2 ];
            //u.Role = ( SystemRoles ) Enum.Parse( typeof( SystemRoles ), dat[ 3 ] );
            //u.Type = ( ProgramType ) Enum.Parse( typeof( ProgramType ), dat[ 4 ] );
            //u.EmployeeId = dat[ 5 ];
            //u.AreaId = dat[ 6 ];
            //u.YearId = decimal.Parse( dat[ 7 ] );
            //u.TermId = decimal.Parse( dat[ 8 ] );
            //u.ClassId = decimal.Parse( dat[ 9 ] );
            //u.Remember = bool.Parse( dat[ 10 ] );

            //if ( dat[ 11 ].Length > 0 )
            //     u.Culture = dat[ 11 ];

            //if ( dat[ 12 ] != null && dat[ 12 ] != "-1" )
            //     u.LanguageId = decimal.Parse( dat[ 12 ] );

            //return u;

            /*------------------------------------------------------------*/
            #endregion

            #region New Deserialization
            /*------------------------------------------------------------*/

            var s = new XmlSerializer(typeof(MemcacheUser));
            using (var reader = new StringReader(data))
            {
                return s.Deserialize(reader) as MemcacheUser;
            }


            /*------------------------------------------------------------*/
            #endregion
        }

    }
}