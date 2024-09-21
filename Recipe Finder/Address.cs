using System.ComponentModel.DataAnnotations;

namespace Recipe_Finder
{
    public class Address
    {
		private int _iD;

		public int ID
		{
			get { return _iD; }
			set { _iD = value; }
		}

		private string? _streetsName;

        public string? StreetsName
		{
			get { return _streetsName; }
			set { _streetsName = value; }
		}

		private string _housenumber;


        public string Housenumber
		{
			get { return _housenumber; }
			set { _housenumber = value; }
		}

		private string _city;


        public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		private string _postalCode;

        public string PostalCode
		{
			get { return _postalCode; }
			set { _postalCode = value; }
		}

	}
}
