using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace NotebookRCv001.Models.HomeSideMenu
{

    class SideMenuModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        internal ObservableCollection<MenuItem> MenuItems { get; set; }


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
            throw new NotImplementedException();
        }
        internal void Execute_PageLoaded(object obj)
        {
            throw new NotImplementedException();
        }



        private ObservableCollection<MenuItem> Create_MenuItems()
        {
            var menu = new ObservableCollection<MenuItem>()
            {
                new MenuItem()
                {
                    Name=" File",
                    Icon="\xE7C3",
                    SubItems=new ObservableCollection<MenuItem>()
                    {
                        new MenuItem()
                        {
                            Name=" Open",
                            Icon="\xE8E5"
                        },
                        new MenuItem()
                        {
                            Name=" Save",
                            Icon="\xE74E"
                        }
                    }
                }
            };



            return menu;
        }

    }
}
