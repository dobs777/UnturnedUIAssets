using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MenuUI.Commands
{
    public class InfoCommand : IRocketCommand
    {
        public string Name        => "info";
        public string Help        => "Abre informacoes do servidor";
        public string Syntax      => "/info";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public bool AllowSimultaneousCalls => true;
        public List<string> Aliases        => new List<string> { "informacoes" };
        public List<string> Permissions    => new List<string> { "menuui.info" };

        public void Execute(IRocketPlayer caller, string[] command)
            => MenuUIPlugin.Instance.UIManager.Open((UnturnedPlayer)caller, "info");
    }
}
