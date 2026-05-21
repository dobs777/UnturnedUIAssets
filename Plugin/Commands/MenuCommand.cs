using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MenuUI.Commands
{
    public class MenuCommand : IRocketCommand
    {
        public string Name        => "menu";
        public string Help        => "Abre o menu principal";
        public string Syntax      => "/menu [aba]";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public bool AllowSimultaneousCalls => true;
        public List<string> Aliases        => new List<string> { "m" };
        public List<string> Permissions    => new List<string> { "menuui.menu" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            string tab = command.Length > 0 ? command[0].ToLower() : null;
            MenuUIPlugin.Instance.UIManager.Open(player, tab);
        }
    }
}
