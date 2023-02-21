using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.IO;
using System.Linq;
using Controls = System.Windows.Controls;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.Runtime.CompilerServices;
using System.Threading;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Helpers;
using System.Windows.Controls;
using NotebookRCv001.Interfaces;
using System.Drawing;

namespace NotebookRCv001.Styles.CustomizedWindow
{
    //Реализация перехода между страницами с помощью кнопок и выбора в комбобоксе
    public class NavigateService : ViewModelBase
    {
        public MainWindowViewModel MainWindowViewModel => mainWindowViewModel ??= (MainWindowViewModel)Application.Current.MainWindow.DataContext;
        MainWindowViewModel mainWindowViewModel;

        BehaviorComboBox BehaviorComboBox { get; set; }

        public ObservableCollection<string> ToolTips => MainWindowViewModel.Language.ToolTipsMainWindow;

        public string PathToLastFile
        {
            get => pathToLastFile;
            set => SetProperty(ref pathToLastFile, value);
        }
        string pathToLastFile;

        public string LastFileName
        {
            get => lastFileName;
            set => SetProperty(ref lastFileName, value);
        }
        string lastFileName;

        public NavigateService()
        {
            //подписываемся на изменения в коллекции страниц фрейма(FrameList)
            MainWindowViewModel.FrameList.CollectionChanged += FrameList_CollectionChanged;
            //заносим адреса изображений в коллекции для дальнейшего помещения их в ресурсы
            MainWindowViewModel.Language.PropertyChanged += (s, e) =>
            OnPropertyChanged(new string[] { "ToolTips", "Headers" });
        }

