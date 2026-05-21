using MenuUI.Models;
using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MenuUI
{
    public class MenuUIConfig : IRocketPluginConfiguration
    {
        [XmlArray("Tabs")]
        [XmlArrayItem("Tab")]
        public List<TabConfig> Tabs { get; set; } = new();

        [XmlArray("Kits")]
        [XmlArrayItem("Kit")]
        public List<KitData> Kits { get; set; } = new();

        [XmlArray("Vips")]
        [XmlArrayItem("Vip")]
        public List<VipData> Vips { get; set; } = new();

        [XmlArray("Infos")]
        [XmlArrayItem("Info")]
        public List<InfoData> Infos { get; set; } = new();

        // Maximum number of homes a player can set with /sethome
        [XmlElement("MaxHomesPerPlayer")]
        public int MaxHomesPerPlayer { get; set; } = 5;

        // Whether closing the UI re-enables player input automatically
        [XmlElement("RestoreInputOnClose")]
        public bool RestoreInputOnClose { get; set; } = true;

        public void LoadDefaults()
        {
            Tabs = new List<TabConfig>
            {
                new() { Id = "kits",  Name = "KITS",        OpenCommand = "/kits"  },
                new() { Id = "vips",  Name = "VIPS",        OpenCommand = "/vips"  },
                new() { Id = "homes", Name = "HOMES",       OpenCommand = "/homes" },
                new() { Id = "info",  Name = "INFORMAÇÕES", OpenCommand = "/info"  },
            };

            Kits = new List<KitData>
            {
                new()
                {
                    Name        = "Iniciante",
                    Image       = "",
                    Command     = "/kit iniciante",
                    Description = "Kit básico para novos jogadores",
                    Cooldown    = "24h",
                },
                new()
                {
                    Name        = "Sobrevivente",
                    Image       = "",
                    Command     = "/kit sobrevivente",
                    Description = "Equipamento de sobrevivência completo",
                    Cooldown    = "12h",
                },
                new()
                {
                    Name        = "VIP",
                    Image       = "",
                    Command     = "/kit vip",
                    Description = "Exclusivo para membros VIP",
                    Cooldown    = "6h",
                },
            };

            Vips = new List<VipData>
            {
                new()
                {
                    Name        = "VIP Prata",
                    Image       = "",
                    Link        = "https://loja.exemplo.com/vip-prata",
                    Description = "Kits exclusivos + cargo no Discord",
                },
                new()
                {
                    Name        = "VIP Ouro",
                    Image       = "",
                    Link        = "https://loja.exemplo.com/vip-ouro",
                    Description = "Todos os benefícios Prata + kits melhores",
                },
                new()
                {
                    Name        = "VIP Diamante",
                    Image       = "",
                    Link        = "https://loja.exemplo.com/vip-diamante",
                    Description = "Máximo acesso + benefícios exclusivos",
                },
            };

            Infos = new List<InfoData>
            {
                new()
                {
                    Name        = "Discord",
                    Image       = "",
                    Link        = "https://discord.gg/exemplo",
                    Description = "Entre na nossa comunidade!",
                },
                new()
                {
                    Name        = "Site",
                    Image       = "",
                    Link        = "https://site.exemplo.com",
                    Description = "Notícias e updates do servidor",
                },
                new()
                {
                    Name        = "Loja",
                    Image       = "",
                    Link        = "https://loja.exemplo.com",
                    Description = "Adquira VIPs e itens exclusivos",
                },
                new()
                {
                    Name        = "Regras",
                    Image       = "",
                    Link        = "https://site.exemplo.com/regras",
                    Description = "Leia as regras do servidor",
                },
            };

            MaxHomesPerPlayer    = 5;
            RestoreInputOnClose  = true;
        }
    }
}
