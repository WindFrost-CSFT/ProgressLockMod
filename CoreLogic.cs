using ProgressLock.Enums;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using static ProgressLock.Utils;


namespace ProgressLock
{

    // Boss控制

    public class ProgressLockGlobalNPC : GlobalNPC
    {

        public override bool PreAI(NPC npc)
        {
            var config = ProgressLockConfig.Config;
            
            if (npc == null || !npc.active) return true;
            if (ProgressLock.allBossEntries == null)
                return true;
            else
            {

                if (!IsUnlocked(npc, out LockStatus status))
                {
                    StopNPC(npc);
                    if (config.ShowMention)
                    {
                        switch (status)
                        {
                            case LockStatus.IsManuallyLocked:
                                Main.NewText($"Boss [{Lang.GetNPCNameValue(npc.type)}] 被手动锁定", 255, 100, 100);
                                break;
                            case LockStatus.NotTimeYet:
                                foreach (var entry in ProgressLock.allBossEntries)
                                {
                                    foreach(var bossWrapper in entry.DefinitionList)
                                    {
                                        if (ContentSamples.NpcsByNetId[bossWrapper.Definition.Type] == npc)
                                        {
                                            DateTime unlockTime = DateTime.Parse(config.FirstTime).AddSeconds(entry.UnlockTimeSec);
                                            Main.NewText($"Boss [{Lang.GetNPCNameValue(npc.type)}] 将在 {unlockTime:MM-dd HH:mm:ss} 解锁", 255, 100, 100);
                                        }
                                    }
                                    

                                }
                               
                                break;
                        }
                    }
                }
                return false;
            }
        }
    }

    // 事件控制
    public class ProgressLockWorld : ModSystem
    {
        
        public override void PreUpdateWorld()
        {
            
            var config = ProgressLockConfig.Config;
            if (ProgressLock.allEventEntries == null) return;
            foreach (var entry in ProgressLock.allEventEntries)
            {
                if (!IsUnlocked(out LockStatus status, out var matchedEntry))
                {
                    matchedEntry.Stop();
                    if (config.ShowMention)
                    {
                        switch (status)
                        {
                            case LockStatus.IsManuallyLocked:
                                Main.NewText($"事件 [{matchedEntry.Name}] 被手动锁定", 255, 100, 100);
                                break;
                            case LockStatus.NotTimeYet:
                                DateTime unlockTime = DateTime.Parse(config.FirstTime).AddSeconds(matchedEntry.UnlockTimeSec);
                                Main.NewText($"事件 [{matchedEntry.Name}] 将在 {unlockTime:MM-dd HH:mm:ss} 解锁", 255, 100, 100);
                                break;
                        }
                    }
                }
            }
        }


    }
}