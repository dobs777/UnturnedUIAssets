using MenuUI.UI;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MenuUI.Commands
{
    // ─────────────────────────────────────────────────────────────────────────
    // /menu — opens the menu on the default (first) tab
    // ─────────────────────────────────────────────────────────────────────────
    public class MenuCommand : IRocketCommand
    {
        public string Name        => "menu";
        public string Help        => "Abre o menu principal.";
        public string Syntax      => "/menu";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Aliases        => new() { "m" };
        public List<string> Permissions    => new() { "menuui.menu" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is UnturnedPlayer)
                MenuUIManager.Open();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // /kits — opens the menu on the KITS tab
    // ─────────────────────────────────────────────────────────────────────────
    public class KitsCommand : IRocketCommand
    {
        public string Name        => "kits";
        public string Help        => "Abre o menu de kits.";
        public string Syntax      => "/kits";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Aliases        => new();
        public List<string> Permissions    => new() { "menuui.kits" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is UnturnedPlayer)
                MenuUIManager.Open("kits");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // /vips — opens the menu on the VIPS tab
    // ─────────────────────────────────────────────────────────────────────────
    public class VipsCommand : IRocketCommand
    {
        public string Name        => "vips";
        public string Help        => "Abre o menu de VIPs.";
        public string Syntax      => "/vips";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Aliases        => new() { "vip" };
        public List<string> Permissions    => new() { "menuui.vips" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is UnturnedPlayer)
                MenuUIManager.Open("vips");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // /homes — opens the menu on the HOMES tab
    // ─────────────────────────────────────────────────────────────────────────
    public class HomesMenuCommand : IRocketCommand
    {
        public string Name        => "homes";
        public string Help        => "Abre o menu de homes.";
        public string Syntax      => "/homes";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Aliases        => new();
        public List<string> Permissions    => new() { "menuui.homes" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is UnturnedPlayer)
                MenuUIManager.Open("homes");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // /info — opens the menu on the INFORMAÇÕES tab
    // ─────────────────────────────────────────────────────────────────────────
    public class InfoCommand : IRocketCommand
    {
        public string Name        => "info";
        public string Help        => "Abre o menu de informações do servidor.";
        public string Syntax      => "/info";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Aliases        => new() { "informacoes", "informações" };
        public List<string> Permissions    => new() { "menuui.info" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is UnturnedPlayer)
                MenuUIManager.Open("info");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // /sethome <nome> — salva a posição atual como uma home
    // ─────────────────────────────────────────────────────────────────────────
    public class SetHomeCommand : IRocketCommand
    {
        public string Name        => "sethome";
        public string Help        => "Salva sua posição atual como uma home.";
        public string Syntax      => "/sethome <nome>";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Aliases        => new();
        public List<string> Permissions    => new() { "menuui.sethome" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is not UnturnedPlayer player) return;

            if (command.Length == 0)
            {
                UnturnedChat.Say(player, "Uso: /sethome <nome>", UnityEngine.Color.red);
                return;
            }

            string name = command[0];
            var cfg     = MenuUIPlugin.Instance.Configuration.Instance;
            bool ok     = MenuUIPlugin.Instance.Homes.SetHome(
                              player.CSteamID.m_SteamID, name, cfg.MaxHomesPerPlayer);

            if (ok)
                UnturnedChat.Say(player, $"Home '{name}' salva com sucesso!", UnityEngine.Color.green);
            else
                UnturnedChat.Say(player,
                    $"Limite de homes atingido ({cfg.MaxHomesPerPlayer}).", UnityEngine.Color.red);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // /delhome <nome> — remove uma home salva
    // ─────────────────────────────────────────────────────────────────────────
    public class DelHomeCommand : IRocketCommand
    {
        public string Name        => "delhome";
        public string Help        => "Remove uma home salva.";
        public string Syntax      => "/delhome <nome>";
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public List<string> Aliases        => new() { "removehome" };
        public List<string> Permissions    => new() { "menuui.delhome" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is not UnturnedPlayer player) return;

            if (command.Length == 0)
            {
                UnturnedChat.Say(player, "Uso: /delhome <nome>", UnityEngine.Color.red);
                return;
            }

            string name = command[0];
            bool ok     = MenuUIPlugin.Instance.Homes.DeleteHome(
                              player.CSteamID.m_SteamID, name);

            UnturnedChat.Say(player,
                ok ? $"Home '{name}' removida." : $"Home '{name}' não encontrada.",
                ok ? UnityEngine.Color.green : UnityEngine.Color.red);
        }
    }
}
