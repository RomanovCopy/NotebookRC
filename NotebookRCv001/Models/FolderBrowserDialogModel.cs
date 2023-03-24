using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using controls = System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Helpers;
using System.Runtime.CompilerServices;
using System.Threading;
using NotebookRCv001.Properties;
using System.IO;

namespace NotebookRCv001.Models
{
    public class FolderBrowserDialogModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private BehaviorTreeView BehaviorTreeView { get; set; }

        private HomeMenuFileViewModel HomeMenuFileViewModel { get; set; }

        private Languages language => mainWindowViewModel.Language;
        /// <summary>
        /// контент для кнопок окна
        /// </summary>
        internal ObservableCollection<string> Headers => language.FolderBrowserDialog;
        /// <summary>
        /// подсказки для кнопок окна
        /// </summary>
        internal ObservableCollection<string> ToolTips => language.ToolTipsFolderBrowserDialog;

        /// <summary>
        /// коллекция добавленных директорий
        /// </summary>
        internal ObservableCollection<DirectoryInfo> AddedDirectories
        { get => addedDirectories ??= new ObservableCollection<DirectoryInfo>(); }
        private ObservableCollection<DirectoryInfo> addedDirectories;

        internal string WorkingDirectory { get => workingDirectory; private set => SetProperty(ref workingDirectory, value); }
        private string workingDirectory;


        #region ________________________Положение и размеры главного окна___________________
        internal double WindowHeight
        {
            get => windowHeight;
            set => SetProperty(ref windowHeight, value);
        }
        double windowHeight;
        internal double WindowWidth
        {
            get => windowWidth;
            set => SetProperty(ref windowWidth, value);
        }
        double windowWidth;
        internal double WindowLeft
        {
            get => windowLeft;
            set => SetProperty(ref windowLeft, value);
        }
        double windowLeft;
        internal double WindowTop
        {
            get => windowTop;
            set => SetProperty(ref windowTop, value);
        }
        double windowTop;

        #endregion

