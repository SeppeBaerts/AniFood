using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Selectors
{
    class EmptyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyTemplate { get; set; } = (DataTemplate)Application.Current.Resources["EmptyTemplate"];
        public DataTemplate ContainsItemsTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is IEnumerable<object> values)
            {
                if (values.Any())
                    return ContainsItemsTemplate;
                else
                    return EmptyTemplate;
            }
            else if (item.GetType() == typeof(Enumerable))
                return EmptyTemplate;
            else
                throw new NotImplementedException();
        }
    }
}
