using ProgressLock.UI;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace ProgressLock.Models.Entries
{
    public class BossEntry
    {

        public List<NPCDefinition> DefinitionList = new List<NPCDefinition>{
            new NPCDefinition("Terraria", "KingSlime"),
        };

        public List<string> Alias  = new List<string>();


        [Range(0, 7776000)]
        public long UnlockTimeSec  = 50;

        public string UnlockTimeReadable
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(UnlockTimeSec);
                string result = Utils.FormatTimeSpan(ts);
                return result;
            }
        }


        public bool IsManuallyLocked  = false;

        public void Stop(NPC npc)
        {
            npc.active = false;
            NetMessage.SendData(
            MessageID.SyncNPC,
            -1, // 所有客户端
            -1, // 不排除自己
            null, // 文本参数通常为空
            npc.whoAmI // 发送哪一个 NPC
            );
        }
        public override bool Equals(object obj)
        {
            if (obj is BossEntry other)
                return DefinitionList == other.DefinitionList && UnlockTimeSec == other.UnlockTimeSec;
            return false;
        }


        public override int GetHashCode()
        {
            return (DefinitionList, UnlockTimeSec).GetHashCode();
        }
    }
}
