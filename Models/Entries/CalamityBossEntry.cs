using ProgressLock.Enums;
using ProgressLock.Models.Interfaces;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using Newtonsoft.Json;

namespace ProgressLock.Models.Entries
{
    public class CalamityBossEntry : IBossEntry
    {
        
        [CustomModConfigItem(typeof(ScrollableEnumElement))]
        public CalamityBoss Name { get; set; }
        //  public BossModType BossModType => BossModType.Calamity;

        [JsonIgnore]
        public Enum BossEnumName => Name;


        [Range(0, 7776000)]

        public long UnlockTimeSec { get; set; } = 50;


        public string UnlockTimeReadable
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(UnlockTimeSec);
                return $"{(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
            }
        }

        [DefaultValue(false)]
        public bool IsManuallyLocked { get; set; } = false;


        public bool Match(NPC npc)
        {
            if (npc.ModNPC?.Mod.Name != "CalamityMod")
                return false;

            string calamityNPC = npc.ModNPC.GetType().Name;

            return BossEnumName switch
            {
                CalamityBoss.DesertScourge => calamityNPC.Contains("DesertScourge"),
                CalamityBoss.Crabulon => calamityNPC.Contains("Crabulon"),
                CalamityBoss.TheHiveMind => calamityNPC.Contains("HiveMind"),
                CalamityBoss.ThePerforators => calamityNPC.Contains("Perforator"),
                CalamityBoss.TheSlimeGod => calamityNPC.Contains("SlimeGod"),

                CalamityBoss.Cryogen => calamityNPC.Contains("Cryogen"),
                CalamityBoss.AquaticScourge => calamityNPC.Contains("AquaticScourge"),
                CalamityBoss.BrimstoneElemental => calamityNPC.Contains("BrimstoneElemental"),
                CalamityBoss.CalamitasClone => calamityNPC.Contains("CalamitasClone"),
                CalamityBoss.LeviathanAndAnahita => calamityNPC.Contains("Leviathan") || calamityNPC.Contains("Anahita"),

                CalamityBoss.AstrumAureus => calamityNPC.Contains("AstrumAureus"),
                CalamityBoss.ThePlaguebringerGoliath => calamityNPC.Contains("PlaguebringerGoliath"),
                CalamityBoss.Ravager => calamityNPC.Contains("Ravager"),
                CalamityBoss.AstrumDeus => calamityNPC.Contains("AstrumDeus"),

                CalamityBoss.ProfanedGuardians => calamityNPC.Contains("ProfanedGuardian"),
                CalamityBoss.Dragonfolly => calamityNPC.Contains("Dragonfolly"),
                CalamityBoss.ProvidenceTheProfanedGoddess => calamityNPC.Contains("Providence"),
                CalamityBoss.StormWeaver => calamityNPC.Contains("StormWeaver"),
                CalamityBoss.CeaselessVoid => calamityNPC.Contains("CeaselessVoid"),
                CalamityBoss.SignusEnvoyOfTheDevourer => calamityNPC.Contains("Signus"),
                CalamityBoss.Polterghast => calamityNPC.Contains("Polterghast"),
                CalamityBoss.TheOldDuke => calamityNPC.Contains("OldDuke"),
                CalamityBoss.TheDevourerOfGods => calamityNPC.Contains("DevourerofGods"),
                CalamityBoss.YharonDragonOfRebirth => calamityNPC.Contains("Yharon"),
                CalamityBoss.ExoMechs => calamityNPC.Contains("ExoMechs"),
                CalamityBoss.SupremeWitchCalamitas => calamityNPC.Contains("SupremeCalamitas"),

                _ => false
            };
        }

        
        public override bool Equals(object obj)
        {
            if (obj is CalamityBossEntry other)
                return BossEnumName == other.BossEnumName && UnlockTimeSec == other.UnlockTimeSec;
            return false;
        }

        public override int GetHashCode()
        {
            return (BossEnumName, UnlockTimeSec).GetHashCode();
        }
    }
}
