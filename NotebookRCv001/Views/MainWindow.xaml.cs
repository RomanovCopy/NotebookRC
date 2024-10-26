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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotebookRCv001.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMenuOpen = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            if(isMenuOpen)
            {
                // Задвигаем меню
                CloseMenu();
            } else
            {
                // Выдвигаем меню и скрываем кнопку Menu
                OpenMenu();
            }
        }

        private void OpenMenu()
        {
            Storyboard slideIn = (Storyboard)FindResource("SlideInMenu");
            slideIn.Begin();
            MenuButton.Visibility = Visibility.Collapsed; // Скрываем кнопку Menu
            ContentPanel.Visibility = Visibility.Visible; // Показываем прозрачную область для закрытия меню
            isMenuOpen = true;
        }

        private void CloseMenu()
        {
            Storyboard slideOut = (Storyboard)FindResource("SlideOutMenu");
            slideOut.Begin();
            MenuButton.Visibility = Visibility.Visible; // Показываем кнопку Menu
            ContentPanel.Visibility = Visibility.Collapsed; // Скрываем прозрачную область
            isMenuOpen = false;
        }

        private void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Закрываем меню при клике на прозрачный Overlay
            CloseMenu();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Закрываем меню при клике за пределами окна
            if(isMenuOpen)
            {
                CloseMenu();
            }
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dashboard selected");
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Reports selected");
        }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Preferences selected");
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Account selected");
        }

        private void Version_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Version selected");
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help selected");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
