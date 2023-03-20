using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotebookRCv001.MyControls
{
    /// <summary>
    /// Логика взаимодействия для MediaPlayer.xaml
    /// </summary>
    public partial class MediaPlayer : Page
    {

        public MediaPlayer()
        {
            InitializeComponent();
        }

        private void DragStarted( object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e )
        {
            if (DataContext is ViewModels.MediaPlayerViewModel vm)
                vm.UserIsDraggingSlider = true;
        }

        private void DragCompleted( object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e )
        {
            if (DataContext is ViewModels.MediaPlayerViewModel vm)
            {
                mplayer.Position = TimeSpan.FromSeconds( timelineSlider.Value );
                vm.UserIsDraggingSlider = false;
            }
        }
    }
}
