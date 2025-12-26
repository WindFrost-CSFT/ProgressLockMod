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
using Terraria.ModLoader.Config;
using tModPorter;
using static ProgressLock.ProgressLockWorld;

namespace ProgressLock
{
    public class Utils
    {

       

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

        public static string GetNpcProgressInfo(Player player, string pageStr)
        {
            
            string GetMsg(string key, params object[] args) => Language.GetTextValue($"Mods.ProgressLock.ProgressInfo.{key}", args);

            StringBuilder sb = new StringBuilder();
            var config = ProgressLockConfig.Config;

            
            sb.AppendLine(GetMsg("NpcHeader"));
            sb.AppendLine(GetMsg("FirstTime", config.FirstTime));

            if (config.NpcEntries == null || config.NpcEntries.Count == 0)
            {
                sb.AppendLine(GetMsg("AllUnlocked"));
                return sb.ToString();
            }

            
            if (!DateTime.TryParse(config.FirstTime, out DateTime baseTime))
            {
                return "Error: Invalid FirstTime format in config.";
            }

            
            int pageSize = 10;
            int totalCount = config.NpcEntries.Count;
            int maxPage = (int)Math.Ceiling((double)totalCount / pageSize);

            if (!int.TryParse(pageStr, out int page) || page < 1) page = 1;
            if (page > maxPage) page = maxPage;

            
            var currentPageEntries = config.NpcEntries
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            int entryIndex = (page - 1) * pageSize + 1;
            DateTime now = DateTime.Now;

           
            foreach (var entry in currentPageEntries)
            {
                // 过滤掉未加载的项
                if (entry.DefinitionList == null || entry.DefinitionList.Count == 0 || entry.DefinitionList[0].IsUnloaded)
                {
                    continue;
                }

                string npcName = Lang.GetNPCNameValue(entry.DefinitionList[0].Type);
                DateTime unlockDate = baseTime.AddSeconds(entry.UnlockTimeSec);

                
                if (entry.LockMode == LockMode.ManuallyLocked)
                {
                    sb.AppendLine($"[{entryIndex}] {npcName}: {GetMsg("ManuallyLocked")}");
                }
                else if (entry.LockMode == LockMode.ManuallyUnlocked || (entry.LockMode == LockMode.Automatic && now >= unlockDate))
                {
                    sb.AppendLine($"[{entryIndex}] {npcName}: {GetMsg("Unlocked")}");
                }
                else // 锁定状态（倒计时）
                {
                    string timeLeft = FormatTimeSpan(unlockDate - now);
                    string dateStr = unlockDate.ToString("MM-dd HH:mm:ss");
                    sb.AppendLine($"[{entryIndex}] {npcName}: {GetMsg("UnlockCountdown", timeLeft, dateStr)}");
                }

                entryIndex++;
            }

            // 6. 页脚
            sb.AppendLine($"======== {page} / {maxPage} ========");
            return sb.ToString();
        }
        public static string GetEventProgressInfo(Player player, string pageStr)
        {
            
            string GetMsg(string key, params object[] args) => Language.GetTextValue($"Mods.ProgressLock.ProgressInfo.{key}", args);

            StringBuilder sb = new StringBuilder();
            var config = ProgressLockConfig.Config;

            // 基础头部信息
            sb.AppendLine(GetMsg("EventHeader"));
            sb.AppendLine(GetMsg("FirstTime", config.FirstTime));

            if (config.EventEntries == null || config.EventEntries.Count == 0)
            {
                sb.AppendLine(GetMsg("AllUnlocked"));
                return sb.ToString();
            }

            
            if (!DateTime.TryParse(config.FirstTime, out DateTime baseTime))
            {
                return "Error: Invalid FirstTime format in config.";
            }

            // 计算分页逻辑
            int pageSize = 10;
            int totalCount = config.EventEntries.Count;
            int maxPage = (int)Math.Ceiling((double)totalCount / pageSize);

            if (!int.TryParse(pageStr, out int page) || page < 1) page = 1;
            if (page > maxPage) page = maxPage;

            
            var currentPageEntries = config.EventEntries
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            int entryIndex = (page - 1) * pageSize + 1;
            DateTime now = DateTime.Now;

            
            foreach (var entry in currentPageEntries)
            {
                
                string eventName = Language.GetTextValue($"Mods.ProgressLock.Configs.VanillaEvent.{entry.Name}.Label");
                DateTime unlockDate = baseTime.AddSeconds(entry.UnlockTimeSec);

                // 使用新的 LockMode 枚举进行判断
                if (entry.LockMode == LockMode.ManuallyLocked)
                {
                    sb.AppendLine($"[{entryIndex}] {eventName}: {GetMsg("ManuallyLocked")}");
                }
                else if (entry.LockMode == LockMode.ManuallyUnlocked || (entry.LockMode == LockMode.Automatic && now >= unlockDate))
                {
                    sb.AppendLine($"[{entryIndex}] {eventName}: {GetMsg("Unlocked")}");
                }
                else 
                {
                    string timeLeft = FormatTimeSpan(unlockDate - now);
                    string dateStr = unlockDate.ToString("MM-dd HH:mm:ss");
                    sb.AppendLine($"[{entryIndex}] {eventName}: {GetMsg("UnlockCountdown", timeLeft, dateStr)}");
                }

                entryIndex++;
            }

            
            sb.AppendLine($"======== {page} / {maxPage} ========");

            return sb.ToString();
        }



