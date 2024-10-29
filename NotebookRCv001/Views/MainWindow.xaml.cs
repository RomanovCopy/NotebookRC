using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotebookRCv001.Views
{

    public class MenuItem 
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public ObservableCollection<MenuItem> SubItems { get; set; }
        public MenuItem() 
        { 
            SubItems = new ObservableCollection<MenuItem>(); 
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MenuItems = new ObservableCollection<MenuItem> 
            { 
                new MenuItem 
                { 
                    Name = "Home", 
                    Icon = "\uE80F", 
                    SubItems = new ObservableCollection<MenuItem> 
                    { 
                        new MenuItem { Name = "Dashboard", Icon = "\uE80F" }, 
                        new MenuItem { Name = "Reports", Icon = "\uE721" } 
                    } 
                }, 
                new MenuItem { 
                    Name = "Settings", 
                    Icon = "\uE713", 
                    SubItems = new ObservableCollection<MenuItem> 
                    { 
                        new MenuItem { Name = "Preferences", Icon = "\uE713" }, 
                        new MenuItem { Name = "Account", Icon = "\uE13D" } 
                    } 
                } 
            };
            DataContext = this;
        }

        private void SideMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            // Обработка выбора пункта меню
            var selectedItem = (MenuItem)SideMenu.SelectedItem; 
            MessageBox.Show($"Selected: {selectedItem.Name}"); 
        }

        private void SideMenu_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) 
        { 
            if(e.NewValue is MenuItem selectedItem) 
            { 
                MessageBox.Show($"Selected: {selectedItem.Name}"); 
            } 
        }
    }
}
