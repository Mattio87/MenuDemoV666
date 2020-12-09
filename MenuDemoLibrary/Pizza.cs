using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class Pizza:Meals
    {
        private string _toppings;
        public string Toppings
        {
            get { return _toppings; }
            set { _toppings = value; }
        }

        public Pizza()
        {

        }

    }
}
