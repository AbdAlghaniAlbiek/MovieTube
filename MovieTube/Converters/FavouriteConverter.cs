using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace MovieTube.UI.Converters
{
    public class FavouriteConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0)
            {
                if (MovieTube.UI.Services.AppSettings.Settings.Theme == "Light")
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/MovieDetailsViewImages/Favourite_Light_20px.png"));
                }
                else if (MovieTube.UI.Services.AppSettings.Settings.Theme == "Dark")
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/MovieDetailsViewImages/Favourite_20px.png"));
                }
                else
                {
                    var DefaultTheme = new Windows.UI.ViewManagement.UISettings();
                    var uiTheme = DefaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();

                    if (uiTheme == "#FFFFFFFF")
                    {
                        return new BitmapImage(new Uri("ms-appx:///Assets/Images/MovieDetailsViewImages/Favourite_Light_20px.png"));
                    }
                    else if (uiTheme == "#FF000000")
                    {
                        return new BitmapImage(new Uri("ms-appx:///Assets/Images/MovieDetailsViewImages/Favourite_20px.png"));
                    }
                }
            }
            else
            {
                return new BitmapImage(new Uri("ms-appx:///Assets/Images/MovieDetailsViewImages/Favourite_Pressed_20px.png"));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
