using CustomerManagement.Core.Models;
using Xunit;

namespace CustomerManagement.Tests.Models
{
    /// <summary>
    /// Модульные тесты для класса Purchase
    /// Проверяют корректность расчётов сумм, скидок и валидацию данных покупки
    /// </summary>
    public class PurchaseTests
    {
        #region Тесты конструкторов

        [Fact]
        public void DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            var purchase = new Purchase();

            Assert.NotEmpty(purchase.PurchaseId);
            Assert.Equal(string.Empty, purchase.ProductName);
            Assert.Equal(0, purchase.Quantity);
            Assert.Equal(0m, purchase.Price);
            Assert.Equal(0m, purchase.DiscountPercent);
            Assert.True((DateTime.Now - purchase.PurchaseDate).TotalSeconds < 1);
        }

        [Fact]
        public void ParameterizedConstructor_ShouldSetCorrectValues()
        {
            string productName = "Ноутбук ASUS";
            int quantity = 2;
            decimal price = 2500.50m;

            var purchase = new Purchase(productName, quantity, price);

            Assert.NotEmpty(purchase.PurchaseId);
            Assert.Equal(productName, purchase.ProductName);
            Assert.Equal(quantity, purchase.Quantity);
            Assert.Equal(price, purchase.Price);
        }

        #endregion

        #region Тесты расчёта TotalPrice

        [Fact]
        public void TotalPrice_ShouldBeCalculatedCorrectly()
        {
            var purchase = new Purchase("Мышь", 5, 25.00m);

            decimal totalPrice = purchase.TotalPrice;

            Assert.Equal(125.00m, totalPrice);
        }

        [Fact]
        public void TotalPrice_WithZeroQuantity_ShouldReturnZero()
        {
            var purchase = new Purchase("Товар", 0, 100m);

            decimal totalPrice = purchase.TotalPrice;

            Assert.Equal(0m, totalPrice);
        }

        [Theory]
        [InlineData(1, 100.00, 100.00)]
        [InlineData(10, 50.00, 500.00)]
        [InlineData(100, 1.50, 150.00)]
        [InlineData(3, 999.99, 2999.97)]
        public void TotalPrice_VariousCombinations_ShouldCalculateCorrectly(
            int quantity, decimal price, decimal expected)
        {
            var purchase = new Purchase("Товар", quantity, price);

            decimal totalPrice = purchase.TotalPrice;

            Assert.Equal(expected, totalPrice);
        }

        #endregion

        #region Тесты расчёта скидки

        [Fact]
        public void DiscountAmount_WithZeroDiscount_ShouldReturnZero()
        {
            var purchase = new Purchase("Товар", 2, 100m)
            {
                DiscountPercent = 0m
            };

            decimal discountAmount = purchase.DiscountAmount;

            Assert.Equal(0m, discountAmount);
        }

        [Theory]
        [InlineData(1000, 10, 100)]     // 10% от 1000 = 100
        [InlineData(500, 25, 125)]      // 25% от 500 = 125
        [InlineData(2000, 5, 100)]      // 5% от 2000 = 100
        [InlineData(750, 30, 225)]      // 30% от 750 = 225
        public void DiscountAmount_VariousPercentages_ShouldCalculateCorrectly(
            decimal totalPrice, decimal discountPercent, decimal expectedDiscount)
        {
            var purchase = new Purchase("Товар", 1, totalPrice)
            {
                DiscountPercent = discountPercent
            };

            decimal discountAmount = purchase.DiscountAmount;

            Assert.Equal(expectedDiscount, discountAmount);
        }

        #endregion

        #region Тесты расчёта FinalAmount

        [Fact]
        public void FinalAmount_WithoutDiscount_ShouldEqualTotalPrice()
        {

            var purchase = new Purchase("Товар", 3, 200m)
            {
                DiscountPercent = 0m
            };

            decimal finalAmount = purchase.FinalAmount;

            Assert.Equal(600m, finalAmount);
        }

