using Microsoft.Xna.Framework.Input;
using ProgressLock.Enums;
using ProgressLock.Models.Entries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tModPorter;
using static ProgressLock.ProgressLockWorld;

namespace ProgressLock
{
    public class Utils
    {

        //    public class LockDict
        //    {
        //        // 原版 Boss
        //        public OrderedDictionary vanillaBossDict = new OrderedDictionary()
        //{
        //    { VanillaBossLockItem.史莱姆王, false },
        //    { VanillaBossLockItem.克苏鲁之眼, false },
        //    { VanillaBossLockItem.克苏鲁之脑, false },
        //    { VanillaBossLockItem.世界吞噬者, false },
        //    { VanillaBossLockItem.骷髅王, false },
        //    { VanillaBossLockItem.蜂王, false },
        //    { VanillaBossLockItem.血肉墙, false },
        //    { VanillaBossLockItem.机械骷髅王, false },
        //    { VanillaBossLockItem.双子魔眼, false },
        //    { VanillaBossLockItem.毁灭者, false },
        //    { VanillaBossLockItem.史莱姆皇后, false },
        //    { VanillaBossLockItem.世纪之花, false },
        //    { VanillaBossLockItem.石巨人, false },
        //    { VanillaBossLockItem.猪鲨公爵, false },
        //    { VanillaBossLockItem.光之女皇, false },
        //    { VanillaBossLockItem.拜月教邪教徒, false },
        //    { VanillaBossLockItem.月亮领主, false },
        //};

        //        // 灾厄 Boss
        //        public OrderedDictionary calamityBossDict = new OrderedDictionary()
        //{
        //    { CalamityBossLockItem.荒漠灾虫, false },
        //    { CalamityBossLockItem.菌生蟹, false },
        //    { CalamityBossLockItem.腐巢意志, false },
        //    { CalamityBossLockItem.血肉宿主, false },
        //    { CalamityBossLockItem.史莱姆之神, false },
        //    { CalamityBossLockItem.渊海灾虫, false },
        //    { CalamityBossLockItem.硫磺火元素, false },
        //    { CalamityBossLockItem.极地之灵, false },
        //    { CalamityBossLockItem.灾厄之影, false },
        //    { CalamityBossLockItem.利维坦和阿娜希塔, false },
        //    { CalamityBossLockItem.白金星舰, false },
        //    { CalamityBossLockItem.瘟疫使者歌莉娅, false },
        //    { CalamityBossLockItem.毁灭魔像, false },
        //    { CalamityBossLockItem.星神游龙, false },
        //    { CalamityBossLockItem.亵渎守卫, false },
        //    { CalamityBossLockItem.痴愚金龙, false },
        //    { CalamityBossLockItem.亵渎天神普罗维登斯, false },
        //    { CalamityBossLockItem.风暴编织者, false },
        //    { CalamityBossLockItem.无尽虚空, false },
        //    { CalamityBossLockItem.神之使徒西格纳斯, false },
        //    { CalamityBossLockItem.噬魂幽花, false },
        //    { CalamityBossLockItem.硫海遗爵, false },
        //    { CalamityBossLockItem.神明吞噬者, false },
        //    { CalamityBossLockItem.重生之龙犽戎, false },
        //    { CalamityBossLockItem.星流巨械, false },
        //    { CalamityBossLockItem.至尊女巫灾厄, false },
        //};

        //        // 原版事件
        //        public OrderedDictionary vanillaEventDict = new OrderedDictionary()
        //{
        //    { VanillaEventLockItem.哥布林军队, false },
        //    { VanillaEventLockItem.血月, false },
        //    { VanillaEventLockItem.撒旦军队, false },
        //    { VanillaEventLockItem.海盗入侵, false },
        //    { VanillaEventLockItem.南瓜月, false },
        //    { VanillaEventLockItem.雪人军团, false },
        //    { VanillaEventLockItem.火星暴乱, false },
        //    { VanillaEventLockItem.月亮事件, false },
        //    { VanillaEventLockItem.霜月, false },
        //    { VanillaEventLockItem.日食, false },
        //};
        //    }

        //public static LockDict GetLockDict()
        //{
        //    LockDict dict = new LockDict();
        //    var instance = ProgressLockConfig.Instance;

