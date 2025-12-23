using ProgressLock.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace ProgressLock.Models.Interfaces
{
    public interface IBossEntry 
    {
       // BossModType BossModType { get; }

        Enum BossEnumName { get;  }
        
        long UnlockTimeSec { get; set; }

        bool IsManuallyLocked { get; set; }
        bool Match(NPC npc);
        void Stop(NPC npc) {
            npc.active = false;
            NetMessage.SendData(
            MessageID.SyncNPC,
            -1, // 所有客户端
            -1, // 不排除自己
            null, // 文本参数通常为空
            npc.whoAmI // 发送哪一个 NPC
            );
        }
    }
}
