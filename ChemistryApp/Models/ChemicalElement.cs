namespace ChemistryApp.Models
{
    /// <summary>
    /// Модель химического элемента. Содержит все данные для отображения в таблице Менделеева.
    /// </summary>
    public class ChemicalElement
    {
        /// <summary>
        /// Уникальный номер записи в базе данных.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Атомный номер элемента (число протонов). Определяет место в таблице.
        /// </summary>
        public int AtomicNumber { get; set; }
        /// <summary>
        /// Химический символ (например, "Fe", "O"). Используется для поиска.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Полное название элемента на русском языке.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Атомная масса элемента. Отображается в карточке элемента.
        /// </summary>
        public double AtomicMass { get; set; }
        /// <summary>
        /// Номер периода (горизонтальный ряд). Нужен для расчёта позиции на экране.
        /// </summary>
        public int PeriodNumber { get; set; }
        /// <summary>
        /// Номер группы (вертикальный столбец). Нужен для расчёта позиции на экране.
        /// </summary>
        public int GroupNumber { get; set; }
        /// <summary>
        /// Категория элемента (металл, неметалл и т.д.). Отображается в описании.
        /// </summary>
        public string Category { get; set; } = string.Empty;
        /// <summary>
        /// Агрегатное состояние при нормальных условиях.
        /// </summary>
        public string State { get; set; } = string.Empty;
        /// <summary>
        /// Текстовое описание свойств и применения элемента.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Координата X для размещения кнопки на холсте таблицы Менделеева.
        /// </summary>
        public double CanvasX { get; set; }
        /// <summary>
        /// Координата Y для размещения кнопки на холсте таблицы Менделеева.
        /// </summary>
        public double CanvasY { get; set; }
    }
}