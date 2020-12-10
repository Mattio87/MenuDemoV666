using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class MealManager
    {

        private DataManager dataManager;

        private Restaurant _selectedRestaurant;

        public  Restaurant SelectedRestaurant
        {
            get { return _selectedRestaurant; }
            set { _selectedRestaurant = value; }
        }


        public MealManager(DataManager dataManager)
        {
            this.dataManager = dataManager;
            var restaurant = DataManager.GetAllRestaurantsListFromDB()[0];
            if (restaurant != null)
            {
                SelectedRestaurant=DataManager.GetAllRestaurantDataFromDB(restaurant.Id); 
            }
        }

        public void ShowMealData(Meals meal)
        {
            Console.WriteLine();
            Console.WriteLine($"Id: {meal.Id}");
            Console.WriteLine($"{meal.Name}");
            if(meal.Description != null)
            {
                Console.WriteLine($"{meal.Description}");
            }
            Type type = meal.GetType();
            if(type == typeof(Burger))
            {
                Burger burger = (Burger)meal;
                Console.WriteLine($"Patty: {burger.Patty}");
                Console.WriteLine($"Cheese: {burger.Cheese}");
                Console.WriteLine($"Bun: {burger.Bun}");

            }

            if(type == typeof(Pizza))
            {
                Pizza pizza = (Pizza)meal;
                Console.WriteLine($"Toppings: {pizza.Toppings}");

            }

            if(type == typeof(Pasta))
            {
                Pasta pasta = (Pasta)meal;
                Console.WriteLine($"Type of pasta: {pasta.Pastatype}");
                Console.WriteLine($"Meat: {pasta.Meat}");

            }

            if(type == typeof(Steak))
            {
                Steak steak = (Steak)meal;
                Console.WriteLine($"Type: {steak.Steaktype}");
                Console.WriteLine($"Side: {steak.Side}");

            }

            if(type == typeof(Drinks))
            {
                Drinks drink = (Drinks)meal;
                Console.WriteLine($"Amout: {drink.Amount}cl");
                if (drink.isAlcoholic == true)
                {
                    Console.WriteLine("Alcoholic");
                }
                else
                {
                    Console.WriteLine("Non-Alcoholic");
                }
            }

            if(meal.Sauce != null)
            {
                Console.WriteLine($"Sauce: {meal.Sauce}");
            }
            
            foreach(var allergen in meal.Allergentypes)
            {
                if(allergen == Allergen.AllergenType.glutenFree)
                {
                    Console.WriteLine("This dish is gluten free, " +Allergen.allergenChars[allergen]);
                    
                }

                else if(allergen == Allergen.AllergenType.lactoseFree)
                {
                    Console.WriteLine("This dish is lactose free, " +Allergen.allergenChars[allergen]);
                }

                else if (allergen == Allergen.AllergenType.milkFree)
                {
                    Console.WriteLine("This dish is milk free, " +Allergen.allergenChars[allergen]);
                }

                else if (allergen == Allergen.AllergenType.lowLactose)
                {
                    Console.WriteLine("This dish is low lactose, " +Allergen.allergenChars[allergen]);
                }
            }
            
            Console.WriteLine($"Price: {meal.Price}€");
        }



        public void SuperMainMenu()
        {
            bool showMenu = true;
            while (showMenu == true)
            {

                Console.Clear();
                Console.WriteLine("\n ----- Main menu -----");
                Console.WriteLine($"Restaurant selected: {SelectedRestaurant.RestaurantName}");
                Console.WriteLine("1. Show all dishes");
                Console.WriteLine("2. Show restaurant menu");
                Console.WriteLine("3. Create a new dish");
                Console.WriteLine("4. Create a new restaurant");
                Console.WriteLine("5. Create a new menu for a restaurant");
                Console.WriteLine("6. Create a new category for a menu");

                Console.WriteLine("9. Change selected restaurant");
                Console.WriteLine("0. Exit");
                ConsoleKey selected = Console.ReadKey(true).Key;
                switch (selected)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        ShowMealsFromAllMealsList();
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey(true);
                        break;

                    case ConsoleKey.D2:
                        Console.Clear();
                        Restaurant r = SelectedRestaurant;
                        RestaurantMenu rm = SelectMenuFromRestaurant(r);
                        Category C1 = null;
                        if (rm != null) C1 = SelectCategoryFromMenu(rm);
                        if (C1 != null)
                        {
                            ShowCategory(C1);
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey(true);
                        }
                        break;

                    case ConsoleKey.D3:
                        Console.Clear();
                        Console.WriteLine("Create your new dish:");
                        Meals meal = CreateMeal();
                        Console.Clear();
                        Console.WriteLine("The dish you created:");
                        this.ShowMealData(meal);

                        Console.WriteLine("Do you want to add this dish to database?");
                        Console.WriteLine("Answer Y or N");
                        bool happyness = GetYesOrNoFromUser();
                        if (happyness == true)
                        {
                            int mealId = DataManager.InsertMealToDB(meal);
                            Console.WriteLine($"Dish added to the database with the Id: {mealId}");
                        }
                        Console.WriteLine("Dish creation complete.");
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey(true);
                        break;

                    case ConsoleKey.D4:
                        Console.Clear();
                        Console.WriteLine("Create the new restaurant");
                        Restaurant restaurant = this.CreateNewRestaurant();
                        Console.Clear();
                        Console.WriteLine("The restaurant you created:");
                        this.ShowRestaurantInfo(restaurant);

                        Console.WriteLine("Do you want to add this restaurant to all restaurants list?");
                        Console.WriteLine("Answer Y or N");
                        bool acception = GetYesOrNoFromUser();
                        if (acception == true)
                        {
                            dataManager.AddRestaurantToAllRestaurantsList(restaurant);
                            Console.WriteLine("Restaurant is now added to the list.");
                        }
                        Console.WriteLine("Restaurant is now created.");
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey(true);
                        break;

                    case ConsoleKey.D5:
                        Console.Clear();
                        Restaurant r2 = SelectedRestaurant;
                        RestaurantMenu restaurantMenu = CreateNewMenu();
                        r2.restaurants.Add(restaurantMenu);
                        Console.WriteLine($"Current menus in {r2.RestaurantName}:");
                        ShowRestaurantWithMenuNumbers(r2);

                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey(true);
                        break;

                    case ConsoleKey.D6:
                        Console.Clear();
                        Category category = CreateCategory();
                        Console.WriteLine($"You created: {category.categoryName}");
                        Console.WriteLine("Do you want to add it to an existing menu?");
                        Console.WriteLine("Answer Y or N");
                        bool hopeThisWorks = GetYesOrNoFromUser();
                        if (hopeThisWorks == true)
                        {
                            Console.WriteLine("Choose the restaurant.");
                            Restaurant r4 = SelectRestaurantFromAllRestaurantsList();
                            RestaurantMenu rm5 = SelectMenuFromRestaurant(r4);
                            rm5.menus.Add(category);
                            ShowMenuWithCategoryNumbers(rm5);
                        }
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey(true);
                        break;

                    case ConsoleKey.D9:
                        SelectedRestaurant = this.SelectRestaurantFromAllRestaurantsList();
                        break;

                    case ConsoleKey.D0:
                        showMenu = false;
                        break;
                    

                }
            }
        }


        public void ShowRestaurantInfo(Restaurant restaurant)
        {
            Console.WriteLine();
            Console.WriteLine(restaurant.RestaurantName);
            Console.WriteLine(restaurant.RestaurantAddress);
        }


        public void ShowCategory(Category category)
        {
            
            Console.WriteLine();
            Console.WriteLine(category.categoryName);
            foreach(Meals meal in category.meals)
            {
                Console.WriteLine();
                ShowMealData(meal);
            }

        }


        public void ShowRestaurantMenu(RestaurantMenu menu)
        {
            Console.WriteLine(menu.MenuName);
            foreach(Category category in menu.menus)
            {
                Console.WriteLine();
                ShowCategory(category);
            }

        }

        public void ShowRestaurantInfoWithMenu(Restaurant restaurant)
        {
            Console.WriteLine(restaurant.RestaurantName);
            Console.WriteLine(restaurant.RestaurantAddress);
            foreach (RestaurantMenu menu in restaurant.restaurants)
            {
                Console.WriteLine();
                ShowRestaurantMenu(menu);
            }
        }

        public void ShowCategoryWithNumbers(Category category)
        {
            Console.WriteLine($"Dishes in category: {category.categoryName}");
            for (int i = 0; i < category.meals.Count; i++)
            {
                Console.WriteLine($"{i+1}. {category.meals[i].Name}");
            }
        }

        public Meals SelectMealFromCategory(Category category)
        {
            Console.WriteLine("Dishes: ");
            ShowCategoryWithNumbers(category);
            Console.WriteLine("\nChoose your meal");
            int selection = int.Parse(Console.ReadLine());
            return category.meals[selection - 1];
        }


        public void RemoveSelectedMealFromCategory(Category category)
        {
            Meals selectedMeal = SelectMealFromCategory(category);
            category.meals.Remove(selectedMeal);
            
            
        }

        public void ShowMenuWithCategoryNumbers(RestaurantMenu menu)
        {
            Console.WriteLine($"Categories in menu: {menu.MenuName}");
            for (int i=0; i < menu.menus.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"{i+1}. {menu.menus[i].categoryName}");
            }

        }

        public Category SelectCategoryFromMenu(RestaurantMenu menu)
        {
            if (menu.menus.Count == 0)
            {
                Console.WriteLine("There are currently no categorie in this menu.");
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                return null;
            }
            ShowMenuWithCategoryNumbers(menu);
            Console.WriteLine("\nChoose your category.");
            int selection = int.Parse(Console.ReadLine());
            return menu.menus[selection - 1];
            
        }

        public void RemoveSelectedCategoryFromMenu(RestaurantMenu menu)
        {
            Category selectedCategory = SelectCategoryFromMenu(menu);
            menu.menus.Remove(selectedCategory);

        }

        public void ShowRestaurantWithMenuNumbers(Restaurant restaurant)
        {

            Console.WriteLine($"Menus in: {restaurant.RestaurantName}");
            for (int i = 0; i < restaurant.restaurants.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"{i + 1}. {restaurant.restaurants[i].MenuName}");
            }

        }

        public RestaurantMenu SelectMenuFromRestaurant(Restaurant restaurant)
        {
            
            if (restaurant.restaurants.Count == 0)
            {
                Console.WriteLine("There are currently no active menus for this restaurant.");
                Console.WriteLine("\nPress any key to return to main menu.");
                Console.ReadKey();
                return null;
            }
            ShowRestaurantWithMenuNumbers(restaurant);
            Console.WriteLine("\nChoose your menu.");
            int selection = int.Parse(Console.ReadLine());
            return restaurant.restaurants[selection - 1];

        }

        public void RemoveSelectedMenuFromRestaurant(Restaurant restaurant)
        {
            RestaurantMenu selectedMenu = SelectMenuFromRestaurant(restaurant);
            restaurant.restaurants.Remove(selectedMenu);
        }

        public void ShowMealsFromAllMealsList()
        {
            ShowMealsFromACategoryDBList(DataManager.GetAllDishes());


        }

        public void ShowMealsFromACategoryDBList(List<Meals> list)
        {
            //Console.WriteLine("All dishes:");
            foreach (var item in list)
            {
                
                ShowMealData(DataManager.GetMealWithID(item.Id));
            }
        }


        public void ShowCategoryMealsWithCategoryInfoFromDB()
        {
            int[] categoryIds = DataManager.GetCategoryIdsFromMenuDB(1);
            foreach (int category in categoryIds)
            {
                Category category1 = DataManager.GetCategoryWithIDFromDB(category);
                Console.WriteLine($"\n-------{category1.categoryName}------");
                Console.WriteLine($"-------{category1.CategoryDescription}-------");
                Console.WriteLine();
                this.ShowMealsFromACategoryDBList(DataManager.GetListOfMealsInDBCategory(category));
            }
        }
        




        public void ShowAllRestaurants()
        {
            List<Restaurant> restaurants1 = dataManager.GetRestaurantsFromAllRestaurantsList();
            Console.WriteLine("Restaurants: ");
            foreach (Restaurant restaurant in restaurants1)
            {
                this.ShowRestaurantInfo(restaurant);
            }

        }

        public Restaurant SelectRestaurantFromAllRestaurantsList()
        {
            List<Restaurant> restaurants1 = dataManager.GetRestaurantsFromAllRestaurantsList();
            Console.WriteLine("Restaurants: ");
            for (int i = 0; i < restaurants1.Count; i++)
            {
                Console.WriteLine($"{i+1}. {restaurants1[i].RestaurantName}");
            }
            Console.WriteLine("\nChoose the restaurant:");
            int selection = int.Parse(Console.ReadLine());
            Restaurant restaurant = restaurants1[selection - 1];
            restaurant = DataManager.GetAllRestaurantDataFromDB(restaurant.Id);
            return restaurant;

        }
        
        public void testi()
        {
            Restaurant r = SelectRestaurantFromAllRestaurantsList();
            RestaurantMenu rm = SelectMenuFromRestaurant(r);
            Category c = SelectCategoryFromMenu(rm);
            ShowCategoryWithNumbers(c);
        }

        public Category CreateCategory()
        {
            Category category = new Category();
            category.categoryName = GetCategoryName();
            return category;
        }

        public RestaurantMenu CreateNewMenu()
        {
            RestaurantMenu restaurantMenu = new RestaurantMenu();
            restaurantMenu.MenuName = GetMenuName();
            return restaurantMenu;
        }


        public Restaurant CreateNewRestaurant()
        {
            Restaurant restaurant = new Restaurant();
            restaurant.RestaurantName = GetRestaurantName();
            restaurant.RestaurantAddress = GetRestaurantAddress();
            return restaurant;
        }

        public Meals CreateMeal()
        {

            string name = GetMealName();
            Console.WriteLine("Choose the type of the meal you want to create:");
            Console.WriteLine("1. Drink");
            Console.WriteLine("2. Burger");
            Console.WriteLine("3. Pizza");
            Console.WriteLine("4. Pasta");
            Console.WriteLine("5. Steak");
            Console.WriteLine("6. Starter");
            Console.WriteLine("7. Dessert");

            int selected = int.Parse(Console.ReadLine());
            Meals meal = null;
            if (selected == 1)
            {
                meal = new Drinks();
                meal.Dishtype = 7;
            }

            if (selected == 2)
            {
                meal = new Burger();
                meal.Dishtype = 2;
            }

            if (selected == 3)
            {
                meal = new Pizza();
                meal.Dishtype = 3;
            }

            if (selected == 4)
            {
                meal = new Pasta();
                meal.Dishtype = 4;
            }

            if (selected == 5)
            {
                meal = new Steak();
                meal.Dishtype = 5;
            }

            if (selected == 6)
            {
                meal = new Meals();
                meal.Dishtype = 1;
            }

            if (selected == 7)
            {
                meal = new Meals();
                meal.Dishtype = 6;
            }
            
            if (meal.GetType() == typeof(Drinks))
            {
                Console.WriteLine("Is the drink alcoholic?");
                Console.WriteLine("Answer Y or N");
                bool alcoholic = GetYesOrNoFromUser();
                if (alcoholic != true)
                {
                    (meal as Drinks).isAlcoholic = true;
                }
                else
                {
                    (meal as Drinks).isAlcoholic = false;
                }

                Console.WriteLine("Give the drink amount in centilitres:");
                int amount = int.Parse(Console.ReadLine());
                (meal as Drinks).Amount = amount;
                

            }
            
            if (meal.GetType() == typeof(Burger))
            {

                Console.WriteLine("Give patty info:");
                (meal as Burger).Patty = StringChecker(Console.ReadLine());

                Console.WriteLine("Give cheese info:");
                (meal as Burger).Cheese = StringChecker(Console.ReadLine());

                Console.WriteLine("Give bun info:");
                (meal as Burger).Bun = StringChecker(Console.ReadLine());
                

            }
            
            if (meal.GetType() == typeof(Pizza))
            {
                Console.WriteLine("Give the pizza toppings:");
                (meal as Pizza).Toppings = StringChecker(Console.ReadLine());
                

            }

            if (meal.GetType() == typeof(Pasta))
            {
                Console.WriteLine("Give the pastatype:");
                (meal as Pasta).Pastatype = StringChecker(Console.ReadLine());

                Console.WriteLine("Give the meat:");
                (meal as Pasta).Meat = StringChecker(Console.ReadLine());

                (meal as Pasta).Sauce = GetSauce();

            }

            if (meal.GetType() == typeof(Steak))
            {
                Console.WriteLine("Give the type of the steak:");
                (meal as Steak).Steaktype = StringChecker(Console.ReadLine());

                Console.WriteLine("Give the side with the steak:");
                (meal as Steak).Side = StringChecker(Console.ReadLine());

                (meal as Steak).Sauce = GetSauce();
            }



            Console.WriteLine("Do you want to add a description to the dish?");
            Console.WriteLine("Answer Y or N");
            bool answer = GetYesOrNoFromUser();
            if (answer == true)
            {
                meal.Description = GetMealDescription();
            }
            

            GetAllergenFromUSer(meal);

            float price = GetPrice();
            meal.Name = name;
            meal.Price = price;
            
            return meal;
        }

        public Meals GetAllergenFromUSer(Meals meal)
        {
            Console.WriteLine("Is the food allergen free?");
            Console.WriteLine("Answer Y or N");
            var allergens = Enum.GetNames(typeof(Allergen.AllergenType));
            foreach (string str in allergens)
            {
                if (str != "negative")
                {
                    Console.WriteLine();
                    Console.WriteLine(str);
                    bool answer = GetYesOrNoFromUser();
                    if (answer == true)
                    {
                        Allergen.AllergenType an = (Allergen.AllergenType)Enum.Parse(typeof(Allergen.AllergenType), str);
                        meal.AddAllergen(an);
                    }
                }

            }
            return meal;
        }

        static public bool GetYesOrNoFromUser(string question = null)
        {
            bool success = false;
            while (!success)
            {
                if (question != null) Console.WriteLine(question);
                ConsoleKey consoleKey = Console.ReadKey(true).Key;
                if (consoleKey == ConsoleKey.D1 || consoleKey == ConsoleKey.Y || consoleKey == ConsoleKey.K)
                {
                    return true;
                }
                if (consoleKey == ConsoleKey.D2 || consoleKey == ConsoleKey.N || consoleKey == ConsoleKey.E)
                {
                    return false;
                }
            }
            return false;
        }

        public string GetCategoryName()
        {
            string categoryName = null;
            while (categoryName == null)
            {
                Console.WriteLine();
                Console.WriteLine("Give the name of the new category:");
                categoryName = StringChecker(Console.ReadLine());
            }
            return categoryName;
        }

        public string GetMenuName()
        {
            string menuName = null;
            while (menuName == null)
            {
                Console.WriteLine();
                Console.WriteLine("Give the name of the menu:");
                menuName = StringChecker(Console.ReadLine());
            }
            return menuName;
        }

        public string GetRestaurantName()
        {
            string RestaurantName = null;
            while (RestaurantName == null)
            {
                Console.WriteLine();
                Console.WriteLine("Give the name of the new restaurant: ");
                RestaurantName = StringChecker(Console.ReadLine());
            }
            return RestaurantName;
        }

        public string GetRestaurantAddress()
        {
            string RestaurantAddress = null;
            while (RestaurantAddress == null)
            {
                Console.WriteLine();
                Console.WriteLine("Give the address of the new restaurant: ");
                RestaurantAddress = StringChecker(Console.ReadLine());
            }
            return RestaurantAddress;
        }

        public string GetMealName()
        {
            string _name = null;
            while (_name == null)
            {
                Console.WriteLine();
                Console.WriteLine("Give the name of the Dish: ");
                _name = StringChecker(Console.ReadLine());
            }
            return _name;
        }

        public string GetMealDescription()
        {
            string _description = null;
            while (_description == null)
            {
                Console.WriteLine();
                Console.WriteLine("Give the description of the dish: ");
                _description = StringChecker(Console.ReadLine());
            }
            return _description;
        }

        public string GetSauce()
        {
            string _sauce = null;
            while (_sauce == null)
            {
                Console.WriteLine();
                Console.WriteLine("Give the sauce info");
                _sauce = StringChecker(Console.ReadLine());
            }
            return _sauce;
        }

        public float GetPrice()
        {
            Console.WriteLine();
            Console.WriteLine("Give the price: ");
            var _price = float.Parse(Console.ReadLine());
            return _price;
        }


        public string StringChecker(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            return str.Trim();
        }




    }
}
