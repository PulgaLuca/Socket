using System;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ChatServer();
            server.Start();
        }
    }
}