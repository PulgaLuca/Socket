class Program
{
    static void Main(string[] args)
    {
        var client = new ChatClient.ChatClient();
        client.Start();
    }
}