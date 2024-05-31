﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Recipe_Finder
{
    public class UsersProfile
    {
        private string _firstname;
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        private string _lastName;
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }


        private string? _email;
        [Required(ErrorMessage = "Email is required.")]

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _password;
        [Required(ErrorMessage = "Password is required.")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private Address _address = new Address();

        public Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private List<Recipe> _favoriteRecipes = new List<Recipe>();

        public List<Recipe> FavoriteRecipes
        {
            get { return _favoriteRecipes; }
            set { _favoriteRecipes = value; }
        }

        private PaymentMethod _paymentMethods;

        public PaymentMethod PaymentMethods
        {
            get { return _paymentMethods; }
            set { _paymentMethods = value; }
        }

        private List<Recipe> _weeklyPlan;

        public List<Recipe> WeeklyPlan
        {
            get { return _weeklyPlan; }
            set { _weeklyPlan = value; }
        }
    }
}

