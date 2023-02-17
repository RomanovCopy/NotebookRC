using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using controls = System.Windows.Controls;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Models
{
    public class SearchForFilesModel : ViewModelBase
    {

        internal MainWindowViewModel MainWindowViewModel => mainWindowViewModel;
        private readonly MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// инициатор запроса только Home
        /// </summary>
        internal object RequestInitiator { get => requestInitiator; set => SetProperty(ref requestInitiator, value); }
        private object requestInitiator;


        /// <summary>
        /// обнаруженные при поиске файлы
        /// </summary>
        internal ObservableCollection<FileInfo> DetectedFiles
        {
            get => detectedFiles;
            set => SetProperty(ref detectedFiles, value);
        }
        private ObservableCollection<FileInfo> detectedFiles;

        internal Languages Language => language;
        private readonly Languages language;

        /// <summary>
        /// размеры колонок в процентах
        /// </summary>
        internal ObservableCollection<double> ColumnsWidth 
        { 
            get => columnsWidth; 
            set => SetProperty(ref columnsWidth, value); 
        }
        private ObservableCollection<double> columnsWidth;

        internal ObservableCollection<string> Headers => Language.SearchForFiles;
        internal ObservableCollection<string> ToolTips => Language.ToolTipsSearchForFiles;

        /// <summary>
        /// делегат выполняемый после определения BehaviorFlowDocumentReader
        /// </summary>
        internal Action BehaviorReady { get => behaviorReady; set => SetProperty(ref behaviorReady, value); }
        private Action behaviorReady;

        public SearchForFilesModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language = MainWindowViewModel.Language;
            Language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            DetectedFiles = new ObservableCollection<FileInfo>();
            //Properties.Settings.Default.SearchForFilesColumnsWidth = null;
            if (Properties.Settings.Default.SearchForFilesColumnsWidth == null)
            {
                Properties.Settings.Default.SearchForFilesColumnsWidth = new System.Collections.Specialized.StringCollection()
                {
                    "30", "30", "40"
                };
            }
            ColumnsWidth = new ObservableCollection<double>();
            foreach(var str in Properties.Settings.Default.SearchForFilesColumnsWidth)
            {
                ColumnsWidth.Add(double.Parse(str));
            }
        }

        internal bool CanExecute_ListViewPreviewMouseDoubleClick( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ListViewPreviewMouseDoubleClick( object obj )
        {
            try
            {
                if (obj is FileInfo info && RequestInitiator is System.Windows.Controls.Page page)
                {
                    var home = MainWindowViewModel.FrameList.Where((x) => x is Views.Home).LastOrDefault();
                    MainWindowViewModel.CurrentPage = page;
                    var menuhome = (MyControls.MenuHome)home.FindResource("menuhome");
                    var homefile = (ViewModels.HomeMenuFileViewModel)menuhome.FindResource("menufile");
                    if (homefile.OpenFile.CanExecute(info.FullName))
                        homefile.OpenFile.Execute(info.FullName);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_DeleteFile(object obj)
        {
            try
            {
                bool c = false;
                c = obj is string path && File.Exists(path);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_DeleteFile(object obj)
        {
            try
            {
                if(obj is string path)
                {
                    var file = DetectedFiles.Where((x) => x.FullName == path).FirstOrDefault();
                    if (file != null)
                    {
                        DetectedFiles.Remove(file);
                        File.Delete(file.FullName);
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_ListViewSelectionChanged( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ListViewSelectionChanged( object obj )
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_ListViewLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ListViewLoaded( object obj )
        {
            try
            {
                if (obj is controls.ListView list)
                {
                    list.SizeChanged += ( s, e ) => { OnPropertyChanged(new string[] { "ColumnsWidth" }); };
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageClose( object obj )
        {
            try
            {
                bool c = false;
                c = MainWindowViewModel.FrameList.Count > 1;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose( object obj )
        {
            try
            {
                if (obj is controls.Page page && MainWindowViewModel.PageClosed.CanExecute(page))
                {
                    if (Properties.Settings.Default.SearchForFilesColumnsWidth == null)
                        Properties.Settings.Default.SearchForFilesColumnsWidth = new System.Collections.Specialized.StringCollection();
                    else
                        Properties.Settings.Default.SearchForFilesColumnsWidth.Clear();
                    foreach(double d in ColumnsWidth)
                    {
                        Properties.Settings.Default.SearchForFilesColumnsWidth.Add(d.ToString());
                    }
                    MainWindowViewModel.PageClosed.Execute(page);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }






        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"SearchForFilesModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


    }
}
