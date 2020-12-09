using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoLibrary
{
    public class Allergen
    {
        public string allergenName;

        public AllergenType allergenType = AllergenType.negative;
        
        public enum AllergenType
        {
            lowLactose=1,
            lactoseFree,
            milkFree,
            glutenFree,
            negative

        }

        static public Dictionary<AllergenType, string> allergenChars = new Dictionary<AllergenType, string>()
        {  {AllergenType.lowLactose, "LL"},{AllergenType.lactoseFree, "L"},{AllergenType.glutenFree, "G"},{AllergenType.milkFree, "M"} };



        public Allergen()
        {

        }




    }
}
