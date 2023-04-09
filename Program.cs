using Discord;
using Discord.WebSocket;

namespace WHBOT.IronWarrior
{
    class Program
    {
        private DiscordSocketClient _client;

        public static Task Main(string[] args) => new Program().MainAsync();

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            var token = Environment.GetEnvironmentVariable("DSC_TOKEN");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "ping")
            {
                await message.Channel.SendMessageAsync("pong");
            }
        }
    }
}
