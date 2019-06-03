using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panacea.Modules.MediaPlayerContainer
{
    static class FrameworkElementExtensions
    {
        public static void RemoveChild(this FrameworkElement element)
        {
            var objs = new List<DependencyObject>()
            {
                 element.Parent,
                 VisualTreeHelper.GetParent(element)
            };
            foreach (var dobjs in objs.Where(o => o != null))
            {
                var panel = dobjs as Panel;
                if (panel != null)
                {
                    panel.Children.Remove(element);
                    return;
                }

                var decorator = dobjs as Decorator;
                if (decorator != null)
                {
                    if (decorator.Child == element)
                    {
                        decorator.Child = null;
                    }
                    return;
                }

                var contentPresenter = dobjs as ContentPresenter;
                if (contentPresenter != null)
                {
                    if (contentPresenter.Content == element)
                    {
                        contentPresenter.Content = null;
                    }
                    return;
                }

                var contentControl = dobjs as ContentControl;
                if (contentControl != null)
                {
                    if (contentControl.Content == element)
                    {
                        contentControl.Content = null;
                    }
                }
            }
        }
    }
}
