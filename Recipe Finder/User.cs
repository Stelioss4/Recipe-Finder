using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Finder
{
    public class User
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private List<Recipe> _favoriteRecipes = new List<Recipe>();

        public List<Recipe> FavoriteRecipes
        {
            get { return _favoriteRecipes; }
            set { _favoriteRecipes = value; }
        }


        private string? _name;
      
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string? _email;

        [EmailAddress]
        public string? Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private bool _rememberMe;
        [Display(Name = "Remember me?")]
        public bool RememberMe
        {
            get { return  _rememberMe; }
            set {  _rememberMe = value; }
        }

      
        private PaymentMethod? _paymentMethods;

        public PaymentMethod? PaymentMethods
        {
            get { return _paymentMethods; }
            set { _paymentMethods = value; }
        }

        private List<Recipe>? _weeklyPlan;

        public List<Recipe>? WeeklyPlan
        {
            get { return _weeklyPlan; }
            set { _weeklyPlan = value; }
        }

    }
}

