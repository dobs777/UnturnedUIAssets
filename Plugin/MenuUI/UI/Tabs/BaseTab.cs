using SDG.Unturned;
using UnityEngine;

namespace MenuUI.UI.Tabs
{
    /// <summary>
    /// Base class for every tab. Each tab owns a root container that sits
    /// inside the scroll view; subclasses populate it in <see cref="Build"/>.
    /// </summary>
    public abstract class BaseTab
    {
        protected ISleekScrollView ScrollView { get; }

        // Root box added directly to the scroll view
        protected ISleekElement? Root { get; private set; }

        protected BaseTab(ISleekScrollView scrollView)
        {
            ScrollView = scrollView;
        }

        public void Create()
        {
            var box = Glazier.Get().CreateBox();
            box.SizeScale_X = 1f;
            box.SizeOffset_Y = 0; // set by Build
            box.BackgroundColor = Palette.TRANSPARENT;
            Root = box;
            Build(box);
            ScrollView.AddChild(Root);
        }

        public void SetVisible(bool visible)
        {
            if (Root != null)
                Root.IsVisible = visible;
        }

        public void Refresh()
        {
            if (Root == null) return;
            // Remove old children and rebuild
            Root.RemoveAllChildren();
            Build(Root);
        }

        /// <summary>Populate <paramref name="container"/> with card elements.</summary>
        protected abstract void Build(ISleekElement container);

        // ── helpers ──────────────────────────────────────────────────────────

        protected static ISleekBox CreateCard(float x, float y, float w, float h)
        {
            var card = Glazier.Get().CreateBox();
            card.PositionOffset_X = x;
            card.PositionOffset_Y = y;
            card.SizeOffset_X     = w;
            card.SizeOffset_Y     = h;
            card.BackgroundColor  = Palette.CARD;
            return card;
        }

        protected static ISleekLabel CreateLabel(
            float x, float y, float w, float h,
            string text,
            TextAnchor alignment    = TextAnchor.MiddleLeft,
            ESleekFontSize fontSize = ESleekFontSize.Default)
        {
            var lbl = Glazier.Get().CreateLabel();
            lbl.PositionOffset_X = x;
            lbl.PositionOffset_Y = y;
            lbl.SizeOffset_X     = w;
            lbl.SizeOffset_Y     = h;
            lbl.Text             = text;
            lbl.TextColor        = Palette.TEXT;
            lbl.FontSize         = fontSize;
            lbl.TextAlignment    = alignment;
            return lbl;
        }

        protected static ISleekButton CreateButton(
            float x, float y, float w, float h,
            string text, Color bg)
        {
            var btn = Glazier.Get().CreateButton();
            btn.PositionOffset_X = x;
            btn.PositionOffset_Y = y;
            btn.SizeOffset_X     = w;
            btn.SizeOffset_Y     = h;
            btn.Text             = text;
            btn.TextColor        = Palette.TEXT;
            btn.BackgroundColor  = bg;
            btn.FontSize         = ESleekFontSize.Small;
            return btn;
        }

        protected static ISleekImage CreateImage(float x, float y, float w, float h)
        {
            var img = Glazier.Get().CreateImage();
            img.PositionOffset_X = x;
            img.PositionOffset_Y = y;
            img.SizeOffset_X     = w;
            img.SizeOffset_Y     = h;
            return img;
        }

        /// <summary>
        /// Sets the total scrollable height on the parent scroll view
        /// after all cards have been placed.
        /// </summary>
        protected void SetScrollHeight(float totalHeight)
        {
            if (Root != null)
                Root.SizeOffset_Y = totalHeight;
            ScrollView.ContentSizeOffset = new Vector2(0f, totalHeight);
        }
    }
}
