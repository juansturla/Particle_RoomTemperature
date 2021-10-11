using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Particle_RoomTemperature.Models;

namespace Particle_RoomTemperature.Converters
{
    public class VariationToImage:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var variation = (Variation)value;
            
            return variation switch
            {
                Variation.HIGHER => "arrow_downward",
                Variation.LOWER => "arrow_upward",
                _ => "arrow_right"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

