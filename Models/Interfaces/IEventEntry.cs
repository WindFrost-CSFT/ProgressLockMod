using ProgressLock.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ProgressLock.Models.Interfaces
{
    public interface IEventEntry 
    {

       // EventModType EventModType { get; }
        Enum EventEnumName { get; }

        long UnlockTimeSec { get; set; }

        bool IsManuallyLocked { get; set; }
        bool Match();
        void Stop();
    }
}
