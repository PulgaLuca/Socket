using System;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private readonly ChatServer _server;
        private NetworkStream _stream;

        public string Username { get; private set; }

        public ClientHandler(TcpClient client, ChatServer server)
        {
            _client = client;
            _server = server;
        }

        public void Handle()
        {
            _stream = _client.GetStream();
            byte[] buffer = new byte[Config.BUFFER_SIZE];

            try
            {
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                Username = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"[LOGIN] {Username}");
                _server.Broadcast($"[SERVER] {Username} è entrato nella chat", this);

                while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[{Username}] {message}");

                    _server.Broadcast($"{Username}: {message}", this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRORE] {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"[LOGOUT] {Username}");
                _server.Broadcast($"[SERVER] {Username} è uscito", this);

                _server.RemoveClient(this);
                _client.Close();
            }
        }

        public void Send(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
            catch
            {
                // client probabilmente disconnesso
            }
        }
    }
}