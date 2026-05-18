using System.Windows;
using ChemistryApp.ViewModels;

namespace ChemistryApp
{
    /// <summary>
    /// Главное окно приложения. Содержит меню навигации и область для отображения контента.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор: устанавливает модель представления и запускает загрузку тем.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            DataContext = vm;
            Dispatcher.BeginInvoke(new System.Action(async () => await vm.LoadTopicsAsync()));
        }
    }
}