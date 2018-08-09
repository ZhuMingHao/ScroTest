using Microsoft.Xaml.Interactivity;
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
    public abstract class Behavior<T> : Behavior where T : DependencyObject
    {
        protected T AssociatedObject
        {
            get { return base.AssociatedObject as T; }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject == null) throw new InvalidOperationException("AssociatedObject is not of the right type");
        }
    }

    public abstract class Behavior : DependencyObject, IBehavior
    {
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            OnAttached();
        }

        public void Detach()
        {
            OnDetaching();
        }

        protected virtual void OnAttached() { }

        protected virtual void OnDetaching() { }

        protected DependencyObject AssociatedObject { get; set; }

        DependencyObject IBehavior.AssociatedObject
        {
            get { return this.AssociatedObject; }
        }
    }

    public class HtmlTextBehavior : Behavior<ScrollViewer>
    {
        private static ScrollViewer _scrollViewer;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            AssociatedObject.LayoutUpdated += OnAssociatedObjectLayoutUpdated;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
            AssociatedObject.LayoutUpdated -= OnAssociatedObjectLayoutUpdated;
        }

        private void OnAssociatedObjectLayoutUpdated(object sender, object o)
        {
            UpdateText();
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateText();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
        }

        private void UpdateText()
        {
            if (AssociatedObject == null) return;

            AssociatedObject.ViewChanged += AssociatedObject_ViewChanged;
        }

        private void AssociatedObject_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var MyScrollViewer = sender as ScrollViewer;
            this.SetValue(PointOffSetYProperty, MyScrollViewer.VerticalOffset);

        }

        public double PointOffSetY
        {
            get { return (double)GetValue(PointOffSetYProperty); }
            set { SetValue(PointOffSetYProperty, value); }
        }
     
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointOffSetYProperty =
            DependencyProperty.Register("PointOffSetY", typeof(double), typeof(HtmlTextBehavior), new PropertyMetadata(0.0, CallBack));

        private static void CallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as HtmlTextBehavior;
            var temScrollViewer = current.AssociatedObject;
            if (e.NewValue != e.OldValue & (double)e.NewValue != 0)
            {
                temScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }


    }

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
            this.SetValue(PointOffSetYProperty, MyScrollViewer.VerticalOffset);
        }

        public double PointOffSetY
        {
            get { return (double)GetValue(PointOffSetYProperty); }
            set { SetValue(PointOffSetYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointOffSetYProperty =
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