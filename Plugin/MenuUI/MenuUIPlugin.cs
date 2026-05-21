using MenuUI.Commands;
using MenuUI.Data;
using MenuUI.UI;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;

namespace MenuUI
{
    public class MenuUIPlugin : RocketPlugin<MenuUIConfig>
    {
        public static MenuUIPlugin Instance { get; private set; } = null!;

        /// <summary>Per-player home storage (persisted to XML).</summary>
        public HomesData Homes { get; } = new HomesData();

        protected override void Load()
        {
            Instance = this;
            Homes.Load();

            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;

            Logger.Log("[MenuUI] Plugin carregado com sucesso!");
            Logger.Log($"[MenuUI]  Kits: {Configuration.Instance.Kits.Count}  " +
                       $"VIPs: {Configuration.Instance.Vips.Count}  " +
                       $"Infos: {Configuration.Instance.Infos.Count}");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;

            MenuUIManager.Destroy();
            Homes.Save();

            Instance = null!;
            Logger.Log("[MenuUI] Plugin descarregado.");
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            // If the disconnecting player is the local client, clean up
            if (SDG.Unturned.Player.player == player.Player)
                MenuUIManager.Destroy();
        }
    }
}
