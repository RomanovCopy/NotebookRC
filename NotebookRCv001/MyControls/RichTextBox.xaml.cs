using NotebookRCv001.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotebookRCv001.MyControls
{
    /// <summary>
    /// Логика взаимодействия для RichTextBox.xaml
    /// </summary>
    public partial class RichTextBox : UserControl
    {
        public RichTextBox()
        {
            InitializeComponent();
        }

        //private void TextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    if(sender is System.Windows.Controls.RichTextBox textbox)
        //    {
        //        var datacontext = (RichTextBoxViewModel)textbox.DataContext;
        //        var contextmenu = (System.Windows.Controls.ContextMenu)textbox.ContextMenu;
        //        var viewmodel = (MyContextMenuViewModel)textbox.ContextMenu.DataContext;

        //    }
        //}
    }
}
