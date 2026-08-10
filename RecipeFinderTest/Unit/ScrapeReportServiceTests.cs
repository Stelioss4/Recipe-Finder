using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Recipe_Finder;
using RecipeFinder_WebApp.Data;
using Xunit;

namespace RecipeFinderTest.Unit
{
    public class ScrapeReportServiceTests
    {
        [Fact]
        public async Task SendScrapeReportEmailAsync_SendsFailureEmail_WhenScrapingFails()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();

            var service = new ScrapeReportService(emailSenderMock.Object);

            var scrapeResults = new List<ScrapeCheckResult>
            {
                new ScrapeCheckResult
                {
                    IsSuccess = false
                }
            };

            // Act
            await service.SendScrapeReportEmailAsync(scrapeResults);

            // Assert
            emailSenderMock.Verify(
                sender => sender.SendEmailAsync(
                    It.IsAny<string>(),
                     "Scraping Issues Detected",
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task SendScrapeReportEmailAsync_SendsSuccessEmail_WhenAllScrapingSucceeds()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();

            var service = new ScrapeReportService(emailSenderMock.Object);

            var scrapeResults = new List<ScrapeCheckResult>
    {
        new ScrapeCheckResult
        {
            IsSuccess = true
        },
        new ScrapeCheckResult
        {
            IsSuccess = true
        }
    };

            // Act
            await service.SendScrapeReportEmailAsync(scrapeResults);

            // Assert
            emailSenderMock.Verify(
                sender => sender.SendEmailAsync(
                    It.IsAny<string>(),
                    "✅ Scraping Successful",
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task SendScrapeReportEmailAsync_SendsFailureEmail_WhenAnyScrapingFails()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();

            var service = new ScrapeReportService(emailSenderMock.Object);

            var scrapeResults = new List<ScrapeCheckResult>
    {
        new ScrapeCheckResult
        {
            IsSuccess = true
        },
        new ScrapeCheckResult
        {
            IsSuccess = true
        },
        new ScrapeCheckResult
        {
            IsSuccess = false
        }
    };

            // Act
            await service.SendScrapeReportEmailAsync(scrapeResults);

            // Assert
            emailSenderMock.Verify(
                sender => sender.SendEmailAsync(
                    It.IsAny<string>(),
                    "Scraping Issues Detected",
                    It.IsAny<string>()),
                Times.Once);

            emailSenderMock.Verify(
                sender => sender.SendEmailAsync(
                    It.IsAny<string>(),
                    "✅ Scraping Successful",
                    It.IsAny<string>()),
                Times.Never);
        }
    }
}