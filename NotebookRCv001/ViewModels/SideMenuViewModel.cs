using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotebookRCv001.ViewModels
{
    class SideMenuViewModel:ViewModelBase
    {
        private SideMenuModel sideMenuModel;

        public ObservableCollection<MenuItem> MenuItems => sideMenuModel.MenuItems;


        public ObservableCollection<string> Headers => sideMenuModel.Headers;

        public ObservableCollection<string> ToolTips => sideMenuModel.ToolTips;

        public Action<object> BehaviorReady => sideMenuModel.BehaviorReady;

        public string PathToLastFile => sideMenuModel.PathToLastFile;

        public string LastFileName => sideMenuModel.LastFileName;



        public SideMenuViewModel()
        {
            sideMenuModel = new SideMenuModel();
            sideMenuModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }




        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(sideMenuModel.Execute_PageLoaded, sideMenuModel.CanExecute_PageLoaded);
        RelayCommand pageLoaded;

        public ICommand PageClose => throw new NotImplementedException();

        public ICommand PageClear => throw new NotImplementedException();

    }
}
