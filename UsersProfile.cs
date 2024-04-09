using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class UsersProfile
    {
        public class UsersProfil
        {
            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _email;

            public string Email
            {
                get { return _email; }
                set { _email = value; }
            }

            private string _password;

            public string Password
            {
                get { return _password; }
                set { _password = value; }
            }
        }
    }
}
