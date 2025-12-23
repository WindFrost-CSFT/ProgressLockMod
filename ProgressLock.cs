using FullSerializer;
using ProgressLock.Models.Interfaces;
using System.Collections.Generic;
using Terraria.ModLoader;
using tModPorter;

namespace ProgressLock
{
    public class ProgressLock : Mod
    {
       public static List<IBossEntry> allBossEntries;
       public static List<IEventEntry> allEventEntries;
        public override void Load()
        {
            //获得配置文件实例
            var config = ProgressLockConfig.Config;

            allBossEntries = new List<IBossEntry>();
            allEventEntries = new List<IEventEntry>();

            var configType = config.GetType();
            var fields = configType.GetFields(); 

            foreach (var field in fields)
            {
                // 获取字段值
                var value = field.GetValue(config);

                // 判断是不是 IEnumerable<IBossEntry>
                if (value is IEnumerable<IBossEntry> bossEntries)
                {
                    //读取 config 并填充 allBossEntries
                    allBossEntries.AddRange(bossEntries);
                }
                
                if(value is IEnumerable<IEventEntry> eventEntries)
                {
                    allEventEntries.AddRange(eventEntries);
                }
            }
        }
    }
}