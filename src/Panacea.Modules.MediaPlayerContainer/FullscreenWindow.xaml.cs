using Panacea.Core;
using System.Windows;
using Panacea.Modularity.MediaPlayerContainer;
using System.Collections.Generic;
using Panacea.Modularity;
using Panacea.Modules.MediaPlayerContainer;
using Panacea.Modularity.Media;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Diagnostics;

namespace Panacea.Modules.MediaPlayerContainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FullscreenWindow : Window
    {
     
        public FullscreenWindow()
        {
            InitializeComponent();
            if (Debugger.IsAttached)
            {
                Topmost = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.Cursor.Hide();
            

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.Cursor.Show();
        }
    }
}
