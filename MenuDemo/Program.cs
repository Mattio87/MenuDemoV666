using System;
using System.Collections.Generic;
using MenuDemoLibrary;

namespace MenuDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "MenuDemo_Matti";

            DataManager dtmngr = new DataManager();

            MealManager mlmngr = new MealManager(dtmngr);


            //var y = DataManager.GetAllRestaurantDataFromDB(1);
            //var z = DataManager.GetListOfCategoriesInDBMenus(1);
            //var x = DataManager.GetListOfMealsInDBCategory(2);
            //var f = DataManager.GetMealWithID(10);
            //var k = DataManager.GetCategoryIdsFromMenuDB(1);


            mlmngr.SuperMainMenu();
            //mlmngr.SelectRestaurantFromAllRestaurantsList();

            //DataManager.GetAllDishes();
            //mlmngr.ShowCategoryMealsWithCategoryInfoFromDB();

            /*
            Meals meal = new Meals
            {
                Name = "Sausage Burger",
                Description = "Tradional Finnish sausage burger also" +
                "known as Porilainen",
                Price = 8.0F,
                Dishtype = 2
            };
            meal.AddAllergen(Allergen.AllergenType.glutenFree);
            meal.AddAllergen(Allergen.AllergenType.milkFree);
            DataManager.InsertMealToDB(meal);
            */

        }
    }
}
