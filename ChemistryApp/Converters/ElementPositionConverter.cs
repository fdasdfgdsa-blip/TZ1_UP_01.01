using System;
using System.Globalization;
using System.Windows.Data;

namespace ChemistryApp.Converters
{
    public class ElementPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] is not int period || values[1] is not int group)
                return null;

            // Простая сетка: период = строка, группа = колонка. Умножаем на 55 для отступов.
            double x = (group - 1) * 55;
            double y = (period - 1) * 55;
            if (period >= 6 && group >= 3) y += 60; // Смещение для лантаноидов/актиноидов
            return new System.Windows.Point(x, y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}