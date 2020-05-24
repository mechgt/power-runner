using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PowerRunner.Settings;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Util;
using PowerRunner.Data;

namespace PowerRunner.Actions
{
    // TODO: Design Criteria
    /*
     *  Calculate power tracks easily for previous activities
     *  Recalculate partial activities easily (automatically?) after upgrade
     */

    internal class CreatePowerTrack : IAction
    {
        #region Fields

        private const string newLine = "\r\n";

        #endregion

        #region Constructors

        public CreatePowerTrack(IView view)
        {
        }

        #endregion

        #region IAction Members

        public bool Enabled
        {
            get
            {
                List<IActivity> activities = GetActivities() as List<IActivity>;

                foreach (IActivity activity in activities)
                {
                    if (activity.GPSRoute != null && 0 < activity.GPSRoute.Count)
                        return true;
                }

                return false;
            }
        }

        public bool HasMenuArrow
        {
            get { return false; }
        }

        public Image Image
        {
            get
            {
                return Resources.Images.PowerRunner16;
            }
        }

        public IList<string> MenuPath
        {
            get { return new List<string> { CommonResources.Text.LabelPower }; }
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Run(Rectangle rectButton)
        {
            IEnumerable<IActivity> activities = GetActivities();

            foreach (IActivity activity in activities)
            {
                DialogResult result = DialogResult.Yes;

                if (activity.PowerWattsTrack != null)
                {
                    // Warn if replacing an existing track
                    result = MessageDialog.Show(Resources.Strings.Text_OverwritePowerTrack + newLine
                        + activity.StartTime.Add(activity.TimeZoneUtcOffset) + newLine
                        + CommonResources.Text.LabelAvgPower + ": " + activity.PowerWattsTrack.Avg.ToString("0") + " " + CommonResources.Text.LabelWatts,
                        Resources.Strings.PowerRunner + ": " + activity.StartTime.Add(activity.TimeZoneUtcOffset), System.Windows.Forms.MessageBoxButtons.YesNo);
                }

                if (result == DialogResult.Yes)
                {
                    INumericTimeDataSeries powerTrack = PowerActivity.GetRunningPowerTrack(activity, true);

                    if (powerTrack != null)
                        PowerActivity.StorePowerTrack(activity, powerTrack);
                }
                else if (result == DialogResult.Cancel)
                {
                    // Cancel updating activities if user clicks 'X'
                    return;
                }
            }
        }

        public string Title
        {
            get
            {
                return Resources.Strings.Label_CreatePowerTrack;
            }
        }

        public bool Visible
        {
            get
            {
                List<IActivity> activities = GetActivities() as List<IActivity>;

                foreach (IActivity activity in activities)
                {
                    if (LogbookSettings.Main.Categories.GetPRCategory(activity.Category).Enable)
                        return true;
                }

                return false;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Get all activities, or only those selected
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<IActivity> GetActivities()
        {
            IList<IActivity> activities = new List<IActivity>();

            // Prevent null ref error during startup
            if (PluginMain.GetApplication().Logbook == null ||
                PluginMain.GetApplication().ActiveView == null)
            {
                return activities;
            }

            IView view = PluginMain.GetApplication().ActiveView;

            if (view != null && view.Id == GUIDs.DailyActivityView)
            {
                IDailyActivityView activityView = view as IDailyActivityView;
                activities = CollectionUtils.GetItemsOfType<IActivity>(activityView.SelectionProvider.SelectedItems);
            }
            else if (view != null && view.Id == GUIDs.ActivityReportsView)
            {
                IActivityReportsView reportsView = view as IActivityReportsView;
                activities = CollectionUtils.GetItemsOfType<IActivity>(reportsView.SelectionProvider.SelectedItems);
            }

            return activities;
        }

        #endregion
    }
}
