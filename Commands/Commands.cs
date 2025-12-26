using Microsoft.Xna.Framework;
using ProgressLock.Enums;
using ReLogic.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static ProgressLock.ProgressLockWorld;
using static ProgressLock.Utils;

namespace ProgressLock.Commands
{
    internal class Commands : ModCommand
    {

        public override CommandType Type => CommandType.Chat;
        public override string Command => "progress";

        public override string Description => Language.ActiveCulture.Name == "zh-Hans"
                                                                            ? "查看或管理服务器进度限制"
                                                                            : "View or manage server-side progress lock";

        public override string Usage => Language.GetTextValue($"Mods.ProgressLock.Mention.Usage");
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply(GetMentionMsg("NoArgs"), Color.Red);
                return;
            }

            // 处理分页逻辑：如果第二个参数存在则作为页码，否则默认为 "1"
            string pageStr = args.Length > 1 ? args[1] : "1";

            switch (args[0].ToLower())
            {
                case "boss":
                case "b":
                case "1":
                case "npc":
                    {
                        // 注意：这里传入 pageStr 而不是 args[0]
                        caller.Reply(GetNpcProgressInfo(caller.Player, pageStr));
                    }
                    break;

                case "事件":
                case "入侵":
                case "event":
                case "vanillaevent":
                case "invasion":
                case "2":
                case "e":
                    {
                        caller.Reply(Utils.GetEventProgressInfo(caller.Player, pageStr));
                    }
                    break;

                case "switch":
                case "切换":
                case "toggle":
                case "t":
                    {
                        if (args.Length < 2)
                        {
                            caller.Reply(GetMentionMsg("NoLockType"), Color.Red);
                            return;
                        }

                        if (args.Length < 3)
                        {
                            caller.Reply(GetMentionMsg("NoTargetName"), Color.Red); // 需新增：请指定名称
                            return;
                        }

                        string subType = args[1].ToLower();
                        string targetName = args[2];

                        if (subType == "npc" || subType == "boss" || subType == "b" || subType == "n" || subType == "1")
                        {
                            if (ToggleNpcLock(caller.Player, targetName, out LockMode newMode))
                            {
                                SendModeReply(caller, targetName, newMode);
                            }
                            else
                            {
                                caller.Reply(GetMentionMsg("NoneFound", targetName), Color.Red);
                            }
                        }
                        else if (subType == "event" || subType == "e" || subType == "2" || subType == "invasion" || subType == "i")
                        {
                            if (ToggleEventLock(caller.Player, targetName, out LockMode newMode))
                            {
                                SendModeReply(caller, targetName, newMode);
                            }
                            else
                            {
                                caller.Reply(GetMentionMsg("NoneFound", targetName), Color.Red);
                            }
                        }
                        else
                        {
                            caller.Reply(GetMentionMsg("NoLockType"), Color.Red);
                        }
                    }
                    break;
            }
        }

        // 辅助方法：根据切换后的模式发送对应的本地化回复
        private void SendModeReply(CommandCaller caller, string name, LockMode mode)
        {
            switch (mode)
            {
                case LockMode.ManuallyLocked:
                    caller.Reply(GetMentionMsg("ModeManuallyLocked", name), Color.Orange);
                    break;
                case LockMode.ManuallyUnlocked:
                    caller.Reply(GetMentionMsg("ModeManuallyUnlocked", name), Color.Green);
                    break;
                case LockMode.Automatic:
                    caller.Reply(GetMentionMsg("ModeAutomatic", name), Color.Cyan);
                    break;
            }
        }


    }
    
}
