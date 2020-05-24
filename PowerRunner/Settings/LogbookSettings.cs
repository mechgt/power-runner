namespace PowerRunner.Settings
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using ZoneFiveSoftware.Common.Data.Fitness;
    using System.ComponentModel;

    /// <summary>
    /// User settings
    /// </summary>
    [XmlRootAttribute(ElementName = "PowerRunner", IsNullable = false)]
    public class LogbookSettings
    {
        #region Fields

        internal static event PropertyChangedEventHandler PropertyChanged;

        private static bool loaded;
        private static LogbookSettings main;
        private static PRCategoryCollection categories = new PRCategoryCollection();
        private static bool updateNotes = true;

        #endregion

        #region Constructors

        public LogbookSettings()
        {
        }

        #endregion

        #region Properties

        [XmlIgnore]
        internal static LogbookSettings Main
        {
            get
            {
                if (main == null)
                    main = new LogbookSettings();

                return main;
            }
        }

        [XmlIgnore]
        internal static bool Modified
        {
            get { return PluginMain.GetLogbook().Modified; }
            set
            {
                if (loaded && value)
                {
                    PluginMain.GetLogbook().Modified = value;
                }
            }
        }

        public PRCategoryCollection Categories
        {
            get { return categories; }
            set
            {
                categories = value;

                RaisePropertyChanged("Categories");
            }
        }

        /// <summary>
        /// Include the log actions in the Activity Notes field.
        /// For example, 'Power Track created by Power Runner Plugin v0.10.'
        /// </summary>
        public bool UpdateActivityNotes
        {
            get
            {
                return updateNotes;
            }
            set
            {
                if (updateNotes == value)
                    return;

                updateNotes = value;
                RaisePropertyChanged("UpdateActivityNotes");
            }
        }

        #endregion

        #region Methods

        internal static void LoadSettings()
        {
            ILogbook logbook = PluginMain.GetApplication().Logbook;

            if (logbook != null)
            {
                try
                {
                    // Byte Data
                    byte[] byteArray = logbook.GetExtensionData(GUIDs.PluginMain);

                    if (byteArray.Length > 0)
                    {
                        // Deserialization
                        LogbookSettings settings;
                        XmlSerializer xs = new XmlSerializer(typeof(LogbookSettings));

                        MemoryStream memoryStream = new MemoryStream(byteArray);

                        object deserialize = xs.Deserialize(memoryStream);

                        settings = (LogbookSettings)deserialize;

                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                    else
                    {
                        // Default settings
                    }
                }
                catch
                {
                }
                finally
                {
                    loaded = true;
                }
            }
        }

        internal static void SaveSettings()
        {
            if (!loaded) return;

            string xmlizedString;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(LogbookSettings));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, LogbookSettings.Main);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            UTF8Encoding encoding = new UTF8Encoding();
            xmlizedString = encoding.GetString(memoryStream.ToArray());

            // Bytes
            PluginMain.GetApplication().Logbook.SetExtensionData(GUIDs.PluginMain, memoryStream.ToArray());
            PluginMain.GetApplication().Logbook.Modified = true;

            xmlTextWriter.Close();
            memoryStream.Close();
            memoryStream.Dispose();
        }

        private static void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(null, new PropertyChangedEventArgs(property));

            Modified = true;
            SaveSettings();
        }

        #endregion
    }
}
