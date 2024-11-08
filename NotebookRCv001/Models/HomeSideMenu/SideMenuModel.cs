using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using NotebookRCv001.ViewModels.HomeSideMenu;
using NotebookRCv001.MyControls.HomeSideMenu;


namespace NotebookRCv001.Models.HomeSideMenu
{

    class SideMenuModel: ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        internal ObservableCollection<MenuItemViewModel> MenuItems { get; set; }


        internal Dictionary<string, BitmapSource> Icons { get => icons; private set => icons = value; }
        Dictionary<string, BitmapSource> icons;


        internal Languages Language { set => SetProperty(ref language, value); get => language; }
        Languages language;


        internal ObservableCollection<string> Headers { get; set; }

        internal ObservableCollection<string> ToolTips { get; set; }

        internal Action<object> BehaviorReady { get; set; }

        internal string PathToLastFile { set => SetProperty(ref pathToLastFile, value); get => pathToLastFile; }
        private string pathToLastFile;

        internal string LastFileName { get; set; }


        internal SideMenuModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language = mainWindowViewModel.Language;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            MenuItems = Create_MenuItems();
        }





        internal bool CanExecute_PageLoaded(object obj)
        {
            return true;
        }
        internal void Execute_PageLoaded(object obj)
        {
            if(obj is string item && item == " Open")
            {
                var sidemenu = Application.Current.MainWindow.FindResource("sideMenu");
                if(sidemenu != null && sidemenu is SideMenu menu)
                {
                    var view = menu.FindResource("menufileviewmodel");
                    if(view != null && view is HomeMenuFileViewModel viewmodel)
                    {
                        viewmodel.OpenFile.Execute(null);
                    }
                }
            }
        }



        private ObservableCollection<MenuItemViewModel> Create_MenuItems()
        {
            var menu = new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel()
                {
                    Name=" File",
                    Icon="file",
                    Size=2.0,
                    IconVisible=true,
                    SubItems=new ObservableCollection<MenuItemViewModel>()
                    {
                        new MenuItemViewModel()
                        {
                            Name=" Open",
                            Icon=null,
                            IconVisible=false,
                            Size=2.0
                        },
                        new MenuItemViewModel()
                        {
                            Name=" Save",
                            Icon=null,
                            IconVisible=false,
                            Size=2.0
                        }
                    }
                }
            };
            return menu;
        }

    }


}
