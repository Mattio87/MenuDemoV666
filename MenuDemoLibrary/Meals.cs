using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MenuDemoLibrary
{


    public class Meals:Category
    {


        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }



        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _sauce;

        public string Sauce
        {
            get { return _sauce; }
            set { _sauce = value; }
        }

        private float _price;

        public float Price
        {
            get { return _price; }
            set { _price = value; }
        }


        private List<Allergen.AllergenType> _allergenTypes = new List<Allergen.AllergenType>();

        public  List<Allergen.AllergenType> Allergentypes
        {
            get { return _allergenTypes; }
            set { _allergenTypes = value; }
        }

        public void AddAllergen(Allergen.AllergenType type)
        {
            if (_allergenTypes.Contains(type)) return;
            _allergenTypes.Add(type);
        }

        public void AddAllergen(List<Allergen.AllergenType> list)
        {
            foreach(Allergen.AllergenType item in list)
            {
                AddAllergen(item);
            }
        }

        public List<Allergen.AllergenType> GetAllergens()
        {
            return _allergenTypes;
        }

        private int _mealtype;

        public int Dishtype
        {
            get { return _mealtype; }
            set { _mealtype = value; }
        }


        public static Meals GetMealClassWithDishType(int type, Meals template = null)
        {
            Meals meal = null;
            if (type == 1 || type == 6)
            {
                meal = new Meals();
            }
            if (type == 2)
            {
                meal = new Burger();
            }
            if (type == 3)
            {
                meal = new Pizza();
            }
            if (type == 4)
            {
                meal = new Pasta();
            }
            if (type == 5)
            {
                meal = new Steak();
            }
            if (type == 7)
            {
                meal = new Drinks();
            }
            if (template != null)
            {
                meal.Name = template.Name;
                meal.Description = template.Description;
                meal.Sauce = template.Sauce;
                meal.Price = template.Price;
                meal.Id = template.Id;
            }
            return meal;
        }


        public Meals()
        {

        }

        public override string ToString()
        {
            return $"{Name}, price: {Price}";
        }

    }
}
