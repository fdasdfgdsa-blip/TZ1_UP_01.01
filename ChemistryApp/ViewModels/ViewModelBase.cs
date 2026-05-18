using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ChemistryApp.ViewModels
{
    /// <summary>
    /// Базовый класс для всех моделей представления (ViewModel).
    /// Реализует уведомление об изменении свойств и валидацию данных для паттерна MVVM.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new();
        /// <summary>
        /// Событие: вызывается при изменении значения свойства, чтобы обновить интерфейс.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// Событие: вызывается при изменении ошибок валидации, чтобы показать/скрыть сообщение.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        /// <summary>
        /// Уведомляет интерфейс, что свойство с именем <paramref name="propertyName"/> изменилось.
        /// </summary>
        /// <param name="propertyName">Имя изменившегося свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        /// <summary>
        /// Сохраняет список ошибок для свойства и уведомляет интерфейс об обновлении.
        /// </summary>
        /// <param name="propertyName">Имя свойства, в котором возникла ошибка.</param>
        /// <param name="errors">Список текстов ошибок.</param>
        protected void SetErrors(string propertyName, List<string> errors)
        {
            _errors[propertyName] = errors;
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        /// <summary>
        /// Возвращает true, если в текущей модели есть хотя бы одна ошибка валидации.
        /// </summary>
        public bool HasErrors => _errors.Any(x => x.Value.Count > 0);
        /// <summary>
        /// Возвращает список ошибок для указанного свойства. Если ошибок нет — возвращает пустой список.
        /// </summary>
        /// <param name="propertyName">Имя проверяемого свойства.</param>
        public IEnumerable GetErrors(string? propertyName) =>
            _errors.TryGetValue(propertyName ?? "", out var errors) ? errors : Enumerable.Empty<string>();
    }
}