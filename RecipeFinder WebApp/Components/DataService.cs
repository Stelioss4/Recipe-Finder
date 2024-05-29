﻿using Recipe_Finder;
using RecipeFinder_WebApp.Components;
using System.Xml.Serialization;

namespace RecipeFinder_WebApp.Components
{
    public class DataService
    {

        const string PATH = "Users.xml";

        private List<User> _users = new List<User>();
        private List<Recipe> searchResults;

        public List<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public List<Recipe> Recipies { get; set; }

        public DataService()
        {
            Recipies = TestData.RecipeList();
        }

        public static List<Recipe> SearchRecipes(Recipe recipe)
        {
            List<Recipe> allRecipes = new List<Recipe>();


            return allRecipes.Where(r =>
                (string.IsNullOrEmpty(recipe.RecipeName) || r.RecipeName.Contains(recipe.RecipeName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(recipe.Videolink) || r.Videolink.Contains(recipe.Videolink, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(recipe.CuisineType) || r.CuisineType.Contains(recipe.CuisineType, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public static void SaveUser(List<UsersProfile> users)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<UsersProfile>));
            using (FileStream file = File.Create(PATH))
            {
                serializer.Serialize(file, users);
            }
        }

        public static List<UsersProfile> LoadUser()
        {
            List<UsersProfile>? users = new List<UsersProfile>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<UsersProfile>));

            if (File.Exists(PATH))
            {
                using (FileStream file = File.OpenRead(PATH))
                {
                    users = serializer.Deserialize(file) as List<UsersProfile>;
                }
            }
            return users;
        }


        public static void UserExist()
        {
            bool userExist = true;
            if (userExist)
            {
                LoadUser();
            }
        }

        public static void SearchRecipe()
        {

        }

    }
}
