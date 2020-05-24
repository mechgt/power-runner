using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using PowerRunner.Settings;

namespace PowerRunner.Data
{
    public static class Shared
    {
        public static bool IsRunningCategory(IActivityCategory category)
        {
            return LogbookSettings.Main.Categories.GetPRCategory(category).Enable;
        }
    }
}
