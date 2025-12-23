using Microsoft.Xna.Framework;
using ReLogic.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ProgressLock.ProgressLockWorld;
using static ProgressLock.Utils;

namespace ProgressLock.Commands
{
    internal class Commands : ModCommand
    {

        public override CommandType Type => CommandType.Chat;
        public override string Command => "progress";

        public override string Description => "Show progress lock info";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply("请提供参数！", Color.Red);
                return;
            }
            switch (args[0].ToLower())
            {
                case "原版":
                case "vaniila":
                case "1":
                    {
                        caller.Reply(Utils.GetProgressInfo(caller.Player, args[1])); 
                    }
                    break;
                case "灾厄":
                case   "calamity":
                case "2":
                    {
                        caller.Reply(Utils.GetProgressInfo(caller.Player, args[1]));
                    }
                    break;
                case "事件":
                case "入侵":
                case "event":
                case "invasion":
                case "3":
                    {
                        caller.Reply(Utils.GetProgressInfo(caller.Player, args[1]));
                    }
                    break;
                case "switch":
                case "切换":
                case "toggle":
                    {
                        if (args.Length < 2)
                        {
                            caller.Reply("请提供要切换限制锁的名称！", Color.Red);
                            return;
                        }
                        bool? lockStatus;
                        bool flag = ToggleLock(caller.Player, args[1],out lockStatus);
                        if (flag)
                        {
                            if ((bool)lockStatus)
                            {
                                caller.Reply($"已解锁 {args[1]} 的限制锁！", Color.Green);

                            }
                            else
                            {
                                caller.Reply($"已将 {args[1]} 的限制锁上锁！", Color.Green);
                            }
                        }
                        else
                        {
                            caller.Reply($"未找到名称为 {args[1]} 的限制锁！", Color.Red);
                        }
                        break;
                    }
            }
            
        }

        
    }
    
}
