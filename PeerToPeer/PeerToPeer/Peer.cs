using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeer
{
    public class Peer
    {
        private readonly List<ConnectionHandler> _connections = new();
        private readonly object _lock = new();

        private TcpListener _listener;

        public string Username { get; }

        public Peer(string username)
        {
            Username = username;
        }

        public void Start(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            Console.WriteLine($"[PEER] {Username} in ascolto sulla porta {port}");

            Task.Run(AcceptLoop);
        }

        private void AcceptLoop()
        {
            while (true)
            {
                var client = _listener.AcceptTcpClient();
                AddConnection(client);
            }
        }

        public void Connect(string host, int port)
        {
            var client = new TcpClient(host, port);
            AddConnection(client);
        }

        private void AddConnection(TcpClient client)
        {
            var handler = new ConnectionHandler(client, this);

            lock (_lock)
                _connections.Add(handler);

            handler.Start();

            // invia presentazione
            handler.Send(Protocol.Hello(Username));
        }

        public void Broadcast(string message)
        {
            lock (_lock)
            {
                foreach (var conn in _connections)
                    conn.Send(message);
            }
        }

        public void RemoveConnection(ConnectionHandler conn)
        {
            lock (_lock)
                _connections.Remove(conn);
        }
    }
}
