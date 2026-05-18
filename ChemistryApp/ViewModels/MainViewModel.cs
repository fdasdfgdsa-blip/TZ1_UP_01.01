using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ChemistryApp.Models;
using ChemistryApp.Services;
using ChemistryApp.Views;

namespace ChemistryApp.ViewModels
{
    /// <summary>
    /// Главная модель представления окна. Управляет переключением между темами и таблицей Менделеева.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Список тем, загруженных из базы данных. Отображается в меню слева.
        /// </summary>
        public ObservableCollection<Topic> Topics { get; } = new();
        public object CurrentView { get; set; } = new WelcomeView();

        /// <summary>
        /// Команда: загружает список тем из базы данных при старте.
        /// </summary>
        public ICommand LoadTopicsCommand { get; }
        /// <summary>
        /// Команда: открывает выбранную пользователем тему.
        /// </summary>
        public ICommand SelectTopicCommand { get; }
        /// <summary>
        /// Команда: переключает вид на интерактивную таблицу Менделеева.
        /// </summary>
        public ICommand OpenPeriodicTableCommand { get; }

        /// <summary>
        /// Конструктор: инициализирует команды для кнопок меню.
        /// </summary>
        public MainViewModel()
        {
            LoadTopicsCommand = new RelayCommand(async _ => await LoadTopicsAsync());
            SelectTopicCommand = new RelayCommand(obj =>
            {
                if (obj is Topic topic)
                {
                    CurrentView = new TopicView(topic);
                    OnPropertyChanged(nameof(CurrentView));
                }
            });
            OpenPeriodicTableCommand = new RelayCommand(_ =>
            {
                CurrentView = new PeriodicTableView();
                OnPropertyChanged(nameof(CurrentView));
            });
        }
        /// <summary>
        /// Асинхронно загружает темы из базы данных и добавляет их в список для отображения.
        /// </summary>
        public async Task LoadTopicsAsync()
        {
            await DatabaseService.InitializeDatabaseAsync();
            var topics = await DatabaseService.GetTopicsAsync();
            Topics.Clear();
            foreach (var t in topics) Topics.Add(t);
        }
    }
}