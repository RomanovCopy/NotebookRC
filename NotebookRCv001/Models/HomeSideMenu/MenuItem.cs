using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels.HomeSideMenu;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.Models.HomeSideMenu
{
    internal class MenuItem:ViewModelBase
    {
        internal string Name { get=>name; set=>SetProperty(ref name, value); }
        string name;
        internal string Icon { get => icon; set => SetProperty(ref icon, value); }
        string icon;
        internal string ToolTipe { get=>toolTipe; set=>SetProperty(ref toolTipe, value); }
        string toolTipe;
        internal double Size { get => size; set => SetProperty(ref size, value); }
        double size;
        internal bool IconVisible { get=>iconVisible; set=>SetProperty(ref iconVisible, value); }
        bool iconVisible;
        internal ObservableCollection<MenuItemViewModel> SubItems { get => subItems; set => SetProperty(ref subItems, value); }
        ObservableCollection<MenuItemViewModel> subItems;

        internal MenuItem()
        {
        }
    }
}
