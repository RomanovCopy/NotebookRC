using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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



        internal ObservableCollection<string> Headers => throw new NotImplementedException();

        internal ObservableCollection<string> ToolTips => throw new NotImplementedException();

        internal Action<object> BehaviorReady { get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); }

        /// <summary>
        /// коллекция доступных дисков
        /// </summary>
        internal ObservableCollection<DriveInfo> DriveInfos { get => driveInfos; set => SetProperty(ref driveInfos, value); }
        private ObservableCollection<DriveInfo> driveInfos;
        /// <summary>
        /// индекс выбранного элемента коллекции доступных дисков
        /// </summary>
        internal int SelectedIndexDrives { get => selectedIndexDrives; set => SetProperty(ref selectedIndexDrives, value); }
        private int selectedIndexDrives;

        internal FileExplorerModel()
        {

        }




        /// <summary>
        /// переход к родительскому элементу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickToParentDirectory(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_ClickToParentDirectory(object obj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// выбор диска
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxDrivesSelectionChanged(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_ComboBoxDrivesSelectionChanged(object obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// окончание загрузки коллекции доступных дисков(ComboBox)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxDrivesLoaded(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_ComboBoxDrivesLoaded(object obj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// окончание загрузки страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageLoaded(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_PageLoaded(object obj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// закрытие страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClose(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_PageClose(object obj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// очистка страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
