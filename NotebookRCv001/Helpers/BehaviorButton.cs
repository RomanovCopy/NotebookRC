using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace NotebookRCv001.Helpers
{
    public class BehaviorButton : Behavior<Button>, IDisposable
    {

        public string Name
        {
            get => AssociatedObject.Name;
            set => AssociatedObject.Name = value;
        }

        public ICommand Command
        {
            get => AssociatedObject.Command;
            set => AssociatedObject.Command = value;
        }

        public object Content
        {
            get => AssociatedObject.Content;
            set => AssociatedObject.Content = value;
        }

        public Visibility Visibility
        {
            get => AssociatedObject.Visibility;
            set => AssociatedObject.Visibility = value;
        }





        public bool IsDispose { get; private set; }
        public void Dispose()
        {
            if (!IsDispose)
            {

                IsDispose = true;
            }
        }
    }
}
