namespace Recipe_Finder
{
    public class Address
    {
		private string _streetsname;

		public string StreetsName
		{
			get { return _streetsname; }
			set { _streetsname = value; }
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
