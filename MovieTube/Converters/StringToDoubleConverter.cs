using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MovieTube.UI.Converters
{
    public class StringToDoubleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string strVal = value as string;
            return double.Parse(strVal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
