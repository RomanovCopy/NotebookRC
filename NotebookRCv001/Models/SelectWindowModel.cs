using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Infrastructure;
using System.Runtime.CompilerServices;
using NotebookRCv001.Views;

namespace NotebookRCv001.Models
{
    internal class SelectWindowModel : ViewModelBase
    {
        private object window;
        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;


        #region _________________________Window dimensions and position______________________________

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

        /// <summary>
        /// результат - выбранная кнопка
        /// </summary>
        internal string Result { get => result; set => SetProperty(ref result, value); }
        string result;

        /// <summary>
        /// заголовок окна
        /// </summary>
        internal string Title { get => title; set => SetProperty(ref title, value); }
        private string title;
        /// <summary>
        /// сообщение окна
        /// </summary>
        internal string Message { get => message; set => SetProperty(ref message, value); }
        private string message;
        /// <summary>
        /// текст левой кнопки
        /// </summary>
        internal string LeftButtonContent { get => leftButtonContent; set => SetProperty(ref leftButtonContent, value); }
        string leftButtonContent;
        /// <summary>
        /// текст центральной кнопки
        /// </summary>
        internal string CenterButtonContent { get => centerButtonContent; set => SetProperty(ref centerButtonContent, value); }
        string centerButtonContent;
        /// <summary>
        /// текст правой кнопки
        /// </summary>
        internal string RightButtonContent { get => rightButtonContent; set => SetProperty(ref rightButtonContent, value); }
        string rightButtonContent;

        internal ObservableCollection<string> Headers => language.SelectWindowHeaders;
        internal ObservableCollection<string> ToolTips => language.ToolTipsSelectWindow;
        internal ObservableCollection<string> Messages => language.MessagesSelectWindow;

        internal SelectWindowModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers", "ToolTips", "Messages" });
            if (Properties.Settings.Default.FirstStart)
            {
                WindowHeight = 20;
                WindowWidth = 40;
                WindowLeft = 40;
                WindowTop = 40;
                WindowState = "Normal";
                Properties.Settings.Default.FirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.SelectWindowHeight;
                WindowWidth = Properties.Settings.Default.SelectWindowWidth;
                WindowLeft = Properties.Settings.Default.SelectWindowLeft;
                WindowTop = Properties.Settings.Default.SelectWindowTop;
                WindowState = Properties.Settings.Default.SelectWindowState;
            }
        }


        internal bool CanExecute_ButtonsClick( object obj )
        {
            try
            {
                bool c = false;
                if (obj is System.Windows.Controls.Button button && button.Name is string name)
                {
                    if (name == "leftbutton")
                    {
                        c = LeftButtonContent != null;
                    }
                    else if (name == "centerbutton")
                    {
                        c = CenterButtonContent != null;
                    }
                    else if (name == "rightbutton")
                    {
                        c = RightButtonContent != null;
                    }
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonsClick( object obj )
        {
            try
            {
                if (obj is System.Windows.Controls.Button button && button.Name is string name)
                {
                    Result = name;
                    if (this.window is System.Windows.Window window)
                        window.Close();

                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_WindowLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowLoaded( object obj )
        {
            try
            {
                window = obj;
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_WindowClosing( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowClosing( object obj )
        {
            try
            {
                Properties.Settings.Default.SelectWindowHeight = WindowHeight;
                Properties.Settings.Default.SelectWindowWidth = WindowWidth;
                Properties.Settings.Default.SelectWindowLeft = WindowLeft;
                Properties.Settings.Default.SelectWindowTop = WindowTop;
                Properties.Settings.Default.SelectWindowState = WindowState.ToString();
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Views.MyMessages myMessages = new MyMessages();
            var viewmodel = (ViewModels.MyMessagesViewModel)myMessages.DataContext;
            var mytype = GetType().ToString().Split('.').LastOrDefault();
            viewmodel.SetTitle.Execute($"{mainWindowViewModel.Language.MyMessagesHeaders[0]}! ({mytype}.{name})");
            viewmodel.SetMessage.Execute(e.Message);
            myMessages.Show();
        }
    }
}
