using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using controls = System.Windows.Controls;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Models
{
    internal class PopUpTextBoxModel : ViewModelBase
    {

        private readonly MainWindowViewModel mainWindowViewModel;
        private object window { get; set; }
        private Languages language => mainWindowViewModel.Language;
        /// <summary>
        /// допустиаые символы при вводе в TextBox
        /// </summary>
        private readonly Key[] validInputCharacters;
        internal string Text { get => text; set => SetProperty(ref text, value); }
        private string text;
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

        internal ObservableCollection<string> Headers => language.HeadersPopUpTextBox;
        internal ObservableCollection<string> ToolTips => language.ToolTipsPopUpTextBox;

        internal string Title { get => title; set => SetProperty(ref title, value); }
        private string title;
        internal string Header { get => header; set => SetProperty(ref header, value); }
        private string header;
        internal string ToolTip { get => toolTip; set => SetProperty(ref toolTip, value); }
        private string toolTip;
        internal PopUpTextBoxModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            if (Properties.Settings.Default.PopUpTextBoxFirstRun)
            {
                Properties.Settings.Default.PopUpTextBoxWidth = 30;
                Properties.Settings.Default.PopUpTextBoxHeight = 20;
                Properties.Settings.Default.PopUpTextBoxLeft = 50;
                Properties.Settings.Default.PopUpTextBoxTop = 50;
                Properties.Settings.Default.PopUpTextBoxFirstRun = false;
            }
            WindowWidth = Properties.Settings.Default.PopUpTextBoxWidth;
            WindowHeight = Properties.Settings.Default.PopUpTextBoxHeight;
            WindowLeft = Properties.Settings.Default.PopUpTextBoxLeft;
            WindowTop = Properties.Settings.Default.PopUpTextBoxTop;
            validInputCharacters = new[] { Key.Back, Key.LeftShift, Key.RightShift, Key.LeftCtrl, Key.RightCtrl };
        }





        internal bool CanExecute_TextBoxLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = window != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_TextBoxLoaded(object obj)
        {
            try
            {
                if (obj is controls.TextBox textbox)
                {
                    textbox.Focus();
                    textbox.CaretIndex = textbox.Text.Length;
                    textbox.PreviewKeyDown += (s, e) =>
                    {
                        if (e.Key == Key.Enter)
                        {
                            if (window is Window win)
                                win.Close();
                        }
                        bool c = validInputCharacters.Any((x) => e.Key == x);
                        if (char.IsControl((char)e.Key) && !c)
                        if (char.IsControl((char)e.Key) && e.Key != System.Windows.Input.Key.Back)
                            e.Handled = true;
                    };
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        internal bool CanExecute_WindowLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowLoaded(object obj)
        {
            try
            {
                window = obj;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_WindowClosing(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowClosing(object obj)
        {
            try
            {
                Properties.Settings.Default.PopUpTextBoxWidth = WindowWidth;
                Properties.Settings.Default.PopUpTextBoxHeight = WindowHeight;
                Properties.Settings.Default.PopUpTextBoxLeft = WindowLeft;
                Properties.Settings.Default.PopUpTextBoxTop = WindowTop;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"PopUpTextBoxModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
