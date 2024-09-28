using System.ComponentModel.DataAnnotations;

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

        public virtual List<Recipe> FavoriteRecipes
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

        private bool _rememberMe;
        [Display(Name = "Remember me?")]
        public bool RememberMe
        {
            get { return  _rememberMe; }
            set {  _rememberMe = value; }
        }

      
        private PaymentMethod? _paymentMethods;

        public virtual PaymentMethod? PaymentMethods
        {
            get { return _paymentMethods; }
            set { _paymentMethods = value; }
        }
    }
}

