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
            People.Add(new Person { type = "male", name = "man" });
            People.Add(new Person { type = "female", name = "woman" });
            People.Add(new Person { type = "female", name = "woman" });
            People.Add(new Person { type = "male", name = "man" });
            People.Add(new Person { type = "male", name = "man" });
            People.Add(new Person { type = "male", name = "man" });

            listView.ChoosingItemContainer += ListView_ChoosingItemContainer;
        }

        private void ListView_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            var person = args.Item as Person;
            var male = person.type is "male";
            string tag = male ? "male" : "female";
            if (args.ItemContainer != null)
            {
                if (args.ItemContainer.Tag.Equals(tag))
                {
                    //好极了：系统建议使用一个实际上可以正常工作的容器。
                }
                else
                {
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
                    ListViewItem item = new ListViewItem();
                    item.ContentTemplate = this.Resources[tag] as DataTemplate;
                    item.Tag = tag;
                    args.ItemContainer = item;
                }
            }
        }

        public DataTemplate MaleTemplate { get; set; }
        public DataTemplate FemaleTemplate { get; set; }

        //示例显示如何使用ChoosingItemContainer返回正确的
        //一个可用的DataTemplate。 这个例子展示了如何返回不同
        // 数据模板基于FileItem的类型。 可用的ListViewItems保留
        // 根据所需的DataTemplate类型在两个单独的列表中。
        private void ListView_ChoosingItemContainer1
            (ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            //根据传入的项目确定FileItem的类型。
            bool special = args.Item is DifferentFileItem;

            //使用Tag属性跟踪特定ListViewItem的
            // datatemplate应该是简单的还是特殊的。
            string tag = special ? "specialFiles" : "simpleFiles";

            //根据所需的datatemplate类型返回正确的ListViewItems，
            //也可以使用哈希表来处理。 这些两个列表用于跟踪可重复使用的ItemContainer。
            List<UIElement> relevantStorage = special ? specialFileItemTrees : simpleFileItemTrees;

            // args.ItemContainer用于指示ListView是否提议
            //要使用的ItemContainer（ListViewItem）。如果是args.Itemcontainer，则存在一个
            //回收的ItemContainer可重复使用。
            if (args.ItemContainer != null)
            {
                //标签用于确定这是一个特殊文件还是
                //一个简单的文件。
                if (args.ItemContainer.Tag.Equals(tag))
                {
                    //好极了：系统建议使用一个实际上可以正常工作的容器。
                }
                else
                {
                    // ItemContainer的数据模板与所需的数据模板不匹配。
                    args.ItemContainer = null;
                }
            }

            if (args.ItemContainer == null)
            {
                //查看是否可以从正确的列表中获取。
                if (relevantStorage.Count > 0)
                {
                    args.ItemContainer = relevantStorage[0] as SelectorItem;
                }
                else
                {
                    //没有可用的（回收的）ItemContainer。所以一个新的
                    //需要创建。
                    ListViewItem item = new ListViewItem();
                    item.ContentTemplate = this.Resources[tag] as DataTemplate;
                    item.Tag = tag;
                    args.ItemContainer = item;
                }
            }
        }
    }

    public class Person
    {
        public string type { get; set; }
        public string name { get; set; }
    }
}