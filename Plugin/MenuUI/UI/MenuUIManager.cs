using MenuUI.Models;
using MenuUI.UI.Tabs;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MenuUI.UI
{
    /// <summary>
    /// Owns the single Glazier window that renders the menu for the LOCAL player.
    ///
    /// SERVER-SIDE NOTE:
    ///   Glazier only exists on the game client (Unity renderer).
    ///   On a pure dedicated server Player.player is null, so all methods
    ///   guard against that. For dedicated servers swap in an EffectManager
    ///   bundle and replace the Open/Close/SwitchTab calls accordingly.
    /// </summary>
    public static class MenuUIManager
    {
        // ── layout constants ─────────────────────────────────────────────────
        private const float W           = 700f;
        private const float H           = 600f;
        private const float HEADER_H    = 52f;
        private const float TABBAR_H    = 38f;
        private const float FOOTER_H    = 52f;
        private const float CONTENT_H   = H - HEADER_H - TABBAR_H - FOOTER_H;
        private const float PAD         = 6f;

        // ── state ────────────────────────────────────────────────────────────
        private static ISleekElement?   _overlay;
        private static ISleekScrollView? _scroll;
        private static List<ISleekButton> _tabBtns   = new();
        private static List<BaseTab>      _tabs       = new();
        private static int                _activeTab  = -1;
        private static bool               _built      = false;

        public static bool IsOpen => _overlay?.IsVisible == true;

        // ── public API ───────────────────────────────────────────────────────

        public static void Open(string tabId = "kits")
        {
            if (Player.player == null) return; // client-side guard

            if (!_built) Build();

            _overlay!.IsVisible = true;

            // Block player movement/look while the menu is open
            Player.player.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);

            var idx = TabIndexFor(tabId);
            SwitchTab(idx >= 0 ? idx : 0);
        }

        public static void LocalClose()
        {
            if (_overlay == null) return;
            _overlay.IsVisible = false;
            Player.player?.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
        }

        public static void Destroy()
        {
            if (_overlay == null) return;
            PlayerUI.window.RemoveChild(_overlay);
            _overlay  = null;
            _scroll   = null;
            _tabBtns.Clear();
            _tabs.Clear();
            _built    = false;
            _activeTab = -1;
        }

        /// <summary>
        /// Sends a chat message as the local player, which Unturned routes to
        /// the server as if the player typed the command themselves.
        /// </summary>
        public static void ExecuteCommandAsPlayer(string command)
        {
            if (Player.player == null || string.IsNullOrWhiteSpace(command)) return;
            ChatManager.sendChat(EChatMode.GLOBAL, command);
        }

        // ── build ─────────────────────────────────────────────────────────────

        private static void Build()
        {
            var cfg = MenuUIPlugin.Instance.Configuration.Instance;

            // ── full-screen clickable overlay ─────────────────────────────
            var overlayBox = Glazier.Get().CreateBox();
            overlayBox.SizeScale_X      = 1f;
            overlayBox.SizeScale_Y      = 1f;
            overlayBox.BackgroundColor  = Palette.OVERLAY;
            overlayBox.IsVisible        = false;
            _overlay = overlayBox;

            // ── main panel (centred) ──────────────────────────────────────
            var panel = Glazier.Get().CreateBox();
            panel.PositionOffset_X  = -W / 2f;
            panel.PositionOffset_Y  = -H / 2f;
            panel.PositionScale_X   = 0.5f;
            panel.PositionScale_Y   = 0.5f;
            panel.SizeOffset_X      = W;
            panel.SizeOffset_Y      = H;
            panel.BackgroundColor   = Palette.PANEL;
            overlayBox.AddChild(panel);

            // ── header ────────────────────────────────────────────────────
            var header = Glazier.Get().CreateBox();
            header.SizeOffset_X     = W;
            header.SizeOffset_Y     = HEADER_H;
            header.BackgroundColor  = Palette.HEADER;
            panel.AddChild(header);

            var title = Glazier.Get().CreateLabel();
            title.SizeOffset_X   = W;
            title.SizeOffset_Y   = HEADER_H;
            title.Text           = "MENU";
            title.TextColor      = Palette.TEXT;
            title.FontSize       = ESleekFontSize.Large;
            title.TextAlignment  = TextAnchor.MiddleCenter;
            header.AddChild(title);

            // ── tab bar ───────────────────────────────────────────────────
            var tabBar = Glazier.Get().CreateBox();
            tabBar.PositionOffset_Y = HEADER_H;
            tabBar.SizeOffset_X     = W;
            tabBar.SizeOffset_Y     = TABBAR_H;
            tabBar.BackgroundColor  = Palette.TAB_BAR;
            panel.AddChild(tabBar);

            float tabW = W / Math.Max(1, cfg.Tabs.Count);
            _tabBtns.Clear();
            for (int i = 0; i < cfg.Tabs.Count; i++)
            {
                int idx = i;
                var tb = Glazier.Get().CreateButton();
                tb.PositionOffset_X = i * tabW + 1f;
                tb.PositionOffset_Y = 2f;
                tb.SizeOffset_X     = tabW - 2f;
                tb.SizeOffset_Y     = TABBAR_H - 4f;
                tb.Text             = cfg.Tabs[i].Name;
                tb.TextColor        = Palette.TEXT;
                tb.BackgroundColor  = Palette.TAB_INACTIVE;
                tb.FontSize         = ESleekFontSize.Small;
                tb.OnClicked        += _ => SwitchTab(idx);
                tabBar.AddChild(tb);
                _tabBtns.Add(tb);
            }

            // ── scroll view (content area) ────────────────────────────────
            _scroll = Glazier.Get().CreateScrollView();
            _scroll.PositionOffset_X        = PAD;
            _scroll.PositionOffset_Y        = HEADER_H + TABBAR_H + PAD;
            _scroll.SizeOffset_X            = W - PAD * 2;
            _scroll.SizeOffset_Y            = CONTENT_H - PAD * 2;
            _scroll.IsScrollbarVisible      = true;
            _scroll.ContentSizeOffset       = Vector2.zero;
            panel.AddChild(_scroll);

            // ── footer / close button ─────────────────────────────────────
            var footer = Glazier.Get().CreateBox();
            footer.PositionOffset_Y = H - FOOTER_H;
            footer.SizeOffset_X     = W;
            footer.SizeOffset_Y     = FOOTER_H;
            footer.BackgroundColor  = Palette.FOOTER;
            panel.AddChild(footer);

            var closeBtn = Glazier.Get().CreateButton();
            closeBtn.PositionOffset_X = W / 2f - 80f;
            closeBtn.PositionOffset_Y = (FOOTER_H - 34f) / 2f;
            closeBtn.SizeOffset_X     = 160f;
            closeBtn.SizeOffset_Y     = 34f;
            closeBtn.Text             = "FECHAR";
            closeBtn.TextColor        = Palette.TEXT;
            closeBtn.BackgroundColor  = Palette.BTN_CLOSE;
            closeBtn.OnClicked        += _ => LocalClose();
            footer.AddChild(closeBtn);

            // ── build tabs ─────────────────────────────────────────────────
            _tabs.Clear();
            ulong steamId = Player.player?.channel.owner.playerID.steamID.m_SteamID ?? 0UL;

            foreach (var tabCfg in cfg.Tabs)
            {
                BaseTab tab = tabCfg.Id.ToLower() switch
                {
                    "kits"  => new KitsTab(_scroll, cfg.Kits),
                    "vips"  => new VipsTab(_scroll, cfg.Vips),
                    "homes" => new HomesTab(_scroll, MenuUIPlugin.Instance.Homes.GetHomes(steamId)),
                    "info"  => new InfoTab(_scroll, cfg.Infos),
                    _       => new InfoTab(_scroll, new List<InfoData>()),
                };
                tab.Create();
                tab.SetVisible(false);
                _tabs.Add(tab);
            }

            PlayerUI.window.AddChild(overlayBox);
            _built = true;
        }

        // ── helpers ───────────────────────────────────────────────────────────

        private static void SwitchTab(int index)
        {
            _activeTab = index;

            for (int i = 0; i < _tabBtns.Count; i++)
                _tabBtns[i].BackgroundColor = i == index ? Palette.TAB_ACTIVE : Palette.TAB_INACTIVE;

            for (int i = 0; i < _tabs.Count; i++)
                _tabs[i].SetVisible(i == index);

            // Refresh the active tab every time it is selected so dynamic
            // content (e.g. homes list) stays up to date
            if (index < _tabs.Count)
                _tabs[index].Refresh();
        }

        private static int TabIndexFor(string tabId)
        {
            var tabs = MenuUIPlugin.Instance.Configuration.Instance.Tabs;
            for (int i = 0; i < tabs.Count; i++)
                if (tabs[i].Id.Equals(tabId, StringComparison.OrdinalIgnoreCase))
                    return i;
            return -1;
        }
    }
}
