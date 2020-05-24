using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Visuals;
using System.Globalization;
using ZoneFiveSoftware.Common.Data.Fitness;

namespace PowerRunner.Settings
{
    class CategoryTreeRenderer : TreeList.DefaultRowDataRenderer
    {
        public CategoryTreeRenderer(TreeList tree)
            : base(tree)
        {

        }

        protected override FontStyle GetCellFontStyle(object element, TreeList.Column column)
        {
            IActivityCategory category = element as IActivityCategory;
            if (category != null)
            {
                PRCategory pCat = LogbookSettings.Main.Categories.GetPRCategory(category);
                if (pCat.AutoCalculate && pCat.Enable)
                    return FontStyle.Bold;
            }

            return FontStyle.Regular;
        }

        protected override Brush GetCellTextBrush(TreeList.DrawItemState rowState, object element, TreeList.Column column)
        {
            IActivityCategory category = element as IActivityCategory;
            if (category != null)
            {
                PRCategory pCat = LogbookSettings.Main.Categories.GetPRCategory(category);
                if (!pCat.Enable)
                {
                    return new SolidBrush(ControlPaint.Light(PluginMain.GetApplication().VisualTheme.ControlText, .95f));
                }
            }

            return base.GetCellTextBrush(rowState, element, column);
        }

        protected override StringFormat GetCellStringFormat(object rowElement, TreeList.Column column)
        {
            return base.GetCellStringFormat(rowElement, column);
        }

        protected override void DrawBackground(Graphics graphics, Rectangle clipRect, Rectangle rectDraw)
        {
            Image skeleton = Resources.Images.PowerRunnerBG;
            //Image skeleton = Resources.Images.PowerRunnerLG;
            Point p = new Point(Tree.Width - skeleton.Width - 30, (Tree.Height - skeleton.Height) / 2);
            graphics.DrawImage(skeleton, p);
        }

        protected override void DrawCell(Graphics graphics, TreeList.DrawItemState rowState, object element, TreeList.Column column, Rectangle cellRect)
        {
            //IActivityCategory category = element as IActivityCategory;

            //if (category != null)
            //{
            //    // Gets custom formatted text
            //    string text = Tree.LabelProvider.GetText(element, column);

            //    // Draw String if custom text defined
            //    if (text != null)
            //    {

            //        graphics.DrawString(text,
            //                        base.Font(GetCellFontStyle(element, column)),
            //                        new SolidBrush(PluginMain.GetApplication().VisualTheme.ControlText),
            //                        cellRect);

            //        return;
            //    }
            //}

            // Default handler
            base.DrawCell(graphics, rowState, element, column, cellRect);
        }
    }
}
