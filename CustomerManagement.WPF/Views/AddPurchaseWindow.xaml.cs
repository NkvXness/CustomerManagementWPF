using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CustomerManagement.Core.Models;

namespace CustomerManagement.WPF.Views
{
    /// <summary>
    /// Окно добавления покупки
    /// </summary>
    public partial class AddPurchaseWindow : Window
    {
        private readonly Customer _customer;
        private readonly List<Purchase> _purchases;

        public AddPurchaseWindow(Customer customer)
        {
            InitializeComponent();

            _customer = customer;
            _purchases = new List<Purchase>();

            // Привязываем список к DataGrid
            ItemsDataGrid.ItemsSource = _purchases;

            // Отображаем информацию о покупателе
            CustomerNameText.Text = customer.FullName;
            CustomerTypeText.Text = $"Тип: {customer.Type}";

            UpdateTotals();
        }

        /// <summary>
        /// Добавить товар в список
        /// </summary>
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            // Валидация
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text))
            {
                ShowError("Введите название товара");
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                ShowError("Введите корректное количество");
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                ShowError("Введите корректную цену");
                return;
            }

            // Создаем покупку
            var purchase = new Purchase(
                ProductNameTextBox.Text.Trim(),
                quantity,
                price
            );

            // Добавляем в список
            _purchases.Add(purchase);

            // Обновляем отображение
            ItemsDataGrid.Items.Refresh();

            // Очищаем поля
            ProductNameTextBox.Clear();
            QuantityTextBox.Text = "1";
            PriceTextBox.Text = "0";
            ProductNameTextBox.Focus();

            // Обновляем итоги
            UpdateTotals();
        }

        /// <summary>
        /// Удалить товар из списка
        /// </summary>
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Purchase purchase)
            {
                _purchases.Remove(purchase);
                ItemsDataGrid.Items.Refresh();
                UpdateTotals();
            }
        }

        /// <summary>
        /// Обновить расчёты
        /// </summary>
        private void UpdateTotals()
        {
            if (_purchases.Count == 0)
            {
                TotalBeforeDiscountText.Text = "0,00 BYN";
                DiscountText.Text = "0%";
                DiscountAmountText.Text = "0,00 BYN";
                FinalAmountText.Text = "0,00 BYN";
                return;
            }

            // Считаем общую сумму без скидки
            decimal totalBefore = _purchases.Sum(p => p.TotalPrice);

            // Получаем процент скидки от покупателя
            decimal discountPercent = 0;
            if (_customer.DiscountStrategy != null)
            {
                discountPercent = _customer.DiscountStrategy.GetDiscountPercentage(totalBefore);
            }

            // Применяем процент скидки ко ВСЕМ покупкам
            foreach (var purchase in _purchases)
            {
                purchase.DiscountPercent = discountPercent;
            }

            // Считаем итоги
            decimal discountAmount = _purchases.Sum(p => p.DiscountAmount);
            decimal finalAmount = _purchases.Sum(p => p.FinalAmount);

            // Отображаем
            TotalBeforeDiscountText.Text = $"{totalBefore:N2} BYN";
            DiscountText.Text = $"{discountPercent:F1}%";
            DiscountAmountText.Text = $"{discountAmount:N2} BYN";
            FinalAmountText.Text = $"{finalAmount:N2} BYN";
        }

        /// <summary>
        /// Сохранить покупку
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            if (_purchases.Count == 0)
            {
                ShowError("Добавьте хотя бы один товар");
                return;
            }

            try
            {
                // Добавляем все покупки покупателю (скидка уже применена)
                foreach (var purchase in _purchases)
                {
                    _customer.AddPurchase(purchase);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Отмена
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Показать ошибку
        /// </summary>
        private void ShowError(string message)
        {
            ErrorMessageTextBlock.Text = message;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }
    }
}