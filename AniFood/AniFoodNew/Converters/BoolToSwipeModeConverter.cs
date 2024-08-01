using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Converters
{
    internal class BoolToSwipeModeConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is bool canExecuteOnSwipe)
                return canExecuteOnSwipe ? SwipeMode.Execute : SwipeMode.Reveal;
            else
                return SwipeMode.Reveal;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
