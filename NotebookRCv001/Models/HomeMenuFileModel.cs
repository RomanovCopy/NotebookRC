using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Views;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Documents;
using System.IO;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Xml;
using System.Xaml;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Helpers;
using System.Windows.Media;

namespace NotebookRCv001.Models
{
    public class HomeMenuFileModel : ViewModelBase, IDisplayProgressTarget
    {

        private readonly MainWindowViewModel mainWindowViewModel;
        private RichTextBoxViewModel richTextBoxViewModel { get; set; }
        private HomeMenuEncryptionViewModel HomeMenuEncryptionViewModel { get; set; }
        private BehaviorRichTextBox behaviorRichTextBox { get; set; }
        /// <summary>
        /// поддерживаемые расширения файлов
        /// </summary>
        internal string[] SupportedFileExtensions => supportedFileExtensions ??= new[] { ".xps", ".txt", ".cs", ".rtf", ".xaml" };
        private string[] supportedFileExtensions;
        internal ObservableCollection<string> Headers => mainWindowViewModel.Language.HomeMenuFile;
        internal ObservableCollection<string> ToolTips => mainWindowViewModel.Language.ToolTipsHomeMenuFile;

        /// <summary>
        /// путь к текущему рабочему каталогу сохранения файла
        /// </summary>
        internal string CurrentDirectorySave { get => currentDirectorySave; set => SetProperty( ref currentDirectorySave, value ); }
        private string currentDirectorySave;

        /// <summary>
        /// путь к текущему рабочему каталогу открытия файла
        /// </summary>
        internal string CurrentDirectoryOpen { get => currentDirectoryOpen; set => SetProperty( ref currentDirectoryOpen, value ); }
        private string currentDirectoryOpen;

        /// <summary>
        /// путь к рабочей директории
        /// </summary>
        internal string WorkingDirectory
        {
            get => workingDirectory;
            set => SetProperty( ref workingDirectory, value, new string[] { "WorkingDirectory", "WorkingDirectoryName" } );
        }
        private string workingDirectory;

        /// <summary>
        /// имя каталога рабочей директории
        /// </summary>
        internal string WorkingDirectoryName => string.IsNullOrWhiteSpace( WorkingDirectory ) ? "" : new DirectoryInfo( WorkingDirectory ).Name;

        /// <summary>
        /// путь к последнему открытому или сохраненному файлу 
        /// </summary>
        internal string PathToLastFile
        {
            get => pathToLastFile;
            set => SetProperty( ref pathToLastFile, value, new string[] { "LastFileName" } );
        }
        private string pathToLastFile;

        /// <summary>
        /// Имя последнего открытого или сохраненного файла
        /// </summary>
        internal string LastFileName => string.IsNullOrWhiteSpace( PathToLastFile ) ? "" : Path.GetFileName( PathToLastFile );

        internal string Filter =>
            "All files (*.*)|*.*|Text Files (*.txt)|*.txt|" +
            "RichText Files (*.rtf)|*.rtf|" +
            "Source Code Files (*.cs)|*.cs|" +
            "Xaml files (*.xaml)|*.xaml|" +
            "PDF files (*.pdf)|*.pdf|" +
            "XPS files (*.xps)|*.xps";

        /// <summary>
        /// прогресс выполнения синхронизации каталогов
        /// </summary>
        public double ProgressValue { get => progressValue; set => SetProperty( ref progressValue, value ); }
        double progressValue;

