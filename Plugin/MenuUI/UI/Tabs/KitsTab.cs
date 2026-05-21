using MenuUI.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MenuUI.UI.Tabs
{
    /// <summary>
    /// Grid of kit cards. Each card: image banner → name → description → cooldown → PEGAR button.
    /// Clicking PEGAR executes the kit's command as the local player.
    /// </summary>
    public class KitsTab : BaseTab
    {
        private const float CARD_W      = 210f;
        private const float CARD_H      = 210f;
        private const float IMG_H       = 100f;
        private const float PAD         = 10f;
        private const int   COLUMNS     = 3;

        private readonly List<KitData> _kits;

        public KitsTab(ISleekScrollView scrollView, List<KitData> kits) : base(scrollView)
        {
            _kits = kits;
        }

        protected override void Build(ISleekElement container)
        {
            if (_kits.Count == 0)
            {
                container.AddChild(CreateLabel(0, 0, 0, 60,
                    "Nenhum kit configurado.",
                    TextAnchor.MiddleCenter, ESleekFontSize.Medium));
                container.SizeOffset_Y = 60;
                SetScrollHeight(60);
                return;
            }

            for (int i = 0; i < _kits.Count; i++)
            {
                int col = i % COLUMNS;
                int row = i / COLUMNS;
                float x = PAD + col * (CARD_W + PAD);
                float y = PAD + row * (CARD_H + PAD);

                container.AddChild(BuildCard(x, y, _kits[i]));
            }

            int rows        = (int)Math.Ceiling(_kits.Count / (float)COLUMNS);
            float totalH    = PAD + rows * (CARD_H + PAD);
            SetScrollHeight(totalH);
        }

        private ISleekBox BuildCard(float x, float y, KitData kit)
        {
            var card = CreateCard(x, y, CARD_W, CARD_H);

            // Image area
            var imgBg = Glazier.Get().CreateBox();
            imgBg.SizeOffset_X     = CARD_W;
            imgBg.SizeOffset_Y     = IMG_H;
            imgBg.BackgroundColor  = Palette.CARD_IMG_BG;
            card.AddChild(imgBg);

            if (!string.IsNullOrWhiteSpace(kit.Image))
            {
                var img = CreateImage(0, 0, CARD_W, IMG_H);
                img.SetTextureUrl(kit.Image, null, false);
                imgBg.AddChild(img);
            }
            else
            {
                // Fallback: kit name as large text inside the image area
                imgBg.AddChild(CreateLabel(0, 0, CARD_W, IMG_H,
                    kit.Name.ToUpper(),
                    TextAnchor.MiddleCenter, ESleekFontSize.Medium));
            }

            float cursor = IMG_H + 4;

            // Kit name
            card.AddChild(CreateLabel(6, cursor, CARD_W - 12, 24,
                kit.Name, TextAnchor.MiddleLeft, ESleekFontSize.Default));
            cursor += 24;

            // Description (optional)
            if (!string.IsNullOrWhiteSpace(kit.Description))
            {
                var descLbl = Glazier.Get().CreateLabel();
                descLbl.PositionOffset_X = 6;
                descLbl.PositionOffset_Y = cursor;
                descLbl.SizeOffset_X     = CARD_W - 12;
                descLbl.SizeOffset_Y     = 28;
                descLbl.Text             = kit.Description;
                descLbl.TextColor        = Palette.TEXT_DIM;
                descLbl.FontSize         = ESleekFontSize.Tiny;
                descLbl.TextAlignment    = TextAnchor.UpperLeft;
                card.AddChild(descLbl);
                cursor += 28;
            }

            // Cooldown badge (optional)
            if (!string.IsNullOrWhiteSpace(kit.Cooldown))
            {
                card.AddChild(CreateLabel(6, cursor, CARD_W - 12, 18,
                    $"⏱ {kit.Cooldown}", TextAnchor.MiddleLeft, ESleekFontSize.Tiny));
                cursor += 18;
            }

            // PEGAR button — pinned to card bottom
            string cmd = kit.Command;
            var btn = CreateButton(6, CARD_H - 36, CARD_W - 12, 30, "PEGAR", Palette.BTN_ACTION);
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
