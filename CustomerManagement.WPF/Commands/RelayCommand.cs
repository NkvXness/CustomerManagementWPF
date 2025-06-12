using System;
using System.Windows.Input;

namespace CustomerManagement.WPF.Commands
{
    /// <summary>
    /// Реализация ICommand для привязки команд к кнопкам в XAML
    /// Позволяет выполнять действия и проверять возможность выполнения
    /// </summary>
    public class RelayCommand : ICommand
    {
        // Делегат для действия, которое выполняет команда
        private readonly Action<object?> _execute;

        // Делегат для проверки, может ли команда быть выполнена
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Событие, которое срабатывает при изменении возможности выполнения команды
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор с обязательным действием
        /// </summary>
        /// <param name="execute">Действие для выполнения</param>
        /// <param name="canExecute">Проверка возможности выполнения (опционально)</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Конструктор для команд без параметров
        /// </summary>
        /// <param name="execute">Действие для выполнения</param>
        /// <param name="canExecute">Проверка возможности выполнения (опционально)</param>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
            : this(
                execute: _ => execute(),
                canExecute: canExecute != null ? _ => canExecute() : null)
        {
        }

        /// <summary>
        /// Определяет, может ли команда быть выполнена
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        /// <returns>True если команда может быть выполнена</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Выполняет команду
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Принудительно вызывает проверку CanExecute
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
