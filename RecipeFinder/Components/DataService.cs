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
