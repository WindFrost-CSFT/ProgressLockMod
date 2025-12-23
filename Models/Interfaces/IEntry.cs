using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgressLock.Models.Interfaces
{
    public interface IEntry
    {
        
        Enum Name { get; }

        long UnlockTimeSec { get; set; }
        bool IsManuallyLocked { get; set; }
    }
}
