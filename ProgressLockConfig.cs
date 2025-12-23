using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ProgressLock.Enums;
using ProgressLock.Models.Entries;
using ProgressLock.Models.Interfaces;
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
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static ProgressLockConfig Config;


        [DefaultValue("2025-12-19 10:30:10")]
        public string FirstTime { get; set; } = "2025-12-19 10:30:10";

        public List<VanillaBossEntry> VanillaBossEntryList;

        public List<CalamityBossEntry> CalamityBossEntryList;

        public List<VanillaEventEntry> VanillaEventEntryList;

        [DefaultValue(true)]
        public bool ShowMention = true;

        // 构造函数设置默认值
        public ProgressLockConfig()
        {
            VanillaBossEntryList = new List<VanillaBossEntry>
            {
                new VanillaBossEntry { Name = VanillaBoss.KingSlime, UnlockTimeSec = 1000,  },
                new VanillaBossEntry { Name = VanillaBoss.EaterOfWorlds, UnlockTimeSec = 2000 },
                new VanillaBossEntry { Name = VanillaBoss.WallOfFlesh, UnlockTimeSec = 3000 }
            };

            CalamityBossEntryList = new List<CalamityBossEntry>
            {
                new CalamityBossEntry { Name = CalamityBoss.DesertScourge, UnlockTimeSec = 1000 },
                new CalamityBossEntry { Name = CalamityBoss.Crabulon, UnlockTimeSec = 2000 },
                new CalamityBossEntry { Name = CalamityBoss.TheSlimeGod, UnlockTimeSec = 3000 }
            };

            VanillaEventEntryList = new List<VanillaEventEntry>
            {
                new VanillaEventEntry { Name = VanillaEvent.GoblinArmy, UnlockTimeSec = 1000 },
                new VanillaEventEntry { Name = VanillaEvent.SolarEclipse, UnlockTimeSec = 2000 },
                new VanillaEventEntry { Name = VanillaEvent.FrostMoon, UnlockTimeSec = 3000 }
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

        public IEnumerable<IEntry> GetAllEntries()
        {
            var fields = GetType().GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!field.FieldType.IsGenericType)
                    continue;

                var genericType = field.FieldType.GetGenericArguments()[0];

                if (!typeof(IEntry).IsAssignableFrom(genericType))
                    continue;

                if (field.GetValue(this) is IEnumerable list)
                {
                    foreach (var entry in list)
                        yield return (IEntry)entry;
                }
            }
        }
    }

    

    

   

   
    

    

    
}