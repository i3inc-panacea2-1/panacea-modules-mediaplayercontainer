using System.Windows;
using System.Windows.Controls;


namespace Panacea.Modules.MediaPlayerContainer
{
    /// <summary>
    /// Interaction logic for MediaPlayerContainerControl.xaml
    /// </summary>
    public partial class MediaPlayerContainerControl : UserControl
    {
        public MediaPlayerContainerControl()
        {
            InitializeComponent();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if(constraint.Width != double.PositiveInfinity && constraint.Height != double.PositiveInfinity)
                return constraint;
            return base.MeasureOverride(constraint);
        }

    }
}
