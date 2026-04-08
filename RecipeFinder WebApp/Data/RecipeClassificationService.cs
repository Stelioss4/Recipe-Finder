using System.Net;
using System.Text.RegularExpressions;

namespace RecipeFinder_WebApp.Data
{
    public class RecipeClassificationService
    {
        private static readonly Dictionary<string, string[]> RecipeRootPatterns = new(StringComparer.OrdinalIgnoreCase)
        {
            ["burger"] = new[]
            {
                "burger",
                "cheeseburger",
                "cheese burger",
                "hamburger",
                "lachsburger",
                "fisch burger",
                "fish burger",
                "chicken burger",
                "crispy chicken burger",
                "halloumi burger",
                "bacon burger",
                "bbq burger",
                "american burger",
                "smash burger",
                "smashed burger",
                "pulled pork burger",
                "pulled turkey burger",
                "burgerkreation"
            },

            ["banana bread"] = new[]
            {
                "banana bread",
                "bananenbrot"
            },

            ["pastitsio"] = new[]
            {
                "pastitsio",
                "pastitio",
                "pastizio",
                "pasticcio",
                "pasticio",
                "griechischer nudelauflauf",
                "griechische lasagne",
                "griechischer makkaroniauflauf",
                "griechischer maccheroni auflauf",
                "griechischer maccheroniauflauf",
                "griechischer nudelauflauf mit hackfleisch",
                "griechischer nudelauflauf mit hackfleisch und bechamelsauce"
            },

            ["ratatouille"] = new[]
            {
                "ratatouille"
            },

            ["arme ritter"] = new[]
            {
                "arme ritter",
                "armer ritter",
                "rostige ritter"
            },

            ["pizza"] = new[]
            {
                "pizza",
                "calzone",
                "pizzabroetchen",
                "pizzabrötchen",
                "pizzaschnecken",
                "pizzateig",
                "pizzarolle",
                "pizza rolle",
                "pizza suppe",
                "pizzasuppe",
                "pan pizza",
                "deep dish pizza",
                "fladenbrotpizza",
                "blitz pizza",
                "wrap pizza"
            },

            ["spaghetti"] = new[]
            {
                "spaghetti"
            },

            ["schupfnudel"] = new[]
            {
                "schupfnudel",
                "schupfnudeln"
            },

            ["gnocchi"] = new[]
            {
                "gnocchi"
            },

            ["lasagne"] = new[]
            {
                "lasagne",
                "lasagna"
            },

            ["bread and butter pudding"] = new[]
            {
                "bread and butter pudding",
                "brot und butter pudding"
            },

            ["garlic bread"] = new[]
            {
                "garlic bread",
                "knoblauchbrot",
                "selbstgebackenes knoblauchbrot"
            },

            ["naan"] = new[]
            {
                "naan",
                "naanbrot",
                "naan brot",
                "naan bread"
            },

            ["soda bread"] = new[]
            {
                "irish soda bread"
            },

            ["cloud bread"] = new[]
            {
                "cloud bread"
            },

            ["bread"] = new[]
            {
                "bread",
                "brot"
            }
        };

        private static readonly HashSet<string> IgnoredWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "american",
            "australian",
            "italienischer",
            "vegan",
            "vegetarisch",
            "vegetarische",
            "vegetarisches",
            "klassisch",
            "klassische",
            "klassischer",
            "classic",
            "creamy",
            "low",
            "carb",
            "high",
            "protein",
            "mit",
            "und",
            "and",
            "a",
            "la",
            "de",
            "deluxe",
            "style",
            "art",
            "einfach",
            "einfaches",
            "einfacher",
            "best",
            "beste",
            "bestes",
            "schnell",
            "schnelle",
            "schneller",
            "saftig",
            "saftige",
            "saftiges",
            "warm",
            "warmer",
            "frisch",
            "frische",
            "frischer",
            "hausgemacht",
            "homemade",
            "original",
            "echte",
            "echt",
            "wie",
            "vom",
            "von",
            "fuer",
            "für",
            "nach",
            "auf",
            "im",
            "in",
            "aus",
            "der",
            "die",
            "das",
            "des",
            "den",
            "dem"
        };

        public string GetRecipeRoot(string recipeName)
        {
            var normalizedName = NormalizeRecipeName(recipeName);

            if (string.IsNullOrWhiteSpace(normalizedName))
                return string.Empty;

            foreach (var family in RecipeRootPatterns)
            {
                foreach (var pattern in family.Value)
                {
                    var normalizedPattern = NormalizeRecipeName(pattern);

                    if (normalizedName.Contains(normalizedPattern))
                    {
                        Console.WriteLine($"MATCH: '{recipeName}' -> '{normalizedName}' matched family '{family.Key}' with pattern '{normalizedPattern}'");
                        return family.Key;
                    }
                }
            }

            return BuildFallbackRoot(normalizedName);
        }

        public bool HaveSameRoot(string recipeName1, string recipeName2)
        {
            var root1 = GetRecipeRoot(recipeName1);
            var root2 = GetRecipeRoot(recipeName2);

            if (string.IsNullOrWhiteSpace(root1) || string.IsNullOrWhiteSpace(root2))
                return false;

            return string.Equals(root1, root2, StringComparison.OrdinalIgnoreCase);
        }

        public string NormalizeRecipeName(string recipeName)
        {
            if (string.IsNullOrWhiteSpace(recipeName))
                return string.Empty;

            var normalized = recipeName.Trim().ToLowerInvariant();

            normalized = WebUtility.HtmlDecode(normalized);

            normalized = normalized
                .Replace("á", "a")
                .Replace("à", "a")
                .Replace("ä", "ae")
                .Replace("ö", "oe")
                .Replace("ü", "ue")
                .Replace("ß", "ss");

            normalized = Regex.Replace(normalized, @"[-_/]", " ");
            normalized = Regex.Replace(normalized, @"[^\w\s]", " ");
            normalized = Regex.Replace(normalized, @"\s+", " ");

            return normalized.Trim();
        }

        private string BuildFallbackRoot(string normalizedName)
        {
            var words = normalizedName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(word => !IgnoredWords.Contains(word))
                .ToList();

            if (words.Count == 0)
                return string.Empty;

            if (words.Count >= 2)
                return $"{words[0]} {words[1]}";

            return words[0];
        }
    }
}
