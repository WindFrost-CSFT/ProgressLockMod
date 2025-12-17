using Microsoft.Xna.Framework.Audio;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProgressLock
{
    // Boss控制
    public class ProgressLockGlobalNPC : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {

            var config = ProgressLockConfig.Instance;
            if (config?.VanillaBossEntryList == null)
                return true;
            else
            {
                foreach (var lockEntry in config.VanillaBossEntryList)
                {

                    if (IsLockedBoss(npc, lockEntry) && !IsUnlocked(lockEntry.UnlockTimeSec))
                    {
                        npc.active = false;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
                        }



                        if (config.ShowMention)
                        {
                            DateTime unlockTime = DateTime.Parse(ProgressLockConfig.Instance.FirstTime).AddSeconds(lockEntry.UnlockTimeSec);
                            Main.NewText($"Boss [{lockEntry.Name}] 将在 {unlockTime:MM-dd HH:mm:ss} 解锁", 255, 100, 100);
                        }

                        return false;

                    }
                }
            }



            if (config?.CalamityBossEntryList == null)
                return true;
            else
            {
                foreach (var lockEntry in config.CalamityBossEntryList)
                {

                    if (IsLockedBoss(npc, lockEntry) && !IsUnlocked(lockEntry.UnlockTimeSec))
                    {
                        npc.active = false;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
                        }

                        // 只在启用调试时显示消息

                        if (config.ShowMention)
                        {
                            DateTime unlockTime = DateTime.Parse(ProgressLockConfig.Instance.FirstTime).AddSeconds(lockEntry.UnlockTimeSec);
                            Main.NewText($"Boss [{lockEntry.Name}] 将在 {unlockTime:MM-dd HH:mm:ss} 解锁", 255, 100, 100);
                        }

                        return false;

                    }
                }
            }
            return true;
        }

        private bool IsLockedBoss(NPC npc, VanillaBossEntry entry)
        {

            // 原版Boss检测


            // 原版Boss检测
            if (entry.Name == VanillaLockItem.史莱姆王 && npc.type == NPCID.KingSlime) return true;
            if (entry.Name == VanillaLockItem.克苏鲁之眼 && npc.type == NPCID.EyeofCthulhu) return true;

            // 蠕虫（世界吞噬者）各部位
            if (entry.Name == VanillaLockItem.世界吞噬者 &&
                (npc.type == NPCID.EaterofWorldsHead
              || npc.type == NPCID.EaterofWorldsBody
              || npc.type == NPCID.EaterofWorldsTail)) return true;

            // Brain of Cthulhu 是单体但也可能分身（它自己在 npc.type 列表里）
            if (entry.Name == VanillaLockItem.克苏鲁之脑 && npc.type == NPCID.BrainofCthulhu) return true;

            // Skeletron 各部位（头 + 手）
            if (entry.Name == VanillaLockItem.骷髅王 &&
                (npc.type == NPCID.SkeletronHead
              || npc.type == NPCID.SkeletronHand)) return true;

            // Bee Queen
            if (entry.Name == VanillaLockItem.蜂王 && npc.type == NPCID.QueenBee) return true;

            // Wall of Flesh（肉墙 + 眼睛/嘴部其实都算 Wall of Flesh）
            if (entry.Name == VanillaLockItem.血肉墙 && npc.type == NPCID.WallofFlesh) return true;

            // 机械骷髅王 各部位
            if (entry.Name == VanillaLockItem.机械骷髅王 &&
                (npc.type == NPCID.SkeletronPrime
              || npc.type == NPCID.PrimeCannon
              || npc.type == NPCID.PrimeSaw
              || npc.type == NPCID.PrimeVice
              || npc.type == NPCID.PrimeLaser)) return true;

            // Twins 各部位（两个魔眼）
            if (entry.Name == VanillaLockItem.双子魔眼 &&
                (npc.type == NPCID.Retinazer
              || npc.type == NPCID.Spazmatism)) return true;

            // The Destroyer 各段
            if (entry.Name == VanillaLockItem.毁灭者 &&
                (npc.type == NPCID.TheDestroyer
              || npc.type == NPCID.TheDestroyerBody
              || npc.type == NPCID.TheDestroyerTail)) return true;

            // Queen Slime
            if (entry.Name == VanillaLockItem.史莱姆皇后 && npc.type == NPCID.QueenSlimeBoss) return true;

            // Plantera
            if (entry.Name == VanillaLockItem.世纪之花 && npc.type == NPCID.Plantera) return true;

            // Golem
            if (entry.Name == VanillaLockItem.石巨人 && npc.type == NPCID.Golem) return true;

            // Duke Fishron
            if (entry.Name == VanillaLockItem.猪鲨公爵 && npc.type == NPCID.DukeFishron) return true;

            // Empress of Light
            if (entry.Name == VanillaLockItem.光之女皇 && npc.type == NPCID.HallowBoss) return true;

            // Lunatic Cultist
            if (entry.Name == VanillaLockItem.拜月教邪教徒 && npc.type == NPCID.CultistBoss) return true;

            // Moon Lord（所有部位）
            if (entry.Name == VanillaLockItem.月亮领主 &&
                (npc.type == NPCID.MoonLordCore
              || npc.type == NPCID.MoonLordHead
              || npc.type == NPCID.MoonLordHand
              || npc.type == NPCID.MoonLordFreeEye)) return true;





            return false;
        }

        private bool IsLockedBoss(NPC npc, CalamityBossEntry entry)
        {

            if (npc.ModNPC != null && npc.ModNPC.Mod.Name == "CalamityMod")
            {
                // 灾厄Boss检测
                string modNPCName = npc.ModNPC.GetType().Name;

                // 灾厄Mod Boss - 困难模式前
                if (entry.Name == CalamityLockItem.荒漠灾虫 && modNPCName.Contains("DesertScourge")) return true;
                if (entry.Name == CalamityLockItem.菌生蟹 && modNPCName.Contains("Crabulon")) return true;
                if (entry.Name == CalamityLockItem.腐巢意志 && modNPCName.Contains("HiveMind")) return true;
                if (entry.Name == CalamityLockItem.血肉宿主 && modNPCName.Contains("Perforator")) return true;
                if (entry.Name == CalamityLockItem.史莱姆之神 && modNPCName.Contains("SlimeGod")) return true;

                // 灾厄Mod Boss - 困难模式
                if (entry.Name == CalamityLockItem.极地之灵 && modNPCName.Contains("Cryogen")) return true;
                if (entry.Name == CalamityLockItem.渊海灾虫 && modNPCName.Contains("AquaticScourge")) return true;
                if (entry.Name == CalamityLockItem.硫磺火元素 && modNPCName.Contains("BrimstoneElemental")) return true;
                if (entry.Name == CalamityLockItem.灾厄之影 && modNPCName.Contains("CalamitasClone")) return true;
                if (entry.Name == CalamityLockItem.利维坦和阿娜希塔 && (modNPCName.Contains("Leviathan") || modNPCName.Contains("Anahita"))) return true;
                if (entry.Name == CalamityLockItem.白金星舰 && modNPCName.Contains("AstrumAureus")) return true;
                if (entry.Name == CalamityLockItem.瘟疫使者歌莉娅 && modNPCName.Contains("PlaguebringerGoliath")) return true;
                if (entry.Name == CalamityLockItem.毁灭魔像 && modNPCName.Contains("Ravager")) return true;
                if (entry.Name == CalamityLockItem.星神游龙 && modNPCName.Contains("AstrumDeus")) return true;

                // 灾厄Mod Boss - 月后
                if (entry.Name == CalamityLockItem.亵渎守卫 && modNPCName.Contains("ProfanedGuardian")) return true;
                if (entry.Name == CalamityLockItem.痴愚金龙 && modNPCName.Contains("Dragonfolly")) return true;
                if (entry.Name == CalamityLockItem.亵渎天神普罗维登斯 && modNPCName.Contains("Providence")) return true;
                if (entry.Name == CalamityLockItem.风暴编织者 && modNPCName.Contains("StormWeaver")) return true;
                if (entry.Name == CalamityLockItem.无尽虚空 && modNPCName.Contains("CeaselessVoid")) return true;
                if (entry.Name == CalamityLockItem.神之使徒西格纳斯 && modNPCName.Contains("Signus")) return true;
                if (entry.Name == CalamityLockItem.噬魂幽花 && modNPCName.Contains("Polterghast")) return true;
                if (entry.Name == CalamityLockItem.硫海遗爵 && modNPCName.Contains("OldDuke")) return true;
                if (entry.Name == CalamityLockItem.神明吞噬者 && modNPCName.Contains("DevourerofGods")) return true;
                if (entry.Name == CalamityLockItem.重生之龙犽戎 && modNPCName.Contains("Yharon")) return true;
                if (entry.Name == CalamityLockItem.星流巨械 && modNPCName.Contains("ExoMechs")) return true;
                if (entry.Name == CalamityLockItem.至尊女巫灾厄 && modNPCName.Contains("SupremeCalamitas")) return true;

            }




            return false;
        }

        private bool IsUnlocked(double unlockTimeSec)
        {
            try
            {
                DateTime unlockTime = DateTime.Parse(ProgressLockConfig.Instance.FirstTime).AddSeconds(unlockTimeSec);
                return DateTime.Now >= unlockTime;
            }
            catch
            {
                return true;
            }
        }
    }

    // 事件控制
    public class ProgressLockWorld : ModSystem
    {
        public override void PreUpdateWorld()
        {

            var config = ProgressLockConfig.Instance;
            if (config?.VanillaEventEntryList == null) return;

            foreach (var lockEntry in config.VanillaEventEntryList)
            {
                if (!IsUnlocked(lockEntry.UnlockTimeSec))
                {

                    if (lockEntry.Name == VanillaEventLockItem.哥布林军队 && Main.invasionType == 1)
                        Main.invasionType = 0;
                    else if (lockEntry.Name == VanillaEventLockItem.雪人军团 && Main.invasionType == 2)
                        Main.invasionType = 0;
                    else if (lockEntry.Name == VanillaEventLockItem.海盗入侵 && Main.invasionType == 3)
                        Main.invasionType = 0;
                    else if (lockEntry.Name == VanillaEventLockItem.火星暴乱 && Main.invasionType == 4)
                        Main.invasionType = 0;
                    else if (lockEntry.Name == VanillaEventLockItem.南瓜月)
                        Main.pumpkinMoon = false;
                    else if (lockEntry.Name == VanillaEventLockItem.撒旦军队 && DD2Event.Ongoing)
                        DD2Event.StopInvasion();
                    else if (lockEntry.Name == VanillaEventLockItem.霜月)
                        Main.snowMoon = false;
                    else if (lockEntry.Name == VanillaEventLockItem.血月)
                        Main.bloodMoon = false;
                    else if (lockEntry.Name == VanillaEventLockItem.日食)
                        Main.eclipse = false;
                    else if (lockEntry.Name == VanillaEventLockItem.月亮事件)
                    {
                        
                        if (Main.moonPhase == 0 && !Main.dayTime)
                        {
                            Main.moonPhase = 1; // 改变月相以阻止事件触发
                        }
                    }

                }

                
                
                if (config.ShowMention)
                {
                   // DateTime unlockTime = DateTime.Parse(ProgressLockConfig.Instance.FirstTime).AddSeconds(lockEntry.UnlockTimeSec);
                    //Main.NewText($"事件 [{lockEntry.Name}] 将在 {unlockTime:MM-dd HH:mm:ss} 解锁", 255, 100, 100);
                }
                
            }
        }

        private bool IsUnlocked(double unlockTimeSec)
        {
            try
            {
                DateTime unlockTime = DateTime.Parse(ProgressLockConfig.Instance.FirstTime).AddSeconds(unlockTimeSec);
                return DateTime.Now >= unlockTime;
            }
            catch
            {
                return true;
            }
        }
    }
}