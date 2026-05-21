using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MenuUI
{
    public class MenuUIConfig : IRocketPluginConfiguration
    {
        // ID do efeito do Workshop (mude para o ID do seu item no Workshop)
        public ushort EffectID = 49000;

        public List<TabConfig> Tabs;

        public void LoadDefaults()
        {
            EffectID = 49000;
            Tabs = new List<TabConfig>
            {
                new TabConfig
                {
                    Id = "kits",
                    DisplayName = "KITS",
                    Items = new List<MenuItemConfig>
                    {
                        new MenuItemConfig { Name = "Inicial",   ImageURL = "", Command = "/kit inicial",   Description = "Kit inicial do servidor", Cooldown = "24h" },
                        new MenuItemConfig { Name = "Medico",    ImageURL = "", Command = "/kit medico",    Description = "Itens medicos",           Cooldown = "1h"  },
                        new MenuItemConfig { Name = "Construtor",ImageURL = "", Command = "/kit construtor",Description = "Ferramentas de build",   Cooldown = "12h" },
                    }
                },
                new TabConfig
                {
                    Id = "vips",
                    DisplayName = "VIPS",
                    Items = new List<MenuItemConfig>
                    {
                        new MenuItemConfig { Name = "VIP Prata",  ImageURL = "", Command = "https://loja.exemplo.com/prata",  Description = "Kits exclusivos + 2 homes",  IsLink = true, ButtonLabel = "COMPRAR" },
                        new MenuItemConfig { Name = "VIP Ouro",   ImageURL = "", Command = "https://loja.exemplo.com/ouro",   Description = "Todos os beneficios VIP",    IsLink = true, ButtonLabel = "COMPRAR" },
                        new MenuItemConfig { Name = "VIP Platina",ImageURL = "", Command = "https://loja.exemplo.com/platina",Description = "Beneficios maximos",         IsLink = true, ButtonLabel = "COMPRAR" },
                    }
                },
                new TabConfig
                {
                    Id = "homes",
                    DisplayName = "HOMES",
                    IsDynamic = true,
                    Items = new List<MenuItemConfig>()
                },
                new TabConfig
                {
                    Id = "info",
                    DisplayName = "INFORMACOES",
                    Items = new List<MenuItemConfig>
                    {
                        new MenuItemConfig { Name = "Discord", ImageURL = "", Command = "https://discord.gg/xxxx",       Description = "Entre no nosso Discord!",   IsLink = true, ButtonLabel = "ENTRAR"  },
                        new MenuItemConfig { Name = "Loja",    ImageURL = "", Command = "https://loja.exemplo.com",     Description = "Compre VIP e itens!",       IsLink = true, ButtonLabel = "ACESSAR" },
                        new MenuItemConfig { Name = "Site",    ImageURL = "", Command = "https://site.exemplo.com",     Description = "Site oficial do servidor",   IsLink = true, ButtonLabel = "ACESSAR" },
                        new MenuItemConfig { Name = "Regras",  ImageURL = "", Command = "https://regras.exemplo.com",   Description = "Leia as regras!",           IsLink = true, ButtonLabel = "LER"     },
                        new MenuItemConfig { Name = "YouTube", ImageURL = "", Command = "https://youtube.com/@canal",  Description = "Nosso canal no YouTube",    IsLink = true, ButtonLabel = "ACESSAR" },
                        new MenuItemConfig { Name = "TikTok",  ImageURL = "", Command = "https://tiktok.com/@canal",   Description = "Nos siga no TikTok!",       IsLink = true, ButtonLabel = "SEGUIR"  },
                    }
                }
            };
        }
    }

    public class TabConfig
    {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string DisplayName;
        [XmlAttribute] public bool   IsDynamic;
        public List<MenuItemConfig> Items = new List<MenuItemConfig>();
    }

    public class MenuItemConfig
    {
        [XmlAttribute] public string Name;
        [XmlAttribute] public string ImageURL;
        [XmlAttribute] public string Command;
        [XmlAttribute] public string Description;
        [XmlAttribute] public string Cooldown;
        [XmlAttribute] public bool   IsLink;
        [XmlAttribute] public string ButtonLabel;
    }
}
