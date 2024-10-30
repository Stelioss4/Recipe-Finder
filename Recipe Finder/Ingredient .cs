namespace Recipe_Finder
{
    public class Ingredient
    {
          private int _iD;

  public int Id
  {
      get { return _iD; }
      set { _iD = value; }
  }
        private string _ingredientsName;

        public string IngredientsName
        {
            get { return _ingredientsName; }
            set { _ingredientsName = value; }
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
        /// <summary>
        /// Represents the amount of carbohydrates in grams per 100g
        /// </summary>
        private decimal _carbohydrate;   
        public decimal Carbohydrate  
        {
            get { return _carbohydrate; }
            set { _carbohydrate = value; }
        }
        /// <summary>
        /// Represents the amount of protein in grams grams per 100g
        /// </summary>
        private decimal _protein;   

        public decimal Protein      
        {
            get { return _protein; }
            set { _protein = value; }
        }

        private string _amount; 

        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private string _amountUnit;

        public string AmountUnit
        {
            get { return _amountUnit; }
            set { _amountUnit = value; }
        }


        private double _unit;

        public double Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        private string _productLink;
        public string ProductLink
        {
            get { return _productLink; }
            set { _productLink = value; }
        }
    }
}
