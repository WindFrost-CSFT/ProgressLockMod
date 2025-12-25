using FullSerializer;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tModPorter;
using ProgressLock.Models.Entries;

namespace ProgressLock
{
    public class ProgressLock : Mod
    {
        public static List<BossEntry> allBossEntries;
        public static List<EventEntry> allEventEntries;
        public override void Load()
        {
            //获得配置文件实例
            var config = ProgressLockConfig.Config;

            allBossEntries = new List<BossEntry>();
            allEventEntries = new List<EventEntry>();

            var configType = config.GetType();
            var fields = configType.GetFields();

            foreach (var field in fields)
            {
                // 获取字段值
                var value = field.GetValue(config);

                // 判断是不是 IEnumerable<IBossEntry>
                if (value is IEnumerable<BossEntry> bossEntries)
                {
                    //读取 config 并填充 allBossEntries
                    allBossEntries.AddRange(bossEntries);
                }

                if (value is IEnumerable<EventEntry> eventEntries)
                {
                    allEventEntries.AddRange(eventEntries);
                }
            }
        }


    }
}
        