﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApplication3
{
    public class testConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //this is trick to debug the xaml control. When you set a break here you can examine the value
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This method should never be called");
        }
    }
}
