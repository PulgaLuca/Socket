using System.Net.Sockets;
using System.Text;

namespace Broker
{
    public class ClientHandler
    {
        private TcpClient _client;
        private BrokerServer _server;

        public ClientHandler(TcpClient client, BrokerServer server)
        {
            _client = client;
            _server = server;
        }

        public void Handle()
        {
            var stream = _client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = stream.Read(buffer);
                string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

                var (cmd, parts) = Protocol.Parse(msg);

                switch (cmd)
                {
                    case "SUB":
                        _server.Subscribe(parts[1], this);
                        break;

                    case "UNSUB":
                        _server.Unsubscribe(parts[1], this);
                        break;

                    case "PUB":
                        _server.Publish(parts[1], parts[2]);
                        break;
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