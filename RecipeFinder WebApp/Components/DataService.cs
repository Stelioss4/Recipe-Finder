using Recipe_Finder;
using System.IO;
using System.Xml.Serialization;


namespace RecipeFinder_WebApp.Components
{
    public class DataService
    {
        private User? user { get; set; } = new User();

        public Address Address { get; set; } = new Address();

        public List<User> users { get; set; } = new List<User>();

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        private readonly IHttpClientFactory _clientFactory;

        public DataService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public DataService()
        {
            Recipes = TestData.RecipeList();
            users = TestData.UserProfil();
        }
        public bool UserExists(string email)
        {
            return users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public static void SaveUsersToXmlFile(List<User> users, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream file = File.Create(Constants.XML_USER_PATH))
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

        public static List<User> LoadUsersFromXmlFile(string filePath)
        {
            List<User> users = new List<User>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

            try
            {
                if (File.Exists(Constants.XML_USER_PATH))
                {
                    using (FileStream file = File.OpenRead(Constants.XML_USER_PATH))
                    {
                        var loadedUsers = serializer.Deserialize(file) as List<User>;
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
    }
}
