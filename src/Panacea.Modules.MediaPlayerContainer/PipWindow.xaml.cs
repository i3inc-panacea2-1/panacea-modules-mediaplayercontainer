using Panacea.Core;
using System.Windows;
using Panacea.Modularity.MediaPlayerContainer;
using System.Collections.Generic;
using Panacea.Modularity;
using Panacea.Modules.MediaPlayerContainer;
using Panacea.Modularity.Media;
using System.Linq;
using System.Windows.Input;

namespace Panacea.Modules.MediaPlayerContainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PipWindow : Window
    {
        public PipWindow()
        {
            InitializeComponent();
        }

        private void Window_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

        }
    }
}
