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
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent | GatewayIntents.GuildMessages
            };

            _client = new DiscordSocketClient(config);
            _client.Log += Log;
            _client.MessageReceived += OnMessageReceived;

            var token = Environment.GetEnvironmentVariable("DSC_TOKEN");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            Console.WriteLine("got message: " + message.Content);

            if (message.Content.Contains("ping"))
            {
                Console.WriteLine($"PONG from {message.Author.Username}");
                await message.Channel.SendMessageAsync("pong");
            }
        }
    }
}
