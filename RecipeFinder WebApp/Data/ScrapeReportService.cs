using Microsoft.AspNetCore.Identity.UI.Services;
using Recipe_Finder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinder_WebApp.Data
{
    public class ScrapeReportService
{

        private readonly IEmailSender _emailSender;
        public ScrapeReportService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task SendScrapeReportEmailAsync(List<ScrapeCheckResult> scrapeResults)
        {
            var failed = scrapeResults.Where(r => !r.IsSuccess).ToList();
            var success = scrapeResults.Where(r => r.IsSuccess).ToList();

            if (failed.Any())
            {
                var message = "Scraping completed but some nodes failed:\n\n";

                foreach (var fail in failed)
                {
                    message += $"- Recipe: {fail.RecipeName} ({fail.RecipeUrl}), Missing: {fail.CheckedNode}\n";
                }

                await _emailSender.SendEmailAsync(Constants.ADMIN_EMAIL, "Scraping Issues Detected", message);
                return;
            }

          
            await _emailSender.SendEmailAsync(
                Constants.ADMIN_EMAIL,
                "✅ Scraping Successful",
                "All recipes scraped successfully with no issues detected.");
        }
    }
}

