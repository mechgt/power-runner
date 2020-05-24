using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness.CustomData;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using PowerRunner.Settings;

namespace PowerRunner.Data
{
    internal class PowerActivity
    {
        #region Fields

        private const string newLine = "\r\n";
        private static readonly DateTime epoch = new DateTime(2012, 1, 1);

        #endregion

        internal static ICustomDataFieldDefinition VDOTField
        {
            get
            {
                // TODO: VDOT field settings
                return CustomDataFields.GetCustomProperty(CustomDataFields.TLCustomFields.VDOT, false);
            }
        }

        internal static float GetVDOT(DateTime date)
        {
            ICustomDataFieldDefinition field = VDOTField;

            if (VDOTField == null)
            {
                // Default value if field doesn't exist
                return 45;
            }

            double? value = PluginMain.GetLogbook().Athlete.InfoEntries.LastEntryAsOfDate(date).GetCustomDataValue(field) as double?;

            if (value != null)
            {
                return (float)value;
            }
            else
            {
                // Default value if not specified
                return 45;
            }
        }

        internal static float GetPercentVDOT(DateTime date, double speed)
        {
            float vdot = GetVDOT(date);
            // TODO: Perform lookup (somehow?) to find percent vdot
            return 80;
        }

        internal static INumericTimeDataSeries GetRunningPowerTrack(IActivity activity, bool overwrite)
        {
            int maxPower = 2000;

            // Return existing track if selected
            if (!overwrite && activity.PowerWattsTrack != null)
            {
                return activity.PowerWattsTrack;
            }

            double Ci, gp, grade, power;
            ITimeValueEntry<float> speed;
            double a0 = 1.68;   // minimum possible energy cost (J/m/kg)
            double gp0 = .184;  // descending g' of min energy cost (%)
            double a1 = 54.9;   // coefficient (J/m/kg)
            double a2 = -102;   // coefficient (J/m/kg)
            double a3 = 200;    // coefficient (J/m/kg)

            INumericTimeDataSeries gradeTrack = Utilities.GetGradeTrack(activity);
            INumericTimeDataSeries speedTrack = Utilities.GetSpeedTrack(activity);

            if (gradeTrack == null || gradeTrack.Count == 0 || speedTrack == null || speedTrack.Count == 0)
            {
                // Bad or invalid data
                return null;
            }

            INumericTimeDataSeries powerTrack = new NumericTimeDataSeries();
            ITimeValueEntry<float> lastPoint = null;

            // Define default weight if not specified
            float weight = PluginMain.GetLogbook().Athlete.InfoEntries.LastEntryAsOfDate(activity.StartTime.Add(activity.TimeZoneUtcOffset)).WeightKilograms;

            // Add weight of equipment
            // TODO:  Is this the same or different depending on the nature of the equipment (weight vest vs. stroller)?
            foreach (IEquipmentItem equip in activity.EquipmentUsed)
            {
                weight += equip.WeightKilograms;
            }

            if (float.IsNaN(weight))
            {
                if (PluginMain.GetLogbook().Athlete.Sex == AthleteSex.Female)
                {
                    weight = 55;
                }
                else
                {
                    weight = 80;
                }
            }

            // Create power track
            foreach (ITimeValueEntry<float> item in gradeTrack)
            {
                if (lastPoint != null)
                {
                    grade = item.Value;
                    speed = speedTrack.GetInterpolatedValue(gradeTrack.EntryDateTime(item));

                    if (speed != null && !double.IsNaN(speed.Value))
                    {
                        // gp = g' (gprime)
                        gp = grade / Math.Sqrt(1 + grade * grade);
                        Ci = a0 + a1 * Math.Pow((gp + gp0), 2) + a2 * Math.Pow((gp + gp0), 4) + a3 * Math.Pow((gp + gp0), 6);
                        power = Ci * weight * speed.Value * GetRunningEfficiency(speed.Value);

                        // Clip power at 0 and 2000 (max power)
                        power = Math.Min(Math.Max(0, power), maxPower);

                        if (!double.IsNaN(power))
                            powerTrack.Add(gradeTrack.EntryDateTime(item), (float)power);
                    }
                }

                lastPoint = item;
            }

            return powerTrack;
        }

        /// <summary>
        /// Based off of a linear coorelation with velocity, get the cooresponding running energy efficiency
        /// </summary>
        /// <param name="speedMetersPerSecond">speed in m/s</param>
        /// <returns>Returns efficiency</returns>
        internal static double GetRunningEfficiency(double speedMetersPerSecond)
        {
            // Efficiency at velocity.  Increases linearly with velocity: .3 slowest running speed -> .6 @ 8.33 m/s
            double m = 0.04894f;
            double eff = (m * (speedMetersPerSecond - 2.2)) + .3;

            return eff;
        }

        /// <summary>
        /// Gets the factor (score earned per minute) given a specific %VDOT
        /// </summary>
        /// <param name="VDOTpct">%VDOT - This is a generic value that can be derived from a 
        /// specific pace.  It's based on an athlete's current capabilities (similar to FTP).</param>
        /// <returns>Returns a factor (score per minute)</returns>
        internal static double GetDanielsFactor(double VDOTpct)
        {
            /*
             * Below are polynomials to calculate the factors.
             * These values were determined from an Excel best-fit trendline
             * based on the values given in Jack Daniels table in Daniels' Running Formula
             */
            double p0 = -1.776955379;
            double p1 = 0.061961106;
            double p2 = -0.000772856;
            double p3 = 4.36622E-06;
            /* 
            * score is calculated as time at a particular speed
            * foreach point:
            * time = deltaTime
            * score += factor (from formula above) * %VDOT speed * time (in MINUTES)
            * 
            * How to calculate VDOT speed?
            * */

            return p0 + (p1 * VDOTpct) + (p2 * Math.Pow(VDOTpct, 2)) + (p3 * Math.Pow(VDOTpct, 3));
        }

        internal static void StorePowerTrack(IActivity activity)
        {
            StorePowerTrack(activity, GetRunningPowerTrack(activity, false));
        }

        internal static void StorePowerTrack(IActivity activity, INumericTimeDataSeries powerTrack)
        {
            if (powerTrack != null && 0 < powerTrack.Count)
            {
                string meta = string.Format("{0}|{1}|{2}", new PluginMain().Version, (DateTime.Now - epoch).Days, true ? "A" : "E");

                activity.PowerWattsTrack = powerTrack;
                activity.SetExtensionText(GUIDs.PluginMain, meta);

                if (LogbookSettings.Main.UpdateActivityNotes)
                    activity.Notes = string.Format(Resources.Strings.Text_NotesMessage, "Running", Resources.Strings.PowerRunner, new PluginMain().Version) + newLine + newLine + activity.Notes;
            }
        }
    }
}
