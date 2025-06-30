using CustomerManagement.Core.Strategies;
using Xunit;

namespace CustomerManagement.Tests.Strategies
{
    /// <summary>
    /// Модульные тесты для VIPDiscountStrategy
    /// Проверяют корректность расчёта VIP-скидок 25-30%
    /// </summary>
    public class VIPDiscountStrategyTests
    {
        #region Тесты конструктора по умолчанию

        [Fact]
        public void DefaultConstructor_ShouldSet25PercentBaseDiscount()
        {
            var strategy = new VIPDiscountStrategy();
            Assert.Equal(25m, strategy.BaseVIPDiscountPercentage);
        }

        #endregion

        #region Тесты конструктора с параметром

        [Theory]
        [InlineData(25)]
        [InlineData(27)]
        [InlineData(30)]
        public void ParameterizedConstructor_WithValidPercentage_ShouldSetCorrectly(
            decimal basePercentage)
        {
            var strategy = new VIPDiscountStrategy(basePercentage);
            Assert.Equal(basePercentage, strategy.BaseVIPDiscountPercentage);
        }

        [Theory]
        [InlineData(10, 20)]   // Меньше 20 - установится 20
        [InlineData(15, 20)]
        [InlineData(19.99, 20)]
        public void ParameterizedConstructor_BelowMinimum_ShouldSet20Percent(
            decimal inputPercentage, decimal expectedPercentage)
        {
            var strategy = new VIPDiscountStrategy(inputPercentage);
            Assert.Equal(expectedPercentage, strategy.BaseVIPDiscountPercentage);
        }

        [Theory]
        [InlineData(40, 35)]   // Больше 35 - установится 35
        [InlineData(50, 35)]
        [InlineData(100, 35)]
        public void ParameterizedConstructor_AboveMaximum_ShouldSet35Percent(
            decimal inputPercentage, decimal expectedPercentage)
        {
            var strategy = new VIPDiscountStrategy(inputPercentage);
            Assert.Equal(expectedPercentage, strategy.BaseVIPDiscountPercentage);
        }

        #endregion

        #region Тесты GetDiscountPercentage

        [Theory]
        [InlineData(0, 25)]
        [InlineData(1000, 25)]
        [InlineData(4999.99, 25)]
        public void GetDiscountPercentage_LessThan5000_ShouldReturnBase25Percent(
            decimal amount, decimal expected)
        {
            var strategy = new VIPDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5000, 26)]
        [InlineData(10000, 26)]
        [InlineData(19999.99, 26)]
        public void GetDiscountPercentage_Between5000And20000_ShouldReturn26Percent(
            decimal amount, decimal expected)
        {
            var strategy = new VIPDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20000, 27)]
        [InlineData(35000, 27)]
        [InlineData(49999.99, 27)]
        public void GetDiscountPercentage_Between20000And50000_ShouldReturn27Percent(
            decimal amount, decimal expected)
        {
            var strategy = new VIPDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(50000, 28)]
        [InlineData(75000, 28)]
        [InlineData(99999.99, 28)]
        public void GetDiscountPercentage_Between50000And100000_ShouldReturn28Percent(
            decimal amount, decimal expected)
        {
            var strategy = new VIPDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(100000, 29)]
        [InlineData(150000, 29)]
        [InlineData(199999.99, 29)]
        public void GetDiscountPercentage_Between100000And200000_ShouldReturn29Percent(
            decimal amount, decimal expected)
        {
            var strategy = new VIPDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(200000, 30)]
        [InlineData(500000, 30)]
        [InlineData(999999, 30)]
        public void GetDiscountPercentage_MoreThan200000_ShouldReturn30Percent(
            decimal amount, decimal expected)
        {
            var strategy = new VIPDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Тесты CalculateDiscount

        [Fact]
        public void CalculateDiscount_Amount3000_ShouldReturn750()
        {
            var strategy = new VIPDiscountStrategy();
            decimal amount = 3000m; // 25% от 3000 = 750
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(750m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount10000_ShouldReturn2600()
        {
            var strategy = new VIPDiscountStrategy();
            decimal amount = 10000m; // 26% от 10000 = 2600
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(2600m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount100000_ShouldReturn29000()
        {
            var strategy = new VIPDiscountStrategy();
            decimal amount = 100000m; // 29% от 100000 = 29000
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(29000m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount300000_ShouldReturn90000()
        {
            var strategy = new VIPDiscountStrategy();
            decimal amount = 300000m; // 30% от 300000 = 90000
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(90000m, discount);
        }

        #endregion

        #region Тесты GetBonusDiscountPercentage

        [Fact]
        public void GetBonusDiscountPercentage_AmountUnder5000_ShouldReturn0()
        {
            var strategy = new VIPDiscountStrategy(); // Base = 25%
            decimal amount = 3000m; // Получит 25%, бонус = 0
            decimal bonus = strategy.GetBonusDiscountPercentage(amount);
            Assert.Equal(0m, bonus);
        }

        [Fact]
        public void GetBonusDiscountPercentage_Amount10000_ShouldReturn1()
        {
            var strategy = new VIPDiscountStrategy(); // Base = 25%
            decimal amount = 10000m; // Получит 26%, бонус = 1%
            decimal bonus = strategy.GetBonusDiscountPercentage(amount);
            Assert.Equal(1m, bonus);
        }

        [Fact]
        public void GetBonusDiscountPercentage_Amount300000_ShouldReturn5()
        {
            var strategy = new VIPDiscountStrategy(); // Base = 25%
            decimal amount = 300000m; // Получит 30%, бонус = 5%
            decimal bonus = strategy.GetBonusDiscountPercentage(amount);
            Assert.Equal(5m, bonus);
        }

        #endregion

        #region Тест свойства StrategyName

        [Fact]
        public void StrategyName_ShouldReturnCorrectName()
        {
            var strategy = new VIPDiscountStrategy();
            string name = strategy.StrategyName;
            Assert.Equal("VIP скидка", name);
        }

        #endregion
    }
}