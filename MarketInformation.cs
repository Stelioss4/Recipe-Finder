using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	}
}
