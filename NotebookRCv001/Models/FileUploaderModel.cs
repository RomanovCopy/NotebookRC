using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using NotebookRCv001.ViewModels;
using System.Windows.Controls;
using NotebookRCv001.Helpers;
using System.IO;
using System.Windows.Forms;
using NotebookRCv001.Views;
using NotebookRCv001.Converters;
using System.Windows.Controls.Primitives;

namespace NotebookRCv001.Models
{
    internal class FileUploaderModel : ViewModelBase
    {

        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuFileViewModel homeMenuFileViewModel;
        private BehaviorTextBox behaviorTextBox { get; set; }
        private System.Windows.Controls.ListView listView { get; set; }
        internal ObservableCollection<string> Headers => mainWindowViewModel.Language.HeadersFileUploader;
        internal ObservableCollection<string> ToolTips => mainWindowViewModel.Language.ToolTipsFileUploader;

        /// <summary>
        /// полный путь к каталогу с загружаемыми файлами
        /// </summary>
        internal string ContentDisposition
        { get => contentDisposition; set => SetProperty( ref contentDisposition, value ); }
        private string contentDisposition;
        internal string DirectoryPathWithDownloadedFiles
        { get => directoryPathWithDownloadedFiles; set => SetProperty( ref directoryPathWithDownloadedFiles, value ); }
        private string directoryPathWithDownloadedFiles;

        internal double ProgressValue { get => progressValue; set => SetProperty( ref progressValue, value ); }
        private double progressValue;

        internal string Text { get => text; set => SetProperty( ref text, value ); }
        private string text;

        internal ObservableCollection<DownloadItemViewModel> ListDownoadItems
        {
            get => listDowloadItems;
            set => SetProperty( ref listDowloadItems, value );
        }
        private ObservableCollection<DownloadItemViewModel> listDowloadItems;

        /// <summary>
        /// коллекция размеров колонок по горизонтали(Width)
        /// </summary>
        internal ObservableCollection<double> ListView_ColumnsWidth
        {
            get => listView_ColumnsWidth ??= new ObservableCollection<double>();
            set => SetProperty( ref listView_ColumnsWidth, value );
        }
        ObservableCollection<double> listView_ColumnsWidth;

        internal FileUploaderModel()
        {
            mainWindowViewModel = (MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext;
            mainWindowViewModel.Language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );
            var home = (Views.Home)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
            var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
            homeMenuFileViewModel = (HomeMenuFileViewModel)menu.FindResource( "menufile" );
            //устанавливаем размеры колонок
            //Properties.Settings.Default.FileUploader_ListViewColumnsWidth = null;
            if (Properties.Settings.Default.FileUploader_ListViewColumnsWidth == null)
                Properties.Settings.Default.FileUploader_ListViewColumnsWidth = new System.Collections.Specialized.StringCollection()
                { "15" ,"15" ,"10" ,"10" ,"10" ,"20" ,"10" ,"10"  };
            ListView_ColumnsWidth.Clear();
            for (int i = 0; i < Properties.Settings.Default.FileUploader_ListViewColumnsWidth.Count; i++)
                ListView_ColumnsWidth.Add( double.Parse( Properties.Settings.Default.FileUploader_ListViewColumnsWidth[i] ) );
            ListDownoadItems = new ObservableCollection<DownloadItemViewModel>();
            ListDownoadItems.CollectionChanged += ListDownoadItems_CollectionChanged;
        }

        /// <summary>
        /// обработка события готовности BehaviorRichTextBox
        /// </summary>
        public Action<object> BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        private Action<object> behaviorReady;

