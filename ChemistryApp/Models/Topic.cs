namespace ChemistryApp.Models
{
    /// <summary>
    /// Модель учебной темы. Хранит информацию об одной теме курса: её название и описание.
    /// </summary>
    public class Topic
    {
        /// <summary>
        /// Уникальный номер темы в базе данных. Используется для загрузки контента.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название темы, которое отображается в меню слева.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Краткое описание темы для предварительного ознакомления.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}