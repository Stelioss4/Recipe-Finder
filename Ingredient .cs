﻿namespace Recipe_Finder
{
    public class Ingredient
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private decimal _calories;
        public decimal Calories
        {
            get { return _calories; }
            set { _calories = value; }
        }

        private decimal _fat;
        /// <summary>
        /// Represents the amount of fat in grams per grams per 100g
        /// </summary>
        public decimal Fat
        {
            get { return _fat; }
            set { _fat = value; }
        }

        private decimal _carbohydrate;   // Represents the amount of carbohydrates in grams per 100g

        public decimal Carbohydrate  
        {
            get { return _carbohydrate; }
            set { _carbohydrate = value; }
        }

        private decimal _protein;   // Represents the amount of protein in grams grams per 100g

        public decimal Protein      
        {
            get { return _protein; }
            set { _protein = value; }
        }

        private IngredientAmount _amount; 

        public IngredientAmount Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }


    }
}
