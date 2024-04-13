using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class Ingredient
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _calories;

        public int Calories
        {
            get { return _calories; }
            set { _calories = value; }
        }

        private int _fat;   // Represents the amount of fat in grams per serving.

        public int Fat
        {
            get { return _fat; }
            set { _fat = value; }
        }

        private int _carbohydrate;   // Represents the amount of carbohydrates in grams per serving.

        public int Carbohydrate  
        {
            get { return _carbohydrate; }
            set { _carbohydrate = value; }
        }

        private int _protein;   // Represents the amount of protein in grams per serving.

        public int Protein      
        {
            get { return _protein; }
            set { _protein = value; }
        }

        private double _amount;    // Represents the amount of the ingredient, typically measured in units such as grams, milliliters, etc.

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }
}
