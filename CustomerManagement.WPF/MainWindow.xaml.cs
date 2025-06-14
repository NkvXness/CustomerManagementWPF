using System.Windows;
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