        private void FrameList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add://элемент добавлен в коллекцию
                        {
                            if (e.NewItems[0] is Page p && p.DataContext is ILastFileName lastname)
                            {
                                lastname.PropertyChanged += (s, e) =>
                                {
                                    if ( e.PropertyName == "LastFileName")
                                    {
                                        LastFileName = lastname.LastFileName;
                                    }
                                    else if (e.PropertyName == "PathToLastFile")
                                    {
                                        PathToLastFile = lastname.PathToLastFile;
                                    }
                                };
                                p.Loaded += (s, e) =>
                                {
                                    LastFileName = lastname.LastFileName;
                                    PathToLastFile = lastname.PathToLastFile;
                                };
                                p.Unloaded += (s, e) =>
                                {
                                    LastFileName = "";
                                    PathToLastFile = "";
                                };
                            }
                            else
                            {
                                LastFileName = "";
                                PathToLastFile = "";
                            }

                            var page = (Page)MainWindowViewModel.CurrentPage;
                            Controls.ComboBoxItem item = new()
                            { Content = page.Title, IsSelected = true, Tag = e.NewItems[0] };
                            item.PreviewMouseLeftButtonDown += ComboBoxItem_PreviewMouseLeftButtonDown;
                            Items.Add(item);
                            break;
                        }
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove://элемент был удален из коллекции
                        {
                            Controls.ComboBoxItem item = null;//удаляемая страница
                            Controls.ComboBoxItem item1 = null;//выбранная страница
                            foreach (var a in Items)
                            {
                                if (a.Tag is Controls.Page b)
                                {
                                    if (b.Equals(e.OldItems[0]))
                                    {//определение айтема соответсвующего удаленной странице
                                        item = a;
                                    }
                                    if (b.Equals(MainWindowViewModel.CurrentPage))
                                    {//определение итема соответствующего выбранной странице
                                        item1 = a;
                                    }
                                }
                            }

                            if (item1 != null)
                            {
                                //item1.IsSelected = true;
                                BehaviorComboBox.SelectedItem = item1;
                            }
                            Items.Remove(item);
                            break;
                        }
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace://замена элемента в коллекции
                        {
                            break;
                        }
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move://перемещение элемента в пределах коллекции
                        {
                            break;
                        }
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset://содержимое коллекции резко изменилось
                        {
                            break;
                        }
                }
            }
            catch (Exception e1)
            {
                ErrorWindow(e1);
            }
        }


        private void ComboBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Controls.ComboBoxItem item = sender as Controls.ComboBoxItem;
                if (item.Tag is Controls.Page a)
                {
                    if (!a.Equals(MainWindowViewModel.CurrentPage))
                    {//выбранная страница не является текущей
                        MainWindowViewModel.CurrentPage = a;
                        item.IsSelected = true;
                        if (a is Views.Home home)
                        {
                            var menuhome = (MyControls.MenuHome)home.FindResource("menuhome");
                            var menufile = (HomeMenuFileViewModel)menuhome.FindResource("menufile");
                            PathToLastFile = menufile.PathToLastFile;
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                ErrorWindow(e1);
            }
        }

        public ObservableCollection<Controls.ComboBoxItem> Items
        {
            get => items ??= new ObservableCollection<Controls.ComboBoxItem>();
            private set => items = value;
        }
        ObservableCollection<Controls.ComboBoxItem> items;



        /// <summary>
        /// контент обнаружен и начата его загрузка во фрейм
        /// </summary>
        public ICommand FrameNavigate => frameNavigate ??= new RelayCommand(Execute_FrameNavigate, CanExecute_FrameNavigate);
        RelayCommand frameNavigate;
        private bool CanExecute_FrameNavigate(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        private void Execute_FrameNavigate(object obj)
        {
            try
            {
                if (obj is Controls.Frame frame)
                {
                    Controls.ComboBoxItem item = null;
                    if ((item = Items.Where((x) => x.Tag.Equals(frame.Content)).FirstOrDefault()) != null)
                    {
                        BehaviorComboBox.SelectedItem = item;
                    }
                    else
                    {
                        MainWindowViewModel.FrameList.Add((Controls.Page)frame.Content);//далее сработает метод FrameList_CollectionChanged
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }




        /// <summary>
        /// нажатие на одну из кнопок навигации
        /// </summary>
        public ICommand NavigateButton_Click
        {
            get => navigateButton_Click =
                navigateButton_Click ?? new RelayCommand(ExecuteNavigateButton_Click, CanExecuteNavigateButton_Click);
        }
        RelayCommand navigateButton_Click;
        private bool CanExecuteNavigateButton_Click(object obj)
        {
            try
            {
                bool c = false;
                if (obj is string button)
                {
                    switch (button)
                    {
                        case "back":
                            {
                                c = MainWindowViewModel.Frmelist_GoBack.CanExecute(null);
                                break;
                            }
                        case "forward":
                            {
                                c = MainWindowViewModel.Frmelist_GoForward.CanExecute(null);
                                break;
                            }
                    }
                }
                return c;
            }
            catch
            {
                return false;
            }
        }
        private void ExecuteNavigateButton_Click(object obj)
        {
            try
            {
                if (obj is string button)
                {
                    if (button == "back")
                    {
                        MainWindowViewModel.Frmelist_GoBack.Execute(null);
                    }
                    else if (button == "forward")
                    {
                        MainWindowViewModel.Frmelist_GoForward.Execute(null);
                    }
                    UpdateComboBox();
                }
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }

        public ICommand ComboBoxLoaded => comboBoxLoaded ??= new RelayCommand(Execute_ComboBoxLoaded, CanExecute_ComboBoxLoaded);
        RelayCommand comboBoxLoaded;

        private bool CanExecute_ComboBoxLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        private void Execute_ComboBoxLoaded(object obj)
        {
            try
            {
                if (obj is BehaviorComboBox behavior)
                    BehaviorComboBox = behavior;
            }
            catch (Exception e) { ErrorWindow(e); }
        }










        /// <summary>
        /// обновление текущего состояния ComboBox 
        /// </summary>
        public void UpdateComboBox()
        {
            try
            {
                foreach (Controls.ComboBoxItem a in BehaviorComboBox.Items)
                {
                    //a.IsSelected = (a.Tag as Controls.Page).Equals(MainWindowViewModel.CurrentPage);
                    if ((a.Tag as Controls.Page).Equals(MainWindowViewModel.CurrentPage))
                    {//определение айтема соотвествующего текущей странице
                        var page = (System.Windows.Controls.Page)MainWindowViewModel.CurrentPage;
                        a.Content = page.Title;
                        BehaviorComboBox.SelectedItem = a;
                        break;
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        #region ________________________TextBox Search_________________________________________________________________


        /// <summary>
        /// окончание загрузки TextBox search
        /// </summary>
        public ICommand TextBoxSearchLoaded => textBoxSearchLoaded ??= new RelayCommand(Execute_TextBoxSearchLoaded);
        private RelayCommand textBoxSearchLoaded;

        /// <summary>
        /// обработка команды окончания загрузки TextBox search
        /// </summary>
        /// <param name="obj">загруженный textbox</param>
        private void Execute_TextBoxSearchLoaded(object obj)
        {
            try
            {
                if (obj is Controls.TextBox textBox)
                    textBox.PreviewKeyDown += (s, e) => TextBoxSearchPreviewKeyDown(s, e);//подписываемся на событие ввода символа
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// обработка события ввода символа в TextBox search
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void TextBoxSearchPreviewKeyDown(object s, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && s is Controls.TextBox textbox)
                {
                    var l = SearchFiles(textbox.Text);
                    if (l != null && l.Count > 0)
                    {
                        var search = new Views.SearchForFiles();
                        var viewmodel = (SearchForFilesViewModel)search.DataContext;
                        viewmodel.RequestInitiator = MainWindowViewModel.CurrentPage;
                        foreach (var info in l)
                        {
                            if (info != null)
                            {
                                viewmodel.DetectedFiles.Add(info);
                            }
                        }
                        if (mainWindowViewModel.FrameListAddPage.CanExecute(search))
                            MainWindowViewModel.FrameListAddPage.Execute(search);
                    }
                }
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        private List<FileInfo> SearchFiles(string key)
        {
            try
            {
                List<FileInfo> list = new();
                var home = MainWindowViewModel.FrameList.Where((x) => x is Views.Home).LastOrDefault();
                if (home != null)
                {
                    var menuhome = (MyControls.MenuHome)home.FindResource("menuhome");
                    var menufile = (HomeMenuFileViewModel)menuhome.FindResource("menufile");
                    string path = menufile.WorkingDirectory;
                    DirectoryInfo dir = null;
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        dir = new DirectoryInfo(path);
                        var a = dir.GetFiles();
                        foreach (var i in a)
                        {
                            if (i.Name.ToLower().Contains(key.ToLower()))
                                list.Add(i);
                        }
                    }
                }
                else
                    return null;
                return list;
            }
            catch (Exception e) { ErrorWindow(e); return null; }
        }


        #endregion


        public void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new Thread(() => System.Windows.MessageBox.Show(e.Message, $"NavigateService.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }

    internal static class LocalExtensions
    {
        public static void ForWindowFromChild(this object childDependencyObject, Action<Window> action)
        {
            var element = childDependencyObject as DependencyObject;
            while (element != null)
            {
                element = VisualTreeHelper.GetParent(element);
                if (element is Window)
                { action(element as Window); break; }
            }
        }

        public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
        {
            Window window = ((FrameworkElement)templateFrameworkElement).TemplatedParent as Window;
            if (window != null)
                action(window);
        }

        public static IntPtr GetWindowHandle(this Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            return helper.Handle;
        }

    }

    public partial class VS2012WindowStyle
    {

        public VS2012WindowStyle()
        {

        }

        #region sizing event handlers

        void OnSizeSouth(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.South); }
        void OnSizeNorth(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.North); }
        void OnSizeEast(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.East); }
        void OnSizeWest(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.West); }
        void OnSizeNorthWest(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.NorthWest); }
        void OnSizeNorthEast(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.NorthEast); }
        void OnSizeSouthEast(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.SouthEast); }
        void OnSizeSouthWest(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.SouthWest); }

        void OnSize(object sender, SizingAction action)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w =>
                    {
                        if (w.WindowState == WindowState.Normal)
                            DragSize(w.GetWindowHandle(), action);
                    });
            }
        }



        void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                sender.ForWindowFromTemplate(w => w.Close());
            }
            else
            {
                sender.ForWindowFromTemplate(w =>
                    SendMessage(w.GetWindowHandle(), WM_SYSCOMMAND, (IntPtr)SC_KEYMENU, (IntPtr)' '));
            }
        }

        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate((w) =>
            {
                w.Close();
            });
        }

        void MinButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = WindowState.Minimized);
        }

        void MaxButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = (w.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized);
        }

        void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                MaxButtonClick(sender, e);
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w => w.DragMove());
            }
        }

        void TitleBarMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w =>
                    {
                        if (w.WindowState == WindowState.Maximized)
                        {
                            w.BeginInit();
                            double adjustment = 40.0;
                            var mouse1 = e.MouseDevice.GetPosition(w);
                            var width1 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                            w.WindowState = WindowState.Normal;
                            var width2 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                            w.Left = (mouse1.X - adjustment) * (1 - width2 / width1);
                            w.Top = -7;
                            w.EndInit();
                            w.DragMove();
                        }
                    });
            }
        }

        #endregion


        #region P/Invoke

        const int WM_SYSCOMMAND = 0x112;
        const int SC_SIZE = 0xF000;
        const int SC_KEYMENU = 0xF100;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        void DragSize(IntPtr handle, SizingAction sizingAction)
        {
            SendMessage(handle, WM_SYSCOMMAND, (IntPtr)(SC_SIZE + sizingAction), IntPtr.Zero);
            SendMessage(handle, 514, IntPtr.Zero, IntPtr.Zero);
        }

        public enum SizingAction
        {
            North = 3,
            South = 6,
            East = 2,
            West = 1,
            NorthEast = 5,
            NorthWest = 4,
            SouthEast = 8,
            SouthWest = 7
        }

        #endregion

        public void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new Thread(() => System.Windows.MessageBox.Show(e.Message, $"VS2012WindowStyle.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}