using System.Windows.Controls;
namespace ChemistryApp.Views
{
    /// <summary>
    /// Страница интерактивной таблицы Менделеева. Отображает элементы и обрабатывает выбор.
    /// </summary>
    public partial class PeriodicTableView : UserControl
    {
        /// <summary>
        /// Конструктор: инициализирует интерфейс и назначает модель представления.
        /// </summary>
        public PeriodicTableView()
        {
            InitializeComponent();
            DataContext = new ViewModels.PeriodicTableViewModel();
        }
    }
}