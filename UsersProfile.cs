namespace Recipe_Finder
{
    public class UsersProfile
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

        private PaymentMethods _paymentMethods;

        public PaymentMethods PaymentMethods
        {
            get { return _paymentMethods; }
            set { _paymentMethods = value; }
        }

        private int _ratings;

        public int Ratings
        {
            get { return _ratings; }
            set { _ratings = value; }
        }

        private string _reviews;

        public string Reviews
        {
            get { return _reviews; }
            set { _reviews = value; }
        }

        private List<Recipe> _weeklyPlan;

        public List<Recipe> WeeklyPlan
        {
            get { return _weeklyPlan; }
            set { _weeklyPlan = value; }
        }

    }
}

