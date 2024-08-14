using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipe_Finder;

namespace RecipeFinder_WebApp.Data
{
    public class RecipesController : Controller
    {
        private readonly UserService _userService;
        private readonly UserManager<RecipeFinder_WebAppUser> _userManager;

        public RecipesController(UserService userService, UserManager<RecipeFinder_WebAppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> AddToFavorites(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _userService.AddFavoriteRecipeAsync(user.Id, id);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> MyFavorites()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var favorites = await _userService.GetFavoritesAsync(user.Id);
                return View(favorites);
            }
            return RedirectToAction("Index");
        }
    }

}
