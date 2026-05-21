using System.Collections.Generic;
using System.Xml.Serialization;

namespace MenuUI.Models
{
    [XmlRoot("Tab")]
    public class TabConfig
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        // Command that opens the menu directly on this tab (e.g. "/kits" -> tab "kits")
        [XmlAttribute("openCommand")]
        public string OpenCommand { get; set; } = string.Empty;
    }

    [XmlRoot("Kit")]
    public class KitData
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("image")]
        public string Image { get; set; } = string.Empty;

        // Command executed as the player when clicking PEGAR (e.g. "/kit inicial")
        [XmlAttribute("command")]
        public string Command { get; set; } = string.Empty;

        [XmlAttribute("description")]
        public string Description { get; set; } = string.Empty;

        [XmlAttribute("cooldown")]
        public string Cooldown { get; set; } = string.Empty;
    }

    [XmlRoot("Vip")]
    public class VipData
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("image")]
        public string Image { get; set; } = string.Empty;

        // URL opened in the browser when clicking COMPRAR
        [XmlAttribute("link")]
        public string Link { get; set; } = string.Empty;

        [XmlAttribute("description")]
        public string Description { get; set; } = string.Empty;
    }

    [XmlRoot("Info")]
    public class InfoData
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("image")]
        public string Image { get; set; } = string.Empty;

        // URL opened in the browser when clicking this item
        [XmlAttribute("link")]
        public string Link { get; set; } = string.Empty;

        [XmlAttribute("description")]
        public string Description { get; set; } = string.Empty;
    }

    [XmlRoot("Home")]
    public class HomeEntry
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        // Command executed as the player when clicking TELEPORTAR (e.g. "/home bed1")
        [XmlAttribute("command")]
        public string Command { get; set; } = string.Empty;
    }
}
