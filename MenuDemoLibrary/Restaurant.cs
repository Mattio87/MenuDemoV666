using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MenuDemoLibrary
{
    public class Restaurant
    {
        private string _restaurantName;

        public string RestaurantName
        {
            get { return _restaurantName; }
            set { _restaurantName = value; }
        }

        private string _restaurantAddress;

        public string RestaurantAddress
        {
            get { return _restaurantAddress; }
            set { _restaurantAddress = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public List<RestaurantMenu> restaurants = new List<RestaurantMenu>();


        public override string ToString()
        {
            return $"{RestaurantName}";
        }

    }
}