        public HomeMenuFileModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            mainWindowViewModel.Language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );
            mainWindowViewModel.FrameList.CollectionChanged += ( s, e ) =>
            {
                if (mainWindowViewModel.CurrentPage is Views.Home home)
                {
                    var richtextbox = (MyControls.RichTextBox)home.FindResource( "richtextbox" );
                    richTextBoxViewModel = (RichTextBoxViewModel)richtextbox.DataContext;
                    var homemenu = (MyControls.MenuHome)home.FindResource( "menuhome" );
                    HomeMenuEncryptionViewModel = (HomeMenuEncryptionViewModel)homemenu.FindResource( "menuencryption" );
                    var homeViewModel = (HomeViewModel)home.DataContext;
                    richTextBoxViewModel.BehaviorReady = (x) => { behaviorRichTextBox = richTextBoxViewModel.BehaviorRichTextBox; };
                }
            };
            PathToLastFile = null;
        }


        internal bool CanExecute_NewFile( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_NewFile( object obj )
        {
            try
            {
                string path = null;
                if ((path = SaveFileDialog( Filter, CurrentDirectorySave )) != null)
                {
                    behaviorRichTextBox.Document.Blocks.Clear();
                    behaviorRichTextBox.Document.Blocks.Add( new Paragraph() );
                    Execute_SaveFile( path );
                    CurrentDirectorySave = Path.GetDirectoryName( path );
                    PathToLastFile = path;
                    CurrentDirectoryOpen = Path.GetDirectoryName( path );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_OpenFile( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_OpenFile( object obj )
        {
            try
            {
                string path = null;
                if (!(obj is string p && File.Exists( p )))
                {//путь к файлу еще не выбран
                    //информационное сообщение
                    var result = mainWindowViewModel.NewSelectWindow.Invoke(
                        mainWindowViewModel.Language.SelectWindowHeaders[0],
                        mainWindowViewModel.Language.MessagesSelectWindow[0], null,
                        mainWindowViewModel.Language.SelectWindowHeaders[1], null );
                    if (string.IsNullOrWhiteSpace( result ))
                        return;
                    //выбор файла для открытия
                    if (string.IsNullOrEmpty( path = OpenFileDialog( Filter, CurrentDirectoryOpen ) ))
                        //выбор файла отменен
                        return;
                    else
                    {//файл выбран
                        PathToLastFile = Path.GetFullPath( path );
                        CurrentDirectoryOpen = Path.GetDirectoryName( path );
                    }
                }
                else if (obj is string p1 && File.Exists( p1 ))
                {//путь к файлу уже подан на вход метода
                    path = p1;
                    PathToLastFile = Path.GetFullPath( path );
                    CurrentDirectoryOpen = Path.GetDirectoryName( path );
                }
                else return;
                var home = (Views.Home)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
                if (home == null) return;
                var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
                var menuVM = ((ViewModels.MenuHomeViewModel)menu.DataContext);
                Encoding encoding = menuVM.HomeEncoding;
                //определяем текущий режим (чтение/редактирование)
                FlowDocument flowDocument = null;
                if (mainWindowViewModel.CurrentPage.Equals( home ))
                {
                    flowDocument = richTextBoxViewModel.Document;//редактирование
                    var viewmodel = (HomeViewModel)home.DataContext;
                    viewmodel.PathToLastFile = PathToLastFile;
                }
                else if (mainWindowViewModel.CurrentPage is Views.FlowDocumentReader reader)
                {
                    var viewmodel = (FlowDocumentReaderViewModel)reader.DataContext;
                    flowDocument = viewmodel.Document;//чтение
                    viewmodel.PathToLastFile = PathToLastFile;
                    viewmodel.LastFileName = LastFileName;
                }
                TextRange textRange = new( flowDocument?.ContentStart, flowDocument?.ContentEnd );
                string extension = Path.GetExtension( path ).ToLower();
                string keyCrypt = HomeMenuEncryptionViewModel.KeyCript;
                byte[] bytes = null;
                using (FileStream fs = new FileStream( path, FileMode.Open ))
                {
                    try
                    {
                        bytes = new byte[fs.Length];
                        fs.Read( bytes );
                        if (extension == ".rtf" || extension == ".xaml")
                        {
                            if (!string.IsNullOrWhiteSpace( keyCrypt ))
                                bytes = Command_executors.Executors.Decrypt( bytes, keyCrypt );
                            using (MemoryStream ms = new MemoryStream( bytes ))
                            {
                                if (extension == ".rtf")
                                    textRange.Load( ms, DataFormats.Rtf );
                                else
                                    textRange.Load( ms, DataFormats.XamlPackage );
                                var value = textRange.GetPropertyValue( TextElement.BackgroundProperty );
                                textRange.ApplyPropertyValue( TextElement.BackgroundProperty, richTextBoxViewModel.BehaviorRichTextBox.MyFontBackground );
                            }
                        }
                        else if (extension == ".txt" || extension == ".cs")
                        {
                            if (!string.IsNullOrWhiteSpace( keyCrypt ))
                            {
                                string text = null;
                                bytes = Command_executors.Executors.Decrypt( bytes, keyCrypt );
                                text = encoding.GetString( bytes );
                                textRange.Text = string.IsNullOrEmpty( text ) ? "" : text;
                            }
                            else
                            {
                                try
                                { textRange.Load( fs, DataFormats.Text ); }
                                catch { throw new Exception( mainWindowViewModel.Language.MessagesMyMessages[1] ); }
                            }
                            //приводим шрифт полученного текста к принятым настройкам
                            richTextBoxViewModel.BehaviorRichTextBox.SetFontProperties( textRange );
                        }
                        else if (extension == ".doc")
                        {
                            if (!string.IsNullOrWhiteSpace( keyCrypt ))
                            {

                            }
                            else
                            {

                            }
                        }
                        else if (extension == ".xml")
                        {

                        }
                        else if (extension == ".xps")
                        {
                            fs.Close();
                            var page = mainWindowViewModel.FrameList.Where( ( x ) => x is FixedDocumentReader ).LastOrDefault();
                            if (page == null)
                            {
                                page = new FixedDocumentReader() { KeepAlive = true };
                                if (mainWindowViewModel.FrameListAddPage.CanExecute( page ))
                                {
                                    mainWindowViewModel.FrameListAddPage.Execute( page );
                                    var viewmodel = (FixedDocumentReaderViewModel)page.DataContext;
                                    viewmodel.BehaviorReady = (x) =>
                                    {
                                        viewmodel.Document = new XpsDocument( path, FileAccess.Read );
                                    };
                                }
                            }
                            else
                            {
                                var viewmodel = (FixedDocumentReaderViewModel)page.DataContext;
                                viewmodel.Document = new XpsDocument( path, FileAccess.Read );
                            }
                            mainWindowViewModel.CurrentPage = page;
                        }
                        else if (Path.GetExtension( path ).ToLower() == ".pdf")
                        {

                        }
                        else
                        {
                            textRange.Text = "";
                            throw new Exception( mainWindowViewModel.Language.MessagesMyMessages[1] );
                        }
                    }
                    catch
                    {
                        throw new Exception( mainWindowViewModel.Language.MessagesMyMessages[1] );
                    }
                }
            }
            catch (Exception e)
            {
                ErrorWindow( e );
            }
        }

        internal bool CanExecute_SaveFile( object obj )
        {
            try
            {
                bool c = false;
                string s = PathToLastFile;
                var start = richTextBoxViewModel.BehaviorRichTextBox.Document.ContentStart;
                var end = richTextBoxViewModel.BehaviorRichTextBox.Document.ContentEnd;
                var textRange = new TextRange( start, end );
                c = File.Exists( s ) && (!textRange.IsEmpty);
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SaveFile( object obj )
        {
            try
            {
                string path = (string)obj;
                if (path == null)
                    path = PathToLastFile;
                var home = (Views.Home)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
                if (home == null) return;
                var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
                var menuVM = ((ViewModels.MenuHomeViewModel)menu.DataContext);
                Encoding encoding = menuVM.HomeEncoding;
                TextRange textRange = richTextBoxViewModel.BehaviorRichTextBox.TextRange;
                HomeMenuEncryptionModel encryptionModel = HomeMenuEncryptionViewModel.HomeMenuEncryptionModel;
                string keyCrypt = HomeMenuEncryptionViewModel.KeyCript;
                string extension = Path.GetExtension( path ).ToLower();
                byte[] bytes = null;
                using (FileStream fs = new FileStream( path, FileMode.Create ))
                {
                    try
                    {
                        if (extension == ".rtf")
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                textRange.Save( ms, DataFormats.Rtf );
                                bytes = ms.ToArray();
                                if (!string.IsNullOrWhiteSpace( keyCrypt ))
                                    bytes = Command_executors.Executors.Encrypt( bytes, keyCrypt );
                            }
                            fs.Write( bytes );
                        }
                        else if (extension == ".txt" || extension == ".cs")
                        {
                            string text = textRange.Text;
                            bytes = encoding.GetBytes( text );
                            if (!string.IsNullOrWhiteSpace( keyCrypt ))
                                bytes = Command_executors.Executors.Encrypt( bytes, keyCrypt );
                            fs.Write( bytes );
                        }
                        else if (extension == ".doc")
                        {
                            if (!string.IsNullOrWhiteSpace( keyCrypt ))
                            {

                            }
                            else
                            {

                            }
                        }
                        else if (extension == ".xaml")
                        {
                            if (!string.IsNullOrWhiteSpace( keyCrypt ))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    textRange.Save( ms, DataFormats.XamlPackage );
                                    bytes = ms.ToArray();
                                }
                                bytes = encryptionModel.Encrypt( bytes, keyCrypt );
                                fs.Write( bytes );
                            }
                            else
                            {
                                TextRange range;
                                range = new TextRange( richTextBoxViewModel.Document.ContentStart, richTextBoxViewModel.Document.ContentEnd );
                                if (range.CanSave( DataFormats.XamlPackage ))
                                    range.Save( fs, DataFormats.XamlPackage );
                            }
                        }
                    }
                    catch
                    {
                        if (fs != null)
                            fs.Close();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorWindow( e );
            }
        }

        internal bool CanExecute_SaveAsFile( object obj )
        {
            try
            {
                bool c = false;
                c = !richTextBoxViewModel.BehaviorRichTextBox.TextRange.IsEmpty;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SaveAsFile( object obj )
        {
            try
            {
                string path = null;
                if ((path = SaveFileDialog( Filter, CurrentDirectorySave )) != null)
                {
                    Execute_SaveFile( path );
                    PathToLastFile = path;
                    CurrentDirectorySave = Path.GetDirectoryName( path );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// загрузка файлов из интернета
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_UploadingFiles( object obj )
        {
            try
            {
                bool c = false;
                c = mainWindowViewModel.FrameListAddPage.CanExecute( new Views.FileUploader() { KeepAlive = true } );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_UploadingFiles( object obj )
        {
            try
            {
                mainWindowViewModel.FrameListAddPage.Execute( new Views.FileUploader() { KeepAlive = true } );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// обзор файлов с возможностью их расшифровки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_FileOverview( object obj )
        {
            try
            {
                bool c = true;
                foreach(var win in Application.Current.Windows)
                {
                    if (win is Views.FileOverview)
                    {
                        c = false;
                        break;
                    }
                }
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }

        }
        internal void Execute_FileOverview( object obj )
        {
            try
            {
                var overview = new Views.FileOverview();
                overview.Owner = Application.Current.MainWindow;
                overview.Show();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_SelectingAWorkingDirectory( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SelectingAWorkingDirectory( object obj )
        {
            try
            {
                Views.FolderBrowserDialog folder = new FolderBrowserDialog();
                folder.Closing += ( s, e ) =>
                {
                    if (s is FolderBrowserDialog window && window.DataContext is FolderBrowserDialogViewModel viewmodel)
                    {
                        if (!string.IsNullOrWhiteSpace( viewmodel.WorkingDirectory ))
                            WorkingDirectory = viewmodel.WorkingDirectory;
                        Properties.Settings.Default.WorkingDirectory = WorkingDirectory;
                    }
                };
                folder.ShowDialog();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// синхронизация рабочего каталога со сторонним
        /// </summary>
        /// <param name="obj"> путь к стороннему каталогу(string) </param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool WorkingDirectorySynchronization_CanExecute( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( WorkingDirectory );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void WorkingDirectorySynchronization_Execute( object obj )
        {
            try
            {
                bool rezult = false;
                var messages = new MyMessages();
                var messagesVM = (ViewModels.MyMessagesViewModel)messages.DataContext;
                messages.Closing += ( s, e ) => { rezult = messagesVM.Rezult; };
                messagesVM.SetTitle.Execute( mainWindowViewModel.Language.MyMessagesHeaders[3] );
                messagesVM.SetButtonText.Execute( mainWindowViewModel.Language.MyMessagesHeaders[4] );
                messagesVM.SetMessage.Execute( mainWindowViewModel.Language.MessagesMyMessages[6] );
                messages.ShowDialog();
                if (rezult)
                {
                    Views.FolderBrowserDialog dialog = new FolderBrowserDialog();
                    var viewmodel = (ViewModels.FolderBrowserDialogViewModel)dialog.DataContext;
                    dialog.Closing += async ( s, e ) =>
                    {
                        string path = viewmodel.WorkingDirectory;
                        if (path != null)
                        {
                            var progress = new DisplayProgress();
                            var progressVM = (DisplayProgressViewModel)progress.DataContext;
                            progressVM.Target = this;
                            PropertyChanged += ( s, e ) => progressVM.OnPropertyChanged( e.PropertyName );
                            progress.Show();

                            messages = new MyMessages();
                            messagesVM = (ViewModels.MyMessagesViewModel)messages.DataContext;
                            messagesVM.SetButtonText.Execute( "Ok" );
                            var task = await Task<bool>.Factory.StartNew( () => SynchronizationOfTwoDirectories( path, WorkingDirectory ) );
                            if (task)
                            {//синхронизация прошла успешно
                                messagesVM.SetMessage.Execute( mainWindowViewModel.Language.MessagesMyMessages[4] );
                            }
                            else
                            {//ошибка при синхронизации
                                messagesVM.SetMessage.Execute( mainWindowViewModel.Language.MessagesMyMessages[5] );
                            }
                            messages.ShowDialog();
                        }
                    };
                    dialog.ShowDialog();
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal string SaveFileDialog( string filter, string initialDirectory )
        {
            try
            {
                string path = null;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = filter;
                if (!string.IsNullOrWhiteSpace( initialDirectory ))
                    sfd.InitialDirectory = initialDirectory;
                if ((bool)sfd.ShowDialog())
                {
                    path = sfd.FileName;
                }
                return path;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        internal string OpenFileDialog( string filter, string initialDirectory )
        {
            try
            {
                string path = null;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = filter;
                if (!string.IsNullOrWhiteSpace( initialDirectory ) && Directory.Exists( initialDirectory ))
                    ofd.InitialDirectory = initialDirectory;
                if ((bool)ofd.ShowDialog())
                {
                    path = ofd.FileName;
                }
                return path;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        /// <summary>
        /// синхронизация содержимого двух каталогов
        /// </summary>
        /// <param name="path1">полный путь к первому каталогу </param>
        /// <param name="path2">полный путь ко второму каталогу</param>
        /// <returns>разультат выполнения синхронизации: true - успешно; false - ошибка</returns>
        private bool SynchronizationOfTwoDirectories( string path1, string path2 )
        {
            try
            {
                bool c = false;
                c = Directory.Exists( path1 ) && Directory.Exists( path2 );
                if (c)
                {
                    //файлы из catalog1
                    string[] catalog1 = Directory.GetFiles( path1 );
                    //файлы из catalog2
                    string[] catalog2 = Directory.GetFiles( path2 );
                    //библиотека идентичных файлов из catalog1
                    Dictionary<string, FileInfo> identical1 = new Dictionary<string, FileInfo>();
                    //библиотека идентичных файлов из catalog2
                    Dictionary<string, FileInfo> identical2 = new Dictionary<string, FileInfo>();
                    //библиотека неидентичных файлов из обоих каталогов
                    Dictionary<string, FileInfo> nonidentical = new Dictionary<string, FileInfo>();
                    //перебираем catalog1 заполняя библиотеки с идентичными и неидентичными файлами
                    foreach (string path in catalog1)
                    {
                        //имя файла
                        string name = System.IO.Path.GetFileName( path );
                        //возможный путь к файлу во втором catalog2
                        string newPath = System.IO.Path.Combine( path2, name );
                        if (File.Exists( newPath ))
                        {//файл существует в catalog2
                            //добавляем пути к файлам в соотв-ие библиотеки
                            identical1.Add( name, new FileInfo( path ) );
                            identical2.Add( name, new FileInfo( newPath ) );
                        }
                        else
                        {//файл не существует в catalog2
                            //добавляем пути к файлам в библиотеку с неидентичными файлами
                            nonidentical.Add( name, new FileInfo( path ) );
                        }
                    }
                    ProgressValue = 30;
                    //перебираем catalog2 и дополняем библиотеку с неидентичными файлами
                    foreach (string path in catalog2)
                    {
                        if (!identical2.Any( ( x ) => x.Value.FullName == path ))
                        {
                            nonidentical.Add( Path.GetFileName( path ), new FileInfo( path ) );
                        }
                    }
                    ProgressValue = 50;
                    //добавляем в каталоги неидентичные файлы если их там нет
                    foreach (string key in nonidentical.Keys)
                    {
                        var path = Path.Combine( path1, key );
                        if (!File.Exists( path ))
                            File.Copy( nonidentical[key].FullName, path );
                        path = Path.Combine( path2, key );
                        if (!File.Exists( path ))
                            File.Copy( nonidentical[key].FullName, path );
                    }
                    ProgressValue = 70;
                    //заменяем старые файлы на более новые
                    foreach (string key in identical1.Keys)
                    {
                        //проверяем время последнего изменения файлов
                        if (identical1[key].LastWriteTime > identical2[key].LastWriteTime)
                        {//файл в catalog1 изменен позже чем в catalog2
                         //удаляем устаревший файл
                            File.Delete( identical2[key].FullName );
                            //копируем более новый вместо устаревшего
                            File.Copy( identical1[key].FullName, identical2[key].FullName );
                        }
                        else if (identical1[key].LastWriteTime < identical2[key].LastWriteTime)
                        {//файл в catalog2 изменен позже чем в catalog1 
                         //удаляем устаревший файл
                            File.Delete( identical1[key].FullName );
                            //копируем более новый вместо устаревшего
                            File.Copy( identical2[key].FullName, identical1[key].FullName );
                        }
                    }
                    for (int a = 70; a <= 100; a++)
                    {//цикл для красоты, можно убрать(добавить обязательно: ProgressValue = 100;)
                        Thread.Sleep( 50 );
                        ProgressValue = a;
                    }
                    //ProgressValue = 100;
                }
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Views.MyMessages myMessages = new MyMessages();
            var viewmodel = (ViewModels.MyMessagesViewModel)myMessages.DataContext;
            var mytype = GetType().ToString().Split( '.' ).LastOrDefault();
            viewmodel.SetTitle.Execute( $"{mainWindowViewModel.Language.MyMessagesHeaders[0]}! ({mytype}.{name})" );
            viewmodel.SetMessage.Execute( e.Message );
            myMessages.ShowDialog();
        }

    }
}