        /// <summary>
/// 切换指定 NPC 的锁定状态
/// </summary>
/// <param name="player">玩家对象</param>
/// <param name="name">NPC 名称或别名</param>
/// <param name="currentMode">输出参数：切换后的锁定模式。如果返回值为 false，此值无意义</param>
/// <returns>是否找到并成功切换。true=已找到并更新, false=未找到匹配的 NPC</returns>
        public static bool ToggleNpcLock(Player player, string name, out LockMode currentMode)
        {
            var entries = ProgressLockConfig.Config.NpcEntries;
            currentMode = LockMode.Automatic; // 默认值

            if (entries == null) return false;

            foreach (var entry in entries)
            {
                    
                bool matchAlias = entry.Alias.Any(a => a.Equals(name, StringComparison.OrdinalIgnoreCase));

                
                bool matchNPC = entry.DefinitionList.Any(def =>
                    !def.IsUnloaded && (
                    Lang.GetNPCNameValue(def.Type).Equals(name, StringComparison.OrdinalIgnoreCase) ||
                    def.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                );

                
                if (matchAlias || matchNPC)
                {
                    // 切换逻辑：Automatic -> ManuallyLocked -> ManuallyUnlocked -> 循环回到 Automatic
                    entry.LockMode = entry.LockMode switch
                    {
                        LockMode.Automatic => LockMode.ManuallyLocked,
                        LockMode.ManuallyLocked => LockMode.ManuallyUnlocked,
                        LockMode.ManuallyUnlocked => LockMode.Automatic,
                        _ => LockMode.Automatic
                    };

                    currentMode = entry.LockMode;
                    ProgressLockConfig.Config.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 切换指定事件的锁定状态
        /// </summary>
        /// <param name="player">玩家对象</param>
        /// <param name="name">事件名称、内部枚举名或别名</param>
        /// <param name="currentMode">输出参数：切换后的锁定模式</param>
        /// <returns>是否找到并成功切换</returns>
        public static bool ToggleEventLock(Player player, string name, out LockMode currentMode)
        {
            var entries = ProgressLockConfig.Config.EventEntries;
            currentMode = LockMode.Automatic; // 初始化

            if (entries == null) return false;

            foreach (var entry in entries)
            {
                //检查别名列表
                bool matchAlias = entry.Alias != null && entry.Alias.Any(a => a.Equals(name, StringComparison.OrdinalIgnoreCase));

                //检查枚举名 
                bool matchEnumName = entry.Name.ToString().Equals(name, StringComparison.OrdinalIgnoreCase);

                //检查本地化显示的标签
                string label = Language.GetTextValue($"Mods.ProgressLock.Configs.VanillaEvent.{entry.Name}.Label");
                bool matchLocalization = label.Equals(name, StringComparison.OrdinalIgnoreCase);

                // 只要匹配其中任意一项
                if (matchAlias || matchEnumName || matchLocalization)
                {
                    // 切换逻辑：Automatic -> ManuallyLocked -> ManuallyUnlocked -> 循环
                    entry.LockMode = entry.LockMode switch
                    {
                        LockMode.Automatic => LockMode.ManuallyLocked,
                        LockMode.ManuallyLocked => LockMode.ManuallyUnlocked,
                        LockMode.ManuallyUnlocked => LockMode.Automatic,
                        _ => LockMode.Automatic
                    };

                    currentMode = entry.LockMode;

                    // 保存修改后的配置
                    ProgressLockConfig.Config.SaveChanges();
                    return true;
                }
            }

            return false;
        }


        //PreAI()内调用，判断 NPC 是否解锁
        public static bool IsUnlocked(NPC npc, out LockStatus status)
        {
            var config = ProgressLockConfig.Config;
            // 提前解析时间，避免高频解析字符串带来的性能损耗
            if (!DateTime.TryParse(config.FirstTime, out DateTime baseTime))
            {
                status = LockStatus.Other;
                return true;
            }

            DateTime now = DateTime.Now;

            foreach (var entry in config.NpcEntries)
            {
                // 匹配 NPC 类型
                if (entry.DefinitionList != null && entry.DefinitionList.Any(def => def.Type == npc.type))
                {
                    // 1. 优先处理手动覆盖状态
                    if (entry.LockMode == LockMode.ManuallyLocked)
                    {
                        status = LockStatus.IsManuallyLocked;
                        return false;
                    }
                    if (entry.LockMode == LockMode.ManuallyUnlocked)
                    {
                        status = LockStatus.IsManuallyUnlocked;
                        return true;
                    }

                    // 2. 自动模式判断
                    DateTime unlockTime = baseTime.AddSeconds(entry.UnlockTimeSec);
                    if (now >= unlockTime)
                    {
                        status = LockStatus.Unlocked;
                        return true;
                    }
                    else
                    {
                        status = LockStatus.NotTimeYet;
                        return false;
                    }
                }
            }

            status = LockStatus.Unlocked;
            return true;
        }

        //PreUpdateWorld()内调用，判断 事件 是否解锁
        public static bool IsUnlocked(out LockStatus status, out EventEntry matchedEntry)
        {
            matchedEntry = null;
            var config = ProgressLockConfig.Config;

            if (!DateTime.TryParse(config.FirstTime, out DateTime baseTime))
            {
                status = LockStatus.Other;
                return true;
            }

            DateTime now = DateTime.Now;

            foreach (var entry in config.EventEntries)
            {
                if (entry.Match())
                {
                    matchedEntry = entry;

                    // 1. 优先处理手动覆盖状态
                    if (entry.LockMode == LockMode.ManuallyLocked)
                    {
                        status = LockStatus.IsManuallyLocked;
                        return false;
                    }
                    if (entry.LockMode == LockMode.ManuallyUnlocked)
                    {
                        status = LockStatus.IsManuallyUnlocked;
                        return true;
                    }

                    // 2. 自动模式判断
                    DateTime unlockTime = baseTime.AddSeconds(entry.UnlockTimeSec);
                    if (now >= unlockTime)
                    {
                        status = LockStatus.Unlocked;
                        return true;
                    }
                    else
                    {
                        status = LockStatus.NotTimeYet;
                        return false;
                    }
                }
            }

            status = LockStatus.Unlocked;
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

        public static string GetMentionMsg(string key,  params object[] args)
        {
            return Language.GetTextValue($"Mods.ProgressLock.Mention.{key}", args);
        }


    }
}