        //    foreach (var lockEntry in instance.VanillaBossEntryList)
        //    {

        //        if(!(bool)dict.vanillaBossDict[lockEntry.Name] && DateTime.Now >= DateTime.Parse(instance.FirstTime).AddSeconds(lockEntry.UnlockTimeSec))
        //        {
        //            dict.vanillaBossDict[lockEntry.Name] = false;
        //        }
        //        else if (!(bool)dict.vanillaBossDict[lockEntry.Name] && DateTime.Now <= DateTime.Parse(instance.FirstTime).AddSeconds(lockEntry.UnlockTimeSec))
        //        {
        //            dict.vanillaBossDict[lockEntry.Name] = false;

        //        }
        //        else
        //        {
        //            dict.vanillaBossDict[lockEntry.Name] = true;
        //        }


        //    }
        //    foreach (var lockEntry in instance.CalamityBossEntryList)
        //    {

        //        if (DateTime.Now >= DateTime.Parse(instance.FirstTime).AddSeconds(lockEntry.UnlockTimeSec))
        //        {
        //            dict.calamityBossDict[lockEntry.Name] = true;

        //        }


        //    }
        //    foreach (var lockEntry in instance.VanillaEventEntryList)
        //    {

        //        if (DateTime.Now >= DateTime.Parse(instance.FirstTime).AddSeconds(lockEntry.UnlockTimeSec))
        //        {
        //            dict.vanillaEventDict[lockEntry.Name] = true;

        //        }


        //    }

        //    return dict;
        //}

        public static string FormatTimeSpan(TimeSpan ts)
        {
            // 判断游戏客户端是否为中文
            bool isChinese = Language.ActiveCulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase);

            if (ts <= TimeSpan.Zero)
                return isChinese ? "0秒" : "0s";

            var sb = new StringBuilder();

            if (ts.Days > 0)
                sb.Append(isChinese ? $"{ts.Days}天" : $"{ts.Days}d");

            if (ts.Hours > 0)
                sb.Append(isChinese ? $" {ts.Hours}小时" : $" {ts.Hours}h");

            if (ts.Minutes > 0)
                sb.Append(isChinese ? $" {ts.Minutes}分钟" : $" {ts.Minutes}m");

            if (ts.Seconds > 0)
                sb.Append(isChinese ? $" {ts.Seconds}秒" : $" {ts.Seconds}s");

            return sb.ToString().Trim();
        }
        // 修复 CS0472: long 类型的值永不等于 null
        public static string GetBossProgressInfo(Player player , string pageStr)
        {
            string GetMsg(string key, params object[] args) => Language.GetTextValue($"Mods.ProgressLock.ProgressInfo.{key}", args);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GetMsg("BossHeader"));
            sb.AppendLine(GetMsg("FirstStartTime", ProgressLockConfig.Config.FirstTime.ToString()));
            var config = ProgressLockConfig.Config;

            if (config.BossEntries == null)
            {
                sb.AppendLine(GetMsg("AllUnlocked"));
                return sb.ToString();
            }
            
            int size = 10;
            int page;
            int maxPage = config.BossEntries.Count / size + (config.BossEntries.Count % size == 0 ? 0 : 1);
            int listIndex = 0;
            int entryIndex = 1;
            try
            {
                page = int.Parse(pageStr);
            }
            catch
            {
                page = 1;
            }

            if(page < 1 || page > maxPage)
                page = 1;

            listIndex = (page - 1) * size;
            entryIndex = listIndex + 1;


            foreach(var entry in config.BossEntries)
            {
                DateTime unlockDate = DateTime.Parse(config.FirstTime).AddSeconds(entry.UnlockTimeSec);
                if (entry.IsManuallyLocked)
                {
                    sb.AppendLine($"[{entryIndex}] {Lang.GetNPCNameValue(entry.DefinitionList[0].Type)}: {GetMsg("ManuallyLocked")}");
                    entryIndex++;
                    continue;
                }
                if (DateTime.Now >= unlockDate && !entry.IsManuallyLocked)
                {
                    //要本地化
                    sb.AppendLine($"[{entryIndex}] {Lang.GetNPCNameValue(entry.DefinitionList[0].Type)}: {GetMsg("Unlocked")}");
                    entryIndex++;
                    continue;
                }
                else
                {
                    sb.AppendLine($"[{entryIndex}] {Lang.GetNPCNameValue(entry.DefinitionList[0].Type)}: " + $"{GetMsg("UnlockCountdown", FormatTimeSpan((unlockDate - DateTime.Now)), unlockDate.ToString("MM dd HH:mm:ss"))}");
                    entryIndex++;
                    continue;
                }

                
            }

