using Microsoft.Xna.Framework;
using ProgressLock.Enums;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
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
            if (config.NpcEntries == null)
                return true;
            else
            {

                if (!IsUnlocked(npc, out LockStatus status))
                {
                    StopNPC(npc);
                    
                        switch (status)
                        {
                            case LockStatus.IsManuallyLocked:
                                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(GetMentionMsg("NpcManuallyLocked")), Color.IndianRed);
                                break;
                            case LockStatus.NotTimeYet:
                                foreach (var entry in config.NpcEntries)
                                {
                                    foreach(var def in entry.DefinitionList)
                                    {
                                        if (def.Type == npc.type)
                                        {
                                            DateTime unlockTime = DateTime.Parse(config.FirstTime).AddSeconds(entry.UnlockTimeSec);
                                            string formattedDate = unlockTime.ToString("MMM-d HH:mm:ss");
                                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(GetMentionMsg("NpcNotTimeYet", Lang.GetNPCNameValue(npc.type), formattedDate)), Color.IndianRed);
                                        }
                                    }
                                    

                                }
                               
                                break;
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
            if (config.EventEntries == null) return;
            foreach (var entry in config.EventEntries)
            {
                if (!IsUnlocked(out LockStatus status, out var matchedEntry))
                {
                    matchedEntry.Stop();
                   
                        switch (status)
                        {
                            case LockStatus.IsManuallyLocked:
                                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(GetMentionMsg("EventManuallyLocked", Language.GetTextValue($"Mods.ProgressLock.Configs.VanillaEvent.{matchedEntry.Name}.Label"))), Color.IndianRed);
                                break;
                            case LockStatus.NotTimeYet:
                                DateTime unlockTime = DateTime.Parse(config.FirstTime).AddSeconds(matchedEntry.UnlockTimeSec);
                                string formattedDate = unlockTime.ToString("MMM dd HH:mm:ss");
                                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(GetMentionMsg("EventNotTimeYet", Language.GetTextValue($"Mods.ProgressLock.Configs.VanillaEvent.{matchedEntry.Name}.Label"), formattedDate)), Color.IndianRed);
                                break;
                        }
                    
                }
            }
        }


    }
}