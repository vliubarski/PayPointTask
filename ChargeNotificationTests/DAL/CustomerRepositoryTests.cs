using CustomerChargeNotification.DAL;
using CustomerChargeNotification.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerChargeNotificationTests.DAL;

[TestFixture]
public class CustomerRepositoryTests
{
    private CustomerRepository _repository;
    private CustomerContext _context;
    private Mock<ILogger<CustomerRepository>> _mockLogger;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<CustomerContext>()
            .UseInMemoryDatabase(databaseName: "testDb")
            .Options;

        _context = new CustomerContext(options);
        _mockLogger = new Mock<ILogger<CustomerRepository>>();

        _repository = new CustomerRepository(_context, _mockLogger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void GetCustomersByIds_ShouldReturnCustomers_WhenIdsProvided()
    {
        // Arrange
        var ids = new List<int> { 1, 2 };
        var testMsg = "Retrieved 2 customers for IDs: 1, 2";

        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Alice" },
            new Customer { Id = 2, Name = "Bob" },
            new Customer { Id = 3, Name = "Charlie" } // Not included in the IDs
        };

        _context.Customer.AddRange(customers);
        _context.SaveChanges();

        // Act
        var result = _repository.GetCustomersByIds(ids);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Select(c => c.Id), Is.EquivalentTo(ids));

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == testMsg),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void GetCustomersByIds_ShouldReturnEmptyAndLogWarning_WhenNoIdsProvided()
    {
        // Arrange
        var ids = Enumerable.Empty<int>();
        var testMsg = "No customer IDs provided to retrieve.";

        // Act
        var result = _repository.GetCustomersByIds(ids);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.Empty);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == testMsg),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void GetCustomersByIds_ShouldReturnEmptyAndLogWarning_WhenIdsIsNull()
    {
        // Arrange
        var testMsg = "No customer IDs provided to retrieve.";

        // Act
        var result = _repository.GetCustomersByIds(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.Empty);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == testMsg),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
