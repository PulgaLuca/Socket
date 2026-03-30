using System.Net.Sockets;
using System.Text;

namespace GameServer
{
    public class ClientHandler
    {
        private TcpClient _client;
        private Server _server;
        private char _symbol;

        public ClientHandler(TcpClient client, Server server, char symbol)
        {
            _client = client;
            _server = server;
            _symbol = symbol;
        }

        public void Start()
        {
            var stream = _client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = stream.Read(buffer);
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

        public void Send(string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            _client.GetStream().Write(data);
        }
    }
}