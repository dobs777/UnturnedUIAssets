using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MenuUI.Commands
{
    public class VipsCommand : IRocketCommand
    {
        public string Name        => "vips";
        public string Help        => "Abre o menu de VIPs";
        public string Syntax      => "/vips";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public bool AllowSimultaneousCalls => true;
        public List<string> Aliases        => new List<string> { "loja" };
        public List<string> Permissions    => new List<string> { "menuui.vips" };

        public void Execute(IRocketPlayer caller, string[] command)
            => MenuUIPlugin.Instance.UIManager.Open((UnturnedPlayer)caller, "vips");
    }
}
