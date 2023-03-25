using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using System.Windows.Media;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using NotebookRCv001.ViewModels;
using System.ComponentModel;

namespace NotebookRCv001.Helpers
{
    public class BehaviorTextBox : Behavior<TextBox>
    {

        internal MainWindowViewModel MainWindowViewModel => mainWindowViewModel ??= (MainWindowViewModel)Application.Current.MainWindow.
            FindResource("rsmainwindowviewmodel");
        MainWindowViewModel mainWindowViewModel;


        public int CaretIndex
        {
            get => AssociatedObject.CaretIndex;
            set=>AssociatedObject.CaretIndex = value;
        }
        public int SelectionStart
        {
            get => AssociatedObject.SelectionStart;
            set=>AssociatedObject.SelectionStart = value;
        }
        public int SelectionLength
        {
            get => AssociatedObject.SelectionLength;
            set=>AssociatedObject.SelectionLength = value;
        }
        public string SelectedText
        {
            get => AssociatedObject.SelectedText;
            set=>AssociatedObject.SelectedText = value;
        }

        public bool IsEnabled
        {
            get => AssociatedObject.IsEnabled;
            set=>AssociatedObject.IsEnabled = value;
        }

        public string Text
        {
            get => AssociatedObject.Text;
            set=> AssociatedObject.Text = value;
        }


        public void SetFocus()
        {
            try
            {
                AssociatedObject.Focus();
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }

        public event KeyEventHandler PreviewKeyDown
        {
            add
            {
                AssociatedObject.PreviewKeyDown += value;
            }
            remove
            {
                AssociatedObject.PreviewKeyDown -= value;
            }
        }

        public event RoutedEventHandler SelectionChanged
        {
            add
            {
                AssociatedObject.SelectionChanged += value;
            }
            remove
            {
                AssociatedObject.SelectionChanged -= value;
            }
        }

        public event ContextMenuEventHandler ContextMenuOpening
        {
            add
            {
                AssociatedObject.ContextMenuOpening += value;
            }
            remove
            {
                AssociatedObject.ContextMenuOpening -= value;
            }
        }

        public event RoutedEventHandler GotFocus
        {
            add
            {
                AssociatedObject.GotFocus += value;
            }
            remove
            {
                AssociatedObject.GotFocus -= value;
            }
        }


        public void SetForeground(System.Drawing.Color color)
        {
            var solid = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            AssociatedObject.Foreground = solid;
        }
        public System.Drawing.Color GetForeground()
        {
            var brush = AssociatedObject.Foreground;
            var color = ((SolidColorBrush)brush).Color;
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }


        public void SetBackground(System.Drawing.Color color)
        {
            var solid = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            AssociatedObject.Background = solid;
        }
        public System.Drawing.Color GetBackground()
        {
            var brush = AssociatedObject.Background;
            var color = ((SolidColorBrush)brush).Color;
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }


        public void SetCaretBrush(System.Drawing.Color color)
        {
            var solid = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            AssociatedObject.CaretBrush = solid;
        }
        public System.Drawing.Color GetCaretBrush()
        {
            var brush = AssociatedObject.CaretBrush;
            var color = ((SolidColorBrush)brush).Color;
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public void SetSelect(int start, int length)
        {
            AssociatedObject.Select(start, length);
        }

        public FontFamily FontFamily
        {
            get => AssociatedObject.FontFamily;
            set=>AssociatedObject.FontFamily = value;
        }

        public FontWeight FontWeight
        {
            get => AssociatedObject.FontWeight;
            set=>AssociatedObject.FontWeight = value;
        }

        public FontStyle FontStyle
        {
            get => AssociatedObject.FontStyle;
            set=>AssociatedObject.FontStyle = value;
        }

        public double FontSize
        {
            get => AssociatedObject.FontSize;
            set=> AssociatedObject.FontSize = value;
        }


        public Cursor Cursor
        {
            get => AssociatedObject.Cursor;
            set=>AssociatedObject.Cursor = value;
        }



        protected override void OnAttached()
        {
            try
            {
                
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        protected override void OnDetaching()
        {
            try
            {

            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }



        public ICommand Copy => copy ??= new RelayCommand(Execute_Copy, CanExecute_Copy);
        RelayCommand copy;
        private bool CanExecute_Copy(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrEmpty(SelectedText);
                return c;
            }
            catch
            {
                return false;
            }
        }
        private void Execute_Copy(object obj)
        {
            try
            {
                Clipboard.SetText(SelectedText);
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }


        public ICommand CutOut => cutOut ??= new RelayCommand(Execute_CutOut, CanExecute_CutOut);
        RelayCommand cutOut;
        private bool CanExecute_CutOut(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrEmpty(SelectedText);
                return c;
            }
            catch
            {
                return false;
            }
        }
        private void Execute_CutOut(object obj)
        {
            try
            {
                Clipboard.SetText(SelectedText);
                Text = Text.Replace(SelectedText, "");
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }



        public ICommand Paste => paste ??= new RelayCommand(Execute_Paste, CanExecute_Paste);
        RelayCommand paste;
        private bool CanExecute_Paste(object obj)
        {
            try
            {
                bool c = false;
                c = Clipboard.ContainsText() && !string.IsNullOrWhiteSpace(Clipboard.GetText());
                return c;
            }
            catch
            {
                return false;
            }
        }
        private void Execute_Paste(object obj)
        {
            try
            {
                string text = Clipboard.GetText().Trim().Replace('\n', ' ').Replace('\r', ' ');
                if (CaretIndex >= 0)
                    Text = Text.Insert(CaretIndex, text);
                else
                    Text = text;
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }


        public ICommand Delete => delete ??= new RelayCommand(Execute_Delete, CanExecute_Delete);
        RelayCommand delete;
        private bool CanExecute_Delete(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrEmpty(SelectedText);
                return c;
            }
            catch
            {
                return false;
            }
        }
        private void Execute_Delete(object obj)
        {
            try
            {
                Text = Text.Replace(SelectedText, "");
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }


        public ICommand Clear => clear ??= new RelayCommand(Execute_Clear, CanExecute_Clear);
        RelayCommand clear;
        private bool CanExecute_Clear(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrEmpty(Text);
                return c;
            }
            catch
            {
                return false;
            }
        }
        private void Execute_Clear(object obj)
        {
            try
            {
                Text = "";
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }



        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            var mytype = GetType().ToString().Split( '.' ).LastOrDefault();
            System.Windows.Application.Current.Dispatcher.Invoke( (Action)(() =>
            { System.Windows.MessageBox.Show( e.Message, $"{mytype}.{name}" ); }) );
        }

    }
}
