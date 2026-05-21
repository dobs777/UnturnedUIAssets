using UnityEngine;

namespace MenuUI.UI
{
    /// <summary>
    /// Colour palette that matches Unturned's Vanilla dark UI theme.
    /// </summary>
    public static class Palette
    {
        // Backgrounds
        public static readonly Color OVERLAY       = new(0f,    0f,    0f,    0.72f);
        public static readonly Color PANEL         = new(0.12f, 0.12f, 0.12f, 0.97f);
        public static readonly Color HEADER        = new(0.08f, 0.08f, 0.08f, 1.00f);
        public static readonly Color TAB_BAR       = new(0.10f, 0.10f, 0.10f, 1.00f);
        public static readonly Color TAB_ACTIVE    = new(0.25f, 0.25f, 0.25f, 1.00f);
        public static readonly Color TAB_INACTIVE  = new(0.15f, 0.15f, 0.15f, 1.00f);
        public static readonly Color FOOTER        = new(0.08f, 0.08f, 0.08f, 1.00f);
        public static readonly Color CARD          = new(0.18f, 0.18f, 0.18f, 0.95f);
        public static readonly Color CARD_IMG_BG   = new(0.10f, 0.10f, 0.10f, 1.00f);
        public static readonly Color TRANSPARENT   = new(0f,    0f,    0f,    0.00f);

        // Buttons
        public static readonly Color BTN_ACTION    = new(0.20f, 0.45f, 0.20f, 1.00f); // green  – PEGAR / TELEPORTAR
        public static readonly Color BTN_BUY       = new(0.60f, 0.45f, 0.10f, 1.00f); // gold   – COMPRAR
        public static readonly Color BTN_LINK      = new(0.15f, 0.35f, 0.60f, 1.00f); // blue   – links externos
        public static readonly Color BTN_CLOSE     = new(0.55f, 0.12f, 0.12f, 1.00f); // red    – FECHAR
        public static readonly Color BTN_NEUTRAL   = new(0.28f, 0.28f, 0.28f, 1.00f);

        // Text
        public static readonly Color TEXT          = Color.white;
        public static readonly Color TEXT_DIM      = new(0.70f, 0.70f, 0.70f, 1.00f);
        public static readonly Color TEXT_ACCENT   = new(1.00f, 0.85f, 0.35f, 1.00f); // cooldown/info text
    }
}
