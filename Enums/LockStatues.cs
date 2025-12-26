using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressLock.Enums
{
    public enum LockStatus
    {
        Unlocked,           // 已解锁
        NotTimeYet,         // 时间未到
        IsManuallyLocked,     // 手动上锁
        IsManuallyUnlocked,   // 手动解锁
        Other      // 其他
    }
}
