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
    public class BehaviorPasswordBox : Behavior<PasswordBox>
    {

        internal MainWindowViewModel MainWindowViewModel => mainWindowViewModel ??= (MainWindowViewModel)Application.Current.MainWindow.DataContext;
        MainWindowViewModel mainWindowViewModel;


        public string Password { get => AssociatedObject.Password; }

        public int Length { get => AssociatedObject.Password.Length; }

        public bool IsEnabled
        {
            get => AssociatedObject.IsEnabled;
            set=>AssociatedObject.IsEnabled = value;
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


        public void SetForeground( System.Drawing.Color color )
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
        public void SetBackground( System.Drawing.Color color )
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


        public void SetCaretBrush( System.Drawing.Color color )
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
            set=>AssociatedObject.FontSize = value;
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





        public void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new Thread(() => MessageBox.Show(e.Message, $"BehaviorPasswordBox.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }




    }
}
