using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class RestaurantMenu
    {
        private string _menuName;

        public string MenuName
        {
            get { return _menuName; }
            set { _menuName = value; }
        }

        private string _menuDescription;

        public string MenuDescription
        {
            get { return _menuDescription; }
            set { _menuDescription = value; }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public List<Category> menus = new List<Category>();




    }
}
