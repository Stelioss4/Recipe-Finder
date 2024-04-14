using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class PaymentMethods
    {
		private string _acountemail;

		public string AcountEmail
		{
			get { return _acountemail; }
			set { _acountemail = value; }
		}

		private string _acountpasword;

		public string Acountpasword
        {
			get { return _acountpasword; }
			set { _acountpasword = value; }
		}


	}
}
