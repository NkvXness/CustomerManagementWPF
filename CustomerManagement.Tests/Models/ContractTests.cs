using CustomerManagement.Core.Models;
using Xunit;

namespace CustomerManagement.Tests.Models
{
    /// <summary>
    /// Модульные тесты для класса Contract
    /// Проверяют корректность работы договора: создание, расторжение, возобновление
    /// </summary>
    public class ContractTests
    {
        #region Тесты конструктора

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateActiveContract()
        {
            // Arrange
            string contractNumber = "DOG-001";
            DateTime signDate = new(2025, 1, 15);

            // Act
            var contract = new Contract(contractNumber, signDate);

            // Assert
            Assert.Equal(contractNumber, contract.ContractNumber);
            Assert.Equal(signDate, contract.SignDate);
            Assert.True(contract.IsActive);
            Assert.Null(contract.TerminationDate);
        }

        [Fact]
        public void DefaultConstructor_ShouldCreateInactiveContract()
        {
            // Arrange & Act
            var contract = new Contract();

            // Assert
            Assert.Equal(string.Empty, contract.ContractNumber);
            Assert.False(contract.IsActive);
            Assert.Null(contract.TerminationDate);
        }

        #endregion

        #region Тесты расторжения договора

        [Fact]
        public void Terminate_ActiveContract_ShouldSetInactiveAndSetTerminationDate()
        {
            // Arrange
            var contract = new Contract("DOG-002", new DateTime(2025, 1, 10));
            DateTime beforeTerminate = DateTime.Now;

            // Act
            contract.Terminate();

            // Assert
            Assert.False(contract.IsActive);
            Assert.NotNull(contract.TerminationDate);
            Assert.True(contract.TerminationDate >= beforeTerminate);
            Assert.True(contract.TerminationDate <= DateTime.Now);
        }

        [Fact]
        public void Terminate_AlreadyTerminatedContract_ShouldUpdateTerminationDate()
        {
            // Arrange
            var contract = new Contract("DOG-003", new DateTime(2025, 1, 5));
            contract.Terminate();
            DateTime firstTerminationDate = contract.TerminationDate!.Value;

            System.Threading.Thread.Sleep(10); // Небольшая задержка для разных временных меток

            // Act
            contract.Terminate(); // Повторное расторжение

            // Assert
            Assert.False(contract.IsActive);
            Assert.NotNull(contract.TerminationDate);
            Assert.True(contract.TerminationDate > firstTerminationDate);
        }

        #endregion

        #region Тесты возобновления договора

        [Fact]
        public void Renew_TerminatedContract_ShouldMakeActiveAndResetTerminationDate()
        {
            // Arrange
            var contract = new Contract("DOG-004", new DateTime(2025, 1, 1));
            contract.Terminate();
            DateTime newSignDate = new(2025, 2, 1);

            // Act
            contract.Renew(newSignDate);

            // Assert
            Assert.True(contract.IsActive);
            Assert.Equal(newSignDate, contract.SignDate);
            Assert.Null(contract.TerminationDate);
        }

        [Fact]
        public void Renew_ActiveContract_ShouldUpdateSignDate()
        {
            // Arrange
            var contract = new Contract("DOG-005", new DateTime(2025, 1, 1));
            DateTime newSignDate = new(2025, 3, 1);

            // Act
            contract.Renew(newSignDate);

            // Assert
            Assert.True(contract.IsActive);
            Assert.Equal(newSignDate, contract.SignDate);
            Assert.Null(contract.TerminationDate);
        }

        #endregion

        #region Тесты ToString

        [Fact]
        public void ToString_ActiveContract_ShouldReturnCorrectFormat()
        {
            // Arrange
            var contract = new Contract("DOG-123", new DateTime(2025, 5, 20));

            // Act
            string result = contract.ToString();

            // Assert
            Assert.Contains("DOG-123", result);
            Assert.Contains("20.05.2025", result);
            Assert.Contains("Активен", result);
        }

        [Fact]
        public void ToString_TerminatedContract_ShouldIndicateTermination()
        {
            // Arrange
            var contract = new Contract("DOG-456", new DateTime(2025, 3, 10));
            contract.Terminate();

            // Act
            string result = contract.ToString();

            // Assert
            Assert.Contains("DOG-456", result);
            Assert.Contains("Расторгнут", result);
        }

        #endregion
    }
}