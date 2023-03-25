using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NotebookRCv001.Models
{
    internal class MyMessagesModel : ViewModelBase
    {
        private readonly ViewModels.MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;
        internal string Title { get => title; set => SetProperty(ref title, value); }
        string title;
        internal string Message { get => message; set => SetProperty(ref message, value); }
        string message;
        internal string ButtonText { get => buttonText; set => SetProperty(ref buttonText, value); }
        string buttonText;
        internal double WindowWidth { get => windowWidth; set => SetProperty(ref windowWidth, value); }
        double windowWidth;
        internal double WindowHeight { get => windowHight; set => SetProperty(ref windowHight, value); }
        double windowHight;
        internal double WindowTop { get => windowTop; set => SetProperty(ref windowTop, value); }
        double windowTop;
        internal double WindowLeft { get => windowLeft; set => SetProperty(ref windowLeft, value); }
        double windowLeft;
        internal object WindowState { get => windowState; set => SetProperty(ref windowState, value); }
        object windowState;
        internal bool Rezult { get => rezult; private set => SetProperty(ref rezult, value); }
        bool rezult;
        internal ObservableCollection<string> Headers { get => language.MyMessagesHeaders; }
        internal ObservableCollection<string> ToolTips { get => language.ToolTipsMyMessages; }

        internal MyMessagesModel()
        {
            mainWindowViewModel = (ViewModels.MainWindowViewModel)Application.Current.MainWindow.DataContext;
            mainWindowViewModel.Language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            //восстанавливаем размеры и положение окна
            if (Properties.Settings.Default.MyMessagesFirstStart)
            {
                WindowHeight = 13;
                WindowWidth = 36;
                WindowLeft = 25;
                WindowTop = 35;
                Properties.Settings.Default.MyMessagesFirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.MyMessagesWindowHeight;
                WindowWidth = Properties.Settings.Default.MyMessagesWindowWidth;
                WindowLeft = Properties.Settings.Default.MyMessagesWindowLeft;
                WindowTop = Properties.Settings.Default.MyMessagesWindowTop;
            }
            Rezult = false;
            //восстанавливаем состояние окна
            WindowState = Properties.Settings.Default.MyMessagesWindowState;
            Properties.Settings.Default.Save();
        }


        internal bool CanExecute_SetTitle( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SetTitle( object obj )
        {
            try
            {
                if (obj is string title)
                    Title = title;
                else
                    Title = language.MyMessagesHeaders[0];
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_SetButtonText(object obj)
        {
            try
            {
                bool c = false;
                c = obj is string;
                return c;

            }catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SetButtonText(object obj)
        {
            try
            {
                ButtonText = obj is string text ? text : " Text not loaded... ";
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_SetMessage( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SetMessage( object obj )
        {
            try
            {
                if (obj is string message)
                    Message = message;
                else
                    Message = language.MessagesMyMessages[0];
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_WindowLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Message));
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowLoaded( object obj )
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_ClickOk(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickOk(object obj)
        {
            try
            {
                if(obj is Views.MyMessages win)
                {
                    Rezult = true;
                    win.Close();
                }
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
                Properties.Settings.Default.MyMessagesWindowHeight = WindowHeight;
                Properties.Settings.Default.MyMessagesWindowWidth = WindowWidth;
                Properties.Settings.Default.MyMessagesWindowLeft = WindowLeft;
                Properties.Settings.Default.MyMessagesWindowTop = WindowTop;
                Properties.Settings.Default.MyMessagesWindowState = WindowState.ToString();
                Properties.Settings.Default.Save();
            }
            catch (Exception e) { ErrorWindow(e); }
        }



    }
}
