using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.Interfaces
{
    internal interface ILastFileName:INotifyPropertyChanged
    {
        public string PathToLastFile { get; set; }
        public string LastFileName { get; }

    }
}
