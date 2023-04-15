using NUnit.Framework;

namespace Aggregator.Tests;

[TestFixture]
public class PriceEngineTests
{
    private PriceEngine _priceEngine;

    [SetUp]
    public void Setup()
    {
        _priceEngine = new PriceEngine();
    }

    [Test]
    public void GetPrice_GivenDefaultRequest_ShouldReturnQuoteFromZxcvbnm()
    {
        //Arrange
        var request = new PriceRequest()
        {
            RiskData = new RiskData()
            {
                DOB = DateTime.Parse("1980-01-01"),
                FirstName = "John",
                LastName = "Smith",
                Make = "Cool New Phone",
                Value = 500
            }
        };

        //Act
        var actual = _priceEngine.GetPrice(request);

        //Assert
        var expected = new PriceResponse
        {
            Insurer = "zxcvbnm",
            Price = 92.67m,
            Tax = 11.1204m,
        };

        Assert.Multiple(() =>
        {
            Assert.That(actual.Insurer, Is.EqualTo(expected.Insurer));
            Assert.That(actual.Price, Is.EqualTo(expected.Price));
            Assert.That(actual.Tax, Is.EqualTo(expected.Tax));
        });
    }
}