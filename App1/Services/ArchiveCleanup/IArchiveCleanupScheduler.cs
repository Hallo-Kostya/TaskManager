using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Services.ArchiveCleanup
{
    public interface IArchiveCleanupScheduler
    {
        void ScheduleArchiveCleanup(int intervalInHours);
        void CancelArchiveCleanup();
    }

}
