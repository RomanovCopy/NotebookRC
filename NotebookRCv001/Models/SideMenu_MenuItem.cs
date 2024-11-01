using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.Models
{
    public class SideMenu_MenuItem
    {

        internal string Name { get; set; }
        internal string Icon { get; set; }
        internal ObservableCollection<MenuItem> SubItems { get; set; }
    }
}
