using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Moonstocks.Converters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        #region -- Objects --
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Create flag and set to false
            bool flag = false;

            // If value is a bool update flag to boolean
            if (value is bool)
                flag = (bool)value;
            else
            {
                // If value is a nullable boolean update flag to nullable boolean
                if (value is bool?)
                {
                    bool? flag2 = (bool?)value;
                    flag = (flag2.HasValue && flag2.Value);
                }
            }

            //If false is passed as a converter parameter then reverse the value of input value
            if (parameter != null)
            {
                bool par = true;
                if ((bool.TryParse(parameter.ToString(), out par)) && (!par)) flag = !flag;
            }

            // Return visible or collapsed
            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If value is visible return true else false
            if (value is Visibility)
                return (Visibility)value == Visibility.Visible;

            return false;
        }
        #endregion

        #region -- Constructor --
        public BooleanToVisibilityConverter()
        {
        }
        #endregion
    }
}
