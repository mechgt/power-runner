using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Visuals;

namespace PowerRunner.Settings
{
    internal class SettingsPage : ISettingsPage
    {
        private static SettingsPageControl instance;

        #region ISettingsPage Members

        public Guid Id
        {
            get
            {
                return GUIDs.SettingsPage;
            }
        }

        public IList<ISettingsPage> SubPages
        {
            get { return null; }
        }

        #endregion

        #region IDialogPage Members

        public Control CreatePageControl()
        {
            if (instance == null)
                instance = new SettingsPageControl();

            return instance;
        }

        public bool HidePage()
        {
            return true;
        }

        public string PageName
        {
            get { return Resources.Strings.PowerRunner; }
        }

        public void ShowPage(string bookmark)
        {
            instance.RefreshPage();
        }

        public IPageStatus Status
        {
            get { return null; }
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            instance.ThemeChanged(visualTheme);
        }

        public string Title
        {
            get { throw new NotImplementedException(); }
        }

        public void UICultureChanged(CultureInfo culture)
        {
            instance.UICultureChanged(culture);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
