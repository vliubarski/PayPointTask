using CustomerChargeNotification.BackgroundServices;
using CustomerChargeNotification.Domain;
using CustomerChargeNotification.Models;
using CustomerChargeNotification.PDFGeneration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerChargeNotificationTests.Services;

[TestFixture]
public class ChargeNotificationServiceTests
{
    private ChargeNotificationService _service;
    private Mock<IChargeNotificationProcessor> _mockProcessor;
    private Mock<IPdfService> _mockPdfService;
    private Mock<ILogger<ChargeNotificationService>> _mockLogger;

    [SetUp]
    public void SetUp()
    {
        _mockProcessor = new Mock<IChargeNotificationProcessor>();
        _mockPdfService = new Mock<IPdfService>(); 
        _mockLogger = new Mock<ILogger<ChargeNotificationService>>();

        _service = new ChargeNotificationService(
            _mockProcessor.Object,
            _mockPdfService.Object,
            _mockLogger.Object);
    }

    [Test]
    public async Task GenerateChargeNotifications_ShouldGeneratePdfs_WhenNotificationsExist()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);
        var notifications = new List<ChargeNotification>
        {
            new ChargeNotification{CustomerId = 1, CustomerName = "Alice",
                Charges = new List<Charge>{ new() { Date = DateTime.Now, Game = "G1", Cost = 10 }}},
            new ChargeNotification{CustomerId = 2, CustomerName = "Bob",
                Charges = new List<Charge>{ new() { Date = DateTime.Now, Game = "G2", Cost = 12 }}}
        };

        _mockProcessor.Setup(p => p.GetChargeNotificationsForDate(date)).Returns(notifications);

        // Act
        await _service.GenerateChargeNotifications(date);

        // Assert
        _mockPdfService.Verify(s => s.SaveToFileAsync(It.IsAny<ChargeNotification>()), Times.Exactly(2));

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Successfully generated PDF for CustomerId 1."),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Successfully generated PDF for CustomerId 2."),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void GenerateChargeNotifications_ShouldLogErrorAndRethrow_WhenPdfGenerationFails()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);
        var notifications = new List<ChargeNotification>
        {
            new ChargeNotification { CustomerId = 1, CustomerName = "Alice",
            Charges = new List<Charge>{ new() { Date = DateTime.Now, Game = "G1", Cost = 10 }}}
        };

        _mockProcessor.Setup(p => p.GetChargeNotificationsForDate(date)).Returns(notifications);
        _mockPdfService.Setup(s => s.SaveToFileAsync(It.IsAny<ChargeNotification>())).Throws(new Exception("PDF generation error"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _service.GenerateChargeNotifications(date));
        Assert.That(ex.Message, Is.EqualTo("PDF generation error"));

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Failed to generate PDF for CustomerId 1"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public async Task GenerateChargeNotifications_ShouldNotAttemptToGeneratePdfs_WhenNoNotificationsExist()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);
        _mockProcessor.Setup(p => p.GetChargeNotificationsForDate(date)).Returns(new List<ChargeNotification>());

        // Act
        await _service.GenerateChargeNotifications(date);

        // Assert
        _mockPdfService.Verify(s => s.SaveToFileAsync(It.IsAny<ChargeNotification>()), Times.Never); // No PDF should be generated

        _mockLogger.Verify(logger => logger.Log(
         It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
         It.Is<EventId>(eventId => eventId.Id == 0),
         It.Is<It.IsAnyType>((@object, @type) => true),
         It.IsAny<Exception>(),
         It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(2));
    }
}
