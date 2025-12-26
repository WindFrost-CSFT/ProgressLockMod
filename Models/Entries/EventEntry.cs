using Newtonsoft.Json;
using ProgressLock.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace ProgressLock.Models.Entries
{
    public class EventEntry
    {
        
        [CustomModConfigItem(typeof(ScrollableEnumElement))]
        [JsonProperty(Order = 0)]
        public VanillaEvent Name { get; set; }


        public List<string> Alias = new List<string>();

        
        [Range(0, 7776000)]
        public long UnlockTimeSec { get; set; } = 500;

        [JsonPropertyOrder(4)]
        public string UnlockTimeReadable
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(UnlockTimeSec);
                string result = Utils.FormatTimeSpan(ts);
                return result;
            }
        }
       
        
        public LockMode LockMode = LockMode.Automatic;

        public bool Match()
        {
            return Name switch
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
            switch (Name)
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
       
    }
}
