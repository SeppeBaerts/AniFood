using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Converters
{
    //Got to be honest, Had eerst een boolToString converter gemaakt, maar na het bekijken van de documentatie zag ik deze staan en dacht ik, dit is veel handiger
    //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/data-binding/converters?view=net-maui-8.0#binding-converter-properties
    class BoolToObjectConverter<T> : IValueConverter
    {
        public T? TrueObject { get; set; }
        public T? FalseObject { get; set; }

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value != null && TrueObject != null &&  FalseObject != null) 
                return (bool)value ? TrueObject : FalseObject;
            throw new NotSupportedException("You must define TrueObject, falseObject and the value first.");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
