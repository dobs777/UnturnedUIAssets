using MenuUI.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MenuUI.UI.Tabs
{
    /// <summary>
    /// Grid of server information cards (Discord, Site, Loja, etc.).
    /// Clicking an item opens the configured URL in the system browser.
    /// </summary>
    public class InfoTab : BaseTab
    {
        private const float CARD_W  = 210f;
        private const float CARD_H  = 200f;
        private const float IMG_H   = 100f;
        private const float PAD     = 10f;
        private const int   COLUMNS = 3;

        private readonly List<InfoData> _infos;

        public InfoTab(ISleekScrollView scrollView, List<InfoData> infos) : base(scrollView)
        {
            _infos = infos;
        }

        protected override void Build(ISleekElement container)
        {
            if (_infos.Count == 0)
            {
                container.AddChild(CreateLabel(0, 0, 0, 60,
                    "Nenhuma informação configurada.",
                    TextAnchor.MiddleCenter, ESleekFontSize.Medium));
                SetScrollHeight(60);
                return;
            }

            for (int i = 0; i < _infos.Count; i++)
            {
                int col = i % COLUMNS;
                int row = i / COLUMNS;
                float x = PAD + col * (CARD_W + PAD);
                float y = PAD + row * (CARD_H + PAD);
                container.AddChild(BuildCard(x, y, _infos[i]));
            }

            int rows = (int)Math.Ceiling(_infos.Count / (float)COLUMNS);
            SetScrollHeight(PAD + rows * (CARD_H + PAD));
        }

        private ISleekBox BuildCard(float x, float y, InfoData info)
        {
            var card = CreateCard(x, y, CARD_W, CARD_H);

            // Image / logo area
            var imgBg = Glazier.Get().CreateBox();
            imgBg.SizeOffset_X    = CARD_W;
            imgBg.SizeOffset_Y    = IMG_H;
            imgBg.BackgroundColor = Palette.CARD_IMG_BG;
            card.AddChild(imgBg);

            if (!string.IsNullOrWhiteSpace(info.Image))
            {
                var img = CreateImage(0, 0, CARD_W, IMG_H);
                img.SetTextureUrl(info.Image, null, false);
                imgBg.AddChild(img);
            }
            else
            {
                imgBg.AddChild(CreateLabel(0, 0, CARD_W, IMG_H,
                    info.Name.ToUpper(),
                    TextAnchor.MiddleCenter, ESleekFontSize.Large));
            }

            float cursor = IMG_H + 4;

            card.AddChild(CreateLabel(6, cursor, CARD_W - 12, 24,
                info.Name, TextAnchor.MiddleLeft, ESleekFontSize.Default));
            cursor += 24;

            if (!string.IsNullOrWhiteSpace(info.Description))
            {
                var descLbl = Glazier.Get().CreateLabel();
                descLbl.PositionOffset_X = 6;
                descLbl.PositionOffset_Y = cursor;
                descLbl.SizeOffset_X     = CARD_W - 12;
                descLbl.SizeOffset_Y     = 34;
                descLbl.Text             = info.Description;
                descLbl.TextColor        = Palette.TEXT_DIM;
                descLbl.FontSize         = ESleekFontSize.Tiny;
                descLbl.TextAlignment    = TextAnchor.UpperLeft;
                card.AddChild(descLbl);
            }

            string link = info.Link;
            var btn = CreateButton(6, CARD_H - 36, CARD_W - 12, 30, "ACESSAR", Palette.BTN_LINK);
            btn.OnClicked += _ =>
            {
                if (!string.IsNullOrWhiteSpace(link))
                    UnityEngine.Application.OpenURL(link);
            };
            card.AddChild(btn);

            return card;
        }
    }
}
