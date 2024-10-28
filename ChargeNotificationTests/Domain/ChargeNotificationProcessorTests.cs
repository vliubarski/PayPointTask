using CustomerChargeNotification.DAL;
using CustomerChargeNotification.Domain;
using CustomerChargeNotification.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerChargeNotificationTests.Domain;

[TestFixture]
public class ChargeNotificationProcessorTests
{
    private ChargeNotificationProcessor _processor;
    private Mock<ICustomerRepository> _mockCustomerRepository;
    private Mock<ICustomerGameChargeRepository> _mockChargeRepository;
    private Mock<ILogger<ChargeNotificationProcessor>> _mockLogger;

    [SetUp]
    public void SetUp()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockChargeRepository = new Mock<ICustomerGameChargeRepository>();
        _mockLogger = new Mock<ILogger<ChargeNotificationProcessor>>();

        _processor = new ChargeNotificationProcessor(
            _mockCustomerRepository.Object,
            _mockChargeRepository.Object,
            _mockLogger.Object);
    }

    [Test]
    public void GetChargeNotificationsForDate_ShouldReturnNotifications_WhenChargesExist()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);
        var testMsg = $"Getting charge notifications for date: {date:MM/dd/yyyy}";
        var testMsg2 = $"Generated 2 charge notifications for date: {date:MM/dd/yyyy}";

        var charges = new List<CustomerGameCharge>
        {
            new() { ChargeDate = date, CustomerId = 1, GameName = "Game A", TotalCost = 10, Description = "D1" },
            new () { ChargeDate = date, CustomerId = 1, GameName = "Game B", TotalCost = 15, Description = "D2"},
            new () { ChargeDate = date, CustomerId = 2, GameName = "Game C", TotalCost = 20, Description = "D3"}
        };

        var customers = new List<Customer>
        {
            new () { Id = 1, Name = "Alice" },
            new (){ Id = 2, Name = "Bob" }
        };

        _mockChargeRepository.Setup(repo => repo.GetChargesForDate(date)).Returns(charges);
        _mockCustomerRepository.Setup(repo => repo.GetCustomersByIds(new List<int> { 1, 2 })).Returns(customers);

        // Act
        var notifications = _processor.GetChargeNotificationsForDate(date).ToList();

        // Assert
        Assert.IsNotNull(notifications);
        Assert.That(notifications.Count, Is.EqualTo(2));

        var firstNotification = notifications.First();
        Assert.That(firstNotification.CustomerId, Is.EqualTo(1));
        Assert.That(firstNotification.CustomerName, Is.EqualTo("Alice"));
        Assert.That(firstNotification.Total, Is.EqualTo(25));               // TotalCost of Game A and Game B
        Assert.That(firstNotification.Charges.Count(), Is.EqualTo(2));      // Two charges for customer 1

        var secondNotification = notifications.Last();
        Assert.That(secondNotification.CustomerId, Is.EqualTo(2));
        Assert.That(secondNotification.CustomerName, Is.EqualTo("Bob"));
        Assert.That(secondNotification.Total, Is.EqualTo(20));              // TotalCost of Game C
        Assert.That(secondNotification.Charges.Count(), Is.EqualTo(1));     // One charge for customer 2

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString().Contains(testMsg)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString().Contains(testMsg2)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void GetChargeNotificationsForDate_ShouldReturnEmpty_WhenNoChargesExist()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);
        var charges = new List<CustomerGameCharge>();
        var testMsg = $"Getting charge notifications for date: {date:MM/dd/yyyy}";
        var testMsg2 = $"Generated 0 charge notifications for date: {date:MM/dd/yyyy}";

        _mockChargeRepository.Setup(repo => repo.GetChargesForDate(date)).Returns(charges);
        _mockCustomerRepository.Setup(repo => repo.GetCustomersByIds(It.IsAny<IEnumerable<int>>())).Returns(new List<Customer>());

        // Act
        var notifications = _processor.GetChargeNotificationsForDate(date).ToList();

        // Assert
        Assert.IsNotNull(notifications);
        Assert.That(notifications.Count, Is.EqualTo(0));

        // Verify log information
        //_mockLogger.Verify(l => l.LogInformation("Getting charge notifications for date: {Date}", date), Times.Once);
        //_mockLogger.Verify(l => l.LogInformation("Generated {Count} charge notifications for date: {Date}", 0, date), Times.Once);
        _mockLogger.Verify(logger => logger.Log(
        It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
        It.Is<EventId>(eventId => eventId.Id == 0),
        It.Is<It.IsAnyType>((@object, @type) => @object.ToString().Contains(testMsg)),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString().Contains(testMsg2)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void GetChargeNotificationsForDate_ShouldThrow_WhenCustomerNameNotFound()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);
        var testMsg = $"Getting charge notifications for date: {date:MM/dd/yyyy}";
        var charges = new List<CustomerGameCharge>
        {
            new () { ChargeDate = date, CustomerId = 1, GameName = "Game A", TotalCost = 10,Description = "D1" }
        };

        var customers = new List<Customer>(); // No customers found

        _mockChargeRepository.Setup(repo => repo.GetChargesForDate(date)).Returns(charges);
        _mockCustomerRepository.Setup(repo => repo.GetCustomersByIds(new List<int> { 1 })).Returns(customers);

        // Act & Assert
        var ex = Assert.Throws<ValueProviderException>(() => _processor.GetChargeNotificationsForDate(date));
        Assert.That(ex.Message, Is.EqualTo("Customer with id 1 has no name"));

        _mockLogger.Verify(logger => logger.Log(
        It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
        It.Is<EventId>(eventId => eventId.Id == 0),
        It.Is<It.IsAnyType>((@object, @type) => @object.ToString().Contains(testMsg)),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}

