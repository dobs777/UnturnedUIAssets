using MenuUI.Models;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MenuUI.Data
{
    /// <summary>
    /// Persists per-player homes to Rocket's data directory.
    /// Each home stores the command executed when TELEPORTAR is clicked.
    /// </summary>
    public class HomesData
    {
        private static readonly string DataPath =
            Path.Combine(Rocket.Core.Environment.DataDirectory, "MenuUI_Homes.xml");

        private static readonly XmlSerializer Serializer =
            new(typeof(List<PlayerHomes>), new XmlRootAttribute("HomesData"));

        private List<PlayerHomes> _store = new();

        public void Load()
        {
            if (!File.Exists(DataPath)) return;
            try
            {
                using var fs = File.OpenRead(DataPath);
                _store = (List<PlayerHomes>)Serializer.Deserialize(fs)!;
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"[MenuUI] Could not load homes data: {ex.Message}");
                _store = new List<PlayerHomes>();
            }
        }

        public void Save()
        {
            try
            {
                using var fs = File.Create(DataPath);
                Serializer.Serialize(fs, _store);
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"[MenuUI] Could not save homes data: {ex.Message}");
            }
        }

        public List<HomeEntry> GetHomes(ulong steamId)
        {
            var record = _store.Find(p => p.SteamId == steamId);
            return record?.Homes ?? new List<HomeEntry>();
        }

        public bool SetHome(ulong steamId, string name, int maxHomes)
        {
            var record = GetOrCreate(steamId);
            var existing = record.Homes.FindIndex(h =>
                h.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existing >= 0)
            {
                // Update existing home command
                record.Homes[existing] = MakeEntry(name);
                Save();
                return true;
            }

            if (record.Homes.Count >= maxHomes) return false;

            record.Homes.Add(MakeEntry(name));
            Save();
            return true;
        }

        public bool DeleteHome(ulong steamId, string name)
        {
            var record = GetOrCreate(steamId);
            var removed = record.Homes.RemoveAll(h =>
                h.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) > 0;
            if (removed) Save();
            return removed;
        }

        private PlayerHomes GetOrCreate(ulong steamId)
        {
            var record = _store.Find(p => p.SteamId == steamId);
            if (record != null) return record;
            record = new PlayerHomes { SteamId = steamId };
            _store.Add(record);
            return record;
        }

        private static HomeEntry MakeEntry(string name) =>
            new() { Name = name, Command = $"/home {name.ToLower()}" };
    }

    [XmlRoot("PlayerHomes")]
    public class PlayerHomes
    {
        [XmlAttribute("steamId")]
        public ulong SteamId { get; set; }

        [XmlArray("Homes")]
        [XmlArrayItem("Home")]
        public List<HomeEntry> Homes { get; set; } = new();
    }
}
