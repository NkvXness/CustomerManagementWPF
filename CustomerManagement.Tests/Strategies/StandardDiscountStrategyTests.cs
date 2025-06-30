using CustomerManagement.Core.Strategies;
using Xunit;

namespace CustomerManagement.Tests.Strategies
{
    /// <summary>
    /// Модульные тесты для StandardDiscountStrategy
    /// Проверяют корректность расчёта стандартных скидок 0-5%
    /// </summary>
    public class StandardDiscountStrategyTests
    {
        private readonly StandardDiscountStrategy _strategy;

        public StandardDiscountStrategyTests()
        {
            _strategy = new StandardDiscountStrategy();
        }

        #region Тесты GetDiscountPercentage

        [Theory]
        [InlineData(0, 0)]      // Меньше 1000 - нет скидки
        [InlineData(500, 0)]
        [InlineData(999.99, 0)]
        public void GetDiscountPercentage_LessThan1000_ShouldReturn0Percent(
            decimal totalAmount, decimal expectedPercent)
        {
            decimal actualPercent = _strategy.GetDiscountPercentage(totalAmount);
            Assert.Equal(expectedPercent, actualPercent);
        }

        [Theory]
        [InlineData(1000, 2)]    // 1000-5000 = 2%
        [InlineData(2500, 2)]
        [InlineData(4999.99, 2)]
        public void GetDiscountPercentage_Between1000And5000_ShouldReturn2Percent(
            decimal totalAmount, decimal expectedPercent)
        {
            decimal actualPercent = _strategy.GetDiscountPercentage(totalAmount);
            Assert.Equal(expectedPercent, actualPercent);
        }

        [Theory]
        [InlineData(5000, 3)]    // 5000-10000 = 3%
        [InlineData(7500, 3)]
        [InlineData(9999.99, 3)]
        public void GetDiscountPercentage_Between5000And10000_ShouldReturn3Percent(
            decimal totalAmount, decimal expectedPercent)
        {
            decimal actualPercent = _strategy.GetDiscountPercentage(totalAmount);
            Assert.Equal(expectedPercent, actualPercent);
        }

        [Theory]
        [InlineData(10000, 4)]   // 10000-20000 = 4%
        [InlineData(15000, 4)]
        [InlineData(19999.99, 4)]
        public void GetDiscountPercentage_Between10000And20000_ShouldReturn4Percent(
            decimal totalAmount, decimal expectedPercent)
        {
            decimal actualPercent = _strategy.GetDiscountPercentage(totalAmount);
            Assert.Equal(expectedPercent, actualPercent);
        }

        [Theory]
        [InlineData(20000, 5)]   // Свыше 20000 = 5%
        [InlineData(50000, 5)]
        [InlineData(999999, 5)]
        public void GetDiscountPercentage_MoreThan20000_ShouldReturn5Percent(
            decimal totalAmount, decimal expectedPercent)
        {
            decimal actualPercent = _strategy.GetDiscountPercentage(totalAmount);
            Assert.Equal(expectedPercent, actualPercent);
        }

        #endregion

        #region Тесты CalculateDiscount

        [Fact]
        public void CalculateDiscount_Amount500_ShouldReturn0()
        {
            decimal amount = 500m;
            decimal discount = _strategy.CalculateDiscount(amount);
            Assert.Equal(0m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount3000_ShouldReturn60()
        {
            decimal amount = 3000m; // 2% от 3000 = 60
            decimal discount = _strategy.CalculateDiscount(amount);
            Assert.Equal(60m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount7500_ShouldReturn225()
        {
            decimal amount = 7500m; // 3% от 7500 = 225
            decimal discount = _strategy.CalculateDiscount(amount);
            Assert.Equal(225m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount15000_ShouldReturn600()
        {
            decimal amount = 15000m; // 4% от 15000 = 600
            decimal discount = _strategy.CalculateDiscount(amount);
            Assert.Equal(600m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount50000_ShouldReturn2500()
        {
            decimal amount = 50000m; // 5% от 50000 = 2500
            decimal discount = _strategy.CalculateDiscount(amount);
            Assert.Equal(2500m, discount);
        }

        #endregion

        #region Тест свойства StrategyName

        [Fact]
        public void StrategyName_ShouldReturnCorrectName()
        {
            string name = _strategy.StrategyName;
            Assert.Equal("Стандартная скидка", name);
        }

        #endregion

        #region Тесты граничных значений

        [Theory]
        [InlineData(999.99, 0)]     // Граница перед 1000
        [InlineData(1000, 2)]       // Точно 1000
        [InlineData(4999.99, 2)]    // Граница перед 5000
        [InlineData(5000, 3)]       // Точно 5000
        [InlineData(9999.99, 3)]    // Граница перед 10000
        [InlineData(10000, 4)]      // Точно 10000
        [InlineData(19999.99, 4)]   // Граница перед 20000
        [InlineData(20000, 5)]      // Точно 20000
        public void GetDiscountPercentage_BoundaryValues_ShouldReturnCorrectPercent(
            decimal amount, decimal expectedPercent)
        {
            decimal actualPercent = _strategy.GetDiscountPercentage(amount);
            Assert.Equal(expectedPercent, actualPercent);
        }

        #endregion
    }
}