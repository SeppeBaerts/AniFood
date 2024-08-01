using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Converters
{
    internal class IntsToGridDefinitionsConverter : IValueConverter
    {
        /// <summary>
        /// Converts an array of 2 ints to a Column/Row-DefinitionCollection (the first int is the active width, the second int is the second width)
        /// (AKA: first int is food given, second int is food LEFT || NOT total food amount)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int[] values)
                values = [1, 1];

            if (values.Length != 2)
                throw new Exception("Passing parameter must be an array of 2 ints");

            if (values[1] < 0)
                values[1] = 0;


            if (targetType == typeof(ColumnDefinitionCollection))
            {

                ColumnDefinitionCollection columnDefinitions =
                [
                    new(new(values[0], GridUnitType.Star)),
                    new(new(values[1], GridUnitType.Star))
                ];
                return columnDefinitions;
            }
            else if (targetType == typeof(RowDefinitionCollection))
            {
                RowDefinitionCollection rowDefinitions =
                [
                    new(new(values[0], GridUnitType.Star)),
                    new(new(values[1], GridUnitType.Star))
                ];
                return rowDefinitions;
            }
            else
            {
                throw new Exception("Target type must be ColumnDefinitionCollection or RowDefinitionCollection");
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
