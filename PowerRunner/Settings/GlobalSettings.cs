namespace PowerRunner.Settings
{
    using System.ComponentModel;
    using System.Xml.Serialization;

    /// <summary>
    /// User settings
    /// </summary>
    [XmlRootAttribute(ElementName = "PowerRunner", IsNullable = false)]
    public class GlobalSettings
    {
        #region Fields

        internal static event PropertyChangedEventHandler PropertyChanged;

        private static GlobalSettings main;

        #endregion

        #region Enumerations

        #endregion

        #region Properties

        [XmlIgnore]
        internal static GlobalSettings Main
        {
            get
            {
                if (main == null)
                {
                    main = new GlobalSettings();
                }

                return main;
            }
        }

        #endregion

        #region Methods

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
