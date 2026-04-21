using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GameServer
{
    public class Server
    {
        private TcpListener _listener;
        private ClientHandler playerX;
        private ClientHandler playerO;
        private GameManager game = new();

        private char currentTurn = 'X';

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

            // thread separati per efficientare la gestione dei client
            Task.Run(() => playerX.Start());
            Task.Run(() => playerO.Start());
        }

        public void HandleMove(ClientHandler player, int r, int c)
        {
            if (player.Symbol != currentTurn)
            {
                player.Send(Protocol.Format("ERROR", "Non è il tuo turno"));
                return;
            }

            if (!game.MakeMove(r, c))
            {
                player.Send(Protocol.Format("ERROR", "Mossa non valida"));
                return;
            }

            // cambio del turno dei giocatori e relativi controlli
            currentTurn = currentTurn == 'X' ? 'O' : 'X';

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