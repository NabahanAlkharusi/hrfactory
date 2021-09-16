using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public enum SubscriptionPlan
    {
        WarmUp = 1,
        Startup = 2,
        ManualBuilder = 3,
        PlugIn = 4,
        Tech = 5,
        Doctor = 6
    }

    public enum SubscriptionPeriod
    {
        Month = 1,
        Year = 2,
        UpgradeToYear = 3
    }
    public enum Language
    {
        English = 1,
        Arabic = 2
    }
}
