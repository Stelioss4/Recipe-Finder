namespace Recipe_Finder
{
    public class ProductInformation
    {
        private List<Ingredient>? _availableProduct;

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

		private double _price;

		public double Price
        {
			get { return _price; }
			set { _price = value; }
		}

		private PaymentMethod? _paymentMethods;

		public PaymentMethod PaymentMethods
        {
			get { return _paymentMethods; }
			set { _paymentMethods = value; }
		}

		private List<string>? _marketLinks;

		public List<string> MarketLinks
        {
            get { return _marketLinks; }
			set { _marketLinks = value; }
		}
	}
}
