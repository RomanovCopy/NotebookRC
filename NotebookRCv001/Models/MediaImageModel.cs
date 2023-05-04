using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using System.Windows.Media.Imaging;

namespace NotebookRCv001.Models
{
    internal class MediaImageModel:ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;

        internal ObservableCollection<string> Headers => language.HeadersMediaImage;

        internal ObservableCollection<string> ToolTips => language.ToolTipsMediaImage;

        internal BitmapImage Bitmap { get => bitmap; set => SetProperty(ref bitmap, value); }
        private BitmapImage bitmap;

        internal Action<object> BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        private Action<object> behaviorReady;


        internal MediaImageModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
        }



        /// <summary>
        /// очистка страницы
        /// </summary>
        /// <param name="obj">очищаемая страница</param>
        /// <returns></returns>
        internal bool CanExecute_PageClear(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear(object obj)
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// окончание загрузки страницы
        /// </summary>
        /// <param name="obj">загружаемая страница</param>
        /// <returns></returns>
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
        /// <param name="obj">закрываемая страница</param>
        /// <returns></returns>
        internal bool CanExecute_PageClose(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow(e);}
        }

    }
}
