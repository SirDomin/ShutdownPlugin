using Rocket.API;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutdownPlugin.Command
{
    internal class ShutdownServerCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "server";

        public string Help => "server utils";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                UnturnedChat.Say(caller, "Specify action");
            }

            if (command[0] == "close")
            {
                _ = ShutdownServerPlugin.NotifyAndShutdown();
            }
        }
    }
}
