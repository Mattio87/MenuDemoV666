using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MenuDemoLibrary
{
    public class DataManager : DataAccess
    {
        private List<Restaurant> _allRestaurantsList = new List<Restaurant>();

        private List<Meals> _allMealsList = new List<Meals>();


        public static Object GetTest()
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.QueryFirst<Object>("SELECT [Name], [Description], [Price] FROM dbo.Dishes WHERE [Price]=11");
                return toReturn;
            }
        }

        public static List<Meals> GetAllDishes()
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<Meals>("SELECT * FROM dbo.Dishes").ToList();
                return toReturn;
            }
        }

        public static List<Restaurant> GetAllRestaurantsListFromDB()
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<Restaurant>("SELECT Name as RestaurantName , Address as RestaurantAddress, Id FROM dbo.Restaurants").ToList();
                return toReturn;
            }

        }

        public static int InsertMealToDB(Meals meal)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            int mealId;
            using (IDbConnection connection = new SqlConnection(str))
            {
                
                
                mealId = connection.QuerySingle<int>("INSERT dbo.Dishes (Name, Description, Sauce, Price, Dishtype) OUTPUT INSERTED.ID " +
                    "VALUES (@name, @description, @sauce, @price, @dishtype)", new { name = meal.Name, description = meal.Description, sauce=meal.Sauce, price = meal.Price, dishtype = meal.Dishtype });
                
                if (meal.GetType()== typeof(Burger))
                {
                    mealId = connection.QuerySingle<int>("UPDATE dbo.Dishes SET Bun=@bun, Patty=@patty, Cheese=@cheese OUTPUT INSERTED.ID WHERE Id=@id", new {bun=(meal as Burger).Bun, patty = (meal as Burger).Patty, cheese = (meal as Burger).Cheese, id=mealId });
                }

                else if (meal.GetType()== typeof(Pizza))
                {
                    mealId = connection.QuerySingle<int>("UPDATE dbo.Dishes SET Toppings=@toppings OUTPUT INSERTED.ID WHERE Id=@id)", new {toppings = (meal as Pizza).Toppings, id=mealId });
                }

                else if (meal.GetType()== typeof(Pasta))
                {
                    mealId = connection.QuerySingle<int>("UPDATE dbo.Dishes SET Pastatype=@pastatype, Meat=@meat OUTPUT INSERTED.ID WHERE Id=@id", new { meat=(meal as Pasta).Meat, pastatype=(meal as Pasta).Pastatype, id=mealId });
                }

                else if (meal.GetType()== typeof(Steak))
                {
                    mealId = connection.QuerySingle<int>("UPDATE dbo.Dishes SET Steaktype=@steaktype, Side=@side  OUTPUT INSERTED.ID WHERE Id=@id", new { steaktype=(meal as Steak).Steaktype, side=(meal as Steak).Side, id=mealId });
                }

                else if(meal.GetType()== typeof(Drinks))
                {
                    mealId = connection.QuerySingle<int>("UPDATE dbo.Dishes SET Amount=@amount, isAlcoholic=@isalcoholic OUTPUT INSERTED.ID WHERE Id=@id", new { amount=(meal as Drinks).Amount, isalcoholic=(meal as Drinks).isAlcoholic, id=mealId });
                }

            }

            if(mealId>0 && meal.GetAllergens().Count > 0)
            {
                using (IDbConnection connection = new SqlConnection(str))
                {
                    foreach(var allergen in meal.GetAllergens())
                    {
                        connection.Execute("INSERT dbo.AllergensInDishes (AllergenId, DishId) VALUES (@Allergenid, @Dishid)", new { Allergenid = (int)allergen, Dishid = mealId });
                    }
                }
            }
            return mealId;

        }
        public static Meals GetMealWithID(int MealID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.QueryFirst<Meals>("SELECT * FROM dbo.Dishes WHERE id=@id", new { id = MealID });
                Meals meal = Meals.GetMealClassWithDishType(toReturn.Dishtype, toReturn);


                if (meal.GetType() == typeof(Burger))
                {
                    dynamic burgerData = connection.QuerySingle("SELECT * FROM dbo.Dishes WHERE Id=@dishid", new { dishid = meal.Id });
                    (meal as Burger).Bun = burgerData.Bun;
                    (meal as Burger).Cheese = burgerData.Cheese;
                    (meal as Burger).Patty = burgerData.Patty;
                }

                if (meal.GetType() == typeof(Pizza))
                {
                    dynamic pizzaData = connection.QuerySingle("SELECT * FROM dbo.Dishes WHERE Id=@dishid", new { dishid = meal.Id });
                    (meal as Pizza).Toppings = pizzaData.Toppings;
                }

                if (meal.GetType() == typeof(Pasta))
                {
                    dynamic pastaData = connection.QuerySingle("SELECT * FROM dbo.Dishes WHERE Id=@dishid", new { dishid = meal.Id });
                    (meal as Pasta).Meat = pastaData.Meat;
                    (meal as Pasta).Pastatype = pastaData.Pastatype;
                }

                if (meal.GetType() == typeof(Steak))
                {
                    dynamic steakData = connection.QuerySingle("SELECT * FROM dbo.Dishes WHERE Id=@dishid", new { dishid = meal.Id });
                    (meal as Steak).Steaktype = steakData.Steaktype;
                    (meal as Steak).Side = steakData.Side;
                }

                if (meal.GetType() == typeof(Drinks))
                {
                    dynamic drinkData = connection.QuerySingle("SELECT * FROM dbo.Dishes WHERE Id=@dishid", new { dishid = meal.Id });
                    (meal as Drinks).Amount = (int)drinkData.Amount;
                    (meal as Drinks).isAlcoholic = drinkData.isAlcoholic;
                }

                var allergens = connection.Query<Allergen.AllergenType>("SELECT AllergenId FROM AllergensInDishes WHERE DishId=@id", new { id = MealID }).ToArray();
                foreach(var allergenAsInt in allergens)
                {
                    meal.AddAllergen(allergenAsInt);
                }

                return meal;
            }
        }

        public static int[] GetMealidFromCategoryDB(int categoryID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<int>("SELECT DishId as Id FROM dbo.CategoryDishes WHERE CategoryId=@id", new { id = categoryID }).ToArray();
                return toReturn;
            }
        }

        public static List<Meals> GetListOfMealsInDBCategory(int categoryID)
        {
            int[] mealIds = GetMealidFromCategoryDB(categoryID);
            List<Meals> meals = new List<Meals>();
            foreach (int id in mealIds)
            {
                meals.Add(GetMealWithID(id));
            }
            return meals;
        }

       public static Category GetCategoryWithIDFromDB(int categoryID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.QueryFirst<Category>("SELECT Name as categoryName, Description as CategoryDescription, Id FROM dbo.Categories WHERE id=@id", new { id = categoryID });
                return toReturn;
            }
       }

        public static RestaurantMenu GetMenuWithIDFromDB(int menuID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.QueryFirst<RestaurantMenu>("SELECT Name as MenuName ,Description as MenuDescription, Id FROM dbo.Menus WHERE id=@id", new { id = menuID });
                return toReturn;
            }
        }

        
        public static List<RestaurantMenu> GetListofMenusWithRestaurantIDFromDB(int restaurantID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<RestaurantMenu>("SELECT m.Name as MenuName, m.Description as MenuDescription, m.Id FROM dbo.Menus as m INNER JOIN dbo.Restaurants as r ON m.RestaurantID=r.Id WHERE m.RestaurantID=@id", new { id = restaurantID }).ToList();
                return toReturn;
            }
        }
        public static Restaurant GetRestaurantWithIDFromDB(int restaurantID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.QueryFirst<Restaurant>("SELECT Name as RestaurantName , Address as RestaurantAddress, Id FROM dbo.Restaurants WHERE id=@id", new { id = restaurantID });
                return toReturn;
            }
        }

        public static List<RestaurantMenu> GetMenusFromDB()
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<RestaurantMenu>("SELECT Name as menuName, Description as menuDescription, Id FROM dbo.Menus").ToList();
                return toReturn;
            }
        }

        
        public static List<Category> GetListOfCategoriesInDBMenus(int menuID)
        {
            int[] categoryIds = GetCategoryIdsFromMenuDB(menuID);
            List<Category> categories = new List<Category>();
            foreach (int id in categoryIds)
            {
                categories.Add(GetCategoryWithIDFromDB(id));
            }
            return categories;
        }

        public static int[] GetCategoryIdsFromMenuDB(int menuID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<int>("SELECT c.Id FROM dbo.Categories as c INNER JOIN dbo.Menus as m ON c.MenuId=m.Id WHERE m.Id=@id", new { id = menuID }).ToArray();
                return toReturn;
            }

        }

        
        public static Restaurant GetAllRestaurantDataFromDB(int restaurantID)
        {
            Restaurant restaurant = GetRestaurantWithIDFromDB(restaurantID);
            restaurant.restaurants = GetListofMenusWithRestaurantIDFromDB(restaurantID);

            foreach (RestaurantMenu restaurantmenu in restaurant.restaurants)
            {
                restaurantmenu.menus = GetListOfCategoriesInDBMenus(restaurantmenu.Id);

                foreach (Category category in restaurantmenu.menus)
                {
                    category.meals = GetListOfMealsInDBCategory(category.Id);
                    
                }
            }
            return restaurant;
        }

        
        public void AddRestaurantToAllRestaurantsList(Restaurant restaurant)
        {
            _allRestaurantsList.Add(restaurant);
        }


        public void AddMealToAllMealsList(Meals meal)
        {
            _allMealsList.Add(meal);
        }

        public List<Restaurant> GetRestaurantsFromAllRestaurantsList()
        {
            return GetAllRestaurantsListFromDB();
        }

        public List<Meals> GetMealsFromAllMealsList()
        {
            return GetAllDishes();

        }


        
        
        public DataManager()
        {
            //AddTestData();
        }
        
        /*
        public void AddTestData()
        {

            Drinks drink1 = new Drinks("Water", false, 50, 0F);
            Drinks drink2 = new Drinks("Coke", false, 50, 2.5F);
            Drinks drink3 = new Drinks("Fanta", false, 50, 2.5F);
            Drinks drink4 = new Drinks("Sprite", false, 50, 2.5F);
            Drinks drink5 = new Drinks("Milk", false, 30, 1.5F);
            Drinks drink6 = new Drinks("Beer", true, 40, 6.5F);
            Drinks drink7 = new Drinks("Wine", true, 75, 12.5F);
            this.AddMealToAllMealsList(drink1);
            this.AddMealToAllMealsList(drink2);
            this.AddMealToAllMealsList(drink3);
            this.AddMealToAllMealsList(drink4);
            this.AddMealToAllMealsList(drink5);
            this.AddMealToAllMealsList(drink6);
            this.AddMealToAllMealsList(drink7);

            Burger burger1 = new Burger();
            burger1.Name = "Ciabatta mozzarella burger";
            burger1.Description = "Delicious mozzarella burger with beef patty";
            burger1.Bun = "Ciabatta Roll";
            burger1.Cheese = "Delicious Mozzarella";
            burger1.Patty = "150g Beef";
            burger1.Price = 11F;
            burger1.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            this.AddMealToAllMealsList(burger1);

            Burger burger2 = new Burger();
            burger2.Name = "Sesame seed bacon and cheddar burger";
            burger2.Description = "Bacon burger with cheddar and incredible sesame seed bun";
            burger2.Bun = "Sesame seed";
            burger2.Cheese = "Tasty cheddar";
            burger2.Patty = "150g Beef";
            burger2.Price = 11F;
            burger2.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(burger2);

            Burger burger3 = new Burger();
            burger3.Name = "Jalapeno cheddar burger";
            burger3.Description = "HOT! Hottest burger inhouse. Jalapeno with smoky habanero sauce and cheddar cheese";
            burger3.Bun = "Full grain wheat";
            burger3.Cheese = "Tasty cheddar";
            burger3.Patty = "150g Beef";
            burger3.Price = 12F;
            burger3.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(burger3);

            Burger burger4 = new Burger();
            burger4.Name = "Basic bitch burger";
            burger4.Description = "For the ones who want the OG OG burger, nothing special, just meat, cheese and bun, with a great taste nevertheless";
            burger4.Bun = "Full grain wheat";
            burger4.Cheese = "Tasty cheddar";
            burger4.Patty = "150g Beef";
            burger4.Price = 10F;
            burger4.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(burger4);

            Pizza pizza1 = new Pizza();
            pizza1.Name = "Double Pepperoni";
            pizza1.Description = "Basic, yet amazing double pepperoni pizza";
            pizza1.Toppings = "Mozzarella cheese, tomato sauce, double pepperoni";
            pizza1.Price = 9F;
            pizza1.Allergentypes.Add(Allergen.AllergenType.milkFree);
            this.AddMealToAllMealsList(pizza1);

            Pizza pizza2 = new Pizza();
            pizza2.Name = "Margherita";
            pizza2.Description = "Margherita for the OG pizza lovers";
            pizza2.Toppings = "Mozzarella cheese and tomato sauce";
            pizza2.Price = 8F;
            pizza2.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            pizza2.Allergentypes.Add(Allergen.AllergenType.lowLactose);
            this.AddMealToAllMealsList(pizza2);

            Pizza pizza3 = new Pizza();
            pizza3.Name = "Vienna Verde";
            pizza3.Description = "Tasty tuna with pepperoni and blue cheese";
            pizza3.Toppings = "Mozzarella cheese, tomato sauce, tuna, blue cheese and pepperoni";
            pizza3.Price = 11F;
            pizza3.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(pizza3);

            Pizza pizza4 = new Pizza();
            pizza4.Name = "Garlic lovers special";
            pizza4.Description = "You LOVE garlic? We gotchu fam";
            pizza4.Toppings = "Mozzarella cheese, tomato sauce, roasted garlic with fresh basil";
            pizza4.Price = 11F;
            pizza4.Allergentypes.Add(Allergen.AllergenType.lowLactose);
            this.AddMealToAllMealsList(pizza4);

            Pasta pasta1 = new Pasta();
            pasta1.Name = "Al Pollo";
            pasta1.Description = "Fettuccine pasta with breast from a young duck, fried veggies in tomato and cream sauce";
            pasta1.Pastatype = "Fettuccine";
            pasta1.Sauce = "Tomato and cream sauce";
            pasta1.Meat = "Young duck breast";
            pasta1.Price = 18F;
            pasta1.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            this.AddMealToAllMealsList(pasta1);

            Pasta pasta2 = new Pasta();
            pasta2.Name = "Moms spaghetti";
            pasta2.Description = "Ever gaved for that sweet old mom made spaghetti, we got you right here.";
            pasta2.Pastatype = "Spaghetti";
            pasta2.Sauce = "Tomato sauce with fresh onions and garlic";
            pasta2.Meat = "Ground beef";
            pasta2.Price = 16F;
            pasta2.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(pasta2);

            Pasta pasta3 = new Pasta();
            pasta3.Name = "Turkey Sausage Pasta";
            pasta3.Description = "Short tube pasta with turkey sausage, fresh herbs and garlic";
            pasta3.Pastatype = "Short tube pasta";
            pasta3.Sauce = "Tomato sauce with cream";
            pasta3.Meat = "Ground turkey sausages";
            pasta3.Price = 18F;
            pasta3.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            this.AddMealToAllMealsList(pasta3);

            Pasta pasta4 = new Pasta();
            pasta4.Name = "Spaghetti alla carbonara";
            pasta4.Description = "Spaghetti covered with creamy cheese and bacon bits";
            pasta4.Pastatype = "Spaghetti";
            pasta4.Sauce = "Tomato sauce with cream and cheese";
            pasta4.Meat = "Bacon bits";
            pasta4.Price = 17F;
            pasta4.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            pasta4.Allergentypes.Add(Allergen.AllergenType.lowLactose);
            this.AddMealToAllMealsList(pasta4);

            Steak steak1 = new Steak();
            steak1.Name = "Beef steak with pepper";
            steak1.Description = "200g Beef steak with pepper with large french fries and parsnip pyre";
            steak1.Steaktype = "Beef";
            steak1.Side = "French Fries";
            steak1.Sauce = "Parsnip pyre";
            steak1.Price = 21F;
            steak1.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            steak1.Allergentypes.Add(Allergen.AllergenType.lactoseFree);
            this.AddMealToAllMealsList(steak1);

            Steak steak2 = new Steak();
            steak2.Name = "Crispy duck breast";
            steak2.Description = "Fried duck breast in cherry-red wine sauce with mushrooms, onions and bacon";
            steak2.Steaktype = "Duck breast";
            steak2.Side = "Mushrooms, onion and bacon";
            steak2.Sauce = "Cherry-red wine sauce";
            steak2.Price = 21F;
            steak2.Allergentypes.Add(Allergen.AllergenType.milkFree);
            this.AddMealToAllMealsList(steak2);

            Steak steak3 = new Steak();
            steak3.Name = "Black Angus tenderloin";
            steak3.Description = "Black Angus is grain fed and free foraging cattle from Australias Clare Valley.";
            steak3.Steaktype = "Black Angus tenderloin";
            steak3.Side = "Dijon, onions and fresh herbs";
            steak3.Sauce = "Red wine sauce";
            steak3.Price = 30F;
            steak3.Allergentypes.Add(Allergen.AllergenType.milkFree);
            steak3.Allergentypes.Add(Allergen.AllergenType.lowLactose);
            this.AddMealToAllMealsList(steak3);

            Steak steak4 = new Steak();
            steak4.Name = "Finnish reindeer sirloin";
            steak4.Description = "Finnish reindeer sirloin might the best tasting reindeer, guilty pleasure for us all";
            steak4.Steaktype = "Finnish reindeer sirloin";
            steak4.Side = "Mashed poatoes";
            steak4.Sauce = "Creamy pepper sauce";
            steak4.Price = 35F;
            steak4.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            this.AddMealToAllMealsList(steak4);

            Meals starter1 = new Meals();
            starter1.Name = "Crab Cake";
            starter1.Description = "Crab cake with choron and Dijon mustard sauces";
            starter1.Price = 6F;
            starter1.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            this.AddMealToAllMealsList(starter1);

            Meals starter2 = new Meals();
            starter2.Name = "Forest mushroom soup";
            starter2.Description = "Self picked mushrooms and maltcrumble";
            starter2.Price = 6F;
            starter2.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(starter2);

            Meals starter3 = new Meals();
            starter3.Name = "Iceberg wedge";
            starter3.Description = "Iceberg lettuce with stilton cheese, crispy bacon and blue cheese sauce";
            starter3.Price = 8F;
            starter3.Allergentypes.Add(Allergen.AllergenType.lactoseFree);
            this.AddMealToAllMealsList(starter3);

            Meals starter4 = new Meals();
            starter4.Name = "Caesar salad";
            starter4.Description = "Romaine salad with caesar sauce, bread crutons and parmesan cheese";
            starter4.Price = 6F;
            starter4.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(starter4);

            Meals dessert1 = new Meals();
            dessert1.Name = "Ice cream";
            dessert1.Description = "Vanilla ice cream with caramel dressing";
            dessert1.Price = 5F;
            dessert1.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            this.AddMealToAllMealsList(dessert1);

            Meals dessert2 = new Meals();
            dessert2.Name = "Chocolate cake";
            dessert2.Description = "Chocolate cake with ice cream and strawberry sauce";
            dessert2.Price = 7F;
            dessert2.Allergentypes.Add(Allergen.AllergenType.negative);
            this.AddMealToAllMealsList(dessert2);

            Meals dessert3 = new Meals();
            dessert3.Name = "Yogurt pannacotta";
            dessert3.Description = "Cloudberry compote";
            dessert3.Price = 5F;
            dessert3.Allergentypes.Add(Allergen.AllergenType.glutenFree);
            this.AddMealToAllMealsList(dessert3);

            Meals dessert4 = new Meals();
            dessert4.Name = "Pancakes";
            dessert4.Description = "Good old pancakes with strawberry jam and whipped cream";
            dessert4.Price = 5F;
            dessert4.Allergentypes.Add(Allergen.AllergenType.lowLactose);
            this.AddMealToAllMealsList(dessert4);


            Category starters1 = new Category();
            starters1.categoryName = "Starter";
            starters1.meals.Add(starter1);
            starters1.meals.Add(starter2);
            starters1.meals.Add(starter3);
            starters1.meals.Add(starter4);

            Category burgers1 = new Category();
            burgers1.categoryName = "Burger";
            burgers1.meals.Add(burger1);
            burgers1.meals.Add(burger2);
            burgers1.meals.Add(burger3);
            burgers1.meals.Add(burger4);

            Category pizzas1 = new Category();
            pizzas1.categoryName = "Pizza";
            pizzas1.meals.Add(pizza1);
            pizzas1.meals.Add(pizza2);
            pizzas1.meals.Add(pizza3);
            pizzas1.meals.Add(pizza4);

            Category pastas1 = new Category();
            pastas1.categoryName = "Pasta";
            pastas1.meals.Add(pasta1);
            pastas1.meals.Add(pasta2);
            pastas1.meals.Add(pasta3);
            pastas1.meals.Add(pasta4);

            Category steaks1 = new Category();
            steaks1.categoryName = "Steak";
            steaks1.meals.Add(steak1);
            steaks1.meals.Add(steak2);
            steaks1.meals.Add(steak3);
            steaks1.meals.Add(steak4);

            Category desserts1 = new Category();
            desserts1.categoryName = "Dessert";
            desserts1.meals.Add(dessert1);
            desserts1.meals.Add(dessert2);
            desserts1.meals.Add(dessert3);
            desserts1.meals.Add(dessert4);

            Category drinks1 = new Category();
            drinks1.categoryName = "Drinks";
            drinks1.meals.Add(drink1);
            drinks1.meals.Add(drink2);
            drinks1.meals.Add(drink3);
            drinks1.meals.Add(drink4);
            drinks1.meals.Add(drink5);
            drinks1.meals.Add(drink6);
            drinks1.meals.Add(drink7);


            RestaurantMenu restaurantmenu1 = new RestaurantMenu();
            restaurantmenu1.MenuName = "New Commonwealth menu 1";
            restaurantmenu1.menus.Add(starters1);
            restaurantmenu1.menus.Add(burgers1);
            restaurantmenu1.menus.Add(pizzas1);
            restaurantmenu1.menus.Add(pastas1);
            restaurantmenu1.menus.Add(steaks1);
            restaurantmenu1.menus.Add(desserts1);
            restaurantmenu1.menus.Add(drinks1);

            Restaurant restaurant1 = new Restaurant();
            restaurant1.RestaurantName = "Restaurant New commonwealth";
            restaurant1.RestaurantAddress = "Köyhänkuja 1";
            restaurant1.restaurants.Add(restaurantmenu1);

            this.AddRestaurantToAllRestaurantsList(restaurant1);


        }

        */

        



    }
}
