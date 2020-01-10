using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ChoosingItemContainerDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Person> People { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();

            People = new ObservableCollection<Person>();
            People.Add(new Person { type = "male", data = new Data { name = "man" } });
            People.Add(new Person { type = "female", data = new Data { name = "woman" } });
            People.Add(new Person { type = "female", data = new Data { name = "woman" } });
            People.Add(new Person { type = "male", data = new Data { name = "man" } });
            People.Add(new Person { type = "male", data = new Data { name = "man" } });
            People.Add(new Person { type = "male", data = new Data { name = "man" } });

            ListViewItem item = new ListViewItem();
            item.ContentTemplate = Resources["MaleTemplate"] as DataTemplate;
            //listView.ChoosingItemContainer += ListView_ChoosingItemContainer;
        }

        private List<UIElement> maleTrees = new List<UIElement>();
        private List<UIElement> femaleTrees = new List<UIElement>();

        private void ListView_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            var person = args.Item as Person;
            var male = person.type is "male";
            string tag = male ? "MaleTemplate" : "FemaleTemplate";
            List<UIElement> relevantStorage = male ? maleTrees : femaleTrees;
            if (args.ItemContainer != null)
            {
                if (args.ItemContainer.Tag.Equals(tag))
                {
                }
                else
                {
                    relevantStorage.RemoveAt(0);
                    args.ItemContainer = null;
                }
            }

            if (args.ItemContainer == null)
            {
                if (relevantStorage.Count > 0)
                {
                    args.ItemContainer = relevantStorage[0] as SelectorItem;
                }
                else
                {
                    var template = new DataTemplate();
                    template = this.Resources[tag] as DataTemplate;
                    ListViewItem item = new ListViewItem();
                    //item.ContentTemplate = this.Resources[tag] as DataTemplate;
                    item.ContentTemplate = template;
                    item.Tag = tag;
                    args.ItemContainer = item;
                }
            }
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var person = e.ClickedItem as Person;
        }
    }

    public class Person
    {
        public string type { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string name { get; set; }
    }

    public class PersonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MaleTemplate { get; set; }
        public DataTemplate FemaleTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var type = ((Person)item).type;
            if (type == "male")
            {
                return MaleTemplate;
            }
            else if (type == "female")
            {
                return FemaleTemplate;
            }
            else
            {
                return base.SelectTemplateCore(item, container);
            }
        }
    }
}