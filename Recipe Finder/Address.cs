using System.ComponentModel.DataAnnotations;

namespace Recipe_Finder
{
    public class Address
    {
		private string _streetsname;
        [Required(ErrorMessage = "Street name is required.")]

        public string StreetsName
		{
			get { return _streetsname; }
			set { _streetsname = value; }
		}

		private string _housenumber;
        [Required(ErrorMessage = "House number is required.")]

        public string Housenumber
		{
			get { return _housenumber; }
			set { _housenumber = value; }
		}

		private string _city;
        [Required(ErrorMessage = "City is required.")]

        public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		private string _postalCode;
        [Required(ErrorMessage = "Postal code is required.")]

        public string PostalCode
		{
			get { return _postalCode; }
			set { _postalCode = value; }
		}

	}
}
