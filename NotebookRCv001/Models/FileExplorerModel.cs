using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Models
{
    internal class FileExplorerModel:ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;


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
        /// коллекция элементов из отображаемой директории
        /// </summary>
        internal ObservableCollection<DirectoryItem> CurrentDirectoryList { get => currentDirectoryList;
            set => SetProperty(ref currentDirectoryList, value); }
        private ObservableCollection<DirectoryItem> currentDirectoryList;
        /// <summary>
        /// коллекция размеров обложек
        /// </summary>
        internal ObservableCollection<int> CoverSizes { get => coverSizes; set => SetProperty(ref coverSizes, value); }
        private ObservableCollection<int> coverSizes;
        /// <summary>
        /// индекс выбранного элемента коллекции доступных дисков
        /// </summary>
        internal int SelectedIndexDrives { get => selectedIndexDrives; set => SetProperty(ref selectedIndexDrives, value); }
        private int selectedIndexDrives;
        /// <summary>
        /// индекс выбранного размера обложки в коллекции CoverSizes
        /// </summary>
        internal int CoverSizesIndex { get => coverSizesIndex; set => SetProperty(ref coverSizesIndex, value); }
        private int coverSizesIndex;
        /// <summary>
        /// полный путь к текущей дирректории
        /// </summary>
        internal string CurrentDirectoryFullName { get => currentDirectoryFullName; 
            set => SetProperty(ref currentDirectoryFullName, value); }
        private string currentDirectoryFullName;
        /// <summary>
        /// флаг: отображение обложек
        /// </summary>
        internal bool IsCoverEnabled { get => isCoverEnabled; set => SetProperty(ref isCoverEnabled, value); }
        private bool isCoverEnabled;
        /// <summary>
        /// флаг: отображение в виде плиток
        /// </summary>
        internal bool IsTilesEnabled { get => isTilesEnabled; set => SetProperty(ref isTilesEnabled, value); }
        private bool isTilesEnabled;



        internal FileExplorerModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            //IsTilesEnabled = true;
        }




        /// <summary>
        /// активация/дезактивация CheckedIsTilesEnabled
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CheckedIsTilesEnabled(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CheckedIsTilesEnabled(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// активация/дезактивация IsCoverEnabled
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CheckedIsCover(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CheckedIsCover(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// переход к родительскому элементу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickToParentDirectory(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickToParentDirectory(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// изменение выбора размера обложки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CoverSizesSelectionChanged(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CoverSizesSelectionChanged(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// выбор диска
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxDrivesSelectionChanged(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ComboBoxDrivesSelectionChanged(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// окончание загрузки коллекции размеров обложек (ComboBox)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CoverSizesLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CoverSizesLoaded(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// окончание загрузки коллекции доступных дисков(ComboBox)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxDrivesLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ComboBoxDrivesLoaded(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// окончание загрузки страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageLoaded(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// закрытие страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClose(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {
                if(obj is MyControls.FileExplorer exp && mainWindowViewModel.FrameListRemovePage.CanExecute(exp))
                {
                    mainWindowViewModel.FrameListRemovePage.Execute(exp);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// очистка страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClear(object obj)
        {
            try
            {
                bool c = false;
                //c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }

    }
}
