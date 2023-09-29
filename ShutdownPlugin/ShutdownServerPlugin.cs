using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutdownPlugin
{
    public class ShutdownServerPlugin : RocketPlugin<Configuration>
    {
        public static ShutdownServerPlugin Instance;

        public static Configuration Config;

        protected override void Load()
        {
            Instance = this;
            Config = Configuration.Instance;

            Logger.LogWarning($"[{Name}] Plugin loaded successfully!");

            _ = Main();
        }

        static async Task Main()
        {

            string[] parts = Config.ShutdownTime.Split(':');

            if (parts.Length == 2 && int.TryParse(parts[0], out int hours) && int.TryParse(parts[1], out int minutes))
            {
                DateTime currentTime = DateTime.Now;

                DateTime targetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hours, minutes, 0);
                TimeSpan timeRemaining = targetTime - currentTime;

                if (timeRemaining.TotalMilliseconds > 0)
                {
                    int delayMilliseconds = (int)timeRemaining.TotalMilliseconds;

                    Logger.Log($"[{Config.ShutdownTime}] will be executed {targetTime.ToLongTimeString()}, in {delayMilliseconds} ms");

                    await Task.Delay(delayMilliseconds);

                    _ = NotifyAndShutdown();
                } else
                {
                    int delayMilliseconds = (int) timeRemaining.TotalMilliseconds + 86400000;

                    DateTime dateTime = DateTime.Now;

                    dateTime = dateTime.AddMilliseconds(delayMilliseconds);

                    Logger.LogError($"[{Config.ShutdownTime}] remaining time is negative, running tomorrow at {dateTime}");

                    await Task.Delay(delayMilliseconds);

                    _ = NotifyAndShutdown();
                }
            }
            else
            {
                Logger.LogError($"[{Config.ShutdownTime}] is not valid");
            }
        }

        public static async Task NotifyAndShutdown()
        {
            UnturnedChat.Say("Server will restart in 2 minutes");
            int delayMilliseconds = 2 * 60 * 1000;
            await Task.Delay(delayMilliseconds);

            for (int i = 5; i >= 1; i--)
            {
                UnturnedChat.Say($"Server will restart in {i} seconds");
                await Task.Delay(1000);
            }

            R.Commands.Execute(null, "shutdown");
        }

        protected override void Unload()
        {
            Instance = null;
            Config = null;

            Logger.LogWarning($"[{Name}] Plugin unloaded successfully!");
        }
    }
}
