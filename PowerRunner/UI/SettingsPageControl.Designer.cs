using PowerRunner.Controls;
namespace PowerRunner.Settings
{
    partial class SettingsPageControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.catBanner = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
            this.catPanel = new System.Windows.Forms.Panel();
            this.catTree = new PowerRunner.Controls.AutoExpandTreeList();
            this.enbAutoCalculate = new System.Windows.Forms.CheckBox();
            this.enbRunning = new System.Windows.Forms.CheckBox();
            this.enbLogActions = new System.Windows.Forms.CheckBox();
            this.catPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // catBanner
            // 
            this.catBanner.BackColor = System.Drawing.Color.Transparent;
            this.catBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.catBanner.HasMenuButton = false;
            this.catBanner.Location = new System.Drawing.Point(0, 0);
            this.catBanner.Name = "catBanner";
            this.catBanner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.catBanner.Size = new System.Drawing.Size(500, 27);
            this.catBanner.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header2;
            this.catBanner.TabIndex = 11;
            this.catBanner.Text = "Category";
            this.catBanner.UseStyleFont = true;
            // 
            // catPanel
            // 
            this.catPanel.Controls.Add(this.catTree);
            this.catPanel.Controls.Add(this.catBanner);
            this.catPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.catPanel.Location = new System.Drawing.Point(0, 0);
            this.catPanel.Name = "catPanel";
            this.catPanel.Size = new System.Drawing.Size(500, 318);
            this.catPanel.TabIndex = 0;
            // 
            // catTree
            // 
            this.catTree.BackColor = System.Drawing.Color.Transparent;
            this.catTree.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.catTree.CheckBoxes = false;
            this.catTree.DefaultIndent = 15;
            this.catTree.DefaultRowHeight = -1;
            this.catTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.catTree.HeaderRowHeight = 21;
            this.catTree.Location = new System.Drawing.Point(0, 27);
            this.catTree.MultiSelect = false;
            this.catTree.Name = "catTree";
            this.catTree.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.None;
            this.catTree.NumLockedColumns = 0;
            this.catTree.RowAlternatingColors = true;
            this.catTree.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(147)))), ((int)(((byte)(160)))), ((int)(((byte)(112)))));
            this.catTree.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.catTree.RowHotlightMouse = true;
            this.catTree.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.catTree.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.catTree.RowSeparatorLines = true;
            this.catTree.ShowLines = false;
            this.catTree.ShowPlusMinus = false;
            this.catTree.Size = new System.Drawing.Size(500, 291);
            this.catTree.TabIndex = 10;
            this.catTree.SelectedItemsChanged += new System.EventHandler(this.catTree_SelectedItemsChanged);
            // 
            // enbAutoCalculate
            // 
            this.enbAutoCalculate.AutoSize = true;
            this.enbAutoCalculate.Location = new System.Drawing.Point(3, 347);
            this.enbAutoCalculate.Name = "enbAutoCalculate";
            this.enbAutoCalculate.Size = new System.Drawing.Size(142, 17);
            this.enbAutoCalculate.TabIndex = 1;
            this.enbAutoCalculate.Text = "Auto Calculate on Import";
            this.enbAutoCalculate.UseVisualStyleBackColor = true;
            this.enbAutoCalculate.CheckedChanged += new System.EventHandler(this.enbAutoCalc_CheckedChanged);
            // 
            // enbRunning
            // 
            this.enbRunning.AutoSize = true;
            this.enbRunning.Location = new System.Drawing.Point(3, 324);
            this.enbRunning.Name = "enbRunning";
            this.enbRunning.Size = new System.Drawing.Size(155, 17);
            this.enbRunning.TabIndex = 1;
            this.enbRunning.Text = "Contains Running Activities";
            this.enbRunning.UseVisualStyleBackColor = true;
            this.enbRunning.CheckedChanged += new System.EventHandler(this.enbRunning_CheckedChanged);
            // 
            // enbLogActions
            // 
            this.enbLogActions.AutoSize = true;
            this.enbLogActions.Location = new System.Drawing.Point(3, 385);
            this.enbLogActions.Name = "enbLogActions";
            this.enbLogActions.Size = new System.Drawing.Size(199, 17);
            this.enbLogActions.TabIndex = 1;
            this.enbLogActions.Text = "Include log message in activity notes";
            this.enbLogActions.UseVisualStyleBackColor = true;
            this.enbLogActions.CheckedChanged += new System.EventHandler(this.enbLogActions_CheckedChanged);
            // 
            // SettingsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.enbRunning);
            this.Controls.Add(this.enbLogActions);
            this.Controls.Add(this.enbAutoCalculate);
            this.Controls.Add(this.catPanel);
            this.MaximumSize = new System.Drawing.Size(500, 600);
            this.Name = "SettingsPageControl";
            this.Size = new System.Drawing.Size(500, 467);
            this.catPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AutoExpandTreeList catTree;
        private ZoneFiveSoftware.Common.Visuals.ActionBanner catBanner;
        private System.Windows.Forms.Panel catPanel;
        private System.Windows.Forms.CheckBox enbAutoCalculate;
        private System.Windows.Forms.CheckBox enbRunning;
        private System.Windows.Forms.CheckBox enbLogActions;
    }
}
