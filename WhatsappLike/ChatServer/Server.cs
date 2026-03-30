using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public class ChatServer
    {
        private TcpListener _listener;
        private readonly List<ClientHandler> _clients = new();
        private readonly object _lock = new();

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Parse(Config.HOST), Config.PORT);
            _listener.Start();

            Console.WriteLine("[SERVER] Avviato");

            while (true)
            {
                var client = _listener.AcceptTcpClient();
                var handler = new ClientHandler(client, this);

                lock (_lock)
                    _clients.Add(handler);

                Task.Run(() => handler.Handle());
            }
        }

        public void Broadcast(string message, ClientHandler sender = null)
        {
            lock (_lock)
            {
                foreach (var client in _clients)
                {
                    if (client != sender)
                        client.Send(message);
                }
            }
        }

        public void RemoveClient(ClientHandler client)
        {
            lock (_lock)
                _clients.Remove(client);
        }
    }
}