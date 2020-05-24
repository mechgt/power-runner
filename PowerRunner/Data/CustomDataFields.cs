using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness.CustomData;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using System.Diagnostics;

namespace PowerRunner.Data
{
    internal class CustomDataFields
    {
        private static bool warningMsgBadField;

        public enum TLCustomFields
        {
            Trimp, TSS, FTPcycle, FTPrun, NormPwr, VDOT, DanielsPoints
        }

        /// <summary>
        /// Get a Training Load related custom parameter
        /// </summary>
        /// <param name="field"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        public static ICustomDataFieldDefinition GetCustomProperty(TLCustomFields field, bool autoCreate)
        {
            // All data types so far are numbers
            ICustomDataFieldDefinition fieldDef = null;
            ICustomDataFieldDataType dataType = CustomDataFieldDefinitions.StandardDataType(CustomDataFieldDefinitions.StandardDataTypes.NumberDataTypeId);
            ICustomDataFieldObjectType objType;
            Guid id;
            string name = GetCustomText(field);

            switch (field)
            {
                case TLCustomFields.Trimp:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customTrimp;
                    fieldDef = GetCustomProperty(dataType, objType, id, name, autoCreate);
                    break;

                case TLCustomFields.TSS:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customTSS;
                    fieldDef = GetCustomProperty(dataType, objType, id, name, autoCreate);
                    break;

                case TLCustomFields.NormPwr:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customNP;
                    fieldDef = GetCustomProperty(dataType, objType, id, name, autoCreate);
                    break;

                case TLCustomFields.FTPcycle:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IAthleteInfoEntry));
                    id = GUIDs.customFTPcycle;
                    fieldDef = GetCustomProperty(dataType, objType, id, name, autoCreate);
                    break;

                case TLCustomFields.FTPrun:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IAthleteInfoEntry));
                    id = GUIDs.customFTPrun;
                    fieldDef = GetCustomProperty(dataType, objType, id, name, autoCreate);
                    break;

                case TLCustomFields.VDOT:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IAthleteInfoEntry));
                    id = GUIDs.customVDOT;
                    fieldDef = GetCustomProperty(dataType, objType, id, name, autoCreate);
                    break;

                case TLCustomFields.DanielsPoints:
                    objType = CustomDataFieldDefinitions.StandardObjectType(typeof(IActivity));
                    id = GUIDs.customDanielsPoints;
                    fieldDef = GetCustomProperty(dataType, objType, id, name, autoCreate);
                    break;
            }

            return fieldDef;
        }

        /// <summary>
        /// Gets the default string value for this field.
        /// </summary>
        /// <param name="type">Which field type to get name for</param>
        /// <returns>Returns current field name (if available), or default name for this field.</returns>
        public static string GetCustomText(TLCustomFields type)
        {
            return GetCustomText(type, null);
        }

        /// <summary>
        /// Gets the string value assigned to this field, or the default value if one is not already assigned.
        /// </summary>
        /// <param name="type">Which field type to get name for</param>
        /// <param name="field">Field to get name value from.</param>
        /// <returns>Returns current field name (if available), or default name for this field.</returns>
        public static string GetCustomText(TLCustomFields type, ICustomDataFieldDefinition field)
        {
            string name;

            if (field != null)
            {
                // Return user named text
                return field.Name;
            }
            else
            {
                // Default localized custom field names
                switch (type)
                {
                    case TLCustomFields.NormPwr:
                        name = Resources.Strings.Label_NormPower;
                        break;
                    case TLCustomFields.Trimp:
                        name = Resources.Strings.Label_TRIMP;
                        break;
                    case TLCustomFields.TSS:
                        name = Resources.Strings.Label_TSS;
                        break;
                    case TLCustomFields.DanielsPoints:
                        name = Resources.Strings.Label_DanielsPoints;
                        break;
                    default:
                        name = type.ToString();
                        break;
                }
                
                return name;
            }
        }

        /// <summary>
        /// Private helper to dig the logbook for a custom parameter
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="objType"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        private static ICustomDataFieldDefinition GetCustomProperty(ICustomDataFieldDataType dataType, ICustomDataFieldObjectType objType, Guid id, string name, bool autoCreate)
        {
            // Dig all of the existing custom params looking for a match.
            foreach (ICustomDataFieldDefinition customDef in PluginMain.GetLogbook().CustomDataFieldDefinitions)
            {
                if (customDef.Id == id)
                {
                    // Is this really necessary...?
                    if (customDef.DataType != dataType)
                    {
                        // Invalid match found!!! Bad news.
                        // This might occur if a user re-purposes a field.
                        if (!warningMsgBadField)
                        {
                            warningMsgBadField = true;
                            MessageDialog.Show("Invalid " + name + " Custom Field.  Was this field data type modified? (" + customDef.Name + ")", Resources.Strings.PowerRunner);
                        }

                        return null;
                    }
                    else
                    {
                        // Return custom def
                        return customDef;
                    }
                }
            }

            // No match found, create it
            if (autoCreate)
            {
                return PluginMain.GetLogbook().CustomDataFieldDefinitions.Add(id, objType, dataType, name);
            }
            else
            {
                return null;
            }
        }
    }
}
