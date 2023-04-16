using NUnit.Framework;

namespace Aggregator.Tests;

[TestFixture]
public class PriceEngineTests
{
    [Test]
    public void GetPrice_GivenDefaultRequest_ShouldReturnQuoteFromTestName()
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
            Insurer = "Test Name",
            Price = 92.67m,
            Tax = 92.67m * 0.12m,
        };

        Assert.Multiple(() =>
        {
            Assert.That(actual.Insurer, Is.EqualTo(expected.Insurer));
            Assert.That(actual.Price, Is.EqualTo(expected.Price));
            Assert.That(actual.Tax, Is.EqualTo(expected.Tax));
        });
    }

    [Test]
    public void GetPrice_GivenNoDoB_ShouldReturnQuoteFromZxcvbnm()
    {
        //Arrange
        var riskData = new RiskData("John", "Smith", "500", "Cool New Phone");
        var request = new PriceRequest(riskData);
        var priceEngine = new PriceEngine(request);

        //Act
        var actual = priceEngine.GetPrice();

        //Assert
        var expected = new PriceResponse
        {
            Insurer = "zxcvbnm",
            Price = 123.45m,
            Tax = 123.45m * 0.12m,
        };

        Assert.Multiple(() =>
        {
            Assert.That(actual.Insurer, Is.EqualTo(expected.Insurer));
            Assert.That(actual.Price, Is.EqualTo(expected.Price));
            Assert.That(actual.Tax, Is.EqualTo(expected.Tax));
        });
    }

    [Test]
    public void GetPrice_GivenZeroValue_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new RiskData("John", "Smith", "0", "Cool New Phone", "1980-01-01"));
    }

    [Test]
    public void GetPrice_GivenExamplemake1_ShouldReturnQuoteFromQewtrywrh()
    {
        //Arrange
        var riskData = new RiskData("John", "Smith", "500", "examplemake1", "1980-01-01");
        var request = new PriceRequest(riskData);
        var priceEngine = new PriceEngine(request);

        //Act
        var actual = priceEngine.GetPrice();

        //Assert
        var expected = new PriceResponse
        {
            Insurer = "qewtrywrh",
            Price = 77.56m,
            Tax = 77.56m * 0.12m,
        };

        Assert.Multiple(() =>
        {
            Assert.That(actual.Insurer, Is.EqualTo(expected.Insurer));
            Assert.That(actual.Price, Is.EqualTo(expected.Price));
            Assert.That(actual.Tax, Is.EqualTo(expected.Tax));
        });
    }
}