using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using UnityEngine;

namespace MenuUI
{
    public class MenuUIManager
    {
        private readonly MenuUIConfig _cfg;
        private readonly Dictionary<CSteamID, PlayerUIState> _open = new Dictionary<CSteamID, PlayerUIState>();

        private const int KIT_SLOTS  = 12;
        private const int VIP_SLOTS  = 6;
        private const int HOME_SLOTS = 12;
        private const int INFO_SLOTS = 6;

        public MenuUIManager(MenuUIConfig cfg) { _cfg = cfg; }

        public void Open(UnturnedPlayer player, string tabId = null)
        {
            var id = player.CSteamID;
            Close(player);
            EffectManager.sendUIEffect(_cfg.EffectID, Key(id), id, true);
            player.Player.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            var state = new PlayerUIState { CurrentTab = tabId ?? FirstTab() };
            state.Homes = GetPlayerHomes(player);
            _open[id] = state;
            PopulateAll(player, state);
            ShowTab(player, state.CurrentTab);
        }

        public void Close(UnturnedPlayer player)
        {
            var id = player.CSteamID;
            if (!_open.ContainsKey(id)) return;
            EffectManager.askEffectClearByID(_cfg.EffectID, id);
            player.Player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            _open.Remove(id);
        }

        public void CloseAll()
        {
            foreach (var kvp in _open)
            {
                EffectManager.askEffectClearByID(_cfg.EffectID, kvp.Key);
                var p = UnturnedPlayer.FromCSteamID(kvp.Key);
                p?.Player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
            }
            _open.Clear();
        }

        public void OnButtonClicked(Player player, string buttonName)
        {
            var id = player.channel.owner.playerID.steamID;
            if (!_open.TryGetValue(id, out var state)) return;
            var up = UnturnedPlayer.FromPlayer(player);

            if (buttonName == "Btn_Close") { Close(up); return; }

            if (buttonName.StartsWith("Tab_"))
            {
                ShowTab(up, buttonName.Substring(4).ToLower());
                return;
            }

            if (ParseIndex("Btn_Kit_",  buttonName, out int ki)) { RunItem(up, Tab("kits"),  ki); return; }
            if (ParseIndex("Btn_Vip_",  buttonName, out int vi)) { RunItem(up, Tab("vips"),  vi); return; }
            if (ParseIndex("Btn_Info_", buttonName, out int ii)) { RunItem(up, Tab("info"),  ii); return; }
            if (ParseIndex("Btn_Home_", buttonName, out int hi))
            {
                if (hi < state.Homes.Count)
                    Execute(up, state.Homes[hi].Command);
                return;
            }
        }

        private void ShowTab(UnturnedPlayer player, string tabId)
        {
            var id = player.CSteamID;
            if (!_open.TryGetValue(id, out var state)) return;
            state.CurrentTab = tabId;
            var k = Key(id);
            foreach (var t in new[]{"kits","vips","homes","info"})
                EffectManager.sendUIEffectVisibility(k, id, true, "TabContent_" + Cap(t), t == tabId);
        }

