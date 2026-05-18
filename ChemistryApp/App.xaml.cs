using System.Windows;
using Npgsql;

namespace ChemistryApp
{
    /// <summary>
    /// Главный класс приложения. Отвечает за запуск программы и обработку ошибок подключения к базе данных.
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// Строка подключения к базе данных PostgreSQL. Используется всеми сервисами для работы с данными.
        /// </summary>
        public static string ConnectionString = "Host=localhost;Username=postgres;Password=sa;Database=chemistry_db;Port=5432;";

        /// <summary>
        /// Метод запускается при старте приложения. Создаёт главное окно и ловит ошибки подключения к БД.
        /// </summary>
        /// <param name="e">Параметры события запуска.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Npgsql.PostgresException ex)
            {
                MessageBox.Show($"Ошибка БД: {ex.Message}\nПроверьте подключение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}