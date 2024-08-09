using System.ComponentModel.DataAnnotations;

namespace Recipe_Finder
{
    public class User
    {
        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private List<Recipe> _favoriteRecipes;

        public List<Recipe> FavoriteRecipes
        {
            get { return _favoriteRecipes; }
            set { _favoriteRecipes = value; }
        }


        private string _firstname;
      
        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        private string _lastName;
      
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }


        private string? _email;
        [Required]
        [EmailAddress]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _password;
        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _confirmPassword;
        [DataType(DataType.Password)]
        public string ConfirmPassword 
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; }
        }

        private bool _rememberMe;
        [Display(Name = "Remember me?")]
        public bool RememberMe
        {
            get { return  _rememberMe; }
            set {  _rememberMe = value; }
        }



        private Address _address = new Address();

        public Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        //private PaymentMethod _paymentMethods;

        //public PaymentMethod PaymentMethods
        //{
        //    get { return _paymentMethods; }
        //    set { _paymentMethods = value; }
        //}

        private List<Recipe> _weeklyPlan;

        public List<Recipe> WeeklyPlan
        {
            get { return _weeklyPlan; }
            set { _weeklyPlan = value; }
        }


    }
}

