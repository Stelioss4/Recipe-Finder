using RecipeFinder_WebApp.Data;
using Microsoft.AspNetCore.Identity;

namespace RecipeFinder_WebApp.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<RecipeFinder_WebAppUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<RecipeFinder_WebAppUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
