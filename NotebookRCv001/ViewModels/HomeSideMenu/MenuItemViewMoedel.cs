using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models.HomeSideMenu;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.ViewModels.HomeSideMenu
{
    public class MenuItemViewModel: ViewModelBase
    {
        private readonly MenuItem menuItem;

        public string Name { get => menuItem.Name; set => menuItem.Name = value; }
        public string Icon { get => menuItem.Icon; set => menuItem.Icon = value; }
        public string ToolTipe { get => menuItem.ToolTipe; set => menuItem.ToolTipe = value; }
        public double Size { get => menuItem.Size; set => menuItem.Size = value; }
        public bool IconVisible { get => menuItem.IconVisible; set => menuItem.IconVisible = value; }
        public ObservableCollection<MenuItemViewModel> SubItems { get => menuItem.SubItems; set => menuItem.SubItems = value; }


        public MenuItemViewModel()
        {
            menuItem = new();
            menuItem.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }
    }
}
