using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ProgressLock.Enums;
using ProgressLock.Models.Entries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using tModPorter;

namespace ProgressLock
{
    public class ProgressLockConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static ProgressLockConfig Config;


        [DefaultValue("2025-12-19 10:30:10")]
        public string FirstTime { get; set; } = "2025-12-19 10:30:10";

        public List<BossEntry> BossEntries;

        public List<EventEntry> EventEntries;

        // public List<VanillaBossEntry> VanillaBossEntryList;

        // public List<CalamityBossEntry> CalamityBossEntryList;

        // public List<VanillaEventEntry> VanillaEventEntryList;

        [DefaultValue(true)]
        public bool ShowMention = true;

        // 构造函数设置默认值
        public ProgressLockConfig()
        {
            BossEntries = new List<BossEntry>
            {
                new BossEntry { DefinitionList = new List<NPCDefinition>{ new NPCDefinition("Terraria", "KingSlime")}, UnlockTimeSec = 5000, IsManuallyLocked = false },
                new BossEntry { DefinitionList = new List<NPCDefinition>{new ("Terraria", "EaterofWorldsHead")}, UnlockTimeSec = 10000, IsManuallyLocked = false },
                new BossEntry { DefinitionList = new List<NPCDefinition>{new ("CalamityMod","DesertScourge")} , UnlockTimeSec = 7000 , IsManuallyLocked = false},
                new BossEntry { DefinitionList = new List<NPCDefinition>{new ("CalamityMod","TheSlimeGod")} , UnlockTimeSec = 12000 , IsManuallyLocked = false },
            };
            EventEntries = new List<EventEntry>
            {
                new EventEntry { Name = VanillaEvent.FrostLegion, UnlockTimeSec = 8000, IsManuallyLocked = false },
                new EventEntry { Name = VanillaEvent.MartianMadness, UnlockTimeSec = 15000, IsManuallyLocked = false  },
                new EventEntry { Name = VanillaEvent.PumpkinMoon, UnlockTimeSec = 20000, IsManuallyLocked = false  },
            };


        }

        public override void OnLoaded()
        {
            Config = this;
        }

        public override void OnChanged()
        {
            if (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.Server)
            {
                Main.NewText("进度锁配置已更新！", 100, 255, 100);
            }
            //Main.NewText($"配置更新 - Boss数量: {VanillaBossEntryList?.Count ?? 0}", 100, 255, 100);
            //Main.NewText($"FirstTime: {FirstTime}", 100, 255, 100);
            //Main.NewText($"ShowMention: {ShowMention}", 100, 255, 100);
        }

        
        public IEnumerable<BossEntry> GetAllBossEntries()
        {
            var fields = GetType().GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!field.FieldType.IsGenericType)
                    continue;

                var genericType = field.FieldType.GetGenericArguments()[0];

                if (!typeof(BossEntry).IsAssignableFrom(genericType))
                    continue;

                if (field.GetValue(this) is IEnumerable list)
                {
                    foreach (var entry in list)
                        yield return (BossEntry)entry;
                }
            }
        }
        public IEnumerable<EventEntry> GetAllEventEntries()
        {
            var fields = GetType().GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!field.FieldType.IsGenericType)
                    continue;

                var genericType = field.FieldType.GetGenericArguments()[0];

                if (!typeof(EventEntry).IsAssignableFrom(genericType))
                    continue;

                if (field.GetValue(this) is IEnumerable list)
                {
                    foreach (var entry in list)
                        yield return (EventEntry)entry;
                }
            }
        }
    }

    

    

   

   
    

    

    
}