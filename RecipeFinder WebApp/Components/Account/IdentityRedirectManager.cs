using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace RecipeFinder_WebApp.Components.Account
{
    internal sealed class IdentityRedirectManager
    {
        private readonly NavigationManager _navigationManager;

        public IdentityRedirectManager(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public const string StatusCookieName = "Identity.StatusMessage";

        private static readonly CookieBuilder StatusCookieBuilder = new()
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            IsEssential = true,
            MaxAge = TimeSpan.FromSeconds(5),
        };

        public void RedirectTo(string? uri)
        {
            uri ??= "";

            // Prevent open redirects.
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = _navigationManager.ToBaseRelativePath(uri);
            }

            // Navigate to the URI directly
            _navigationManager.NavigateTo(uri);
        }

        public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
        {
            var uriWithoutQuery = _navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
            var newUri = _navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
            RedirectTo(newUri);
        }

        public void RedirectToWithStatus(string uri, string message, HttpContext context)
        {
            context.Response.Cookies.Append(StatusCookieName, message, StatusCookieBuilder.Build(context));
            RedirectTo(uri);
        }

        private string CurrentPath => _navigationManager.ToAbsoluteUri(_navigationManager.Uri).GetLeftPart(UriPartial.Path);

        public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

        public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
            => RedirectToWithStatus(CurrentPath, message, context);
    }
}
