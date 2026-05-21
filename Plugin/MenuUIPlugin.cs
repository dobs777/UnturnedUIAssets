using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using SDG.Unturned;

namespace MenuUI
{
    public class MenuUIPlugin : RocketPlugin<MenuUIConfig>
    {
        public static MenuUIPlugin Instance { get; private set; }
        public MenuUIManager UIManager { get; private set; }

        protected override void Load()
        {
            Instance = this;
            UIManager = new MenuUIManager(Configuration.Instance);
            EffectManager.onEffectButtonClicked += UIManager.OnButtonClicked;
            Logger.Log("[MenuUI] Loaded. EffectID=" + Configuration.Instance.EffectID);
        }

        protected override void Unload()
        {
            EffectManager.onEffectButtonClicked -= UIManager.OnButtonClicked;
            UIManager?.CloseAll();
            Instance = null;
        }
    }
}
