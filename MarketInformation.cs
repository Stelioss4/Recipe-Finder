namespace Recipe_Finder
{
    public class MarketInformation
    {
		private List<Ingredient> _availableProduct;

		public List<Ingredient> AvailableProduct
        {
			get { return _availableProduct; }
			set { _availableProduct = value; }
		}

		private int _numberOfServings;

		public int NumberOfServings
        {
			get { return _numberOfServings; }
			set { _numberOfServings = value; }
		}

		private double _priceOfIngredients;

		public double PriceOfIngredients
        {
			get { return _priceOfIngredients; }
			set { _priceOfIngredients = value; }
		}

		private PaymentMethods _paymentMethods;

		public PaymentMethods PaymentMethods
        {
			get { return _paymentMethods; }
			set { _paymentMethods = value; }
		}


	}
}
