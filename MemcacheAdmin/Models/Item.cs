
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemcacheAdmin.Models
{
    public class Item
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTime Expiration { get; set; }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        public Item(string line)
        {
            //ITEM key2 [2 b; 1368866637 s]
            string[] parts = line.Split(' ');
            Name = parts[1];
            string attr = parts[2].Trim(new char[] { '['});
            Size = Int32.Parse(attr);
            Expiration = UnixTimeStampToDateTime( Int32.Parse(parts[4]));
        }
    }
}