        private void PopulateAll(UnturnedPlayer player, PlayerUIState state)
        {
            var id = player.CSteamID;
            var k  = Key(id);

            var kitsTab = Tab("kits");
            for (int i = 0; i < KIT_SLOTS; i++)
            {
                bool vis = kitsTab != null && i < kitsTab.Items.Count;
                EffectManager.sendUIEffectVisibility(k, id, true, $"KitCard_{i}", vis);
                if (!vis) continue;
                var it = kitsTab.Items[i];
                EffectManager.sendUIEffectText(k, id, true, $"Name_Kit_{i}",     it.Name        ?? "");
                EffectManager.sendUIEffectText(k, id, true, $"Desc_Kit_{i}",     it.Description ?? "");
                EffectManager.sendUIEffectText(k, id, true, $"Cooldown_Kit_{i}", string.IsNullOrEmpty(it.Cooldown) ? "" : "CD: " + it.Cooldown);
                if (!string.IsNullOrEmpty(it.ImageURL))
                    EffectManager.sendUIEffectImageURL(k, id, true, $"Img_Kit_{i}", it.ImageURL, true, true);
            }

            var vipsTab = Tab("vips");
            for (int i = 0; i < VIP_SLOTS; i++)
            {
                bool vis = vipsTab != null && i < vipsTab.Items.Count;
                EffectManager.sendUIEffectVisibility(k, id, true, $"VipCard_{i}", vis);
                if (!vis) continue;
                var it = vipsTab.Items[i];
                EffectManager.sendUIEffectText(k, id, true, $"Name_Vip_{i}", it.Name        ?? "");
                EffectManager.sendUIEffectText(k, id, true, $"Desc_Vip_{i}", it.Description ?? "");
                if (!string.IsNullOrEmpty(it.ImageURL))
                    EffectManager.sendUIEffectImageURL(k, id, true, $"Img_Vip_{i}", it.ImageURL, true, true);
                EffectManager.sendUIEffectText(k, id, true, $"Label_Vip_{i}", it.ButtonLabel ?? "COMPRAR");
            }

            for (int i = 0; i < HOME_SLOTS; i++)
            {
                bool vis = i < state.Homes.Count;
                EffectManager.sendUIEffectVisibility(k, id, true, $"HomeCard_{i}", vis);
                if (!vis) continue;
                EffectManager.sendUIEffectText(k, id, true, $"Name_Home_{i}", state.Homes[i].Name ?? "");
            }

            var infoTab = Tab("info");
            for (int i = 0; i < INFO_SLOTS; i++)
            {
                bool vis = infoTab != null && i < infoTab.Items.Count;
                EffectManager.sendUIEffectVisibility(k, id, true, $"InfoCard_{i}", vis);
                if (!vis) continue;
                var it = infoTab.Items[i];
                EffectManager.sendUIEffectText(k, id, true, $"Name_Info_{i}", it.Name        ?? "");
                EffectManager.sendUIEffectText(k, id, true, $"Desc_Info_{i}", it.Description ?? "");
                if (!string.IsNullOrEmpty(it.ImageURL))
                    EffectManager.sendUIEffectImageURL(k, id, true, $"Img_Info_{i}", it.ImageURL, true, true);
                EffectManager.sendUIEffectText(k, id, true, $"Label_Info_{i}", it.ButtonLabel ?? "ABRIR");
            }
        }

        private void RunItem(UnturnedPlayer player, TabConfig tab, int idx)
        {
            if (tab == null || idx >= tab.Items.Count) return;
            var it = tab.Items[idx];
            if (it.IsLink) OpenLink(player, it.Command);
            else           Execute(player, it.Command);
        }

        private static void Execute(UnturnedPlayer player, string command)
        {
            if (string.IsNullOrEmpty(command)) return;
            R.Commands.Execute(player, command.TrimStart('/'));
        }

        private static void OpenLink(UnturnedPlayer player, string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            player.Player.sendBrowserRequest("", url);
        }

        protected virtual List<MenuItemConfig> GetPlayerHomes(UnturnedPlayer player)
        {
            var tab = Tab("homes");
            return tab?.Items ?? new List<MenuItemConfig>();
        }

        private TabConfig Tab(string id) => _cfg.Tabs?.Find(t => t.Id == id);
        private string FirstTab() => _cfg.Tabs?.Count > 0 ? _cfg.Tabs[0].Id : "kits";
        private static short Key(CSteamID id) => (short)(id.m_SteamID % 32767);
        private static string Cap(string s) => s.Length == 0 ? s : char.ToUpper(s[0]) + s.Substring(1);

        private static bool ParseIndex(string prefix, string name, out int idx)
        {
            idx = -1;
            if (!name.StartsWith(prefix)) return false;
            return int.TryParse(name.Substring(prefix.Length), out idx);
        }
    }

    public class PlayerUIState
    {
        public string CurrentTab;
        public List<MenuItemConfig> Homes = new List<MenuItemConfig>();
    }
}
