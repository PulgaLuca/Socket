using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeer
{
    public static class Protocol
    {
        public static (string Command, string[] Parts) Parse(string message)
        {
            var parts = message.Split('|');
            return (parts[0], parts);
        }

        public static string Hello(string username)
            => $"HELLO|{username}";

        public static string Msg(string username, string msg)
            => $"MSG|{username}|{msg}";
    }
}
