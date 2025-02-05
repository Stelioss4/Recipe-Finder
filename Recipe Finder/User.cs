﻿using System.ComponentModel.DataAnnotations;

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

        private string _name;
      
        public string Name
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

        private List<Recipe> _weeklyPlan; 
        public virtual List<Recipe> WeeklyPlan 
        {
            get { return _weeklyPlan; }
            set { _weeklyPlan = value; }
        }

        private DateTime? _lastWeeklyPlanDate;

        public DateTime? LastWeeklyPlanDate
        {
            get { return _lastWeeklyPlanDate;  }
            set { _lastWeeklyPlanDate = value; }
        }

        private List<Ingredient> _shoppingList;
        public virtual List<Ingredient> ShoppingList 
        {
            get { return _shoppingList; }
            set { _shoppingList = value; }
        }
    }
}

