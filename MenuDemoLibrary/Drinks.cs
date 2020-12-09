using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class Drinks:Meals
    {

        public bool isAlcoholic = false;




        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        



        public Drinks()
        {

        }

        public Drinks(string drinkName, bool alcoholic, int drinkAmount, float price)
        {
            base.Name = drinkName;
            this.isAlcoholic = alcoholic;
            this.Amount = drinkAmount;
            this.Price = price;
        }


    }
}
