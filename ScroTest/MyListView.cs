using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ScroTest
{
    public class MyListView : ListView
    {
        private ScrollViewer _scrollViewer;

        public MyListView()
        {
            this.Loaded += MyListView_Loaded;
        }

        private void MyListView_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = (sender as ListView).GetScrollViewer();
            _scrollViewer.ViewChanged += _scrollViewer_ViewChanged;
        }

        private void _scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var s = default(string);

            var MyScrollViewer = sender as ScrollViewer;
            this.SetValue(MyPropertyProperty, MyScrollViewer.VerticalOffset);
        }

        public double PointOffSetY
        {
            get { return (double)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("PointOffSetY", typeof(double), typeof(MyListView), new PropertyMetadata(0.0, CallBack));

        private static void CallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listview = d as ListView;
            var temScrollViewer = listview.GetScrollViewer();
            if (e.NewValue != e.OldValue & (double)e.NewValue != 0)
            {
                temScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }
    }

    public static class Helper
    {
        public static ScrollViewer GetScrollViewer(this DependencyObject element)
        {
            if (element is ScrollViewer)
            {
                return (ScrollViewer)element;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }
    }
}