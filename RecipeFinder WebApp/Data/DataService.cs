using Microsoft.EntityFrameworkCore;
using Recipe_Finder;
using System.Xml.Serialization;


namespace RecipeFinder_WebApp.Data
{
    public class DataService
    {
        public User? user { get; set; } = new User();

        public Address Address { get; set; } = new Address();

        public List<User> users { get; set; } = new List<User>();

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        private readonly IHttpClientFactory _clientFactory;

        private readonly RecipeFinder_WebAppContext _context;

        public DataService(IHttpClientFactory clientFactory, RecipeFinder_WebAppContext context)
        {
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Recipes = LoadRecipesFromXmlFile(Constants.XML_CACHE_PATH);
        }

        public async Task AddToUserFavAsync(User user, Recipe recipe)
        {
            if (user.FavoriteRecipes == null)
            {
                user.FavoriteRecipes = new List<Recipe>();
            }

            user.FavoriteRecipes.Add(recipe);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
        public static List<Recipe> GetUserFavorites(User user)
        {
            return user.FavoriteRecipes ?? new List<Recipe>();
        }

        public static User GetUser(User user)
        {
            return user?? new User();
        }

        private void Init()
        {
            //load recipied
        }
        public bool UserExists(string email)
        {
            return users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public static void SaveRecipesToXmlFile(List<Recipe> recipes, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, recipes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving recipes to XML file: {ex.Message}");
            }
        }

        public static List<Recipe> LoadRecipesFromXmlFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        var recipes = (List<Recipe>)serializer.Deserialize(reader);
                        return recipes ?? new List<Recipe>(); // Return an empty list if deserialization returns null
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading recipes from XML file: {ex.Message}");
            }
            return new List<Recipe>();
        }

        public List<Recipe> GetCachedRecipes(List<string> searchTerms, string source)
        {
            var normalizedSearchTerms = searchTerms.Select(term => term.Trim().ToLowerInvariant()).ToList();
            source = source.Trim().ToLowerInvariant();

            var existingRecipes = Recipes
                .Where(r => r.RecipeName != null &&
                            r.SourceDomain != null &&
                            r.SourceDomain.Trim().ToLowerInvariant().Equals(source, StringComparison.OrdinalIgnoreCase) &&
                            r.SearchTerms != null)
                .Where(r => normalizedSearchTerms.Any(term => r.RecipeName.Contains(term, StringComparison.OrdinalIgnoreCase)) &&
                            normalizedSearchTerms.Any(term => r.SearchTerms.Any(st => st.Trim().ToLowerInvariant().Equals(term, StringComparison.OrdinalIgnoreCase))))
                .ToList();

            return existingRecipes;
        }
    }
}
