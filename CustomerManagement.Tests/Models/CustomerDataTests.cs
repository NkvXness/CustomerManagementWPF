using CustomerManagement.Core.Models;
using Xunit;

namespace CustomerManagement.Tests.Models
{
    /// <summary>
    /// Модульные тесты для класса CustomerData (DTO)
    /// Проверяют корректность работы с персональными данными покупателя
    /// </summary>
    public class CustomerDataTests
    {
        #region Тесты конструкторов

        [Fact]
        public void DefaultConstructor_ShouldInitializeWithEmptyStrings()
        {
            var customerData = new CustomerData();

            Assert.Equal(string.Empty, customerData.FullName);
            Assert.Equal(string.Empty, customerData.Email);
            Assert.Equal(string.Empty, customerData.Phone);
            Assert.Equal(string.Empty, customerData.Address);
        }

        [Fact]
        public void ParameterizedConstructor_ShouldSetAllProperties()
        {
            
            string fullName = "Иванов Иван Иванович";
            string email = "ivanov@example.com";
            string phone = "+375 (29) 123-45-67";
            string address = "г. Гомель, ул. Советская, д. 10";

            
            var customerData = new CustomerData(fullName, email, phone, address);

            
            Assert.Equal(fullName, customerData.FullName);
            Assert.Equal(email, customerData.Email);
            Assert.Equal(phone, customerData.Phone);
            Assert.Equal(address, customerData.Address);
        }

        #endregion

        #region Тесты метода Clone

        [Fact]
        public void Clone_ShouldCreateExactCopy()
        {
            
            var original = new CustomerData(
                "Петров Пётр Петрович",
                "petrov@test.by",
                "+375 (44) 999-88-77",
                "г. Минск, пр. Независимости, 50"
            );

            
            var cloned = original.Clone();

            
            Assert.NotSame(original, cloned); // Разные объекты
            Assert.Equal(original.FullName, cloned.FullName);
            Assert.Equal(original.Email, cloned.Email);
            Assert.Equal(original.Phone, cloned.Phone);
            Assert.Equal(original.Address, cloned.Address);
        }

        [Fact]
        public void Clone_ModifyingClone_ShouldNotAffectOriginal()
        {
            
            var original = new CustomerData(
                "Сидоров Сидор",
                "sidorov@mail.ru",
                "+7 (900) 111-22-33",
                "Москва"
            );
            var cloned = original.Clone();

            
            cloned.FullName = "Изменённое Имя";
            cloned.Email = "new@email.com";

            
            Assert.Equal("Сидоров Сидор", original.FullName);
            Assert.Equal("sidorov@mail.ru", original.Email);
            Assert.Equal("Изменённое Имя", cloned.FullName);
            Assert.Equal("new@email.com", cloned.Email);
        }

        #endregion

        #region Тесты валидации

        [Fact]
        public void IsValid_WithAllRequiredFields_ShouldReturnTrue()
        {
            
            var customerData = new CustomerData(
                "Кузнецов Кузьма",
                "kuznetsov@example.com",
                "+375 (29) 555-44-33",
                "Адрес" // Адрес необязателен для IsValid
            );

            
            bool isValid = customerData.IsValid();

            
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("", "email@test.com", "+123456789")]
        [InlineData("   ", "email@test.com", "+123456789")]
        [InlineData(null, "email@test.com", "+123456789")]
        public void IsValid_WithInvalidFullName_ShouldReturnFalse(
            string fullName, string email, string phone)
        {
            
            var customerData = new CustomerData(fullName, email, phone, "");

            
            bool isValid = customerData.IsValid();

            
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Иванов И.И.", "", "+123456789")]
        [InlineData("Иванов И.И.", "   ", "+123456789")]
        [InlineData("Иванов И.И.", null, "+123456789")]
        public void IsValid_WithInvalidEmail_ShouldReturnFalse(
            string fullName, string email, string phone)
        {
            
            var customerData = new CustomerData(fullName, email, phone, "");

            
            bool isValid = customerData.IsValid();

            
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Иванов И.И.", "email@test.com", "")]
        [InlineData("Иванов И.И.", "email@test.com", "   ")]
        [InlineData("Иванов И.И.", "email@test.com", null)]
        public void IsValid_WithInvalidPhone_ShouldReturnFalse(
            string fullName, string email, string phone)
        {
            
            var customerData = new CustomerData(fullName, email, phone, "");

            
            bool isValid = customerData.IsValid();

            
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_WithEmptyAddress_ShouldReturnTrue()
        {
            var customerData = new CustomerData(
                "Николаев Николай",
                "nikolaev@test.by",
                "+375 (29) 777-66-55",
                "" // Пустой адрес
            );

            
            bool isValid = customerData.IsValid();

            
            Assert.True(isValid);
        }

        #endregion

        #region Тесты ToString

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            
            var customerData = new CustomerData(
                "Алексеев Алексей",
                "alekseev@gmail.com",
                "+375 (44) 888-77-66",
                "Гродно"
            );

            
            string result = customerData.ToString();

            
            Assert.Contains("Алексеев Алексей", result);
            Assert.Contains("alekseev@gmail.com", result);
            Assert.Contains("+375 (44) 888-77-66", result);
        }

        #endregion

        #region Тесты изменения данных

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            
            var customerData = new CustomerData();

            
            customerData.FullName = "Новое Имя";
            customerData.Email = "new@email.com";
            customerData.Phone = "+123456789";
            customerData.Address = "Новый адрес";

            
            Assert.Equal("Новое Имя", customerData.FullName);
            Assert.Equal("new@email.com", customerData.Email);
            Assert.Equal("+123456789", customerData.Phone);
            Assert.Equal("Новый адрес", customerData.Address);
        }

        #endregion
    }
}