using Broker;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PubSubClient
    {
        public void Start()
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            var stream = client.GetStream();

            Task.Run(() => Receive(stream));

            Console.WriteLine("Comandi:");
            Console.WriteLine("/sub topic");
            Console.WriteLine("/unsub topic");
            Console.WriteLine("/pub topic messaggio\n");

            while (true)
            {
                var input = Console.ReadLine().Split(' ');

                switch (input[0])
                {
                    case "/sub":
                        Send(stream, Protocol.Format("SUB", input[1]));
                        break;

                    case "/unsub":
                        Send(stream, Protocol.Format("UNSUB", input[1]));
                        break;

                    case "/pub":
                        Send(stream, Protocol.Format("PUB", input[1], string.Join(' ', input.Skip(2))));
                        break;
                }
            }
        }

        private void Receive(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = stream.Read(buffer);
                string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

                var (cmd, parts) = Protocol.Parse(msg);

                if (cmd == "MSG")
                {
                    Console.WriteLine($"[{parts[1]}] {parts[2]}");
                }
            }
        }

        private void Send(NetworkStream stream, string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data);
        }
    }
}