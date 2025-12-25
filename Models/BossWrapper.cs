using ProgressLock.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace ProgressLock.Models
{
    public class BossWrapper
    {
        // 标签打在这里，而不是打在 List 上
        [CustomModConfigItem(typeof(NPCDefinitionFilterElement))]
        public NPCDefinition Definition;

        
        public BossWrapper(string mod, string name)
        {
            Definition = new NPCDefinition(mod, name);
        }
    }


    }
