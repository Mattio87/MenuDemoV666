using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class Pasta:Meals
    {
        private string _pastaType;
        public string Pastatype
        {
            get { return _pastaType; }
            set { _pastaType = value; }
        }

        private string _meat;
        public string Meat
        {
            get { return _meat; }
            set { _meat = value; }
        }

        public Pasta()
        {

        }



    }
}
