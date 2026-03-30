using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public static class Protocol
    {
        public static (string Command, string Payload) Parse(string message)
        {
            var parts = message.Split('|', 2);
            return (parts[0], parts.Length > 1 ? parts[1] : "");
        }

        public static string Format(string command, string payload)
        {
            return $"{command}|{payload}";
        }
    }
}
