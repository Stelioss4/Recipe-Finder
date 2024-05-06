using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata;
using System.Xml.Serialization;

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

       const string PATH = "Users.xml";

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
            List<UsersProfile> users = new List<UsersProfile>();
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
    }
}

