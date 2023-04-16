using NUnit.Framework;

namespace Aggregator.Tests;

[TestFixture]
public class PriceEngineTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetPrice_GivenDefaultRequest_ShouldReturnQuoteFromZxcvbnm()
    {
        //Arrange
        var riskData = new RiskData("John", "Smith", "500", "Cool New Phone", "1980-01-01");
        var request = new PriceRequest(riskData);
        var priceEngine = new PriceEngine(request);

        //Act
        var actual = priceEngine.GetPrice();

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