using CustomerManagement.Core.Strategies;
using Xunit;

namespace CustomerManagement.Tests.Strategies
{
    /// <summary>
    /// Модульные тесты для WholesaleDiscountStrategy
    /// Проверяют корректность расчёта оптовых скидок 10-20%
    /// </summary>
    public class WholesaleDiscountStrategyTests
    {
        #region Тесты конструктора

        [Fact]
        public void DefaultConstructor_ShouldSet10000MinimumOrder()
        {
            var strategy = new WholesaleDiscountStrategy();
            Assert.Equal(10000m, strategy.MinimumOrderAmount);
        }

        [Theory]
        [InlineData(5000)]
        [InlineData(15000)]
        [InlineData(25000)]
        public void ParameterizedConstructor_ShouldSetCustomMinimum(decimal minimum)
        {
            var strategy = new WholesaleDiscountStrategy(minimum);
            Assert.Equal(minimum, strategy.MinimumOrderAmount);
        }

        #endregion

        #region Тесты GetDiscountPercentage

        [Theory]
        [InlineData(0, 0)]
        [InlineData(5000, 0)]
        [InlineData(9999.99, 0)]
        public void GetDiscountPercentage_BelowMinimum_ShouldReturn0Percent(
            decimal amount, decimal expected)
        {
            var strategy = new WholesaleDiscountStrategy(); // Min = 10000
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(20000, 10)]
        [InlineData(29999.99, 10)]
        public void GetDiscountPercentage_Between10000And30000_ShouldReturn10Percent(
            decimal amount, decimal expected)
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(30000, 12)]
        [InlineData(40000, 12)]
        [InlineData(49999.99, 12)]
        public void GetDiscountPercentage_Between30000And50000_ShouldReturn12Percent(
            decimal amount, decimal expected)
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(50000, 15)]
        [InlineData(75000, 15)]
        [InlineData(99999.99, 15)]
        public void GetDiscountPercentage_Between50000And100000_ShouldReturn15Percent(
            decimal amount, decimal expected)
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(100000, 18)]
        [InlineData(150000, 18)]
        [InlineData(199999.99, 18)]
        public void GetDiscountPercentage_Between100000And200000_ShouldReturn18Percent(
            decimal amount, decimal expected)
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(200000, 20)]
        [InlineData(500000, 20)]
        [InlineData(999999, 20)]
        public void GetDiscountPercentage_MoreThan200000_ShouldReturn20Percent(
            decimal amount, decimal expected)
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal actual = strategy.GetDiscountPercentage(amount);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Тесты CalculateDiscount

        [Fact]
        public void CalculateDiscount_Amount5000_ShouldReturn0()
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal amount = 5000m; // Меньше минимума
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(0m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount15000_ShouldReturn1500()
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal amount = 15000m; // 10% от 15000 = 1500
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(1500m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount40000_ShouldReturn4800()
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal amount = 40000m; // 12% от 40000 = 4800
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(4800m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount75000_ShouldReturn11250()
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal amount = 75000m; // 15% от 75000 = 11250
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(11250m, discount);
        }

        [Fact]
        public void CalculateDiscount_Amount300000_ShouldReturn60000()
        {
            var strategy = new WholesaleDiscountStrategy();
            decimal amount = 300000m; // 20% от 300000 = 60000
            decimal discount = strategy.CalculateDiscount(amount);
            Assert.Equal(60000m, discount);
        }

        #endregion

        #region Тесты ValidateMinimumOrder

        [Theory]
        [InlineData(10000, true)]
        [InlineData(15000, true)]
        [InlineData(50000, true)]
        public void ValidateMinimumOrder_AmountEqualsOrAboveMinimum_ShouldReturnTrue(
            decimal amount, bool expected)
        {
            var strategy = new WholesaleDiscountStrategy(); // Min = 10000
            bool actual = strategy.ValidateMinimumOrder(amount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(5000, false)]
        [InlineData(9999.99, false)]
        public void ValidateMinimumOrder_AmountBelowMinimum_ShouldReturnFalse(
            decimal amount, bool expected)
        {
            var strategy = new WholesaleDiscountStrategy(); // Min = 10000
            bool actual = strategy.ValidateMinimumOrder(amount);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateMinimumOrder_WithCustomMinimum_ShouldUseCustomValue()
        {
            var strategy = new WholesaleDiscountStrategy(20000m); // Min = 20000
            bool result1 = strategy.ValidateMinimumOrder(15000m); // Меньше
            bool result2 = strategy.ValidateMinimumOrder(20000m); // Равно
            bool result3 = strategy.ValidateMinimumOrder(25000m); // Больше
            Assert.False(result1);
            Assert.True(result2);
            Assert.True(result3);
        }

        #endregion

        #region Тест свойства StrategyName

        [Fact]
        public void StrategyName_ShouldReturnCorrectName()
        {
            var strategy = new WholesaleDiscountStrategy();
            string name = strategy.StrategyName;
            Assert.Equal("Оптовая скидка", name);
        }

        #endregion

        #region Тесты изменения MinimumOrderAmount

        [Fact]
        public void MinimumOrderAmount_ShouldBeSettable()
        {
            var strategy = new WholesaleDiscountStrategy(10000m);
            strategy.MinimumOrderAmount = 25000m;
            Assert.Equal(25000m, strategy.MinimumOrderAmount);
        }

        [Fact]
        public void GetDiscountPercentage_AfterChangingMinimum_ShouldUseNewValue()
        {
            var strategy = new WholesaleDiscountStrategy(10000m);
            strategy.MinimumOrderAmount = 20000m;
            decimal percent1 = strategy.GetDiscountPercentage(15000m); // Ниже нового минимума
            decimal percent2 = strategy.GetDiscountPercentage(25000m); // Выше нового минимума
            Assert.Equal(0m, percent1);
            Assert.Equal(10m, percent2);
        }

        #endregion
    }
}