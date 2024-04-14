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

        private decimal _calories;

        public decimal Calories
        {
            get { return _calories; }
            set { _calories = value; }
        }

        private decimal _fat;   // Represents the amount of fat in grams per serving.

        public decimal Fat
        {
            get { return _fat; }
            set { _fat = value; }
        }

        private decimal _carbohydrate;   // Represents the amount of carbohydrates in grams per serving.

        public decimal Carbohydrate  
        {
            get { return _carbohydrate; }
            set { _carbohydrate = value; }
        }

        private decimal _protein;   // Represents the amount of protein in grams per serving.

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
