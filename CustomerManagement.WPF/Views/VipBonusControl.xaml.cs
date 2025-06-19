using System;
using System.Windows;
using System.Windows.Controls;
using CustomerManagement.Core.Models;

namespace CustomerManagement.WPF.Views
{
    public partial class VipBonusControl : UserControl
    {
        private VIPCustomer? _vipCustomer;

        public VipBonusControl()
        {
            InitializeComponent();
            UpdateUI();
        }

        /// <summary>
        /// Установить VIP покупателя
        /// </summary>
        public void SetCustomer(Customer? customer)
        {
            _vipCustomer = customer as VIPCustomer;
            UpdateUI();
        }

        /// <summary>
        /// Обновить отображение
        /// </summary>
        private void UpdateUI()
        {
            if (_vipCustomer == null)
            {
                IsEnabled = false;
                BonusPointsText.Text = "0";
                PersonalManagerText.Text = "Не назначен";
                return;
            }

            IsEnabled = true;
            BonusPointsText.Text = _vipCustomer.BonusPoints.ToString("N0");
            PersonalManagerText.Text = string.IsNullOrEmpty(_vipCustomer.PersonalManager) || _vipCustomer.PersonalManager == "Не назначен"
                ? "Не назначен"
                : _vipCustomer.PersonalManager;
        }

        /// <summary>
        /// Использовать бонусные баллы
        /// </summary>
        private void UsePointsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vipCustomer == null || _vipCustomer.BonusPoints == 0)
            {
                MessageBox.Show("Недостаточно бонусных баллов",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var inputDialog = new Window
            {
                Title = "Списать баллы",
                Width = 300,
                SizeToContent = SizeToContent.Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this),
                ResizeMode = ResizeMode.NoResize
            };

            var stackPanel = new StackPanel { Margin = new Thickness(20, 20, 20, 20) };

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Доступно баллов: {_vipCustomer.BonusPoints:N0}",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = "Сколько баллов списать?",
                Margin = new Thickness(0, 0, 0, 5)
            });

            var textBox = new TextBox { Height = 30, FontSize = 14, Margin = new Thickness(0, 0, 0, 15) };
            stackPanel.Children.Add(textBox);

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 0, 0)
            };

            var okButton = new Button
            {
                Content = "OK",
                Width = 80,
                Height = 35,
                Padding = new Thickness(5, 5, 5, 5),
                IsDefault = true,
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

            if (inputDialog.ShowDialog() == true)
            {
                if (decimal.TryParse(textBox.Text, out decimal points) && points > 0)
                {
                    try
                    {
                        bool success = _vipCustomer.UseBonusPoints(points);
                        if (success)
                        {
                            UpdateUI();
                            MessageBox.Show($"Списано {points:N0} баллов",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Недостаточно бонусных баллов для списания",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректное количество баллов",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Назначить персонального менеджера
        /// </summary>
        private void AssignManagerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_vipCustomer == null) return;

            var inputDialog = new Window
            {
                Title = "Назначить менеджера",
                Width = 350,
                SizeToContent = SizeToContent.Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this),
                ResizeMode = ResizeMode.NoResize
            };

            var stackPanel = new StackPanel { Margin = new Thickness(20, 20, 20, 20) };

            stackPanel.Children.Add(new TextBlock
            {
                Text = "Введите имя персонального менеджера:",
                Margin = new Thickness(0, 0, 0, 10)
            });

            var textBox = new TextBox
            {
                Height = 35,
                FontSize = 14,
                Padding = new Thickness(5, 5, 5, 5),
                Margin = new Thickness(0, 0, 0, 15),
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = _vipCustomer.PersonalManager == "Не назначен" ? "" : _vipCustomer.PersonalManager
            };
            stackPanel.Children.Add(textBox);

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 0, 0)
            };

            var okButton = new Button
            {
                Content = "Сохранить",
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

            if (inputDialog.ShowDialog() == true)
            {
                string managerName = textBox.Text.Trim();
                if (!string.IsNullOrEmpty(managerName))
                {
                    try
                    {
                        _vipCustomer.AssignPersonalManager(managerName);
                        UpdateUI();
                        MessageBox.Show($"Менеджер '{managerName}' назначен",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}