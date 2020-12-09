using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MenuDemoLibrary
{

    [DebuggerDisplay("{DebuggerString}")]
    public class Category
    {

        private string DebuggerString
        {
            get
            {
                return $"{this.categoryName}, Meal count: {meals.Count}";
            }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string categoryName { get; set; }

        private string _categoryDescription;

        public string  CategoryDescription
        {
            get { return _categoryDescription; }
            set { _categoryDescription = value; }
        }


        public List<Meals> meals = new List<Meals>();

    }
}
