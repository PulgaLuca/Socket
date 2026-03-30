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
                int bytesRead;

                while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string raw = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var (cmd, payload) = Protocol.Parse(raw);

                    HandleCommand(cmd, payload);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRORE] {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        private void HandleCommand(string cmd, string payload)
        {
            switch (cmd)
            {
                case "LOGIN":
                    Username = payload;
                    Console.WriteLine($"[LOGIN] {Username}");

                    _server.Broadcast(
                        Protocol.Format("JOIN", Username),
                        this
                    );
                    break;

                case "MSG":
                    Console.WriteLine($"[{Username}] {payload}");

                    _server.Broadcast(
                        Protocol.Format("MSG", $"{Username}|{payload}")
                    );
                    break;
            }
        }

        private void Disconnect()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                Console.WriteLine($"[LEAVE] {Username}");

                _server.Broadcast(
                    Protocol.Format("LEAVE", Username),
                    this
                );
            }

            _server.RemoveClient(this);
            _client.Close();
        }

        public void Send(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
            catch { }
        }
    }
}