using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maurice.Core.Services
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("C2", CultureInfo.CurrentCulture);
            }
            if (value is double doubleValue)
            {
                return doubleValue.ToString("C2", CultureInfo.CurrentCulture);
            }
            if (value is int intValue)
            {
                return intValue.ToString("C2", CultureInfo.CurrentCulture);
            }
            return "$0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue)
            {
                // Use "C" format to parse with culture-specific currency settings
                if (decimal.TryParse(stringValue, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal parsedValue))
                {
                    return parsedValue;
                }
            }

            // Return a fallback or the original value if parsing fails
            return value;
        }
    }
}
