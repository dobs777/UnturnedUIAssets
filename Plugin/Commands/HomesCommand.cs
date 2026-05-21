using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MenuUI.Commands
{
    public class HomesCommand : IRocketCommand
    {
        public string Name        => "homes";
        public string Help        => "Abre a lista de homes";
        public string Syntax      => "/homes";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public bool AllowSimultaneousCalls => true;
        public List<string> Aliases        => new List<string>();
        public List<string> Permissions    => new List<string> { "menuui.homes" };

        public void Execute(IRocketPlayer caller, string[] command)
            => MenuUIPlugin.Instance.UIManager.Open((UnturnedPlayer)caller, "homes");
    }
}
