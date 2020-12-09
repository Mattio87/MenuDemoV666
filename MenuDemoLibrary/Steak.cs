using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class Steak:Meals
    {
        private string _steakType;
        public string Steaktype
        {
            get { return _steakType; }
            set { _steakType = value; }
        }

        private string _side;
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        public Steak()
        {

        }




    }
}
