using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Newtonsoft.Json;
using tModPorter;

namespace ProgressLock
{
    public class ProgressLockConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static ProgressLockConfig Instance { get; private set; }

        
        public string FirstTime { get; set; } = "2025-12-19 10:30:10";

        public List<VanillaBossEntry> VanillaBossEntryList { get; set; }

        public List<CalamityBossEntry> CalamityBossEntryList { get; set; }

        public List<VanillaEventEntry> VanillaEventEntryList { get; set; }

        [DefaultValue(true)]
        public bool ShowMention { get; set; } = true;

        // 构造函数设置默认值
        public ProgressLockConfig()
        {
            VanillaBossEntryList = new List<VanillaBossEntry>
            {
                new VanillaBossEntry { Name = VanillaLockItem.史莱姆王, UnlockTimeSec = 1000 },
                new VanillaBossEntry { Name = VanillaLockItem.世界吞噬者, UnlockTimeSec = 2000 },
                new VanillaBossEntry { Name = VanillaLockItem.血肉墙, UnlockTimeSec = 3000 }
            };

            CalamityBossEntryList = new List<CalamityBossEntry>
            {
                new CalamityBossEntry { Name = CalamityLockItem.荒漠灾虫, UnlockTimeSec = 1000 },
                new CalamityBossEntry { Name = CalamityLockItem.菌生蟹, UnlockTimeSec = 2000 },
                new CalamityBossEntry { Name = CalamityLockItem.史莱姆之神, UnlockTimeSec = 3000 }
            };

            VanillaEventEntryList = new List<VanillaEventEntry>
            {
                new VanillaEventEntry { Name = VanillaEventLockItem.哥布林军队, UnlockTimeSec = 1000 },
                new VanillaEventEntry { Name = VanillaEventLockItem.日食, UnlockTimeSec = 2000 },
                new VanillaEventEntry { Name = VanillaEventLockItem.霜月, UnlockTimeSec = 3000 }
            };
        }

        public override void OnLoaded()
        {
            Instance = this;
        }

        public override void OnChanged()
        {
            if (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.Server)
            {
                Main.NewText("进度锁配置已更新！", 100, 255, 100);
            }
            Main.NewText($"配置更新 - Boss数量: {VanillaBossEntryList?.Count ?? 0}", 100, 255, 100);
            Main.NewText($"FirstTime: {FirstTime}", 100, 255, 100);
            Main.NewText($"ShowMention: {ShowMention}", 100, 255, 100);
        }
    }

    public class VanillaBossEntry
    {
        [CustomModConfigItem(typeof(ScrollableEnumElement))]
        public VanillaLockItem Name { get; set; }

        [Range(0, 7776000)]
        
        public long UnlockTimeSec { get; set; } = 50;

        [JsonIgnore]
        public string UnlockTimeReadable
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(UnlockTimeSec);
                return $"{(int)ts.TotalDays}天 {ts.Hours}时 {ts.Minutes}分 {ts.Seconds}秒";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is VanillaBossEntry other)
                return Name == other.Name && UnlockTimeSec == other.UnlockTimeSec;
            return false;
        }

        public override int GetHashCode()
        {
            return (Name, UnlockTimeSec).GetHashCode();
        }
    }

    public class CalamityBossEntry
    {
        [CustomModConfigItem(typeof(ScrollableEnumElement))]
        public CalamityLockItem Name { get; set; }

        [Range(0, 7776000)]
        
        public long UnlockTimeSec { get; set; } = 50;

        [JsonIgnore]
        public string UnlockTimeReadable
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(UnlockTimeSec);
                return $"{(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is CalamityBossEntry other)
                return Name == other.Name && UnlockTimeSec == other.UnlockTimeSec;
            return false;
        }

        public override int GetHashCode()
        {
            return (Name, UnlockTimeSec).GetHashCode();
        }
    }

    public class VanillaEventEntry
    {
        [CustomModConfigItem(typeof(ScrollableEnumElement))]
        public VanillaEventLockItem Name { get; set; }

        [Range(0, 7776000)]
        
        public long UnlockTimeSec { get; set; } = 50;

        [JsonIgnore]
        public string UnlockTimeReadable
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(UnlockTimeSec);
                return $"{(int)ts.TotalDays}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is VanillaEventEntry other)
                return Name == other.Name && UnlockTimeSec == other.UnlockTimeSec;
            return false;
        }

        public override int GetHashCode()
        {
            return (Name, UnlockTimeSec).GetHashCode();
        }
    }

    public enum VanillaLockItem
    {
        史莱姆王,
        克苏鲁之眼,
        克苏鲁之脑,
        世界吞噬者,
        骷髅王,
        蜂王,
        血肉墙,
        机械骷髅王,
        双子魔眼,
        毁灭者,
        史莱姆皇后,
        世纪之花,
        石巨人,
        猪鲨公爵,
        光之女皇,
        拜月教邪教徒,
        月亮领主
    }

    public enum CalamityLockItem
    {
        荒漠灾虫,
        菌生蟹,
        腐巢意志,
        血肉宿主,
        史莱姆之神,
        渊海灾虫,
        硫磺火元素,
        极地之灵,
        灾厄之影,
        利维坦和阿娜希塔,
        白金星舰,
        瘟疫使者歌莉娅,
        毁灭魔像,
        星神游龙,
        亵渎守卫,
        痴愚金龙,
        亵渎天神普罗维登斯,
        风暴编织者,
        无尽虚空,
        神之使徒西格纳斯,
        噬魂幽花,
        硫海遗爵,
        神明吞噬者,
        重生之龙犽戎,
        星流巨械,
        至尊女巫灾厄
    }

    public enum VanillaEventLockItem
    {
        哥布林军队,
        血月,
        撒旦军队,
        海盗入侵,
        南瓜月,
        雪人军团,
        火星暴乱,
        月亮事件,
        霜月,
        日食
    }
}