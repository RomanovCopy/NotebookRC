using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Helpers;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NotebookRCv001.Models
{
    public class InputWindowModel : ViewModelBase
    {
        object window;

        private readonly MainWindowViewModel mainWindowViewModel;

        internal Languages Language => mainWindowViewModel.Language;

        internal string KeyCrypt
        {
            get => keyCrypt;
            set => SetProperty(ref keyCrypt, value);
        }
        private string keyCrypt;

        internal ObservableCollection<string> Headers => Language.InputWindow;
        internal ObservableCollection<string> ToolTips => Language.ToolTipsInputWindow;

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

        internal BehaviorPasswordBox PasswordBoxOne { get => passwordBoxOne; set => passwordBoxOne = value; }
        private BehaviorPasswordBox passwordBoxOne;

        internal BehaviorPasswordBox PasswordBoxTwo { get => passwordBoxTwo; set => passwordBoxTwo = value; }
        private BehaviorPasswordBox passwordBoxTwo;





        public InputWindowModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            //восстанавливаем размеры и положение окна
            WindowHeight = Properties.Settings.Default.InputWindowHeight;
            WindowWidth = Properties.Settings.Default.InputWindowWidth;
            WindowLeft = Properties.Settings.Default.InputWindowLeft;
            WindowTop = Properties.Settings.Default.InputWindowTop;
        }





        internal bool CanExecute_PasswordOneLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PasswordOneLoaded( object obj )
        {
            try
            {
                if (obj is BehaviorPasswordBox passwordBox)
                {
                    PasswordBoxOne = passwordBox;
                    PasswordBoxOne.PreviewKeyDown += ( s, e ) =>
                    {
                        if (e.Key == Key.Enter && PasswordBoxOne.Length > 5)
                        {
                            PasswordBoxOne.IsEnabled = false;
                            PasswordBoxTwo.IsEnabled = true;
                            PasswordBoxTwo.SetFocus();
                            e.Handled = true;
                        }
                    };
                    passwordBox.SetFocus();
                }
            }
            catch (Exception e1) { ErrorWindow(e1); }
        }

        internal bool CanExecute_PasswordTwoLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PasswordTwoLoaded( object obj )
        {
            try
            {
                if (obj is BehaviorPasswordBox passwordBox)
                {
                    PasswordBoxTwo = passwordBox;
                    PasswordBoxTwo.IsEnabled = false;
                    PasswordBoxTwo.PreviewKeyDown += ( s, e ) =>
                    {
                        if (e.Key == Key.Enter && PasswordBoxTwo.Password == PasswordBoxOne.Password)
                        {
                            PasswordBoxTwo.IsEnabled = false;
                            KeyCrypt = PasswordBoxTwo.Password;
                            e.Handled = true;
                            if (window is Window win)
                                win.Close();
                        }
                    };
                }
            }
            catch (Exception e1) { ErrorWindow(e1); }
        }



        internal bool CanExecute_InputWindowLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_InputWindowLoaded( object obj )
        {
            try
            {
                window = obj;
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_InputWindowClosing( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_InputWindowClosing( object obj )
        {
            try
            {
                //сохранение размеров и положения окна
                Properties.Settings.Default.InputWindowWidth = WindowWidth;
                Properties.Settings.Default.InputWindowHeight = WindowHeight;
                Properties.Settings.Default.InputWindowLeft = WindowLeft;
                Properties.Settings.Default.InputWindowTop = WindowTop;
                Properties.Settings.Default.Save();
            }
            catch (Exception e) { ErrorWindow(e); }
        }



        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"InputWindowModel.{name}"));
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }
    }
}
