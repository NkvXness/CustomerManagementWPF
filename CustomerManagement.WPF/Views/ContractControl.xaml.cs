using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CustomerManagement.Core.Models;

namespace CustomerManagement.WPF.Views
{
    public partial class ContractControl : UserControl
    {
        private Customer? _customer;

        public ContractControl()
        {
            InitializeComponent();
            UpdateUI();
        }

        /// <summary>
        /// Установить покупателя для управления договором
        /// </summary>
        public void SetCustomer(Customer? customer)
        {
            _customer = customer;
            UpdateUI();
        }

        /// <summary>
        /// Обновить отображение
        /// </summary>
        private void UpdateUI()
        {
            if (_customer == null || _customer.Contract == null)
            {
                ContractNumberText.Text = "Не указан";
                ContractDateText.Text = "—";
                ContractStatusText.Text = "Не оформлен";
                ContractStatusText.Foreground = Brushes.Red;

                SignContractButton.IsEnabled = _customer != null;
                TerminateContractButton.IsEnabled = false;
                RenewContractButton.IsEnabled = false;
            }
            else
            {
                var contract = _customer.Contract;
                ContractNumberText.Text = contract.ContractNumber;
                ContractDateText.Text = contract.SignDate.ToString("dd.MM.yyyy");

                if (contract.IsActive)
                {
                    ContractStatusText.Text = "✓ Активен";
                    ContractStatusText.Foreground = Brushes.Green;
                    SignContractButton.IsEnabled = false;
                    TerminateContractButton.IsEnabled = true;
                    RenewContractButton.IsEnabled = false;
                }
                else
                {
                    ContractStatusText.Text = "✗ Расторгнут";
                    ContractStatusText.Foreground = Brushes.Orange;
                    SignContractButton.IsEnabled = false;
                    TerminateContractButton.IsEnabled = false;
                    RenewContractButton.IsEnabled = true;
                }
            }
        }

        private void SignContractButton_Click(object sender, RoutedEventArgs e)
        {
            if (_customer == null) return;

            string contractNumber = $"DOG-{DateTime.Now:yyyyMMddHHmmss}";

            try
            {
                _customer.SignContract(contractNumber, DateTime.Now);
                MessageBox.Show($"Договор №{contractNumber} успешно подписан!",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подписании договора: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TerminateContractButton_Click(object sender, RoutedEventArgs e)
        {
            if (_customer == null) return;

            var result = MessageBox.Show("Вы уверены что хотите расторгнуть договор?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _customer.TerminateContract();
                    MessageBox.Show("Договор расторгнут",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateUI();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RenewContractButton_Click(object sender, RoutedEventArgs e)
        {
            if (_customer == null) return;

            try
            {
                _customer.RenewContract(DateTime.Now);
                MessageBox.Show("Договор возобновлен!",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}