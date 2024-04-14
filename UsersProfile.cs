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

            private Address _address;

            public Address Address
            {
                get { return _address; }
                set { _address = value; }
            }

            private List<Recipe> _favoriteRecipes;

            public List<Recipe> FavoriteRecipes
            {
                get { return _favoriteRecipes; }
                set { _favoriteRecipes = value; }
            }

        }
    }
}
