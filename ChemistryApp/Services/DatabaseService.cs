using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChemistryApp.Models;
using Npgsql;

namespace ChemistryApp.Services
{
    /// <summary>
    /// Сервис для работы с базой данных PostgreSQL. Отвечает за получение тем, элементов и контента.
    /// </summary>
    public static class DatabaseService
    {
        /// <summary>
        /// Возвращает строку подключения из настроек приложения.
        /// </summary>
        private static string ConnectionString => App.ConnectionString;
        /// <summary>
        /// Проверяет, пуста ли база, и если да — заполняет её тестовыми данными.
        /// </summary>
        public static async Task InitializeDatabaseAsync()
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM chemical_elements;", conn);
            if ((long)await cmd.ExecuteScalarAsync() == 0)
                await GenerateTestDataAsync(conn);
        }
        /// <summary>
        /// Генерирует тестовые данные для 118 элементов, если таблица пуста.
        /// </summary>
        /// <param name="conn">Активное подключение к базе данных.</param>
        private static async Task GenerateTestDataAsync(NpgsqlConnection conn)
        {
            // Генерация тестовых данных, если таблица пуста
            var insert = "INSERT INTO chemical_elements (atomic_number, symbol, name, atomic_mass, period_number, group_number, category, state, description) VALUES (@an, @sym, @name, @mass, @p, @g, @cat, @st, @desc);";
            var rnd = new Random();
            for (int i = 1; i <= 118; i++)
            {
                using var c = new NpgsqlCommand(insert, conn);
                c.Parameters.AddWithValue("an", i);
                c.Parameters.AddWithValue("sym", i < 10 ? $"H{i}" : $"El{i}");
                c.Parameters.AddWithValue("name", $"Элемент {i}");
                c.Parameters.AddWithValue("mass", Math.Round(i * 1.5 + rnd.NextDouble(), 3));
                c.Parameters.AddWithValue("p", GetPeriod(i));
                c.Parameters.AddWithValue("g", GetGroup(i));
                c.Parameters.AddWithValue("cat", i <= 4 ? "Неметалл" : "Металл");
                c.Parameters.AddWithValue("st", i < 3 ? "Газ" : "Твёрдое");
                c.Parameters.AddWithValue("desc", "Тестовое описание.");
                await c.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Вычисляет номер периода (горизонтального ряда) для элемента по его атомному номеру.
        /// </summary>
        /// <param name="n">Атомный номер элемента.</param>
        private static int GetPeriod(int n) => n <= 2 ? 1 : n <= 10 ? 2 : n <= 18 ? 3 : n <= 36 ? 4 : n <= 54 ? 5 : n <= 86 ? 6 : 7;
        /// <summary>
        /// Вычисляет номер группы (вертикального столбца) для элемента по его атомному номеру.
        /// </summary>
        /// <param name="n">Атомный номер элемента.</param>
        private static int GetGroup(int n) => (n % 18 == 0) ? 18 : (n % 18);
        /// <summary>
        /// Загружает список всех тем из базы данных для отображения в меню.
        /// </summary>
        public static async Task<List<Topic>> GetTopicsAsync()
        {
            var list = new List<Topic>();
            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, title, description FROM topics ORDER BY id;", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new Topic { Id = reader.GetInt32(0), Title = reader.GetString(1) ?? "", Description = reader.GetString(2) ?? "" });
            return list;
        }
        /// <summary>
        /// Загружает текстовый контент для указанной темы из базы данных.
        /// </summary>
        /// <param name="topicId">Идентификатор темы.</param>
        public static async Task<string> GetTopicContentAsync(int topicId)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT content_text FROM topic_content WHERE topic_id = @id ORDER BY sort_order;", conn);
            cmd.Parameters.AddWithValue("id", topicId);
            using var reader = await cmd.ExecuteReaderAsync();
            var content = "";
            while (await reader.ReadAsync()) content += (reader.GetString(0) ?? "") + "\n\n";
            return content.Trim();
        }
        /// <summary>
        /// Загружает все химические элементы и рассчитывает их координаты для отображения на таблице.
        /// </summary>
        public static async Task<List<ChemicalElement>> GetElementsAsync()
        {
            var list = new List<ChemicalElement>();
            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT * FROM chemical_elements ORDER BY atomic_number;", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            const double step = 43.0; // Шаг сетки
            while (await reader.ReadAsync())
            {
                var period = reader.GetInt32(5);
                var group = reader.GetInt32(6);
                double canvasX = (group - 1) * step;
                double canvasY = (period - 1) * step;
                if ((period == 6 || period == 7) && group >= 3) canvasY += step * 2; // Сдвиг для лантаноидов
                list.Add(new ChemicalElement
                {
                    Id = reader.GetInt32(0),
                    AtomicNumber = reader.GetInt32(1),
                    Symbol = reader.GetString(2) ?? "",
                    Name = reader.GetString(3) ?? "",
                    AtomicMass = reader.GetDouble(4),
                    PeriodNumber = period,
                    GroupNumber = group,
                    Category = reader.GetString(7) ?? "",
                    State = reader.GetString(8) ?? "",
                    Description = reader.GetString(9) ?? "",
                    CanvasX = canvasX,
                    CanvasY = canvasY
                });
            }
            return list;
        }
    }
}