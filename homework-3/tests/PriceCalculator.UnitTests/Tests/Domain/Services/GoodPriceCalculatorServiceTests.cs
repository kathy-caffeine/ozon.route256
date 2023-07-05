using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Route256.PriceCalculator.Domain.Bll;
using Route256.PriceCalculator.Domain.Bll.Models.PriceCalculator;
using Route256.PriceCalculator.Domain.Bll.Services;
using Route256.PriceCalculator.Domain.Bll.Services.Interfaces;
using Route256.PriceCalculator.Domain.Entities;
using Route256.PriceCalculator.Domain.Separated;
using Xunit;

namespace PriceCalculator.UnitTests.Tests.Domain.Services;

public class GoodPriceCalculatorServiceTests
{
    [Theory]
    [InlineData(default, 10)]
    [InlineData(10, default)]
    public void GoodPriceCalculatorService_WhenDefault_ShouldThrow(
        int id,
        decimal distance)
    {
        // Arrange
        var options = new PriceCalculatorOptions();
        var repositoryMock = new Mock<IGoodsRepository>(MockBehavior.Default);
        var serviceMock = new Mock<IStorageRepository>(MockBehavior.Default);
        var service = new PriceCalculatorService(CreateOptionsSnapshot(options), serviceMock.Object);
        var cut = new GoodPriceCalculatorService(repositoryMock.Object, service);

        // Act, Assert
        Assert.Throws<ArgumentException>(() => cut.CalculatePrice(id, distance));
    }



    private static IOptionsSnapshot<PriceCalculatorOptions> CreateOptionsSnapshot(
        PriceCalculatorOptions options)
    {
        var repositoryMock = new Mock<IOptionsSnapshot<PriceCalculatorOptions>>(MockBehavior.Strict);

        repositoryMock
            .Setup(x => x.Value)
            .Returns(() => options);

        return repositoryMock.Object;
    }

    [Theory]
    [InlineData(10, 10, 100)]
    [InlineData(100, 10, 1000)]
    [InlineData(17.36, 7.42, 128.8112)]
    public void GoodPriceCalculatorService_WhenCalcByVolumeMany_ShouldSuccess(
        decimal distance,
        decimal price,
        decimal expected)
    {
        // Arrange
        var entity = new GoodEntity(
            "",
            1,
            1000,
            2000,
            3000,
            4000,
            10,
            10
            );
        var model = new GoodModel(1000, 2000, 3000, 4000);
        var modelList = new List<GoodModel> { model };

        var goodsRepositoryMock = new Mock<IGoodsRepository>(MockBehavior.Strict);
        goodsRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(entity);
        var priceCalculatorMock = new Mock<IPriceCalculatorService>(MockBehavior.Strict);
        priceCalculatorMock.Setup(x => x.CalculatePrice(modelList)).Returns(price);

        var cut = new GoodPriceCalculatorService(goodsRepositoryMock.Object, priceCalculatorMock.Object);

        // Act
        var result = cut.CalculatePrice(1, distance);

        // Assert
        Assert.Equal(expected, result);
    }
}