        /// <summary>
        /// выбор директории загрузки для коллекции выбранных загрузок
        /// </summary>
        /// <param name="obj">коллекция выбранных загрузок</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectASpecialDirectory( object obj )
        {
            try
            {
                bool c = false;
                if (obj is IList list)
                    c = list.Count > 0;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SelectASpecialDirectory( object obj )
        {
            try
            {
                if (obj is IList list)
                {
                    string newDirectory = null;
                    var dialog = new Views.FolderBrowserDialog();
                    dialog.Closing += ( s, e ) =>
                    {
                        var vm = (FolderBrowserDialogViewModel)dialog.DataContext;
                        newDirectory = vm.WorkingDirectory;
                        if (!string.IsNullOrWhiteSpace( newDirectory ))
                        {
                            foreach (var item in list)
                            {
                                if (item is DownloadItemViewModel viewModel)
                                {
                                    var info = new FileInfo( viewModel.FullPath );
                                    viewModel.FullPath = Path.Combine( newDirectory, info.Name );
                                }
                            }
                        }
                    };
                    dialog.ShowDialog();
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// задание директории для загружаемых файлов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_DirectoryForDownloadedFiles( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_DirectoryForDownloadedFiles( object obj )
        {
            try
            {
                var dialog = new Views.FolderBrowserDialog();
                dialog.Closing += ( s, e ) =>
                {
                    var viewmodel = (FolderBrowserDialogViewModel)dialog.DataContext;
                    if (!string.IsNullOrWhiteSpace( viewmodel.WorkingDirectory ))
                    {
                        ContentDisposition = viewmodel.WorkingDirectory;
                    }
                    homeMenuFileViewModel.WorkingDirectory = ContentDisposition;
                };
                dialog.ShowDialog();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// выбор всего содержимого TextBox url
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_TextBoxSelectAll( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrEmpty( behaviorTextBox?.Text );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_TextBoxSelectAll( object obj )
        {
            try
            {
                behaviorTextBox.SetSelect( 0, behaviorTextBox.Text.Length );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// копирование выделенного текста в TextBox url
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_TexBoxCopy( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( behaviorTextBox.SelectedText );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_TexBoxCopy( object obj )
        {
            try
            {
                System.Windows.Clipboard.SetText( behaviorTextBox.SelectedText );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// очистка TextBox url
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_TextBoxClear( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( behaviorTextBox.Text );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_TextBoxClear( object obj )
        {
            try
            {
                behaviorTextBox.Text = string.Empty;
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// добавление загрузки с url из TextBox
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_AddNewDownloadItem( object obj )
        {
            try
            {
                bool c = false;
                c = obj is string text && !string.IsNullOrWhiteSpace( text ) && !ListDownoadItems.Any( ( x ) => x.Url == text );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_AddNewDownloadItem( object obj )
        {
            try
            {
                if (obj is string url)
                {
                    if (string.IsNullOrWhiteSpace( ContentDisposition ))
                        Execute_DirectoryForDownloadedFiles( null );
                    DownloadItemViewModel itemViewModel = new();
                    itemViewModel.OriginalUrl = obj.ToString();
                    itemViewModel.Id = NewID();
                    itemViewModel.Url = Command_executors.Executors.CastToValidURL( itemViewModel.OriginalUrl );
                    behaviorTextBox.Text = itemViewModel.Url;
                    ListDownoadItems.Add( itemViewModel );
                    listView.Focus();
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// вставка текста в TextBox из буфера обмена
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_PasteToTextBox( object obj )
        {
            try
            {
                bool c = false;
                c = behaviorTextBox != null ? behaviorTextBox.Paste.CanExecute( null ) : false;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PasteToTextBox( object obj )
        {
            try
            {
                behaviorTextBox.Clear.Execute( null );
                behaviorTextBox.Paste.Execute( null );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// окончание загрузки listView
        /// </summary>
        /// <param name="obj">ListView</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ListViewLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ListViewLoaded( object obj )
        {
            try
            {
                if (obj is System.Windows.Controls.ListView list)
                {
                    list.Focus();
                    listView = list;
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_TextBoxLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_TextBoxLoaded( object obj )
        {
            try
            {
                if (obj is BehaviorTextBox behavior)
                {
                    behaviorTextBox = behavior;
                    behaviorTextBox.PreviewKeyDown += ( s, e ) =>
                    {
                        if (e.Key == Key.Enter && CanExecute_AddNewDownloadItem( behaviorTextBox.Text ))
                        {
                            Execute_AddNewDownloadItem( behaviorTextBox.Text );
                        }
                    };
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_PageLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageLoaded( object obj )
        {
            try
            {
                if(obj is Page page)
                {
                    var convert = (ColumnsWidthConverter)page.FindResource( "columnswidth" );
                    if (convert != null)
                        convert.window = System.Windows.Application.Current.MainWindow;
                }
                string path = Properties.Settings.Default.DirectoryPathWithDownloadedFiles;
                if (!string.IsNullOrWhiteSpace( path ) && Directory.Exists( path ))
                {
                    ContentDisposition = path;
                    homeMenuFileViewModel.WorkingDirectory = path;
                }
                if (!string.IsNullOrWhiteSpace( Properties.Settings.Default.DirectoryPathWithDownloadedFiles ))
                    homeMenuFileViewModel.WorkingDirectory = Properties.Settings.Default.DirectoryPathWithDownloadedFiles;
                OnPropertyChanged( "ListView_ColumnsWidth" );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_PageSizeChanged( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageSizeChanged( object obj )
        {
            try
            {
                OnPropertyChanged( "ListView_ColumnsWidth" );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_PageClose( object obj )
        {
            try
            {
                bool c = false;
                c = obj is Page;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageClose( object obj )
        {
            try
            {
                if (obj is Page page && mainWindowViewModel.FrameListRemovePage.CanExecute( page ))
                {
                    Properties.Settings.Default.FileUploader_ListViewColumnsWidth.Clear();
                    foreach (double width in ListView_ColumnsWidth)
                        Properties.Settings.Default.FileUploader_ListViewColumnsWidth.Add( width.ToString() );
                    Properties.Settings.Default.Save();
                    foreach (var item in ListDownoadItems)
                        item.Close.Execute( null );
                    mainWindowViewModel.FrameListRemovePage.Execute( page );
                    if (!string.IsNullOrWhiteSpace( ContentDisposition ))
                        Properties.Settings.Default.DirectoryPathWithDownloadedFiles = ContentDisposition;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// очистка коллекции загрузок
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ListClear( object obj )
        {
            try
            {
                bool c = false;
                c = ListDownoadItems?.Count > 0 ;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ListClear( object obj )
        {
            try
            {
                if (ListDownoadItems?.Count > 0)
                {
                    while (ListDownoadItems.Count > 0)
                    {
                        if (ListDownoadItems.FirstOrDefault().Close.CanExecute( null ))
                        {
                            ListDownoadItems.FirstOrDefault().Close.Execute( null );
                            ListDownoadItems.Remove( ListDownoadItems.First() );
                        }
                    }
                    ListDownoadItems.Clear();
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// удаление загрузки
        /// </summary>
        /// <param name="obj">Id выбранной загрузкки(string)</param>
        /// <returns></returns>
        internal bool CanExecute_ListRemove( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ListRemove( object obj )
        {
            try
            {
                if (obj is string id)
                {
                    var item = ListDownoadItems.Where( ( x ) => x.Id == id ).FirstOrDefault();
                    if (item != null) ListDownoadItems.Remove( item );
                }
                else if (obj is DownloadItemViewModel viewModel)
                {
                    viewModel.Stop.Execute( null );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_PageClear( object obj )
        {
            try
            {
                bool c = false;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageClear( object obj )
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        private async void ListDownoadItems_CollectionChanged( object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e )
        {
            try
            {
                if (sender is ObservableCollection<DownloadItemViewModel> list &&
                    e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = (DownloadItemViewModel)e.NewItems[i];
                        await Task.Factory.StartNew( () =>
                        {
                            if (item.Preparation.CanExecute( null ))
                                item.Preparation.Execute( null );
                        } );
                    }
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private string NewID()
        {
            try
            {
                string id = null;
                id = DateTime.Now.ToString();
                return id;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }
        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new( () => System.Windows.MessageBox.Show( e.Message, $"FileUploaderModel.{name}" ) );
            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
        }
    }
}
