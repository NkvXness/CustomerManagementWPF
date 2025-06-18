using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomerManagement.WPF.Converters
{
    /// <summary>
    /// Конвертер для форматирования валюты в BYN
    /// </summary>
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return $"{decimalValue:N2} BYN";
            }

            if (value is int intValue)
            {
                return $"{intValue:N2} BYN";
            }

            return "0,00 BYN";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}