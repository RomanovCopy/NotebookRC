using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;

namespace NotebookRCv001.Models
{
    internal class FileExplorerModel:ViewModelBase
    {



        public ObservableCollection<string> Headers => throw new NotImplementedException();

        public ObservableCollection<string> ToolTips => throw new NotImplementedException();

        public Action<object> BehaviorReady { get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); }



        internal FileExplorerModel()
        {

        }

        internal bool CanExecute_PageLoaded(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_PageLoaded(object obj)
        {
            throw new NotImplementedException();
        }

        internal bool CanExecute_PageClose(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_PageClose(object obj)
        {
            throw new NotImplementedException();
        }

        internal bool CanExecute_PageClear(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_PageClear(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
