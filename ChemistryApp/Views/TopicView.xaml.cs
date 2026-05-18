using System.Threading.Tasks;
using System.Windows.Controls;
using ChemistryApp.Models;
using ChemistryApp.Services;
namespace ChemistryApp.Views
{
    /// <summary>
    /// Страница отображения темы. Загружает и показывает текст выбранной темы.
    /// </summary>
    public partial class TopicView : UserControl
    {
        /// <summary>
        /// Конструктор: принимает тему и загружает её контент при отображении.
        /// </summary>
        /// <param name="topic">Объект темы, который нужно показать.</param>
        public TopicView(Topic topic)
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                HeaderText.Text = topic.Title;
                ContentText.Text = await DatabaseService.GetTopicContentAsync(topic.Id);
            };
        }
    }
}