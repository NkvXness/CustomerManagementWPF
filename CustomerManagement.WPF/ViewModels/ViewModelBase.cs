using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomerManagement.WPF.ViewModels
{
    /// <summary>
    /// Базовый класс для всех ViewModel
    /// Реализует INotifyPropertyChanged для автоматического обновления UI
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, которое срабатывает при изменении свойства
        /// WPF автоматически обновляет UI когда это событие вызывается
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged для указанного свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства (автоматически подставляется компилятором)</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Устанавливает значение свойства и вызывает OnPropertyChanged если значение изменилось
        /// Упрощает написание свойств в наследниках
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <param name="field">Ссылка на поле</param>
        /// <param name="value">Новое значение</param>
        /// <param name="propertyName">Имя свойства (автоматически)</param>
        /// <returns>True если значение было изменено</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            // Проверяем, изменилось ли значение
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false; // Значение не изменилось
            }

            // Устанавливаем новое значение
            field = value;

            // Уведомляем UI об изменении
            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Вызывает OnPropertyChanged для нескольких свойств
        /// Полезно когда изменение одного свойства влияет на другие
        /// </summary>
        /// <param name="propertyNames">Имена свойств</param>
        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }
    }
}