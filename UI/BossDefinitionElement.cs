using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;     
using Terraria.ModLoader.Config.UI;   // 提供 NPCDefinitionElement

namespace ProgressLock.UI
{
    class NPCDefinitionFilterElement : NPCDefinitionElement
    {
        public override List<DefinitionOptionElement<NPCDefinition>> GetPassedOptionElements()
            => [.. (from elem in base.GetPassedOptionElements()
                    let npc = ContentSamples.NpcsByNetId[elem.Definition.Type]
                    where elem.Definition.Type == 0
                    || npc.boss
                    select elem)];
    }
}