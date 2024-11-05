using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.Models.HomeSideMenu
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ToolTipe { get; set; }
        public ObservableCollection<MenuItem> SubItems { get; set; }
        public MenuItem()
        {
            SubItems = new ObservableCollection<MenuItem>();
        }
    }
}
