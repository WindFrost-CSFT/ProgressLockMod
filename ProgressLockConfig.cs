using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ProgressLock.Enums;
using ProgressLock.Models.Entries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using tModPorter;

namespace ProgressLock
{
    public class ProgressLockConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static ProgressLockConfig Config;


        [DefaultValue("2025-12-19 10:30:10")]
        public string FirstTime { get; set; } = "2025-12-19 10:30:10";

        public List<NpcEntry> NpcEntries { get; set; }

        public List<EventEntry> EventEntries { get; set; }

        


       

        // 构造函数设置默认值
        public ProgressLockConfig()
        {
            NpcEntries = new List<NpcEntry>
            {
                new NpcEntry { DefinitionList = new List<NPCDefinition>{ new NPCDefinition("Terraria", "KingSlime")}, UnlockTimeSec = 50000 },
                new NpcEntry { DefinitionList = new List<NPCDefinition>{new ("Terraria", "EaterofWorldsHead"), new ("Terraria", "EaterofWorldsBody") , new ("Terraria", "EaterofWorldsTail") }, UnlockTimeSec = 100000 },
                new NpcEntry { DefinitionList = new List<NPCDefinition>{new ("CalamityMod","DesertScourge")} , UnlockTimeSec = 70000 },
                new NpcEntry { DefinitionList = new List<NPCDefinition>{new ("CalamityMod","TheSlimeGod")} , UnlockTimeSec = 120000 },
            };
            EventEntries = new List<EventEntry>
            {
                new EventEntry { Name = VanillaEvent.FrostLegion, UnlockTimeSec = 80000,Alias = new List<string>{"军团"}  },
                new EventEntry { Name = VanillaEvent.MartianMadness, UnlockTimeSec = 150000 },
                new EventEntry { Name = VanillaEvent.PumpkinMoon, UnlockTimeSec = 200000 },
            };


        }

        public override void OnLoaded()
        {
            Config = this;
        }

        public override void OnChanged()
        {
            if (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Utils.GetMentionMsg("ConfigChanged")),Color.Aqua);
            }
          
        }

        
       
    }

    

    

   

   
    

    

    
}