using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class Server
    {
        private TcpListener _listener;
        private ClientHandler playerX;
        private ClientHandler playerO;
        private GameManager game = new();

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, 5000);
            _listener.Start();

            Console.WriteLine("[SERVER] Avviato");

            playerX = new ClientHandler(_listener.AcceptTcpClient(), this, 'X');
            Console.WriteLine("Giocatore X connesso");

            playerO = new ClientHandler(_listener.AcceptTcpClient(), this, 'O');
            Console.WriteLine("Giocatore O connesso");

            playerX.Send(Protocol.Format("START", "X"));
            playerO.Send(Protocol.Format("START", "O"));

            playerX.Start();
            playerO.Start();
        }

        public void HandleMove(ClientHandler player, int r, int c)
        {
            if (!game.MakeMove(r, c))
            {
                player.Send(Protocol.Format("ERROR", "Mossa non valida"));
                return;
            }

            string board = game.GetBoard();

            playerX.Send(Protocol.Format("UPDATE", board));
            playerO.Send(Protocol.Format("UPDATE", board));

            char winner = game.CheckWinner();

            if (winner != '.')
            {
                playerX.Send(Protocol.Format("WIN", winner.ToString()));
                playerO.Send(Protocol.Format("WIN", winner.ToString()));
            }
        }
    }
}