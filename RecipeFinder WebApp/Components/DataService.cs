﻿using Microsoft.VisualBasic;
using Recipe_Finder;
using System.Xml.Serialization;

namespace RecipeFinder_WebApp.Components
{
    public class DataService
    {

        const string PATH = "Users.xml";

        private List<User> _users = new List<User>();

        public List<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public UsersProfile? UserProfile { get; set; } = new UsersProfile();
        
        public Address Address { get; set; } = new Address();
        
        public List<UsersProfile> users { get; set; } = new List<UsersProfile>();

        public List<UsersProfile> usersProfiles { get; set; } 

        public List<Recipe> Recipies { get; set; }

        public DataService()
        {
            Recipies = TestData.RecipeList();
            usersProfiles = TestData.UserProfil();
        }

        public static List<Recipe> SearchRecipes(Recipe recipe)
        {
            List<Recipe> allRecipes = new List<Recipe>();
            allRecipes.Add(recipe);
            return allRecipes;

            //return allRecipes.Where(r =>
            //    (string.IsNullOrEmpty(recipe.RecipeName) || r.RecipeName.Contains(recipe.RecipeName, StringComparison.OrdinalIgnoreCase)) &&
            //    (string.IsNullOrEmpty(recipe.Videolink) || r.Videolink.Contains(recipe.Videolink, StringComparison.OrdinalIgnoreCase)) &&
            //    (string.IsNullOrEmpty(recipe.CuisineType) || r.CuisineType.Contains(recipe.CuisineType, StringComparison.OrdinalIgnoreCase))
            //).ToList();
        }

        //public static void SaveUser(List<UsersProfile> users)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(List<UsersProfile>));
        //    using (FileStream file = File.Create(PATH))
        //    {
        //        serializer.Serialize(file, users);
        //    }
        //}

        //public static List<UsersProfile> LoadUser()
        //{
        //    List<UsersProfile>? users = new List<UsersProfile>();
        //    XmlSerializer serializer = new XmlSerializer(typeof(List<UsersProfile>));

        //    if (File.Exists(PATH))
        //    {
        //        using (FileStream file = File.OpenRead(PATH))
        //        {
        //            users = serializer.Deserialize(file) as List<UsersProfile>;
        //        }
        //    }
        //    return users;
        //}


        public static void SaveUser(List<UsersProfile> users)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<UsersProfile>));
                using (FileStream file = File.Create(PATH))
                {
                    serializer.Serialize(file, users);
                }
                Console.WriteLine("Users saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving users: {ex.Message}");
            }
        }

        public static List<UsersProfile> LoadUser()
        {
            List<UsersProfile> users = new List<UsersProfile>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<UsersProfile>));

            try
            {
                if (File.Exists(PATH))
                {
                    using (FileStream file = File.OpenRead(PATH))
                    {
                        var loadedUsers = serializer.Deserialize(file) as List<UsersProfile>;
                        if (loadedUsers != null)
                        {
                            users = loadedUsers;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading users: {ex.Message}");
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


    }
}
