using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CustomerManagement.Core.Factories;
using CustomerManagement.Core.Models;
using CustomerManagement.Core.Repositories;
using CustomerManagement.WPF.Commands;

namespace CustomerManagement.WPF.ViewModels
{
    /// <summary>
    /// ViewModel для главного окна приложения
    /// Управляет списком покупателей и основными операциями
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Репозиторий для работы с покупателями
        private readonly ICustomerRepository _customerRepository;

        // Приватное поле для выбранного покупателя
        private Customer? _selectedCustomer;

        // Приватное поле для видимости панели деталей
        private bool _isDetailsPanelVisible;

        /// <summary>
        /// Видимость панели деталей
        /// </summary>
        public bool IsDetailsPanelVisible
        {
            get => _isDetailsPanelVisible;
            set => SetProperty(ref _isDetailsPanelVisible, value);
        }

        /// <summary>
        /// Коллекция покупателей для отображения в UI
        /// ObservableCollection автоматически уведомляет UI об изменениях
        /// </summary>
        public ObservableCollection<Customer> Customers { get; }

        /// <summary>
        /// Выбранный покупатель в списке
        /// </summary>
        /// <summary>
        /// Выбранный покупатель в списке
        /// </summary>
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        /// <summary>
        /// Команда добавления покупателя
        /// </summary>
        public ICommand AddCustomerCommand { get; }

        /// <summary>
        /// Команда удаления покупателя
        /// </summary>
        public ICommand DeleteCustomerCommand { get; }

        /// <summary>
        /// Команда редактирования покупателя
        /// </summary>
        public ICommand EditCustomerCommand { get; }

        /// <summary>
        /// Команда обновления списка
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Команда открытия панели деталей
        /// </summary>
        public ICommand ShowDetailsCommand { get; }

        /// <summary>
        /// Команда закрытия панели деталей
        /// </summary>
        public ICommand HideDetailsCommand { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            // Инициализация репозитория
            _customerRepository = new CustomerRepository();

            // Инициализация коллекции
            Customers = new ObservableCollection<Customer>();

            // Инициализация команд
            AddCustomerCommand = new RelayCommand(AddCustomer);
            DeleteCustomerCommand = new RelayCommand(DeleteCustomer, CanDeleteCustomer);
            EditCustomerCommand = new RelayCommand(EditCustomer, CanEditCustomer);
            RefreshCommand = new RelayCommand(RefreshCustomers);
            ShowDetailsCommand = new RelayCommand(ShowDetails);
            HideDetailsCommand = new RelayCommand(HideDetails);

            // Загружаем тестовых покупателей
            LoadTestData();
        }

        /// <summary>
        /// Добавить нового покупателя
        /// </summary>
        private void AddCustomer()
        {
            var formWindow = new Views.CustomerFormWindow();
            formWindow.Owner = Application.Current.MainWindow;

            if (formWindow.ShowDialog() == true && formWindow.ResultCustomer != null)
            {
                _customerRepository.Add(formWindow.ResultCustomer);
                Customers.Add(formWindow.ResultCustomer);
                SelectedCustomer = formWindow.ResultCustomer;
            }
        }

        /// <summary>
        /// Удалить выбранного покупателя
        /// </summary>
        private void DeleteCustomer()
        {
            if (SelectedCustomer != null)
            {
                _customerRepository.Remove(SelectedCustomer.CustomerId);
                Customers.Remove(SelectedCustomer);
                SelectedCustomer = null;
                IsDetailsPanelVisible = false;
            }
        }

        /// <summary>
        /// Проверить, можно ли удалить покупателя
        /// </summary>
        private bool CanDeleteCustomer()
        {
            return SelectedCustomer != null;
        }

        /// <summary>
        /// Обновить список покупателей
        /// </summary>
        private void RefreshCustomers()
        {
            Customers.Clear();
            var customers = _customerRepository.GetAll();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }

        /// <summary>
        /// Загрузить тестовые данные
        /// </summary>
        private void LoadTestData()
        {
            // Создаем трех тестовых покупателей
            var regular = CustomerFactory.CreateTestCustomer(CustomerType.Regular);
            var wholesale = CustomerFactory.CreateTestCustomer(CustomerType.Wholesale);
            var vip = CustomerFactory.CreateTestCustomer(CustomerType.VIP);

            // Добавляем в репозиторий
            _customerRepository.Add(regular);
            _customerRepository.Add(wholesale);
            _customerRepository.Add(vip);

            // Добавляем в коллекцию для UI
            Customers.Add(regular);
            Customers.Add(wholesale);
            Customers.Add(vip);
        }

        /// <summary>
        /// Редактировать выбранного покупателя
        /// </summary>
        private void EditCustomer()
        {
            if (SelectedCustomer == null) return;

            var formWindow = new Views.CustomerFormWindow(SelectedCustomer);
            formWindow.Owner = Application.Current.MainWindow;

            if (formWindow.ShowDialog() == true)
            {
                _customerRepository.Update(SelectedCustomer);
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        /// <summary>
        /// Проверка на возможность редактирования покупателя
        /// </summary>
        private bool CanEditCustomer()
        {
            return SelectedCustomer != null;
        }

        /// <summary>
        /// Показать панель деталей
        /// </summary>
        private void ShowDetails()
        {
            if (SelectedCustomer != null)
            {
                IsDetailsPanelVisible = true;
            }
        }

        /// <summary>
        /// Скрыть панель деталей
        /// </summary>
        private void HideDetails()
        {
            IsDetailsPanelVisible = false;
        }
    }
}