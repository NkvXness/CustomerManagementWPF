using CustomerManagement.Core.Models;
using Xunit;

namespace CustomerManagement.Tests.Models
{
    /// <summary>
    /// Модульные тесты для класса PaymentResult
    /// Проверяют корректность формирования результатов операций оплаты
    /// </summary>
    public class PaymentResultTests
    {
        #region Тесты конструктора по умолчанию

        [Fact]
        public void DefaultConstructor_ShouldInitializeWithFailureState()
        {
            var result = new PaymentResult();
            
            Assert.False(result.IsSuccess);
            Assert.Equal(string.Empty, result.Message);
            Assert.Equal(0m, result.PaidAmount);
            Assert.Equal(0m, result.RemainingAmount);
            Assert.True((DateTime.Now - result.TransactionDate).TotalSeconds < 1);
        }

        #endregion

        #region Тесты Success фабричного метода

        [Fact]
        public void Success_ShouldCreateSuccessfulResult()
        {
            
            decimal paidAmount = 1500.50m;
            decimal remainingAmount = 500.00m;

            
            var result = PaymentResult.Success(paidAmount, remainingAmount);

            
            Assert.True(result.IsSuccess);
            Assert.Equal("Оплата прошла успешно", result.Message);
            Assert.Equal(paidAmount, result.PaidAmount);
            Assert.Equal(remainingAmount, result.RemainingAmount);
            Assert.True((DateTime.Now - result.TransactionDate).TotalSeconds < 1);
        }

        [Fact]
        public void Success_WithZeroRemaining_ShouldIndicateFullPayment()
        {
            
            decimal paidAmount = 2000m;
            decimal remainingAmount = 0m;

            
            var result = PaymentResult.Success(paidAmount, remainingAmount);

            
            Assert.True(result.IsSuccess);
            Assert.Equal(paidAmount, result.PaidAmount);
            Assert.Equal(0m, result.RemainingAmount);
        }

        [Theory]
        [InlineData(100, 0)]
        [InlineData(500.50, 200.25)]
        [InlineData(9999.99, 0.01)]
        public void Success_VariousAmounts_ShouldSetCorrectValues(
            decimal paidAmount, decimal remainingAmount)
        {

            var result = PaymentResult.Success(paidAmount, remainingAmount);

            
            Assert.True(result.IsSuccess);
            Assert.Equal(paidAmount, result.PaidAmount);
            Assert.Equal(remainingAmount, result.RemainingAmount);
        }

        #endregion

        #region Тесты Failure фабричного метода

        [Fact]
        public void Failure_ShouldCreateFailedResult()
        {
            
            string errorMessage = "Недостаточно средств";

            
            var result = PaymentResult.Failure(errorMessage);

            
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
            Assert.Equal(0m, result.PaidAmount);
            Assert.Equal(0m, result.RemainingAmount);
            Assert.True((DateTime.Now - result.TransactionDate).TotalSeconds < 1);
        }

        [Theory]
        [InlineData("Карта заблокирована")]
        [InlineData("Превышен лимит")]
        [InlineData("Неверный CVV код")]
        [InlineData("Транзакция отклонена банком")]
        public void Failure_VariousErrorMessages_ShouldSetCorrectMessage(string errorMessage)
        {

            var result = PaymentResult.Failure(errorMessage);

            
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
        }

        #endregion

        #region Тесты ToString

        [Fact]
        public void ToString_SuccessfulPayment_ShouldReturnSuccessMessage()
        {
            var result = PaymentResult.Success(1000m, 500m);

            string message = result.ToString();

            Assert.Contains("✓", message);
            Assert.Contains("Оплата", message);
            Assert.Contains("BYN", message);
            Assert.Contains("успешно", message);
            Assert.Contains("Остаток", message);

            Assert.NotEmpty(message);
            Assert.Matches(@"\d+", message);
        }

        [Fact]
        public void ToString_FailedPayment_ShouldReturnErrorMessage()
        {
            
            var result = PaymentResult.Failure("Ошибка соединения");

            
            string message = result.ToString();

            
            Assert.Contains("✗", message);
            Assert.Contains("Ошибка оплаты", message);
            Assert.Contains("Ошибка соединения", message);
        }

        #endregion

        #region Тесты граничных случаев

        [Fact]
        public void Success_WithLargeAmounts_ShouldHandleCorrectly()
        {
            
            decimal paidAmount = 999999999.99m;
            decimal remainingAmount = 0.01m;

            
            var result = PaymentResult.Success(paidAmount, remainingAmount);

            
            Assert.True(result.IsSuccess);
            Assert.Equal(paidAmount, result.PaidAmount);
            Assert.Equal(remainingAmount, result.RemainingAmount);
        }

        [Fact]
        public void Failure_WithEmptyMessage_ShouldAccept()
        {

            var result = PaymentResult.Failure("");

            
            Assert.False(result.IsSuccess);
            Assert.Equal("", result.Message);
        }

        #endregion

        #region Тесты изменения свойств

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            
            var result = new PaymentResult();

            
            result.IsSuccess = true;
            result.Message = "Тестовое сообщение";
            result.PaidAmount = 100m;
            result.RemainingAmount = 50m;
            result.TransactionDate = new DateTime(2025, 11, 13, 10, 30, 0);

            
            Assert.True(result.IsSuccess);
            Assert.Equal("Тестовое сообщение", result.Message);
            Assert.Equal(100m, result.PaidAmount);
            Assert.Equal(50m, result.RemainingAmount);
            Assert.Equal(new DateTime(2025, 11, 13, 10, 30, 0), result.TransactionDate);
        }

        #endregion
    }
}