using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals.Util;

namespace PowerRunner.Settings
{
    public partial class SettingsPageControl : UserControl
    {
        private bool selectionChanging;

        public SettingsPageControl()
        {
            InitializeComponent();

            catTree.ContentProvider = new CategoryContentProvider();
            catTree.RowDataRenderer = new CategoryTreeRenderer(catTree);
            catTree.Columns.Add(new TreeList.Column());
            catTree.ShowLines = false;

            enbLogActions.Checked = LogbookSettings.Main.UpdateActivityNotes;
        }

        private IActivityCategory SelectedCategory
        {
            get
            {
                IActivityCategory category = CollectionUtils.GetFirstItemOfType<IActivityCategory>(catTree.SelectedItems);

                if (category != null)
                    return category;

                return null;
            }
        }

        internal void RefreshPage()
        {
            catTree.RowData = PluginMain.GetApplication().Logbook.ActivityCategories;
            //catTree.RowData = PluginMain.GetApplication().Logbook.ActivityCategories[0].SubCategories;
            catTree.Refresh();
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            catTree.ThemeChanged(visualTheme);
            catBanner.ThemeChanged(visualTheme);

            catTree.RowHotlightColor = Color.FromArgb(50, catTree.RowHotlightColor);
            catTree.RowSelectedColor = Color.FromArgb(100, catTree.RowSelectedColor);
            catTree.RowSeparatorLines = false;
            catTree.RowAlternatingColors = false;
        }

        public void UICultureChanged(CultureInfo culture)
        {
            // TODO: UI Culture changed
        }

        private void UpdateSubcategories(IActivityCategory parent, string prop, bool value)
        {
            PRCategory pCat = null;

            foreach (IActivityCategory category in parent.SubCategories)
            {
                switch (prop)
                {
                    case "AutoCalc":
                        pCat = LogbookSettings.Main.Categories.GetPRCategory(category);
                        pCat.AutoCalculate = value;
                        break;

                    case "Enable":
                        pCat = LogbookSettings.Main.Categories.GetPRCategory(category);
                        pCat.Enable = value;
                        break;
                }

                LogbookSettings.Main.Categories.Update(pCat);

                UpdateSubcategories(category, prop, value);
            }
        }

        private void enbAutoCalc_CheckedChanged(object sender, EventArgs e)
        {
            if (selectionChanging)
                return;

            if (SelectedCategory != null)
            {
                PRCategory pCat = LogbookSettings.Main.Categories.GetPRCategory(SelectedCategory);

                pCat.AutoCalculate = enbAutoCalculate.Checked;

                UpdateSubcategories(SelectedCategory, "AutoCalc", pCat.AutoCalculate);
                LogbookSettings.Main.Categories.Update(pCat);
                catTree.Refresh();
            }
        }

        private void enbRunning_CheckedChanged(object sender, EventArgs e)
        {
            if (selectionChanging)
                return;

            if (SelectedCategory != null)
            {
                PRCategory pCat = LogbookSettings.Main.Categories.GetPRCategory(SelectedCategory);

                pCat.Enable = enbRunning.Checked;
                enbAutoCalculate.Enabled = enbRunning.Checked;

                UpdateSubcategories(SelectedCategory, "Enable", pCat.Enable);
                LogbookSettings.Main.Categories.Update(pCat);
                catTree.Refresh();
            }
        }

        private void enbLogActions_CheckedChanged(object sender, EventArgs e)
        {
            if (LogbookSettings.Main.UpdateActivityNotes != enbLogActions.Checked)
                LogbookSettings.Main.UpdateActivityNotes = enbLogActions.Checked;
        }

        private void catTree_SelectedItemsChanged(object sender, EventArgs e)
        {
            selectionChanging = true;

            if (SelectedCategory != null)
            {
                // Display options for currently selected category
                PRCategory pCat = LogbookSettings.Main.Categories.GetPRCategory(SelectedCategory);
                enbRunning.Enabled = true;
                enbRunning.Checked = pCat.Enable;

                enbAutoCalculate.Enabled = enbRunning.Checked;
                enbAutoCalculate.Checked = pCat.AutoCalculate;
            }
            else
            {
                // Nothing selected
                enbRunning.Checked = false;
                enbAutoCalculate.Checked = false;

                enbRunning.Enabled = false;
                enbAutoCalculate.Enabled = false;
            }

            selectionChanging = false;
        }
    }
}
