namespace PowerRunner
{
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using PowerRunner.Settings;
    using ZoneFiveSoftware.Common.Data.Fitness;
    using ZoneFiveSoftware.Common.Visuals.Fitness;

    internal class PluginMain : IPlugin
    {
        public static event PropertyChangedEventHandler LogbookChanged;

        #region IPlugin Members

        public IApplication Application
        {
            set
            {
                if (app != null)
                {
                    app.PropertyChanged -= new PropertyChangedEventHandler(AppPropertyChanged);
                }

                app = value;

                if (app != null)
                {
                    app.PropertyChanged += new PropertyChangedEventHandler(AppPropertyChanged);
                }
            }
        }

        public System.Guid Id
        {
            get { return GUIDs.PluginMain; }
        }

        public string Name
        {
            get { return "PowerRunnerText"; }
        }

        public string Version
        {
            get { return GetType().Assembly.GetName().Version.ToString(3); }
        }

        public void ReadOptions(System.Xml.XmlDocument xmlDoc, System.Xml.XmlNamespaceManager nsmgr, System.Xml.XmlElement pluginNode)
        {
            try
            {
                // Deserialization
                GlobalSettings settings;
                XmlSerializer xs = new XmlSerializer(typeof(GlobalSettings));
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(pluginNode.InnerText);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                object deserialize = xs.Deserialize(memoryStream);

                settings = (GlobalSettings)deserialize;
                memoryStream.Close();
                memoryStream.Dispose();
            }
            catch
            {
            }
        }

        public void WriteOptions(System.Xml.XmlDocument xmlDoc, System.Xml.XmlElement pluginNode)
        {
            // Serialization
            string xmlizedString;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(GlobalSettings));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, GlobalSettings.Main);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            UTF8Encoding encoding = new UTF8Encoding();
            xmlizedString = encoding.GetString(memoryStream.ToArray());

            pluginNode.InnerText = xmlizedString;

            xmlTextWriter.Close();
            memoryStream.Close();
            memoryStream.Dispose();
        }

        #endregion

        #region Mechgt License Fields

        /// <summary>
        /// Plugin product Id as listed in license application
        /// </summary>
        internal static string ProductId
        {
            get
            {
                return "pr";
            }
        }

        internal static string SupportEmail
        {
            get
            {
                return "support@mechgt.com";
            }
        }

        /// <summary>
        /// Number of workouts supported in eval mode.
        /// </summary>
        internal static int EvalMins
        {
            get { return 15; }
        }

        #endregion

        public static IApplication GetApplication()
        {
            return app;
        }

        /// <summary>
        /// Currently loaded logbook
        /// </summary>
        /// <returns>Returns currently loaded logbook</returns>
        internal static ILogbook GetLogbook()
        {
            return app.Logbook;
        }

        private void AppPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Logbook")
            {
                if (LogbookChanged != null)
                    LogbookChanged.Invoke(app, new PropertyChangedEventArgs("Logbook"));

                PowerRunner.Actions.AutoCalculate.Register(app.Logbook);
                LogbookSettings.LoadSettings();
            }
        }

        private static IApplication app = null;
    }
}
