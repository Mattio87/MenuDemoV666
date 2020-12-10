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



            mlmngr.SuperMainMenu();

        }
    }
}
