using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;

namespace PowerRunner.Settings
{
    public class PRCategory
    {
        public PRCategory()
        {
        }

        public PRCategory(IActivityCategory category)
        {
            CategoryId = category.ReferenceId;
            AutoCalculate = false;
            Enable = false;
        }

        public string CategoryId { get; set; }

        public bool AutoCalculate { get; set; }

        public bool Enable { get; set; }

        public override bool Equals(object obj)
        {
            PRCategory cat1 = obj as PRCategory;

            if (cat1 != null)
                return cat1.CategoryId == this.CategoryId;
            else
                return false;
        }
    }

    public class PRCategoryCollection : CollectionBase
    {
        public event CollectionChangeEventHandler CollectionChanged;

        public PRCategoryCollection() { }

        public PRCategory this[int index]
        {
            get { return (PRCategory)this.List[index]; }
            set { this.List[index] = value; }
        }

        public new int Count
        {
            get { return this.List.Count; }
        }
        
        public PRCategory GetPRCategory(IActivityCategory category)
        {
            PRCategory pCat = new PRCategory(category);

            if (this.Contains(pCat))
                return this[this.IndexOf(pCat)];

            return pCat;
        }

        public void Add(PRCategory category)
        {
            this.List.Add(category);

            if (CollectionChanged != null)
            {
                CollectionChanged.Invoke(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, category));
            }
        }

        public void Remove(PRCategory category)
        {
            this.List.Remove(category);

            if (CollectionChanged != null)
            {
                CollectionChanged.Invoke(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, category));
            }
        }

        public bool Contains(object value)
        {
            return this.List.Contains(value);
        }

        public void Update(PRCategory category)
        {
            if (category == null)
            {
                return;
            }
            else if (this.Contains(category))
            {
                this[IndexOf(category)].Enable = category.Enable;
                this[IndexOf(category)].AutoCalculate = category.AutoCalculate;
            }
            else
            {
                this.Add(category);
            }

            LogbookSettings.SaveSettings();
        }

        public int IndexOf(PRCategory category)
        {
            return List.IndexOf(category);
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is PRCategory))
            {
                throw new ArgumentException("Collection only supports PRCategory objects");
            }
        }
    }
}
