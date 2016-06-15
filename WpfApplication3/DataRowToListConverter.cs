using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Data;

namespace MyImageLoader
{
    [ValueConversion(typeof(DataRowView), typeof(List<Image>))]
    class DataRowToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Image> li = new List<Image>();
            if (value==null)
            {
                return li;
            }
            else
            {
                var item = (DataRowView)value;
                //now the row consists only of items, but in the firs row we want the list of images
                //which is now a property of Item, so in that case replace the row item by this property
                if (item[0].GetType() == typeof(Item))
                {
                    Item thisItem = (Item)item[0];
                    return thisItem.Im;
                }
                else
                {
                    return item[0]; 
                }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
