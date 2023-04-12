using Newtonsoft.Json;

namespace WHBOT.IronWarrior
{
    public class Config
    {
        public Dictionary<string, string[]>? Pairs = new Dictionary<string, string[]>();

        public Config()
        {
            Pairs = new Dictionary<string, string[]>();
        }

        public void Set(string key, string value)
        {
            if (!Has(key))
            {
                var responses = Pairs[key];
                var newResponses = new string[responses.Length + 1];
                responses.CopyTo(newResponses, 0);
                newResponses[responses.Length] = value;
                Pairs[key] = newResponses;
            }
            else
            {
                Pairs!.Add(key, new string[] { value });
            }
        }

        public void Del(string key)
        {
            if (Has(key))
            {
                Pairs!.Remove(key);
            }
        }

        public bool Has(string key)
        {
            return Pairs!.ContainsKey(key);
        }

        public static Config Load(string path, Config? defaultConfig = null)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("config.json not found");
                File.Create(path);

                if (defaultConfig == null)
                {
                    defaultConfig = new Config();
                    defaultConfig.Set("ping", "pong");
                }

                Console.WriteLine("saving default config.json");
                defaultConfig.Save(path);

                return defaultConfig;
            }

            try
            {
                var json = File.ReadAllText(path);
                var config = JsonConvert.DeserializeObject<Config>(json);
                return config!;
            }
            catch (Exception e)
            {
                throw new Exception("Error loading config.json: " + e.Message);
            }
        }

        public void Save(string path)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