        [Fact]
        public void FinalAmount_WithDiscount_ShouldSubtractDiscountFromTotal()
        {
            var purchase = new Purchase("Товар", 2, 500m)
            {
                DiscountPercent = 20m  // 20% скидка
            };

            decimal finalAmount = purchase.FinalAmount;

            // Assert
            // TotalPrice = 2 * 500 = 1000
            // Discount = 1000 * 0.20 = 200
            // Final = 1000 - 200 = 800
            Assert.Equal(800m, finalAmount);
        }

        [Theory]
        [InlineData(1, 1000, 10, 900)]    // 1000 - 10% = 900
        [InlineData(5, 200, 25, 750)]     // 1000 - 25% = 750
        [InlineData(2, 500, 5, 950)]      // 1000 - 5% = 950
        public void FinalAmount_ComplexScenarios_ShouldCalculateCorrectly(
            int quantity, decimal price, decimal discountPercent, decimal expectedFinal)
        {
            var purchase = new Purchase("Товар", quantity, price)
            {
                DiscountPercent = discountPercent
            };

            decimal finalAmount = purchase.FinalAmount;

            Assert.Equal(expectedFinal, finalAmount);
        }

        #endregion

        #region Тесты валидации

        [Fact]
        public void IsValid_WithAllCorrectData_ShouldReturnTrue()
        {

            var purchase = new Purchase("Телевизор Samsung", 1, 15000m);

            bool isValid = purchase.IsValid();

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("", 1, 100)]           // Пустое название
        [InlineData("   ", 1, 100)]        // Пробелы в названии
        [InlineData(null, 1, 100)]         // Null в названии
        public void IsValid_WithInvalidProductName_ShouldReturnFalse(
            string productName, int quantity, decimal price)
        {

            var purchase = new Purchase
            {
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };

            bool isValid = purchase.IsValid();

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void IsValid_WithInvalidQuantity_ShouldReturnFalse(int quantity)
        {

            var purchase = new Purchase("Товар", quantity, 100m);

            bool isValid = purchase.IsValid();

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-0.01)]
        public void IsValid_WithInvalidPrice_ShouldReturnFalse(decimal price)
        {

            var purchase = new Purchase("Товар", 1, price);

            bool isValid = purchase.IsValid();

            Assert.False(isValid);
        }

        #endregion

        #region Тесты ToString

        [Fact]
        public void ToString_ShouldReturnCorrectFormat()
        {

            var purchase = new Purchase("Клавиатура Logitech", 3, 150m);

            string result = purchase.ToString();

            Assert.Contains("Клавиатура Logitech", result);
            Assert.Contains("3", result);
            Assert.Contains("450", result); // 3 * 150
        }

        #endregion

        #region Тесты граничных случаев

        [Fact]
        public void Purchase_WithVeryLargeValues_ShouldHandleCorrectly()
        {

            var purchase = new Purchase("Дорогой товар", 1000, 999999.99m)
            {
                DiscountPercent = 30m
            };

            decimal totalPrice = purchase.TotalPrice;
            decimal discountAmount = purchase.DiscountAmount;
            decimal finalAmount = purchase.FinalAmount;

            Assert.Equal(999999990m, totalPrice);
            Assert.Equal(299999997m, discountAmount);
            Assert.Equal(699999993m, finalAmount);
        }

        [Fact]
        public void Purchase_WithDecimalPrecision_ShouldMaintainAccuracy()
        {

            var purchase = new Purchase("Точный товар", 3, 33.33m)
            {
                DiscountPercent = 15.5m
            };

            decimal totalPrice = purchase.TotalPrice;
            decimal discountAmount = purchase.DiscountAmount;
            decimal finalAmount = purchase.FinalAmount;

            Assert.Equal(99.99m, totalPrice);
            Assert.Equal(15.49845m, discountAmount);
            Assert.Equal(84.49155m, finalAmount);
        }

        #endregion
    }
}