using Microsoft.AspNetCore.Identity;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataService(IHttpClientFactory clientFactory, ApplicationDbContext context)
        {
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Recipes = LoadRecipesFromXmlFile(Constants.XML_CACHE_PATH);
        }

        /// <summary>
        /// Adds a recipe to the user's favorites and saves the changes to the database.
        /// </summary>
        public async Task AddToUserFavAsync(string userId, Recipe recipe)
        {
            try
            {
                if (_context == null)
                {
                    throw new NullReferenceException("Database context is null.");
                }

                var appUser = await _userManager.Users
                    .Include(user => user.User)
                    .ThenInclude(u => u.FavoriteRecipes)
                    .FirstOrDefaultAsync(user => user.Id == userId);

                if (appUser == null)
                {
                    throw new NullReferenceException($"User with ID {userId} not found.");
                }

                if (recipe == null)
                {
                    throw new NullReferenceException("Recipe is null.");
                }

                if (appUser.User.FavoriteRecipes.Any(r => r.RecipeId == recipe.RecipeId))
                {
                    throw new ArgumentException("Recipe is already in the user's favorites.", nameof(recipe));
                }

                appUser.User.FavoriteRecipes.Add(recipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Gets the user's favorite recipes. If none exist, returns an empty list.
        /// </summary>
        public static List<Recipe> GetUserFavorites(User user)
        {
            return user.FavoriteRecipes ?? new List<Recipe>();
        }

        /// <summary>
        /// Retrieves a user by their user ID from the database.
        /// Throws an exception if the user is not found.
        /// </summary>
        public async Task<User> GetUserByIdAsync(string userId)
        {
            var dbUser = await _context.Users
                .Include(u => u.FavoriteRecipes) // Include FavoriteRecipes navigation property
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (dbUser == null)
            {
                throw new Exception($"User with ID {userId} not found.");
            }

            // Return the user entity with their favorite recipes mapped
            return new User
            {
                UserId = dbUser.UserId,
                Name = dbUser.Name,
                Email = dbUser.Email,
                FavoriteRecipes = dbUser.FavoriteRecipes.Select(fr => new Recipe
                {
                    UserId = fr.UserId,
                    RecipeName = fr.RecipeName,
                    Url = fr.Url,
                    Image = fr.Image,
                }).ToList()
            };
        }

        /// <summary>
        /// Checks if a user exists by their email.
        /// </summary>
        public bool UserExists(string email)
        {
            return users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Saves the list of recipes to an XML file.
        /// </summary>
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

        /// <summary>
        /// Loads the list of recipes from an XML file.
        /// </summary>
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

        /// <summary>
        /// Retrieves cached recipes based on search terms and source.
        /// </summary>
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
