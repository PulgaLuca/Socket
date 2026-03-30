namespace GameServer
{
    public static class Protocol
    {
        public static (string cmd, string[] parts) Parse(string msg)
        {
            var parts = msg.Split('|');
            return (parts[0], parts);
        }

        public static string Format(params string[] parts)
            => string.Join("|", parts);
    }
}