using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using controls = System.Windows.Controls;
using Command_executors;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;

using System.Windows.Input;

namespace NotebookRCv001.Models
{
    internal class DownloadItemModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly FileUploaderViewModel fileUploaderViewModel;
        private HttpWebResponse webResponse { get; set; }

        /// <summary>
        /// статусы загрузки
        /// </summary>
        private struct Statuses
        {
            public static string Preparation { get => "Preparation"; }
            public static string Download { get => "Download"; }
            public static string Loaded { get => "Loaded"; }
            public static string Pause { get => "Pause"; }
            public static string Stop { get => "Stop"; }
            public static string Waiting { get => "Waiting"; }
            public static string Error { get => "Error"; }
        }

        /// <summary>
        /// заготовка для создания токена отмены загрузки
        /// </summary>
        private CancellationTokenSource cancel { get; set; }

        /// <summary>
        /// отсчет времени для измерения скорости загрузки
        /// </summary>
        public Stopwatch Stopwatch { get => stopwatch ??= new Stopwatch(); set => stopwatch = value; }
        Stopwatch stopwatch;
        /// <summary>
        /// предпологаемое имя файла
        /// </summary>
        internal string SuggestedFileName { get => suggestedFileName; set => SetProperty( ref suggestedFileName, value ); }
        private string suggestedFileName;
        internal string OriginalUrl { get => originalUrl; set => SetProperty( ref originalUrl, value ); }
        private string originalUrl;
        internal string Url { get => url; set => SetProperty( ref url, value ); }
        private string url;
        internal string Id { get => id; set => SetProperty( ref id, value ); }
        private string id;
        internal string FullPath { get => fullPath; set => SetProperty( ref fullPath, value ); }
        private string fullPath;

        /// <summary>
        /// флаг : размер загружаемого файла не определен
        /// </summary>
        internal bool FileSizeUnknown { get => fileSizeUnknown; set => SetProperty( ref fileSizeUnknown, value ); }
        private bool fileSizeUnknown;

        /// <summary>
        /// переменная потока считывания потока считывания с сервера
        /// </summary>
        internal Stream StreamResponse
        {
            get
            {
                if (streamResponse == null)
                {
                    streamResponse = webResponse != null ? webResponse.GetResponseStream() : null;
                }
                return streamResponse;
            }
        }
        Stream streamResponse;

        /// <summary>
        /// размер массива для приема байт при загрузке файлов
        /// </summary>
        internal long BufferSize { get => bufferSize; set => SetProperty( ref bufferSize, value ); }
        long bufferSize;

        /// <summary>
        /// переменная содержащая предыдущее значение количества загруженных байтов.необходимо для расчета скорости загрузки
        /// </summary>
        private long lastSize { get; set; }

        /// <summary>
        /// имя каталога в который загружается файл
        /// </summary>
        internal string DirectoryName { get => FullPath != null ? Directory.GetParent( FullPath ).Name : ""; }
        internal DateTime? EndTime { get => endTime; set => SetProperty( ref endTime, value ); }
        private DateTime? endTime;
        internal DateTime? StartTime { get => startTime; set => SetProperty( ref startTime, value ); }
        private DateTime? startTime;
        internal long ReceivedBytes { get => receivedBytes; set => SetProperty( ref receivedBytes, value ); }
        private long receivedBytes;
        internal long TotalBytes { get => totalBytes; set => SetProperty( ref totalBytes, value ); }
        private long totalBytes;
        internal int PercentComplete { get => percentComplete; set => SetProperty( ref percentComplete, value ); }
        private int percentComplete;
        internal long CurrentSpeed
        {
            get => currentSpeed;
            set => SetProperty( ref currentSpeed, value );
        }
        private long currentSpeed;
        internal bool IsCancelled { get => isCancelled; set => SetProperty( ref isCancelled, value ); }
        private bool isCancelled;
        internal bool IsComplete { get => isComplete; set => SetProperty( ref isComplete, value ); }
        private bool isComplete;
        /// <summary>
        /// загрузка выполняется
        /// </summary>
        internal bool IsInProgress { get => isInProgress; set => SetProperty( ref isInProgress, value ); }
        private bool isInProgress;
        /// <summary>
        /// флаг: возможность продолжения загрузки файла
        /// </summary>
        internal bool IsValid { get => isValid; set => SetProperty( ref isValid, value ); }
        private bool isValid;
        internal string MimeType { get => mimeType; set => SetProperty( ref mimeType, value ); }
        private string mimeType;

