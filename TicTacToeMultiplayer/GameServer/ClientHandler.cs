using System;
using System.Net.Sockets;
using System.Text;

namespace GameServer
{
    public class ClientHandler
    {
        private TcpClient _client;
        private Server _server;
        public char Symbol { get; }

        public ClientHandler(TcpClient client, Server server, char symbol)
        {
            _client = client;
            _server = server;
            Symbol = symbol;
        }

        public void Start()
        {
            var stream = _client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytes = stream.Read(buffer);
                    if (bytes == 0) break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

                    var (cmd, parts) = Protocol.Parse(msg);

                    if (cmd == "MOVE")
                    {
                        int r = int.Parse(parts[1]);
                        int c = int.Parse(parts[2]);

                        _server.HandleMove(this, r, c);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Client disconnesso dal gioco");
            }
        }

        public void Send(string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            _client.GetStream().Write(data);
        }
    }
}