using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using System.Runtime.CompilerServices;
using System.Threading;
using NotebookRCv001.Converters;

namespace NotebookRCv001.Models
{
    internal class FileOverviewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuFileViewModel homeMenuFileViewModel;
        private Languages language => mainWindowViewModel.Language;



        #region ________________Sizes and Position________________________

        internal double WindowWidth { get => windowWidth; set => SetProperty(ref windowWidth, value); }
        private double windowWidth;
        internal double WindowHeight { get => windowHeight; set => SetProperty(ref windowHeight, value); }
        private double windowHeight;
        internal double WindowTop { get => windowTop; set => SetProperty(ref windowTop, value); }
        private double windowTop;
        internal double WindowLeft { get => windowLeft; set => SetProperty(ref windowLeft, value); }
        private double windowLeft;
        internal object WindowState { get => windowState; set => SetProperty(ref windowState, value); }
        private object windowState;


        #endregion


        #region ________________Columns____________________________

        /// <summary>
        /// коллекция размеров колонок по горизонтали(Width)
        /// </summary>
        internal ObservableCollection<double> ListView_ColumnsWidth
        {
            get => listView_ColumnsWidth ??= new ObservableCollection<double>();
            set => SetProperty( ref listView_ColumnsWidth, value );
        }
        ObservableCollection<double> listView_ColumnsWidth;

        #endregion

        internal ObservableCollection<string> Headers => language.HeadersFileOverview;

        internal ObservableCollection<string> ToolTips => language.ToolTipsFileOverview;

        internal FileOverviewModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );
            var home = (Views.Home)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
            var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
            homeMenuFileViewModel = (HomeMenuFileViewModel)menu.FindResource( "menufile" );
            //восстанавливаем размеры и положение окна
            if (Properties.Settings.Default.FileOverviewFirstStart)
            {
                WindowHeight = 40;
                WindowWidth = 40;
                WindowLeft = 40;
                WindowTop = 40;
                Properties.Settings.Default.FileOverviewFirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.FileOverviewHeight;
                WindowWidth = Properties.Settings.Default.FileOverviewWidth;
                WindowLeft = Properties.Settings.Default.FileOverviewLeft;
                WindowTop = Properties.Settings.Default.FileOverviewTop;
            }
            //восстанавливаем состояние окна
            WindowState = Properties.Settings.Default.FileOverviewState;
            //устанавливаем размеры колонок
            //Properties.Settings.Default.FileUploader_ListViewColumnsWidth = null;
            if (Properties.Settings.Default.FileOverview_ListViewColumnsWidth == null)
                Properties.Settings.Default.FileOverview_ListViewColumnsWidth = new System.Collections.Specialized.StringCollection()
                { "40" ,"20" ,"30" ,"10"  };
            ListView_ColumnsWidth.Clear();
            for (int i = 0; i < Properties.Settings.Default.FileOverview_ListViewColumnsWidth.Count; i++)
                ListView_ColumnsWidth.Add( double.Parse( Properties.Settings.Default.FileOverview_ListViewColumnsWidth[i] ) );
        }






        /// <summary>
        /// при изменении размеров окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_WindowSizeChanged( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_WindowSizeChanged( object obj )
        {
            try
            {
                OnPropertyChanged( "ListView_ColumnsWidth" );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// окончание загрузки окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_WindowLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch(Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_WindowLoaded( object obj )
        {
            try
            {
                if(obj is Window window)
                {
                    var convert = (ColumnsWidthConverter)window.FindResource( "columnswidth" );
                    convert.window = window;
                }
                OnPropertyChanged( "ListView_ColumnsWidth" );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// перед закрытием окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_WindowClosing( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_WindowClosing( object obj )
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow( e ); }
        }


        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new( () => MessageBox.Show( e.Message, $"FileOverviewModel.{name}" ) );
            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
        }

    }
}
