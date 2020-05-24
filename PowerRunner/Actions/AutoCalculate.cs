using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data;
using PowerRunner.Settings;
using PowerRunner.Data;

namespace PowerRunner.Actions
{
	class AutoCalculate
	{
        public static void Register(ILogbook logbook)
        {
            logbook.Activities.CollectionChanged += new NotifyCollectionChangedEventHandler<IActivity>(Activities_CollectionChanged);
        }

        private static void Activities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs<IActivity> e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IActivity activity in e.NewItems)
                {
                    PRCategory pCat = LogbookSettings.Main.Categories.GetPRCategory(activity.Category);
                    if (pCat.Enable && pCat.AutoCalculate && activity.PowerWattsTrack == null)
                    {
                        PowerActivity.StorePowerTrack(activity, PowerActivity.GetRunningPowerTrack(activity, false));
                    }
                }
            }
        }
	}
}
