using Discord;
using Discord.WebSocket;

namespace WHBOT.IronWarrior
{
    class Program
    {
        private const string CONFIG_PATH = "/bot/iron-warrior/config.json";

        public Config? _config;

        public static Task Main(string[] args) =>  new Program().MainAsync();

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task MainAsync()
        {
            try {
                _config = Config.Load(CONFIG_PATH);
            } catch (Exception e) {
                Console.WriteLine($"ERR: {e.Message}");
                Console.WriteLine(e.StackTrace);
                return;
            }

            var _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent | GatewayIntents.GuildMessages
            });

            _client.Log += Log;
            _client.MessageReceived += OnMessageReceived;

            var token = Environment.GetEnvironmentVariable("DSC_TOKEN");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            foreach (var pair in _config!.Pairs!)
            {
                if (message.Content.ToLower().Contains(pair.Key.ToLower()))
                {
                    Console.WriteLine($"Responding to {message.Author.Username}. Prompt match: {pair.Key}");
                    var response = pair.Value[new Random().Next(0, pair.Value.Length)];
                    await message.Channel.SendMessageAsync(response);
                }
            }
        }
    }
}
