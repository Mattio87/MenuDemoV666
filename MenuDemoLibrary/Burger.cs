using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class Burger:Meals
    {
        
        private string _patty;
        public string Patty
        {
            get { return _patty; }
            set { _patty = value; }
        }

        private string _cheese;
        public string Cheese
        {
            get { return _cheese; }
            set { _cheese = value; }
        }

        private string _bun;
        public string Bun
        {
            get { return _bun; }
            set { _bun = value; }
        }


        public Burger()
        {

        }





    }
}
