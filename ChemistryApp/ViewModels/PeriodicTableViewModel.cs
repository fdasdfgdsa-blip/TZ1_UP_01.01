using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using ChemistryApp.Models;
using ChemistryApp.Services;

namespace ChemistryApp.ViewModels
{
    /// <summary>
    /// Модель представления для таблицы Менделеева. Обрабатывает поиск, фильтрацию и выбор элементов.
    /// </summary>  
    public class PeriodicTableViewModel : ViewModelBase
    {
        /// <summary>
        /// Полный список всех химических элементов, загруженных из базы данных.
        /// </summary>
        public ObservableCollection<ChemicalElement> Elements { get; } = new();
        /// <summary>
        /// Отфильтрованный список элементов, который отображается на экране (результат поиска).
        /// </summary>
        public ObservableCollection<ChemicalElement> DisplayedElements { get; } = new();

        private string _searchText = "";
        /// <summary>
        /// Текст, введённый пользователем в поле поиска. При изменении запускает фильтрацию и валидацию.
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ValidateSearch(); // Проверка ввода
                FilterElements(); // Фильтрация списка
            }
        }

        private ChemicalElement _selectedElement = new();
        /// <summary>
        /// Элемент, который пользователь выбрал кликом. Его данные показываются в правой панели.
        /// </summary>
        public ChemicalElement SelectedElement
        {
            get => _selectedElement;
            set { _selectedElement = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Команда: обрабатывает клик по элементу таблицы и сохраняет его в SelectedElement.
        /// </summary>
        public ICommand SelectCommand { get; }
        /// <summary>
        /// Конструктор: инициализирует команду выбора и запускает загрузку данных.
        /// </summary>
        public PeriodicTableViewModel()
        {
            SelectCommand = new RelayCommand(obj =>
            {
                if (obj is ChemicalElement el) SelectedElement = el;
            });
            LoadAsync();
        }
        /// <summary>
        /// Асинхронно загружает элементы из БД и обновляет список для отображения.
        /// </summary>
        private async void LoadAsync()
        {
            var list = await DatabaseService.GetElementsAsync();
            Elements.Clear();
            foreach (var e in list) Elements.Add(e);
            FilterElements();
        }
        /// <summary>
        /// Фильтрует список элементов по поисковому запросу (по названию, символу или номеру).
        /// </summary>
        private void FilterElements()
        {
            DisplayedElements.Clear();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var e in Elements) DisplayedElements.Add(e);
                return;
            }
            var filter = SearchText.ToLower();
            foreach (var e in Elements.Where(x =>
                x.Name.ToLower().Contains(filter) ||
                x.Symbol.ToLower().Contains(filter) ||
                x.AtomicNumber.ToString().Contains(filter)))
            {
                DisplayedElements.Add(e);
            }
        }
        /// <summary>
        /// Проверяет текст поиска на допустимые символы и наличие результатов. Добавляет ошибки при необходимости.
        /// </summary>
        private void ValidateSearch()
        {
            var errors = new System.Collections.Generic.List<string>();
            if (!Regex.IsMatch(SearchText, @"^[a-zA-Zа-яА-Я0-9\s]*$") && !string.IsNullOrEmpty(SearchText))
                errors.Add("Разрешены только буквы, цифры и пробелы.");
            if (DisplayedElements.Count == 0 && !string.IsNullOrWhiteSpace(SearchText) && errors.Count == 0)
                errors.Add("Элемент не найден.");
            SetErrors("SearchText", errors);
        }
    }
}