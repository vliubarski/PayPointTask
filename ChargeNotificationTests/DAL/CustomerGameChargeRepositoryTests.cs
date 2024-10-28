using CustomerChargeNotification.DAL;
using CustomerChargeNotification.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ChargeNotificationTests.DAL;

public class CustomerGameChargeRepositoryTests
{
    private CustomerGameChargeContext _context;
    private Mock<ILogger<CustomerGameChargeRepository>> _mockLogger;
    private CustomerGameChargeRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<CustomerGameChargeContext>()
                     .UseInMemoryDatabase(databaseName: "TestDb")
                     .Options; 
        _mockLogger = new Mock<ILogger<CustomerGameChargeRepository>>();
        _context = new CustomerGameChargeContext(options);
        _repository = new CustomerGameChargeRepository(_context, _mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }


    [Test]
    public void GetChargesForDate_ShouldReturnCharges_WhenChargesExist()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);
        var testMsg = $"Retrieving charges for date: {date:MM/dd/yyyy}";
        var testMsg2 = $"Retrieved 2 charges for date: {date:MM/dd/yyyy}";

        var charges = new List<CustomerGameCharge>
            {
                new() { ChargeDate = date, CustomerId = 1, GameName = "Game A", TotalCost = 10, Description = "D1" },
                new () { ChargeDate = date, CustomerId = 2, GameName = "Game B", TotalCost = 15, Description = "D2" }
            };
        _context.CustomerGameCharge.AddRange(charges);
        _context.SaveChanges();

        // Act
        var result = _repository.GetChargesForDate(date);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString().Contains(testMsg)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _mockLogger.Verify(l => l.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString().Contains(testMsg2)),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);
    }

    [Test]
    public void GetChargesForDate_ShouldReturnEmpty_WhenNoChargesExist()
    {
        // Arrange
        var date = new DateTime(2021, 5, 20);

        // Act
        var result = _repository.GetChargesForDate(date);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(0));
    }
}