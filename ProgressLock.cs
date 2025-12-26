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
        public static List<NpcEntry> allNpcEntries;
        public static List<EventEntry> allEventEntries;
        public override void Load()
        {
            /*
            //获得配置文件实例
            var config = ProgressLockConfig.Config;

            allNpcEntries = new List<NpcEntry>();
            allEventEntries = new List<EventEntry>();

            var configType = config.GetType();
            var fields = configType.GetFields();

            foreach (var field in fields)
            {
                // 获取字段值
                var value = field.GetValue(config);

                
                if (value is IEnumerable<NpcEntry> npcEntries)
                {
                    
                    allNpcEntries.AddRange(npcEntries);
                }

                if (value is IEnumerable<EventEntry> eventEntries)
                {
                    allEventEntries.AddRange(eventEntries);
                }
            }*/
        }


    }
}
        