using System;
using System.Windows;
using System.Windows.Controls;
using CustomerManagement.Core.Models;

namespace CustomerManagement.WPF.Views
{
    public partial class WholesaleInfoControl : UserControl
    {
        private WholesaleCustomer? _wholesaleCustomer;

        public WholesaleInfoControl()
        {
            InitializeComponent();
            UpdateUI();
        }

        /// <summary>
        /// Установить покупателя
        /// </summary>
        public void SetCustomer(Customer? customer)
        {
            _wholesaleCustomer = customer as WholesaleCustomer;
            UpdateUI();
        }

        /// <summary>
        /// Обновить отображение
        /// </summary>
        private void UpdateUI()
        {
            if (_wholesaleCustomer == null)
            {
                IsEnabled = false;
                PaymentDelayText.Text = "0 дней";
                PaymentDelayStatusText.Text = "Не запрошена";
                MinOrderAmountText.Text = "0 ₽";
                RequestDeferralButton.IsEnabled = false;
                CancelDeferralButton.IsEnabled = false;
                return;
            }

            IsEnabled = true;

            // Отображаем количество дней отсрочки
            PaymentDelayText.Text = $"{_wholesaleCustomer.PaymentDeferralDays} дней";

            // Минимальный заказ
            MinOrderAmountText.Text = $"{_wholesaleCustomer.MinimumOrderAmount:N2} BYN";

            // Определяем статус отсрочки
            if (_wholesaleCustomer.PaymentDeferralDays == 0)
            {
                // Отсрочка не запрошена
                PaymentDelayStatusText.Text = "Не запрошена";
                PaymentDelayStatusText.Foreground = System.Windows.Media.Brushes.Gray;
                RequestDeferralButton.IsEnabled = true;
                CancelDeferralButton.IsEnabled = false;
            }
            else
            {
                // Отсрочка запрошена - проверяем сумму заказа
                bool canApply = _wholesaleCustomer.TotalAmount >= _wholesaleCustomer.MinimumOrderAmount;

                if (canApply)
                {
                    PaymentDelayStatusText.Text = "Активна | Применена";
                    PaymentDelayStatusText.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    PaymentDelayStatusText.Text = "Активна | Не применена";
                    PaymentDelayStatusText.Foreground = System.Windows.Media.Brushes.Orange;
                }

                RequestDeferralButton.IsEnabled = true;
                CancelDeferralButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Запросить отсрочку платежа
        /// </summary>
        private void RequestDeferralButton_Click(object sender, RoutedEventArgs e)
        {
            if (_wholesaleCustomer == null) return;

            // Проверяем наличие активного договора
            if (_wholesaleCustomer.Contract == null || !_wholesaleCustomer.Contract.IsActive)
            {
                MessageBox.Show(
                    "Отсрочка платежа доступна только при наличии активного договора.",
                    "Требуется договор",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Создаем диалоговое окно
            var inputDialog = new Window
            {
                Title = "Запросить отсрочку платежа",
                Width = 350,
                SizeToContent = SizeToContent.Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this),
                ResizeMode = ResizeMode.NoResize
            };

            var stackPanel = new StackPanel { Margin = new Thickness(20, 20, 20, 20) };

            stackPanel.Children.Add(new TextBlock
            {
                Text = "Укажите количество дней отсрочки:",
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 5)
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = "(Максимум 30 дней)",
                FontSize = 11,
                Foreground = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(0, 0, 0, 10)
            });

            var textBox = new TextBox
            {
                Height = 35,
                FontSize = 14,
                Padding = new Thickness(5, 5, 5, 5),
                Margin = new Thickness(0, 0, 0, 15),
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = _wholesaleCustomer.PaymentDeferralDays > 0
                    ? _wholesaleCustomer.PaymentDeferralDays.ToString()
                    : "14"
            };
            stackPanel.Children.Add(textBox);

            // Текущая отсрочка
            if (_wholesaleCustomer.PaymentDeferralDays > 0)
            {
                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"Текущая: {_wholesaleCustomer.PaymentDeferralDays} дн.",
                    FontSize = 11,
                    Foreground = System.Windows.Media.Brushes.Blue,
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }

            // Кнопки
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 0, 0)
            };

            var okButton = new Button
            {
                Content = "Запросить",
                Width = 100,
                Height = 35,
                IsDefault = true,
                Padding = new Thickness(5, 5, 5, 5),
                Margin = new Thickness(0, 0, 10, 0)
            };
            okButton.Click += (s, args) => { inputDialog.DialogResult = true; };

            var cancelButton = new Button
            {
                Content = "Отмена",
                Width = 80,
                Height = 35,
                Padding = new Thickness(5, 5, 5, 5),
                IsCancel = true
            };

            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            stackPanel.Children.Add(buttonPanel);

            inputDialog.Content = stackPanel;

            // Показываем диалог
            if (inputDialog.ShowDialog() == true)
            {
                if (int.TryParse(textBox.Text, out int days) && days > 0)
                {
                    bool success = _wholesaleCustomer.RequestPaymentDeferral(days);

                    if (success)
                    {
                        UpdateUI();
                        MessageBox.Show(
                            $"Отсрочка на {days} дней одобрена!",
                            "Успех",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Не удалось запросить отсрочку.\nДни должны быть от 1 до 30.",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Введите корректное количество дней (от 1 до 30)",
                        "Ошибка ввода",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Отменить отсрочку платежа
        /// </summary>
        private void CancelDeferralButton_Click(object sender, RoutedEventArgs e)
        {
            if (_wholesaleCustomer == null || _wholesaleCustomer.PaymentDeferralDays == 0)
                return;

            var result = MessageBox.Show(
                $"Отменить отсрочку платежа на {_wholesaleCustomer.PaymentDeferralDays} дней?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Сбрасываем отсрочку
                _wholesaleCustomer.RequestPaymentDeferral(0); // Устанавливаем 0 дней

                // Альтернативный способ - через рефлексию или прямое обращение
                // Но метод RequestPaymentDeferral не принимает 0, поэтому используем прямое присвоение:
                var paymentDeferralDaysProperty = typeof(WholesaleCustomer).GetProperty("PaymentDeferralDays");
                if (paymentDeferralDaysProperty != null)
                {
                    paymentDeferralDaysProperty.SetValue(_wholesaleCustomer, 0);
                }

                UpdateUI();

                MessageBox.Show(
                    "Отсрочка платежа отменена",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}