using Rocket.API;

namespace ShutdownPlugin
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string ShutdownTime;

        public void LoadDefaults()
        {
            ShutdownTime = "00:00";
        }
    }
}
