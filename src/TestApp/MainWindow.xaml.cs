using Panacea.Core;
using System.Windows;
using Panacea.Modularity.MediaPlayerContainer;
using System.Collections.Generic;
using Panacea.Modularity;
using Panacea.Modules.MediaPlayerContainer;
using Panacea.Modularity.Media;
using System.Linq;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PluginLoader _loader;
        PanaceaServices _core;
        public MainWindow()
        {
            InitializeComponent();
            _loader = new PluginLoader();
            _core = new PanaceaServices(null, null, _loader, null, null, null);
            _loader.LoadPlugin(new MediaPlayerContainer(_core));
            _loader.LoadPlugin(new SampleMediaPlayer());
            _loader.LoadPlugin(new UiManagerPlugin(this.Container));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _core.GetMediaPlayerContainer()
                .Play(new MediaRequest(new MyChannel()
                {
                    Name = "Test TV channel"
                })
                {
                    MediaPlayerPosition = MediaPlayerPosition.Standalone
                });
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            _loader.LoadPlugin(new SampleMediaPlayer());
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var players = _loader.GetPlugins<IMediaPlayerPlugin>();
            if (players.Any())
            {
                _loader.UnloadPlugin(players.Last());
            }
        }
    }

  
}
