using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Views;

namespace NotebookRCv001.Models
{
    internal class HomeMenuEditingModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;
        private MenuHomeViewModel menuHomeViewModel;
        private HomeMenuFileViewModel homeMenuFileViewModel;
        private RichTextBoxViewModel richTextBoxViewModel;


        internal ObservableCollection<string> Headers => language.HomeMenuContent;
        internal ObservableCollection<string> ToolTips => language.ToolTipsHomeMenuContent;

        private string Filter => "All files (*.*)|*.*|Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";



        internal HomeMenuEditingModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            mainWindowViewModel.FrameList.CollectionChanged += ( s, e ) =>
            {
                if (mainWindowViewModel.CurrentPage is Home home)
                {
                    richTextBoxViewModel = (RichTextBoxViewModel)((MyControls.RichTextBox)home.FindResource("richtextbox")).DataContext;
                    var homemenu = (MyControls.MenuHome)home.FindResource("menuhome");
                    menuHomeViewModel = (MenuHomeViewModel)homemenu.DataContext;
                    homeMenuFileViewModel = (HomeMenuFileViewModel)homemenu.FindResource("menufile");
                }
            };
        }



        internal bool CanExecute_InsertImage( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_InsertImage( object obj )
        {
            try
            {
                var win = new SelectAndPasteWindow();
                win.Owner = Application.Current.MainWindow;
                win.Show();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        private void Image_PreviewMouseRightButtonDown( object sender, MouseButtonEventArgs e )
        {

        }

    }
}
