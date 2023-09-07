using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Japanese.Utilities
{
    /// <summary>
    /// Converts a boolean value to a Visibility enumeration value and implements 'IValueConverter'.
    /// </summary>
    class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a Visibility value.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The type of the target property (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion (not used).</param>
        /// <returns>'Visibility.Visible' if the boolean value is true, 'Visibility.Hidden' otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            return boolValue ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Converts a Visibility value back to a boolean value (not implemented).
        /// </summary>
        /// <param name="value">The Visibility value to convert back (not used).</param>
        /// <param name="targetType">The type of the target property (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion (not used).</param>
        /// <returns>'NotImplementedException' is thrown as this method is not implemented.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
