using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChoosingItemContainerDemo
{
    public class TemplateSelector : DataTemplateSelector
    {
        private readonly Dictionary<string, DataTemplate> TemplatesByType;
        private readonly ResourceDictionary Resources;

        public TemplateSelector() : this(new NewDataTemplates())
        {
        }

        public TemplateSelector(ResourceDictionary resources)
        {
            Resources = resources;

            TemplatesByType = new Dictionary<string, DataTemplate>
            {
                { "male", Resources["MaleTemplate"] as DataTemplate },
                { "female", Resources["FemaleTemplate"] as DataTemplate }
            };
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            DataTemplate dataTemplate;

            if (TemplatesByType.TryGetValue(item?.ToString(), out dataTemplate))
            {
                return dataTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}