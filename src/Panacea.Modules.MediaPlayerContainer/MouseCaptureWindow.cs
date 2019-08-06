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
    class MouseCaptureWindow:Window
    {
        public MouseCaptureWindow()
        {
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            Background = Brushes.Transparent;
            WindowState = WindowState.Maximized;
            Topmost = true;

            Content = new Grid()
            {
                Background = Brushes.Transparent,
                IsHitTestVisible = true
            };
        }
    }
}
