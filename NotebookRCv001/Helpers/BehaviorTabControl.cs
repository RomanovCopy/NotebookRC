using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Helpers
{
    public class BehaviorTabControl : Behavior<TabControl>
    {

        MainWindowViewModel MainWindowViewModel => mainWindowViewModel;
        readonly MainWindowViewModel mainWindowViewModel;

        public ItemCollection Items => AssociatedObject.Items;

        public int SelectedIndex
        {
            get => AssociatedObject.SelectedIndex;
            set => AssociatedObject.SelectedIndex = value;
        }

        public object SelectedContent => AssociatedObject.SelectedContent;

        public object SelectedItem
        {
            get => AssociatedObject.SelectedItem;
            set => AssociatedObject.SelectedItem = value;
        }

        public object SelectedValue => AssociatedObject.SelectedValue;

        public object SelectedValuePath => AssociatedObject.SelectedValuePath;


        public BehaviorTabControl()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
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



        public event EventHandler Initialized
        {
            add => AssociatedObject.Initialized += value;
            remove => AssociatedObject.Initialized -= value;
        }

        public event SelectionChangedEventHandler SelectionChanged
        {
            add => AssociatedObject.SelectionChanged += value;
            remove => AssociatedObject.SelectionChanged -= value;
        }


        public void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new Thread(() => MessageBox.Show(e.Message, $"BehaviorTabControl.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }



    }
}
