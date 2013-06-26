using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MemcacheAdmin.Utils
{
    public class MemcacheUtils
    {
        private const string VALUE = "VALUE"; // start of value line from server
        private const string STATS = "STAT"; // start of stats line from server
        private const string DELETED = "DELETED"; // successful deletion
        private const string NOTFOUND = "NOT_FOUND"; // record not found for delete or incr/decr
        private const string STORED = "STORED"; // successful store of data
        private const string NOTSTORED = "NOT_STORED"; // data not stored
        private const string OK = "OK"; // success
        private const string END = "END"; // end of data from server
        private const string ERROR = "ERROR"; // invalid command name from client
        private const string CLIENT_ERROR = "CLIENT_ERROR"; // client error in input line - invalid protocol
        private const string SERVER_ERROR = "SERVER_ERROR";	// server error

        private const int F_COMPRESSED = 2;
        private const int F_SERIALIZED = 8;

        public static void LoadItems(string line, Hashtable hm, bool asString)
        {
            if (line.StartsWith(VALUE))
            {
                int first = line.IndexOf("\r\n");
                if (first < 0)
                    return;

                string valueLine = line.Substring(0, first);
                string[] info = valueLine.Split(' ');
                string key = info[1];
                int flag = int.Parse(info[2], new NumberFormatInfo());

                int length = int.Parse(info[3], new NumberFormatInfo());
                string rest = line.Substring(first + 2, length);

                // ready object
                object o = rest;

                // store the object into the cache
                hm[key] = o;
           }
        }

    }
}