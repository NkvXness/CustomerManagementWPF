using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CustomerManagement.WPF.ViewModels;

namespace CustomerManagement.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Подписываемся на изменение выбранного покупателя
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(MainViewModel.SelectedCustomer))
                    {
                        ContractControlElement.SetCustomer(viewModel.SelectedCustomer);
                    }
                };
            }
        }

        /// <summary>
        /// Обработчик клика по DataGrid - открывает панель даже при повторном клике
        /// </summary>
        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем элемент, по которому кликнули
            var dep = (DependencyObject)e.OriginalSource;

            // Поднимаемся по визуальному дереву до DataGridRow
            while (dep != null && dep is not DataGridRow && dep is not DataGrid)
            {
                dep = System.Windows.Media.VisualTreeHelper.GetParent(dep);
            }

            // Если кликнули на строку
            if (dep is DataGridRow row)
            {
                if (DataContext is MainViewModel viewModel)
                {
                    // Открываем панель независимо от того, выбрана ли уже эта строка
                    viewModel.IsDetailsPanelVisible = true;
                }
            }
        }

        /// <summary>
        /// Клик по оверлею - закрыть панель
        /// </summary>
        private void Overlay_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.HideDetailsCommand.Execute(null);
            }
        }
    }
}