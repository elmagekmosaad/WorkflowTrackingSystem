using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Domain.Enums
{
    public enum ProcessStatus
    {
        Active = 1,
        Completed = 2,
        Pending = 3,
        Rejected = 4
    }
}
