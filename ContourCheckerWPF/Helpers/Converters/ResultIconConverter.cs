using MahApps.Metro.IconPacks.Converter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ContourChecker.UI.Helpers.Converters
{
    public class ResultIconConverter : PackIconKindToImageConverter, IValueConverter
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool didPass)
            {
                if (didPass)
                {
                    return CreateImageSource(MahApps.Metro.IconPacks.PackIconMaterialKind.CheckboxMarkedCircle, new SolidColorBrush(Colors.GreenYellow));
                }
                else
                {
                    return CreateImageSource(MahApps.Metro.IconPacks.PackIconMaterialKind.AlertCircle, new SolidColorBrush(Colors.OrangeRed));
                }
            }

            return null;
        }
    }
}