        public FolderBrowserDialogModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            var page = (Views.Home)mainWindowViewModel.FrameList.Where((x) => x is Views.Home).FirstOrDefault();
            var menuhome = (MyControls.MenuHome)page.FindResource("menuhome");
            HomeMenuFileViewModel = (HomeMenuFileViewModel)menuhome.FindResource("menufile");
            language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers" });
            WindowHeight = Settings.Default.FolderBrowserDialogHeight;
            WindowWidth = Settings.Default.FolderBrowserDialogWidth;
            WindowLeft = Settings.Default.FolderBrowserDialogLeft;
            WindowTop = Settings.Default.FolderBrowserDialogTop;
        }

        /// <summary>
        /// создать новой директории в выбранной
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ClickNewDirectory( object obj )
        {
            try
            {
                bool c = false;
                if (BehaviorTreeView != null)
                    c = BehaviorTreeView.SelectedItem != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickNewDirectory( object obj )
        {
            try
            {
                DirectoryInfo dir;
                BehaviorTreeView.SelectedItem.IsExpanded = true;
                if (BehaviorTreeView.SelectedItem.Tag is DriveInfo)
                {
                    DriveInfo drive = (DriveInfo)BehaviorTreeView.SelectedItem.Tag;
                    dir = drive.RootDirectory;
                }
                else
                    dir = (DirectoryInfo)BehaviorTreeView.SelectedItem.Tag;
                var textbox = new controls.TextBox() { Width = 100, Height = 20 };
                var item = new controls.TreeViewItem() { Header = textbox };
                textbox.PreviewKeyDown += ( s, e ) =>
                {
                    if (e.Key == Key.Enter && !Directory.Exists(Path.Combine(dir.FullName, textbox.Text)))
                    {
                        item.Header = textbox.Text;
                        string path = Path.Combine(dir.FullName, textbox.Text);
                        item.Tag = Directory.CreateDirectory(path);
                        AddedDirectories.Add((DirectoryInfo)item.Tag);
                        BehaviorTreeView.SelectedItem.IsExpanded = false;
                        BehaviorTreeView.SelectedItem.IsExpanded = true;
                        e.Handled = true;
                    }
                };
                BehaviorTreeView.SelectedItem.Items.Add(item);
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// принять изменения и закрыть окно
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ClickAccept( object obj )
        {
            try
            {
                bool c = false;
                if (BehaviorTreeView != null)
                    c = BehaviorTreeView.SelectedItem != null || AddedDirectories.Count>0;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickAccept( object obj )
        {
            try
            {
                WorkingDirectory = BehaviorTreeView.SelectedItem.Tag.ToString();
                //WorkingDirectory = ((DirectoryInfo)BehaviorTreeView.SelectedItem.Tag).FullName;
                var temp = ((controls.Grid)BehaviorTreeView.TreeView.Parent).Parent;
                if (temp is Window window)
                    window.Close();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// отмена внесенных изменений
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ClickCancel( object obj )
        {
            try
            {
                bool c = false;
                c = AddedDirectories.Any(( x ) => x != null);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickCancel( object obj )
        {
            try
            {
                foreach (var a in AddedDirectories.Reverse())
                {
                    if (a is DirectoryInfo info)
                    {
                        Directory.Delete(info.FullName);
                    }
                }
                BehaviorTreeView.SelectedItem.IsExpanded = false;
                BehaviorTreeView.SelectedItem.IsExpanded = true;
                AddedDirectories.Clear();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// сворачивание всех узлов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ClickMinimizeAllDrives( object obj )
        {
            try
            {
                bool c = false;
                if ( BehaviorTreeView!=null  &&  BehaviorTreeView.TreeView != null)
                {
                    foreach (var item in BehaviorTreeView.TreeView.Items)
                    {
                        if (((controls.TreeViewItem)item).IsExpanded)
                        {
                            c = true;
                            break;
                        }
                    }
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickMinimizeAllDrives( object obj )
        {
            try
            {
                foreach (var item in BehaviorTreeView.TreeView.Items)
                {
                    if (((controls.TreeViewItem)item).IsExpanded)
                    {
                        ((controls.TreeViewItem)item).IsExpanded = false;
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// окончание загрузки treeview
        /// </summary>
        /// <param name="obj">BehaviorTreeView</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_TreeViewLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_TreeViewLoaded( object obj )
        {
            try
            {
                if (obj is BehaviorTreeView behavior)
                {
                    BehaviorTreeView = behavior;
                    LoadingDisks();
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// окончание загрузки окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_FolderBrowserDialogLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_FolderBrowserDialogLoaded( object obj )
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// перед закрытием окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_FolderBrowserDialogClosing( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_FolderBrowserDialogClosing( object obj )
        {
            try
            {
                Settings.Default.FolderBrowserDialogHeight = WindowHeight;
                Settings.Default.FolderBrowserDialogWidth = WindowWidth;
                Settings.Default.FolderBrowserDialogTop = WindowTop;
                Settings.Default.FolderBrowserDialogLeft = WindowLeft;
                Settings.Default.Save();
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal void LoadingDisks()
        {
            try
            {
                //загрузка дисков готовых к работе
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    var item = new controls.TreeViewItem();
                    item.Tag = drive;
                    item.Header = drive.ToString();
                    if (drive.IsReady)//диск готов к работе
                    {
                        if (Directory.EnumerateDirectories(drive.Name).Any(( x ) => x != null))
                            item.Items.Add("*");
                        item.Expanded += ItemExpanded;
                        BehaviorTreeView.TreeView.Items.Add(item);
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        private void ItemExpanded( object sender, RoutedEventArgs e )
        {
            try
            {
                var item = (controls.TreeViewItem)e.OriginalSource;
                item.Items.Clear();
                DirectoryInfo dir;
                if (item.Tag is DriveInfo)
                {
                    DriveInfo drive = (DriveInfo)item.Tag;
                    dir = drive.RootDirectory;
                }
                else
                    dir = (DirectoryInfo)item.Tag;
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    var newitem = new controls.TreeViewItem();
                    newitem.Expanded += ItemExpanded;
                    newitem.Tag = subDir;
                    string[] a = subDir.ToString().Split('\\');
                    newitem.Header = a[^1];
                    try
                    {
                        if (subDir.GetDirectories().Length > 0)
                            newitem.Items.Add("*");
                        item.Items.Add(newitem);
                    }
                    catch { }
                }
                e.Handled = true;
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"FolderBrowserDialogModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
