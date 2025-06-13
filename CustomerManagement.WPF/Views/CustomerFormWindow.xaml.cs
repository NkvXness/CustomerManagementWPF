using CustomerManagement.Core.Factories;
using CustomerManagement.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CustomerManagement.WPF.Views
{
    /// <summary>
    /// Окно для добавления/редактирования покупателя
    /// </summary>
    public partial class CustomerFormWindow : Window
    {
        /// <summary>
        /// Созданный или отредактированный покупатель
        /// </summary>
        public Customer? ResultCustomer { get; private set; }

        /// <summary>
        /// Режим редактирования
        /// </summary>
        private readonly bool _isEditMode;

        /// <summary>
        /// Конструктор для добавления нового покупателя
        /// </summary>
        public CustomerFormWindow()
        {
            InitializeComponent();
            _isEditMode = false;
            CustomerTypeComboBox.SelectedIndex = 0; // Regular по умолчанию
        }

        /// <summary>
        /// Конструктор для редактирования существующего покупателя
        /// </summary>
        public CustomerFormWindow(Customer customer) : this()
        {
            _isEditMode = true;
            Title = "Редактирование покупателя";

            // Заполняем поля данными
            FullNameTextBox.Text = customer.FullName;
            EmailTextBox.Text = customer.Email;
            PhoneTextBox.Text = customer.Phone;
            AddressTextBox.Text = customer.Address;

            // Устанавливаем тип
            CustomerTypeComboBox.SelectedIndex = customer.Type switch
            {
                CustomerType.Regular => 0,
                CustomerType.Wholesale => 1,
                CustomerType.VIP => 2,
                _ => 0
            };

            ResultCustomer = customer;
        }

        /// <summary>
        /// Обработчик кнопки "Сохранить"
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (!ValidateInput())
            {
                return;
            }

            // Получаем тип покупателя
            var selectedItem = (ComboBoxItem)CustomerTypeComboBox.SelectedItem;
            var typeTag = selectedItem.Tag.ToString();
            var customerType = typeTag switch
            {
                "Regular" => CustomerType.Regular,
                "Wholesale" => CustomerType.Wholesale,
                "VIP" => CustomerType.VIP,
                _ => CustomerType.Regular
            };

            if (_isEditMode && ResultCustomer != null)
            {
                // Режим редактирования - обновляем существующего
                var data = new CustomerData(
                    FullNameTextBox.Text.Trim(),
                    EmailTextBox.Text.Trim(),
                    PhoneTextBox.Text.Trim(),
                    AddressTextBox.Text.Trim()
                );
                ResultCustomer.UpdatePersonalData(data);
            }
            else
            {
                // Режим создания - создаем нового
                ResultCustomer = CustomerFactory.CreateCustomer(
                    customerType,
                    FullNameTextBox.Text.Trim(),
                    EmailTextBox.Text.Trim(),
                    PhoneTextBox.Text.Trim(),
                    AddressTextBox.Text.Trim()
                );
            }

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Обработчик кнопки "Отмена"
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Валидация введенных данных
        /// </summary>
        private bool ValidateInput()
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                ShowError("Пожалуйста, введите ФИО");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                ShowError("Пожалуйста, введите Email");
                return false;
            }

            if (!EmailTextBox.Text.Contains("@"))
            {
                ShowError("Email должен содержать символ @");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                ShowError("Пожалуйста, введите телефон");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Показать сообщение об ошибке
        /// </summary>
        private void ShowError(string message)
        {
            ErrorMessageTextBlock.Text = message;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }
    }
}