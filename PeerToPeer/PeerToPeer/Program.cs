using PeerToPeer;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();

        Console.Write("Porta locale: ");
        int port = int.Parse(Console.ReadLine());

        var peer = new Peer(username);
        peer.Start(port);

        Console.WriteLine("Comandi:");
        Console.WriteLine("/connect ip porta");
        Console.WriteLine("/msg testo\n");

        while (true)
        {
            string input = Console.ReadLine();

            if (input.StartsWith("/connect"))
            {
                var parts = input.Split(' ');
                string ip = parts[1];
                int p = int.Parse(parts[2]);

                peer.Connect(ip, p);
            }
            else if (input.StartsWith("/msg"))
            {
                string msg = input.Substring(5);
                peer.Broadcast(Protocol.Msg(username, msg));
            }
        }
    }
}