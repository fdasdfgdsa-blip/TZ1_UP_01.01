using System;
using System.Windows.Input;

namespace ChemistryApp.ViewModels
{
    /// <summary>
    /// Команда для привязки действий к кнопкам в XAML. Позволяет запускать код C# при клике.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;
        /// <summary>
        /// Создаёт команду, которая выполняет действие <paramref name="execute"/>.
        /// </summary>
        /// <param name="execute">Метод, который нужно запустить при нажатии.</param>
        /// <param name="canExecute">Условие (необязательно), разрешающее или запрещающее выполнение.</param>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
            : this(_ => execute(), canExecute == null ? null : _ => canExecute()) { }
        /// <summary>
        /// Создаёт команду с поддержкой параметра, который передаётся из интерфейса.
        /// </summary>
        /// <param name="execute">Метод, принимающий параметр.</param>
        /// <param name="canExecute">Условие доступности команды.</param>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        /// <summary>
        /// Событие: уведомляет интерфейс, что изменилось состояние доступности команды.
        /// </summary>
        public event EventHandler? CanExecuteChanged;
        /// <summary>
        /// Проверяет, можно ли сейчас выполнить команду (например, не пустой ли ввод).
        /// </summary>
        /// <param name="parameter">Дополнительные данные от интерфейса.</param>
        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
        /// <summary>
        /// Запускает выполнение команды (вызывает переданный при создании метод).
        /// </summary>
        /// <param name="parameter">Дополнительные данные от интерфейса.</param>
        public void Execute(object? parameter) => _execute(parameter);
        /// <summary>
        /// Принудительно обновляет состояние кнопки в интерфейсе (включена/выключена).
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}