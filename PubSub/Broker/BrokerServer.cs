using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Broker
{
    public class BrokerServer
    {
        private TcpListener _listener;

        // topic → subscribers
        private readonly Dictionary<string, List<ClientHandler>> _topics = new();
        private readonly object _lock = new();

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, 5000);
            _listener.Start();

            Console.WriteLine("[BROKER] Avviato");

            while (true)
            {
                var client = _listener.AcceptTcpClient();
                var handler = new ClientHandler(client, this);

                Task.Run(() => handler.Handle());
            }
        }

        public void Subscribe(string topic, ClientHandler client)
        {
            lock (_lock)
            {
                if (!_topics.ContainsKey(topic))
                    _topics[topic] = new List<ClientHandler>();

                _topics[topic].Add(client);
            }

            Console.WriteLine($"[SUB] client → {topic}");
        }

        public void Unsubscribe(string topic, ClientHandler client)
        {
            lock (_lock)
            {
                if (_topics.ContainsKey(topic))
                    _topics[topic].Remove(client);
            }
        }

        public void Publish(string topic, string message)
        {
            lock (_lock)
            {
                if (!_topics.ContainsKey(topic)) return;

                foreach (var sub in _topics[topic])
                {
                    sub.Send(Protocol.Format("MSG", topic, message));
                }
            }

            Console.WriteLine($"[PUB] {topic}: {message}");
        }
    }
}