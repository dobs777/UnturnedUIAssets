using MenuUI.Models;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace MenuUI.UI.Tabs
{
    /// <summary>
    /// Vertical list of the player's saved homes.
    /// Clicking TELEPORTAR executes the home's command as the local player.
    /// </summary>
    public class HomesTab : BaseTab
    {
        private const float CARD_H = 58f;
        private const float PAD    = 8f;

        private readonly List<HomeEntry> _homes;

        public HomesTab(ISleekScrollView scrollView, List<HomeEntry> homes) : base(scrollView)
        {
            _homes = homes;
        }

        protected override void Build(ISleekElement container)
        {
            if (_homes.Count == 0)
            {
                var empty = CreateLabel(10, 20, 0, 60,
                    "Você não tem homes. Use /sethome <nome> para criar.",
                    TextAnchor.MiddleCenter, ESleekFontSize.Small);
                empty.SizeScale_X = 1f;
                container.AddChild(empty);
                SetScrollHeight(80);
                return;
            }

            for (int i = 0; i < _homes.Count; i++)
            {
                float y = PAD + i * (CARD_H + PAD);
                container.AddChild(BuildRow(y, _homes[i]));
            }

            SetScrollHeight(PAD + _homes.Count * (CARD_H + PAD));
        }

        private ISleekBox BuildRow(float y, HomeEntry home)
        {
            var card = Glazier.Get().CreateBox();
            card.PositionOffset_X = PAD;
            card.PositionOffset_Y = y;
            card.SizeScale_X      = 1f;
            card.SizeOffset_X     = -(PAD * 2);
            card.SizeOffset_Y     = CARD_H;
            card.BackgroundColor  = Palette.CARD;

            // Home name — takes all width except room for the button
            var nameLbl = Glazier.Get().CreateLabel();
            nameLbl.PositionOffset_X = 10;
            nameLbl.PositionOffset_Y = 0;
            nameLbl.SizeScale_X      = 1f;
            nameLbl.SizeOffset_X     = -160;
            nameLbl.SizeOffset_Y     = CARD_H;
            nameLbl.Text             = home.Name;
            nameLbl.TextColor        = Palette.TEXT;
            nameLbl.FontSize         = ESleekFontSize.Default;
            nameLbl.TextAlignment    = TextAnchor.MiddleLeft;
            card.AddChild(nameLbl);

            // TELEPORTAR button pinned to the right side of the card
            string cmd = home.Command;
            var btn = Glazier.Get().CreateButton();
            btn.PositionScale_X  = 1f;
            btn.PositionOffset_X = -150;
            btn.PositionOffset_Y = (CARD_H - 34f) / 2f;
            btn.SizeOffset_X     = 140f;
            btn.SizeOffset_Y     = 34f;
            btn.Text             = "TELEPORTAR";
            btn.TextColor        = Palette.TEXT;
            btn.BackgroundColor  = Palette.BTN_ACTION;
            btn.FontSize         = ESleekFontSize.Small;
            btn.OnClicked += _ =>
            {
                MenuUIManager.ExecuteCommandAsPlayer(cmd);
                MenuUIManager.LocalClose();
            };
            card.AddChild(btn);

            return card;
        }
    }
}
