using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using MovieTube.Functions;

namespace MovieTube.UI.Converters
{
    public class ImageURLConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (parameter as string)
            {
                case "posters":
                    return Common.POSTERS + (string)value;
                case "actors":
                    return Common.ACTORS + (string)value;
                case "banners":
                    return Common.BANNERS + (string)value;
                case "movies_photos":
                    return Common.MOVIES_PHOTOS + (string)value;
                case "users":
                    return Common.USERS + (string)value;
                default:
                    break;
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
