using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameClient
{
    public class GameClient
    {
        public void Start()
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            var stream = client.GetStream();

            Task.Run(() => Receive(stream));

            while (true)
            {
                Console.Write("riga colonna: ");
                var input = Console.ReadLine().Split(' ');

                string msg = Protocol.Format("MOVE", input[0], input[1]);
                Send(stream, msg);
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

                switch (cmd)
                {
                    case "START":
                        Console.WriteLine($"Sei {parts[1]}");
                        break;

                    case "UPDATE":
                        PrintBoard(parts[1]);
                        break;

                    case "WIN":
                        Console.WriteLine($"Vince {parts[1]}");
                        break;

                    case "ERROR":
                        Console.WriteLine(parts[1]);
                        break;
                }
            }
        }

        private void PrintBoard(string data)
        {
            var cells = data.Split(',');

            for (int i = 0; i < 9; i++)
            {
                Console.Write(cells[i] + " ");
                if (i % 3 == 2) Console.WriteLine();
            }
        }

        private void Send(NetworkStream stream, string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data);
        }
    }
}