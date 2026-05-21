using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MenuUI.Commands
{
    public class KitsCommand : IRocketCommand
    {
        public string Name        => "kits";
        public string Help        => "Abre o menu de kits";
        public string Syntax      => "/kits";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public bool AllowSimultaneousCalls => true;
        public List<string> Aliases        => new List<string>();
        public List<string> Permissions    => new List<string> { "menuui.kits" };

        public void Execute(IRocketPlayer caller, string[] command)
            => MenuUIPlugin.Instance.UIManager.Open((UnturnedPlayer)caller, "kits");
    }
}
