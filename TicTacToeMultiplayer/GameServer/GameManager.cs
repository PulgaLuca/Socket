using System;

namespace GameServer
{
    public class GameManager
    {
        private char[,] board = new char[3, 3];
        private char currentPlayer = 'X';

        public GameManager()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    board[i, j] = '.';
        }

        public bool MakeMove(int row, int col)
        {
            if (board[row, col] != '.')
                return false;

            board[row, col] = currentPlayer;
            currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
            return true;
        }

        public string GetBoard()
        {
            return string.Join(",", board.Cast<char>());
        }

        public char CheckWinner()
        {
            // righe, colonne, diagonali
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != '.' && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return board[i, 0];

                if (board[0, i] != '.' && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    return board[0, i];
            }

            if (board[0, 0] != '.' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return board[0, 0];

            if (board[0, 2] != '.' && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return board[0, 2];

            return '.';
        }
    }
}