        internal string Status { get => status; set => SetProperty( ref status, value ); }
        private string status;

        /// <summary>
        /// полный путь к каталогу для сохранения загруженных файлов
        /// </summary>
        internal string ContentDisposition { get => contentDisposition; set => SetProperty( ref contentDisposition, value ); }
        private string contentDisposition;

        internal DownloadItemModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            fileUploaderViewModel = (FileUploaderViewModel)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.FileUploader ).FirstOrDefault()?.DataContext;
            var dir = fileUploaderViewModel.ContentDisposition;
            if (!string.IsNullOrWhiteSpace( dir ) && Directory.Exists( dir ))
                ContentDisposition = dir;
            FileSizeUnknown = false;
        }

        /// <summary>
        /// подготовка к загрузке файла
        /// </summary>
        /// <param name="obj"> null </param>
        /// <returns></returns>
        internal bool CanExecute_Preparation( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( Url );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; };
        }
        internal async void Execute_Preparation( object obj )
        {
            try
            {
                Status = Statuses.Preparation;
                webResponse = await Task<HttpWebResponse>.Factory.StartNew( () => Executors.Request( Url, 50000, 0 ).Result );
                if (webResponse != null)
                {
                    CurrentSpeed = 0;
                    MimeType = webResponse.ContentType;
                    TotalBytes = webResponse.ContentLength < 0 ? 0 : webResponse.ContentLength;
                    SuggestedFileName = webResponse.GetResponseHeader( "content-disposition" );
                    if (!string.IsNullOrWhiteSpace( SuggestedFileName ))
                    {
                        ContentDisposition disp = new ContentDisposition( SuggestedFileName );
                        SuggestedFileName = disp.FileName;
                    }
                    else
                    {
                        var a = Url.Split( '/' );
                        SuggestedFileName = a[a.Length - 1];
                    }
                    SuggestedFileName = SuggestedFileName.Split( '.' )[0];
                    var array = MimeType.Split( '/' );
                    string ex = array[array.Length - 1];
                    if (!string.IsNullOrWhiteSpace( SuggestedFileName ) && !string.IsNullOrWhiteSpace( ContentDisposition ))
                        FullPath = Path.Combine( ContentDisposition, $"{SuggestedFileName}.{ex}" );
                    Status = Statuses.Waiting;
                }
                else
                    Status = Statuses.Error;
            }
            catch (Exception e) { ErrorWindow( e ); Status = Statuses.Error; };
        }

        /// <summary>
        /// обработка команды запуска загрузки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Start( object obj )
        {
            try
            {
                bool c = false;
                c = webResponse != null && Status != Statuses.Download;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; };
        }
        internal async void Execute_Start( object obj )
        {
            try
            {
                FileSizeUnknown = webResponse?.ContentLength < 0;
                bool c = false;
                if (File.Exists( FullPath ) && Status != Statuses.Pause)
                {//обработка уже существующего файла
                    ReceivedBytes = new FileInfo( FullPath ).Length;
                    c = ProcessingAnExistingFile( FullPath );
                    if (!c)
                    {
                        Status = Statuses.Error;
                        return;
                    }
                }
                Status = Statuses.Download;
                c = await DownloadFile();
                Stopwatch.Stop();
                if (c && ReceivedBytes >= TotalBytes)
                    Status = Statuses.Loaded;
            }
            catch (Exception e) { ErrorWindow( e ); Status = Statuses.Error; };
        }

        /// <summary>
        /// обработка команды паузы в загрузке
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Pause( object obj )
        {
            try
            {
                bool c = false;
                if (webResponse != null)
                    c = Status == "Download" /*&& webResponse.GetResponseStream().CanSeek*/;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; };
        }
        internal void Execute_Pause( object obj )
        {
            try
            {
                cancel.Cancel();
                cancel.Dispose();
                CurrentSpeed = 0;
                Status = Statuses.Pause;

            }
            catch (Exception e) { ErrorWindow( e ); Status = Statuses.Error; };
        }

        /// <summary>
        /// обработка команды обновления загрузки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_Reload( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( url ) && !string.IsNullOrWhiteSpace( FullPath );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; };
        }
        internal async void Execute_Reload( object obj )
        {
            try
            {
                StreamDispose();
                Command_executors.Executors.Delete( FullPath );
                ReceivedBytes = 0;
                PercentComplete = 0;
                if (CanExecute_Preparation( null ))
                {
                    await Task.Factory.StartNew(()=> Execute_Preparation( null ));
                }
                else
                {
                    Status = Statuses.Error;
                    return;
                }
                if (CanExecute_Start( null ))
                {
                    await Task.Factory.StartNew(()=> Execute_Start( null ));
                }
                else
                {
                    Status = Statuses.Error;
                }
            }
            catch (Exception e) { ErrorWindow( e ); Status = Statuses.Error; };
        }

        /// <summary>
        /// обработка команды завершения загрузки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Stop( object obj )
        {
            try
            {
                bool c = false;
                c = webResponse != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; };
        }
        internal void Execute_Stop( object obj )
        {
            try
            {
                var win = new Views.SelectWindow();
                var winVM = (SelectWindowViewModel)win.DataContext;
                winVM.Title = winVM.Headers[6];
                winVM.LeftButtonContent = winVM.Headers[7];
                winVM.CenterButtonContent = winVM.Headers[8];
                winVM.RightButtonContent = winVM.Headers[9];
                winVM.Message = winVM.Messages[2];
                win.Closed += ( s, e ) =>
                {
                    if (winVM.Result == "leftbutton")
                    {//удаление вместе с файлом
                        if (cancel != null && !cancel.IsCancellationRequested)
                        {
                            cancel.Token.Register( () => StreamDispose() );
                            cancel.Cancel();
                            cancel.Dispose();
                        }
                        else
                            StreamDispose();
                        Execute_Remove( FullPath );
                    }
                    else if (winVM.Result == "centerbutton")
                    {//удаление только загрузки
                        if (cancel != null && !cancel.IsCancellationRequested)
                        {
                            cancel.Token.Register( () => StreamDispose() );
                            cancel.Cancel();
                        }
                        else
                            StreamDispose();
                        Execute_Remove( null );
                    }
                    else
                    {//отмена

                    }
                };
                win.ShowDialog();
            }
            catch (Exception e) { ErrorWindow( e ); };
        }

        /// <summary>
        /// окончание загрузки текстбокса с именем файла
        /// </summary>
        /// <param name="obj">загружаемый Textbox</param>
        /// <returns></returns>
        internal bool CanExecute_TextBlockPageNameLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_TextBlockPageNameLoaded( object obj )
        {
            try
            {
                if (obj is controls.TextBlock textBlock)
                {
                    textBlock.PreviewMouseRightButtonDown += ( sender, e ) =>
                    {
                        if (sender is controls.TextBlock textblock)
                        {
                            var win = new Views.PopUpTextBox();
                            win.Closing += ( s, e ) =>
                            {
                                if (s is Window window)
                                {
                                    var viewmodel = (PopUpTextBoxViewModel)window.DataContext;
                                    if (!string.IsNullOrWhiteSpace( viewmodel.Text ))
                                    {
                                        FullPath = FullPath.Replace( $"{SuggestedFileName}.", $"{viewmodel.Text}." );
                                        SuggestedFileName = viewmodel.Text;
                                    }
                                }
                            };
                            win.ShowDialog();
                        }
                    };
                }
            }
            catch (Exception e) { ErrorWindow( e ); };
        }

        /// <summary>
        /// удаление текущей загрузки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_Remove( object obj )
        {
            try
            {
                bool c = false;
                c = Status != Statuses.Download && Status != Statuses.Preparation;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; };
        }
        internal void Execute_Remove( object obj )
        {
            try
            {

                if (obj is string path && File.Exists( path ))
                {
                    Command_executors.Executors.DownloadCanceled += () =>
                    {
                        Command_executors.Executors.Delete( path );
                    };
                    Command_executors.Executors.DownloadCanceled = null;
                    Command_executors.Executors.Delete( path );
                }
                if (cancel != null)
                    if ((bool)!cancel.IsCancellationRequested) { cancel?.Cancel(); cancel?.Dispose(); }
                if (fileUploaderViewModel.ListRemove.CanExecute( Id )) fileUploaderViewModel.ListRemove.Execute( Id );
            }
            catch (Exception e) { ErrorWindow( e ); };
        }


        /// <summary>
        /// переименование загржаемого файла
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_RenameTheFileYouWantToDownload( object obj )
        {
            try
            {
                bool c = false;
                c = !File.Exists( FullPath );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; };
        }
        internal void Execute_RenameTheFileYouWantToDownload( object obj )
        {
            try
            {
                var win = new Views.PopUpTextBox();
                var viewmodel = (PopUpTextBoxViewModel)win.DataContext;
                viewmodel.Text = SuggestedFileName;
                viewmodel.Title = viewmodel.Headers[1];
                win.Closing += ( s, e ) =>
                {
                    if (!string.IsNullOrWhiteSpace( viewmodel.Text ))
                    {
                        FullPath = FullPath.Replace( $"{SuggestedFileName}.", $"{viewmodel.Text}." );
                        SuggestedFileName = viewmodel.Text;
                    }
                };
                win.Show();
            }
            catch (Exception e) { ErrorWindow( e ); };
        }

        internal bool CanExecute_Close( object obj )
        {
            try
            {
                bool c = false;
                c = fileUploaderViewModel.ListDownoadItems.Where( ( x ) => x.Url == Url ).FirstOrDefault() != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_Close( object obj )
        {
            try
            {
                if (cancel != null && !cancel.IsCancellationRequested)
                {
                    cancel.Token.Register( () => StreamDispose() );
                    cancel.Cancel();
                }
                else
                    StreamDispose();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// обработка уже существующего файла
        /// </summary>
        /// <param name="fullPath">полный путь к файлу на диске</param>
        /// <returns></returns>
        private bool ProcessingAnExistingFile( string fullPath )
        {
            try
            {
                bool c = false;
                var messages = new Views.SelectWindow();//окно с запросом действий
                var messagesVM = (SelectWindowViewModel)messages.DataContext;
                messagesVM.Title = messagesVM.Headers[2];
                messagesVM.Message = messagesVM.Messages[1];
                messagesVM.CenterButtonContent = FileSizeUnknown || ReceivedBytes < TotalBytes ? messagesVM.Headers[3] : null;
                messagesVM.LeftButtonContent = messagesVM.Headers[4];
                messagesVM.RightButtonContent = messagesVM.Headers[5];
                messages.Closed += async ( s, e ) =>
                {//Получение и обработка результатов запроса
                    var result = messagesVM.Result;
                    if (result == "leftbutton")
                    {//создание копии
                        int count = 1;
                        var dir = Path.GetDirectoryName( fullPath );
                        var name = Path.GetFileNameWithoutExtension( fullPath );
                        var ext = Path.GetExtension( fullPath );
                        var newname = string.Empty;
                        var newpath = string.Empty;
                        do
                        {
                            newname = $"{name}_{count}{ext}";
                            newpath = Path.Combine( dir, newname );
                            count++;
                        }
                        while (File.Exists( newpath ));
                        FullPath = newpath;
                        SuggestedFileName = newname;
                        c = true;
                    }
                    else if (result == "centerbutton")
                    {//файл загружен, но не полностью, требуется дописать
                        ReceivedBytes = new FileInfo( fullPath ).Length;
                        webResponse = await Task<HttpWebResponse>.Factory.StartNew(() => 
                        Executors.Request(Url, 50000, ReceivedBytes).Result);
                        if (FileSizeUnknown)
                        {

                        }
                        else
                        {
                            TotalBytes = webResponse.ContentLength;
                        }
                        c = true;
                    }
                    else if (result == "rightbutton")
                    {//перезапись файла
                        c = webResponse != null;
                        if (c)
                            Command_executors.Executors.Delete( FullPath );
                        else
                            Status = Statuses.Error;
                    }
                };
                messages.ShowDialog();
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }

        /// <summary>
        /// асинхронная загрузка файла
        /// </summary>
        private async Task<bool> DownloadFile()
        {
            try
            {
                Stopwatch.Start();
                cancel = new();
                var token = cancel.Token;
                ReceivedBytes = lastSize = File.Exists( FullPath ) ? new FileInfo( FullPath ).Length : 0;
                var action = new Action<long>( ( x ) =>
                {
                    if ((ReceivedBytes < TotalBytes && webResponse != null) || FileSizeUnknown)
                    {//загружаемый контент определен и его размер известен
                        ReceivedBytes = x;
                        var touple = CalculatingFileDownloadPercentAndSpeed( ReceivedBytes ).Result;
                        CurrentSpeed = touple.speed;
                        if (!FileSizeUnknown)
                            PercentComplete = touple.percent;
                        else
                            TotalBytes = ReceivedBytes;
                    }
                    else
                        CurrentSpeed = 0;
                } );
                bool c = await Executors.DownloadAsinc( action, webResponse, token, FullPath );
                return c;
            }
            catch (Exception e)
            {
                ErrorWindow( e );
                cancel?.Dispose();
                Status = Statuses.Error;
                return false;
            }
        }
        /// <summary>
        /// закрытие потоков
        /// </summary>
        /// <returns></returns>
        private bool StreamDispose()
        {
            try
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse.Dispose();
                    webResponse = null;
                }
                var stream = Command_executors.Executors.FileStream;
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                    stream = null;
                }
                return true;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }

        /// <summary>
        /// расчет текущих показаний процентовки и скорости загрузки
        /// </summary>
        /// <param name="totalsize"> текущий объем загрузки </param>
        /// <param name="lastSize">предыдущий объем загрузки</param>
        /// <returns>кортеж из двух параметров: процентовки и скорости</returns>
        private async Task<(int percent, long speed)> CalculatingFileDownloadPercentAndSpeed( long totalsize )
        {
            try
            {
                //создание выходного кортежа и расчет скорости
                var tuple = (percent: (int)0, speed: (long)0);
                await Task.Factory.StartNew( () =>
                {
                    if (Stopwatch.Elapsed.Seconds > 0)
                    {
                        double temp = totalsize - lastSize;
                        lastSize = totalsize;
                        tuple.speed = (long)temp / Stopwatch.Elapsed.Seconds;
                        Stopwatch.Restart();
                    }
                    else
                        tuple.speed = CurrentSpeed;
                    //расчет процентовки загрузки
                    if (!FileSizeUnknown)
                    {
                        double percent = TotalBytes / 100;
                        if (percent > 0)
                            tuple.percent = (int)((double)totalsize / (double)percent);
                        else
                            tuple.percent = PercentComplete;
                    }
                } );
                return tuple;
            }
            catch (Exception e) { ErrorWindow( e ); return ((int)0, (long)0); }
        }


    }
}
