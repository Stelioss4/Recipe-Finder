namespace Recipe_Finder
{
    public class PaymentMethod
    {
		private int _iD;

		public int Id
		{
			get { return _iD; }
			set { _iD = value; }
		}

		private string _accountemail;

		public string AccountEmail
		{
			get { return _accountemail; }
			set { _accountemail = value; }
		}

		private string _accountPassword;

		public string AccountPassword
        {
			get { return _accountPassword; }
			set { _accountPassword = value; }
		}


	}
}
