using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeer
{
    public class ConnectionHandler
    {
        private readonly TcpClient _client;
        private readonly Peer _peer;
        private NetworkStream _stream;

        public string Username { get; private set; }

        public ConnectionHandler(TcpClient client, Peer peer)
        {
            _client = client;
            _peer = peer;
        }

        public void Start()
        {
            _stream = _client.GetStream();

            Task.Run(ReceiveLoop);
        }

        private void ReceiveLoop()
        {
            byte[] buffer = new byte[Config.BUFFER_SIZE];

            try
            {
                int bytesRead;

                while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string raw = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var (cmd, parts) = Protocol.Parse(raw);

                    Handle(cmd, parts);
                }
            }
            catch { }
            finally
            {
                Console.WriteLine($"X {Username} disconnesso");
                _peer.RemoveConnection(this);
            }
        }

        private void Handle(string cmd, string[] parts)
        {
            switch (cmd)
            {
                case "HELLO":
                    Username = parts[1];
                    Console.WriteLine($"🟢 Connesso a {Username}");
                    break;

                case "MSG":
                    string user = parts[1];
                    string msg = parts[2];

                    Console.WriteLine($"[{user}] {msg}");
                    break;
            }
        }

        public void Send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }
    }
}
