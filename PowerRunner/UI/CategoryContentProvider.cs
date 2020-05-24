using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals;
using System.Collections;
using ZoneFiveSoftware.Common.Data.Fitness;

namespace PowerRunner.Settings
{
    /// <summary>
    /// Assign parent items, children are automatically managed and displayed.
    /// </summary>
    class CategoryContentProvider : IContentProvider
    {
        IActivityCategoryList theList;

        #region IContentProvider Members

        public IList GetChildren(object parentElement)
        {
            IActivityCategory category = parentElement as IActivityCategory;

            if (category != null)
                return GetElements(category.SubCategories);
            else
                return null;
        }

        public IList GetElements(object inputElement)
        {
            IEnumerable<IActivityCategory> categories = inputElement as IEnumerable<IActivityCategory>;

            if (categories == null)
                return null;
            else
            {
                IList elements = new List<IActivityCategory>();
                foreach (IActivityCategory category in categories)
                {
                    elements.Add(category);
                }

                return elements;
            }
        }

        public object GetParent(object element)
        {
            IActivityCategory category = element as IActivityCategory;

            if (category != null)
                return category.Parent;
            else
                return null;
        }

        public bool HasChildren(object element)
        {
            IActivityCategory category = element as IActivityCategory;

            if (category != null)
                return category.SubCategories.Count >= 0;
            else
                return false;
        }

        public void InputChanged(object oldInput, object newInput)
        {

        }

        #endregion
    }
}