            sb.AppendLine($"===== {page} / {maxPage} =====");

            return sb.ToString();

            // Main.NewText(config.VanillaBossEntryList.Count);
            /*
             switch (modType.ToLower())
             {
                 case "vanilla":
                 case "原版":
                     {
                         //这里要本地化

                         sb.AppendLine(GetMsg("VanillaHeader"));
                         sb.AppendLine(GetMsg("FirstStartTime", config.FirstTime.ToString()));
                         if (config.VanillaBossEntryList == null)
                         {
                             sb.AppendLine($"{GetMsg("AllUnlocked")}");
                             return sb.ToString();
                         }
                         int index = 1;


                         foreach (var entry in config.VanillaBossEntryList)
                         {
                             string entryNameKey = $"Mods.ProgressLock.Configs.{entry.GetType().Name.Replace("Entry", "")}.{entry.Name}.Label";
                             string translatedName = Language.GetTextValue(entryNameKey);
                             DateTime unlockDate = DateTime.Parse(config.FirstTime).AddSeconds(entry.UnlockTimeSec);
                             if (DateTime.Now >= unlockDate && !entry.IsManuallyLocked)
                             {
                                 //要本地化
                                 sb.AppendLine($"[{index}] {translatedName}: {GetMsg("Unlocked")}");
                                 index++;
                             }
                             else if (entry.IsManuallyLocked)
                             {
                                 sb.AppendLine($"[{index}] {translatedName}: {GetMsg("ManuallyLocked")}");
                                 index++;
                             }
                             else
                             {
                                 sb.AppendLine($"[{index}] {translatedName}: " + $"{GetMsg("UnlockCountdown", FormatTimeSpan((unlockDate - DateTime.Now)) , unlockDate.ToString("MMM dd HH:mm:ss"))}");
                                 index++;
                             }
                         }
                         return sb.ToString();
                     }

                 case "vanillaevent":
                 case "原版事件":
                     {
                         sb.AppendLine(GetMsg("VanillaEventHeader"));
                         sb.AppendLine(GetMsg("FirstStartTime", config.FirstTime.ToString()));

                         // 注意：这里原代码检查的是 VanillaBossEntryList，建议改为检查对应的列表
                         if (config.VanillaEventEntryList == null)
                         {
                             sb.AppendLine(GetMsg("AllUnlocked"));
                             return sb.ToString();
                         }

                         int index = 1;
                         foreach (var entry in config.VanillaEventEntryList)
                         {
                             // 自动匹配 Mods.ProgressLock.Configs.VanillaEvent.名称.Label
                             string entryNameKey = $"Mods.ProgressLock.Configs.{entry.GetType().Name.Replace("Entry", "")}.{entry.Name}.Label";
                             string translatedName = Language.GetTextValue(entryNameKey);

                             DateTime unlockDate = DateTime.Parse(config.FirstTime).AddSeconds(entry.UnlockTimeSec);
                             if (DateTime.Now >= unlockDate && !entry.IsManuallyLocked)
                             {
                                 sb.AppendLine($"[{index}] {translatedName}: {GetMsg("Unlocked")}");
                             }
                             else if (entry.IsManuallyLocked)
                             {
                                 sb.AppendLine($"[{index}] {translatedName}: {GetMsg("ManuallyLocked")}");
                             }
                             else
                             {
                                 // 使用统一的倒计时本地化格式
                                 string countdown = GetMsg("UnlockCountdown", FormatTimeSpan(unlockDate - DateTime.Now), unlockDate.ToString("MMM dd HH:mm:ss"));
                                 sb.AppendLine($"[{index}] {translatedName}: {countdown}");
                             }
                             index++;
                         }
                         return sb.ToString();
                     }

                 case "calamity":
                 case "灾厄":
                     {
                         sb.AppendLine(GetMsg("CalamityHeader"));
                         sb.AppendLine(GetMsg("FirstStartTime", config.FirstTime.ToString()));

                         if (config.CalamityBossEntryList == null)
                         {
                             sb.AppendLine(GetMsg("AllUnlocked"));
                             return sb.ToString();
                         }

                         int index = 1;
                         foreach (var entry in config.CalamityBossEntryList)
                         {
                             // 自动匹配 Mods.ProgressLock.Configs.CalamityBoss.名称.Label
                             string entryNameKey = $"Mods.ProgressLock.Configs.{entry.GetType().Name.Replace("Entry", "")}.{entry.Name}.Label";
                             string translatedName = Language.GetTextValue(entryNameKey);

                             DateTime unlockDate = DateTime.Parse(config.FirstTime).AddSeconds(entry.UnlockTimeSec);
                             if (DateTime.Now >= unlockDate && !entry.IsManuallyLocked)
                             {
                                 sb.AppendLine($"[{index}] {translatedName}: {GetMsg("Unlocked")}");
                             }
                             else if (entry.IsManuallyLocked)
                             {
                                 sb.AppendLine($"[{index}] {translatedName}: {GetMsg("ManuallyLocked")}");
                             }
                             else
                             {
                                 string countdown = GetMsg("UnlockCountdown", FormatTimeSpan(unlockDate - DateTime.Now), unlockDate.ToString("MMM dd HH:mm:ss"));
                                 sb.AppendLine($"[{index}] {translatedName}: {countdown}");
                             }
                             index++;
                         }
                         return sb.ToString();
                     }


             } */
           
        }

        public static string GetEventProgressInfo(Player player, string pageStr)
        {
            string GetMsg(string key, params object[] args) => Language.GetTextValue($"Mods.ProgressLock.ProgressInfo.{key}", args);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GetMsg("EventHeader"));
            sb.AppendLine(GetMsg("FirstStartTime", ProgressLockConfig.Config.FirstTime.ToString()));
            var config = ProgressLockConfig.Config;

            if (config.EventEntries == null)
            {
                sb.AppendLine(GetMsg("AllUnlocked"));
                return sb.ToString();
            }

            int size = 10;
            int page;
            int maxPage = config.EventEntries.Count / size + (config.EventEntries.Count % size == 0 ? 0 : 1);
            int listIndex = 0;
            int entryIndex = 1;
            try
            {
                page = int.Parse(pageStr);
            }
            catch
            {
                page = 1;
            }

            if (page < 1 || page > maxPage)
                page = 1;

            listIndex = (page - 1) * size;
            entryIndex = listIndex + 1;


            foreach (var entry in config.EventEntries)
            {
                DateTime unlockDate = DateTime.Parse(config.FirstTime).AddSeconds(entry.UnlockTimeSec);
                if (entry.IsManuallyLocked)
                {
                    sb.AppendLine($"[{entryIndex}] {entry.Name.ToString()}: {GetMsg("ManuallyLocked")}");
                    entryIndex++;
                    continue;
                }
                if (DateTime.Now >= unlockDate && !entry.IsManuallyLocked)
                {
                    //要本地化
                    sb.AppendLine($"[{entryIndex}] {entry.Name.ToString()}: {GetMsg("Unlocked")}");
                    entryIndex++;
                    continue;
                }
                else
                {
                    sb.AppendLine($"[{entryIndex}] {entry.Name.ToString()}: " + $"{GetMsg("UnlockCountdown", FormatTimeSpan((unlockDate - DateTime.Now)), unlockDate.ToString("MM dd HH:mm:ss"))}");
                    entryIndex++;
                    continue;
                }


            }

            sb.AppendLine($"===== {page} / {maxPage} =====");

            return sb.ToString();

            

        }
        public static int? GetBossIdByName(string searchName)
        {
            for (int i = 0; i < NPCLoader.NPCCount; i++)
            {
                // 获取该 NPC 的本地化名称（随语言改变）
                string npcName = Lang.GetNPCNameValue(i);

                // 忽略大小写
                if (npcName.Equals(searchName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return null;
        }

        /*
        public static string GetProgressInfoCalamity(Player player)
        {
            var dict = Utils.GetLockDict();

            StringBuilder sb = new StringBuilder();
            var instance = ProgressLockConfig.Config;
            sb.AppendLine("=== 灾厄进度列表 ===");
            sb.AppendLine($"开服时间: {instance.FirstTime}");
            if (instance.CalamityBossEntryList == null || instance.CalamityBossEntryList.Count == 0)
            {
                sb.AppendLine("已全部解锁!");
                return sb.ToString();
            }
            int index = 1;
            foreach (DictionaryEntry entry in dict.calamityBossDict)
            {
                var key = (CalamityBoss)entry.Key;
                bool value = (bool)entry.Value;
                if (instance.CalamityBossEntryList.Find(x => x.BossEnumName == key) != null)
                {
                    long unlockTimeSec = instance.CalamityBossEntryList.Find(x => x.BossEnumName == key).UnlockTimeSec;
                    DateTime unlockDate = DateTime.Parse(instance.FirstTime).AddSeconds(unlockTimeSec);
                    sb.AppendLine($"[{index}] {key}: {(value ? "已解锁" : $"- 解锁时间: {Utils.FormatTimeSpan((unlockDate - DateTime.Now))} 后解锁({unlockDate.ToString("MMM dd日 HH时 mm分 ss秒")})")} ");
                    index++;
                }
                else
                {
                    sb.AppendLine($"[{index}] {key}: 已解锁");
                    index++;
                }
            }

            return sb.ToString();
        }

        public static string GetProgressInfoEvent(Player player)
        {
            var dict = Utils.GetLockDict();

            StringBuilder sb = new StringBuilder();
            var instance = ProgressLockConfig.Config;
            sb.AppendLine("=== 原版事件进度列表 ===");
            sb.AppendLine($"开服时间: {instance.FirstTime}");
            if (instance.VanillaEventEntryList == null || instance.VanillaEventEntryList.Count == 0)
            {
                sb.AppendLine("已全部解锁!");
                return sb.ToString();
            }
            int index = 1;
            foreach (DictionaryEntry entry in dict.vanillaEventDict)
            {
                var key = (VanillaEvent)entry.Key;
                bool value = (bool)entry.Value;
                if (instance.VanillaEventEntryList.Find(x => x.EventEnumName == key) != null)
                {
                    long unlockTimeSec = instance.VanillaEventEntryList.Find(x => x.EventEnumName == key).UnlockTimeSec;
                    DateTime unlockDate = DateTime.Parse(instance.FirstTime).AddSeconds(unlockTimeSec);
                    sb.AppendLine($"[{index}] {key}: {(value ? "已解锁" : $"- 解锁时间: {Utils.FormatTimeSpan((unlockDate - DateTime.Now))} 后解锁({unlockDate.ToString("MMM dd日 HH时 mm分 ss秒")})")} ");
                    index++;
                }
                else
                {
                    sb.AppendLine($"[{index}] {key}: 已解锁");
                    index++;
                }
            }

            return sb.ToString();
        }
        */


        /// <summary>
        /// 切换指定 Boss 的锁定状态
        /// </summary>
        /// <param name="player">玩家对象</param>
        /// <param name="entryName">Boss 名称</param>
        /// <param name="currentManualLock">输出参数：切换后的手动锁定状态。true=已解锁, false=已锁定, null=未找到</param>
        /// <returns>是否找到并成功切换。true=找到, false=未找到</returns>
        /// 
        public static bool ToggleBossLock(Player player, string name, out bool? currentManualLock)
        {
           // Main.NewText(1);
            List<BossEntry> entries = ProgressLockConfig.Config.GetAllBossEntries().ToList();
            //  Main.NewText(entries.Count);
            foreach (var entry in entries)
            {
                // 1. 检查别名列表 (Alias) 是否包含玩家输入的名字
                // StringComparison.OrdinalIgnoreCase 可以忽略大小写（比如输入 eoc 匹配 EoC）
                bool matchAlias = entry.Alias.Any(a => a.Equals(name, StringComparison.OrdinalIgnoreCase));

                // 2. 检查定义列表 (DefinitionList) 里的 NPC 名字
                bool matchNPC = false;
                foreach (var def in entry.DefinitionList)
                {
                    string bossName = Lang.GetNPCNameValue(def.Type);
                    // 如果本地化名字匹配，或者内部 ID 名匹配
                    if (bossName == name || def.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        matchNPC = true;
                        break;
                    }
                }

                // 3. 只要 别名 或 NPC名 有一个对上了，就执行逻辑
                if (matchAlias || matchNPC)
                {
                    entry.IsManuallyLocked = !entry.IsManuallyLocked;
                    currentManualLock = entry.IsManuallyLocked; // 注意：这里通常应该是等于最新状态
                    return true;
                }
            }
            currentManualLock = null;
            return false;
        }

        /*
        /// <summary>
        /// 在字典中根据名称查找并切换锁定状态
        /// </summary>
        /// <param name="dictionary">要搜索的字典</param>
        /// <param name="bossName">Boss 名称</param>
        /// <param name="lockStatus">输出参数：切换后的锁定状态。true=已解锁, false=已锁定, null=未找到</param>
        /// <returns>是否找到并成功切换。true=找到, false=未找到</returns>
        public static bool ToggleByName(System.Collections.IDictionary dictionary, string bossName, out bool? lockStatus)
        {
            // 先检查是否真的存在匹配项
            var key = dictionary.Keys
                                .Cast<object>()
                                .FirstOrDefault(x => x != null && x.ToString() == bossName);

            // 再次验证：key 不为 null 且确实匹配
            if (key != null && dictionary.Contains(key) && dictionary[key] is bool value)
            {
                Main.NewText($"找到了! Key={key}, 修改前={value}");
                dictionary[key] = !value;
                Main.NewText($"修改后={dictionary[key]}");
                lockStatus = !value;
                return true;
            }

            lockStatus = null;
            return false;
        }
        */


        //PreAI()内调用，判断 NPC 是否解锁
        public static bool IsUnlocked(NPC npc, out LockStatus status)
        {
            status = LockStatus.Unlocked;

            foreach (var entry in ProgressLock.allBossEntries)
            {
                foreach(var def in entry.DefinitionList){
                    if (ContentSamples.NpcsByNetId[def.Type] == npc)
                    {

                        // 根据时间和手动锁定判断状态
                        DateTime unlockTime = DateTime.Parse(ProgressLockConfig.Config.FirstTime)
                                                .AddSeconds(entry.UnlockTimeSec);

                        if (DateTime.Now >= unlockTime && !entry.IsManuallyLocked)
                        {
                            status = LockStatus.Unlocked;
                            return true;
                        }
                        else if (DateTime.Now >= unlockTime && entry.IsManuallyLocked)
                        {
                            status = LockStatus.IsManuallyLocked;
                            return false;
                        }
                        else if (DateTime.Now < unlockTime && !entry.IsManuallyLocked)
                        {
                            status = LockStatus.NotTimeYet;
                            return false;
                        }
                    }
                }
            }

            // 没匹配到任何 entry
            status = LockStatus.Unlocked;
            return true;
        }

        //PreUpdateWorld()内调用，判断 事件 是否解锁
        public static bool IsUnlocked(out LockStatus status, out EventEntry matchedEntry)
        {
            status = LockStatus.Unlocked;
            matchedEntry = null;

            foreach (var entry in ProgressLock.allEventEntries)
            {
                if (entry.Match())
                {
                    matchedEntry = entry;

                    // 根据时间和手动锁定判断状态
                    DateTime unlockTime = DateTime.Parse(ProgressLockConfig.Config.FirstTime).AddSeconds(entry.UnlockTimeSec);

                    if (DateTime.Now >= unlockTime && !entry.IsManuallyLocked)
                    {
                        status = LockStatus.Unlocked;
                        return true;
                    }
                    else if (DateTime.Now >= unlockTime && entry.IsManuallyLocked)
                    {
                        status = LockStatus.IsManuallyLocked;
                        return false;
                    }
                    else if (DateTime.Now < unlockTime && !entry.IsManuallyLocked)
                    {
                        status = LockStatus.NotTimeYet;
                        return false;
                    }
                }
            }

            // 没匹配到任何 entry
            status = LockStatus.Unlocked;
            matchedEntry = null;
            return true;
        }

        public static void StopNPC(NPC npc)
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



    }
}
