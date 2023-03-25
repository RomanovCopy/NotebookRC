using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace NotebookRCv001.Helpers
{
    public class BehaviorTreeView : Behavior<TreeView>
    {

        public TreeView TreeView => AssociatedObject;

        public TreeViewItem SelectedItem => (TreeViewItem)AssociatedObject.SelectedItem;

        public BehaviorTreeView()
        {

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
            catch (Exception e) { ErrorWindow(e); }
        }





        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            var mytype = GetType().ToString().Split( '.' ).LastOrDefault();
            System.Windows.Application.Current.Dispatcher.Invoke( (Action)(() =>
            { System.Windows.MessageBox.Show( e.Message, $"{mytype}.{name}" ); }) );
        }



    }
}
