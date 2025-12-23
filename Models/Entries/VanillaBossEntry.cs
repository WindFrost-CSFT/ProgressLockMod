using Newtonsoft.Json;
using ProgressLock.Enums;
using ProgressLock.Models.Interfaces;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace ProgressLock.Models.Entries
{
    public class VanillaBossEntry : IBossEntry
    {

        // public BossModType BossModType => BossModType.Vanilla;
        [CustomModConfigItem(typeof(ScrollableEnumElement))]
        public VanillaBoss Name { get; set; }

        [JsonIgnore]
        public Enum BossEnumName => Name;

        [Range(0, 7776000)]

        public long UnlockTimeSec { get; set; } = 50;


        public string UnlockTimeReadable
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(UnlockTimeSec);
                return $"{(int)ts.TotalDays}天 {ts.Hours}时 {ts.Minutes}分 {ts.Seconds}秒";
            }
        }

        [DefaultValue(false)]


        public bool IsManuallyLocked { get; set; } = false;


        //接口的Match 匹配Boss来判断是哪个BossEntry的Enum里的
        public bool Match(NPC npc)
        {
            if (!npc.active)
                return false;

            switch (Name)
            {
                case VanillaBoss.KingSlime:
                    return npc.type == NPCID.KingSlime;

                case VanillaBoss.EyeOfCthulhu:
                    return npc.type == NPCID.EyeofCthulhu;

                case VanillaBoss.EaterOfWorlds:
                    return npc.type == NPCID.EaterofWorldsHead
                        || npc.type == NPCID.EaterofWorldsBody
                        || npc.type == NPCID.EaterofWorldsTail;

                case VanillaBoss.BrainOfCthulhu:
                    return npc.type == NPCID.BrainofCthulhu;

                case VanillaBoss.Skeletron:
                    return npc.type == NPCID.SkeletronHead
                        || npc.type == NPCID.SkeletronHand;

                case VanillaBoss.QueenBee:
                    return npc.type == NPCID.QueenBee;

                case VanillaBoss.WallOfFlesh:
                    return npc.type == NPCID.WallofFlesh;

                case VanillaBoss.SkeletronPrime:
                    return npc.type == NPCID.SkeletronPrime
                        || npc.type == NPCID.PrimeCannon
                        || npc.type == NPCID.PrimeSaw
                        || npc.type == NPCID.PrimeVice
                        || npc.type == NPCID.PrimeLaser;

                case VanillaBoss.TheTwins:
                    return npc.type == NPCID.Retinazer
                        || npc.type == NPCID.Spazmatism;

                case VanillaBoss.TheDestroyer:
                    return npc.type == NPCID.TheDestroyer
                        || npc.type == NPCID.TheDestroyerBody
                        || npc.type == NPCID.TheDestroyerTail;

                case VanillaBoss.QueenSlime:
                    return npc.type == NPCID.QueenSlimeBoss;

                case VanillaBoss.Plantera:
                    return npc.type == NPCID.Plantera;

                case VanillaBoss.Golem:
                    return npc.type == NPCID.Golem;

                case VanillaBoss.DukeFishron:
                    return npc.type == NPCID.DukeFishron;

                case VanillaBoss.EmpressOfLight:
                    return npc.type == NPCID.HallowBoss;

                case VanillaBoss.LunaticCultist:
                    return npc.type == NPCID.CultistBoss;

                case VanillaBoss.MoonLord:
                    return npc.type == NPCID.MoonLordCore
                        || npc.type == NPCID.MoonLordHead
                        || npc.type == NPCID.MoonLordHand
                        || npc.type == NPCID.MoonLordFreeEye;
            }

            return false;
        }
        public override bool Equals(object obj)
        {
            if (obj is VanillaBossEntry other)
                return Name == other.Name && UnlockTimeSec == other.UnlockTimeSec;
            return false;
        }

        public override int GetHashCode()
        {
            return (Name, UnlockTimeSec).GetHashCode();
        }


    }
}
