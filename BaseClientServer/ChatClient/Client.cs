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

            Send(stream, username);

            Console.WriteLine("Connesso! Scrivi messaggi:\n");

            Task.Run(() => Receive(stream));

            while (true)
            {
                string message = Console.ReadLine();
                Send(stream, message);
            }
        }

        private void Receive(NetworkStream stream)
        {
            byte[] buffer = new byte[Config.BUFFER_SIZE];

            try
            {
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(message);
                }
            }
            catch
            {
                Console.WriteLine("Connessione chiusa.");
            }
        }

        private void Send(NetworkStream stream, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}