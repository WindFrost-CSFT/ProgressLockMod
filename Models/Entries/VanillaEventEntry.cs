using Newtonsoft.Json;
using ProgressLock.Enums;
using ProgressLock.Models.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace ProgressLock.Models.Entries
{
    public class VanillaEventEntry : IEventEntry
    {
        [CustomModConfigItem(typeof(ScrollableEnumElement))]
        public VanillaEvent Name { get; set; }

        
        [JsonIgnore]
        public Enum EventEnumName => Name;


        [Range(0, 7776000)]
        public long UnlockTimeSec { get; set; } = 500;


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

        public bool Match()
        {
            return EventEnumName switch
            {
                VanillaEvent.GoblinArmy => Main.invasionType == 1,
                VanillaEvent.FrostLegion => Main.invasionType == 2,
                VanillaEvent.PirateInvasion => Main.invasionType == 3,
                VanillaEvent.MartianMadness => Main.invasionType == 4,
                VanillaEvent.PumpkinMoon => Main.pumpkinMoon,
                VanillaEvent.ArmyOfDarkness => DD2Event.Ongoing,
                VanillaEvent.FrostMoon => Main.snowMoon,
                VanillaEvent.BloodMoon => Main.bloodMoon,
                VanillaEvent.SolarEclipse => Main.eclipse,
                VanillaEvent.LunarEvents =>
                    NPC.AnyNPCs(NPCID.LunarTowerSolar) ||
                    NPC.AnyNPCs(NPCID.LunarTowerVortex) ||
                    NPC.AnyNPCs(NPCID.LunarTowerNebula) ||
                    NPC.AnyNPCs(NPCID.LunarTowerStardust),
                _ => false
            };
        }

        /// <summary>
        /// 结束事件（把正在进行的事件清掉）
        /// </summary>
        public void Stop()
        {
            switch (EventEnumName)
            {
                case VanillaEvent.GoblinArmy:
                case VanillaEvent.FrostLegion:
                case VanillaEvent.PirateInvasion:
                case VanillaEvent.MartianMadness:
                    Main.invasionType = 0;
                    break;

                case VanillaEvent.PumpkinMoon:
                    Main.pumpkinMoon = false;
                    break;

                case VanillaEvent.ArmyOfDarkness:
                    DD2Event.StopInvasion();
                    break;

                case VanillaEvent.FrostMoon:
                    Main.snowMoon = false;
                    break;

                case VanillaEvent.BloodMoon:
                    Main.bloodMoon = false;
                    break;

                case VanillaEvent.SolarEclipse:
                    Main.eclipse = false;
                    break;

                case VanillaEvent.LunarEvents:
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Main.npc[i].active &&
                            (Main.npc[i].type == NPCID.LunarTowerSolar ||
                             Main.npc[i].type == NPCID.LunarTowerVortex ||
                             Main.npc[i].type == NPCID.LunarTowerNebula ||
                             Main.npc[i].type == NPCID.LunarTowerStardust))
                        {
                            Main.npc[i].active = false;
                        }
                    }
                    break;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is VanillaEventEntry other)
                return EventEnumName == other.EventEnumName && UnlockTimeSec == other.UnlockTimeSec;
            return false;
        }

        public override int GetHashCode()
        {
            return (EventEnumName, UnlockTimeSec).GetHashCode();
        }
    }
}
