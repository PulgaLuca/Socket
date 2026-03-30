using ChatServer;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class ChatClient
    {
        public void Start()
        {
            TcpClient client = new TcpClient(Config.HOST, Config.PORT);
            NetworkStream stream = client.GetStream();

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Send(stream, Protocol.Format("LOGIN", username));

            Task.Run(() => Receive(stream));

            while (true)
            {
                string message = Console.ReadLine();
                Send(stream, Protocol.Format("MSG", message));
            }
        }

        private void Receive(NetworkStream stream)
        {
            byte[] buffer = new byte[Config.BUFFER_SIZE];

            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string raw = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                var (cmd, payload) = Protocol.Parse(raw);

                HandleServerMessage(cmd, payload);
            }
        }

        private void HandleServerMessage(string cmd, string payload)
        {
            switch (cmd)
            {
                case "JOIN":
                    Console.WriteLine($"🟢 {payload} è entrato");
                    break;

                case "LEAVE":
                    Console.WriteLine($"X {payload} è uscito");
                    break;

                case "MSG":
                    var parts = payload.Split('|', 2);
                    string user = parts[0];
                    string msg = parts[1];

                    Console.WriteLine($"[{user}] {msg}");
                    break;
            }
        }

        private void Send(NetworkStream stream, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}