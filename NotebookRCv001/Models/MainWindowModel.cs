using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using System.Runtime.CompilerServices;
using System.Threading;
using System.IO;
using NotebookRCv001.Styles.CustomizedWindow;
using NotebookRCv001.ViewModels;
using System.Reflection;
using System.Collections;
using System.Net.Http;
using System.Resources;
using System.Globalization;

namespace NotebookRCv001.Models
{
    public class MainWindowModel : ViewModelBase
    {
        #region ________________________Положение и размеры главного окна___________________
        internal object WindowState
        {
            get => windowState;
            set => SetProperty(ref windowState, value);
        }
        object windowState;
        internal double WindowHeight
        {
            get => windowHeight;
            set => SetProperty(ref windowHeight, value);
        }
        double windowHeight;
        internal double WindowWidth
        {
            get => windowWidth;
            set => SetProperty(ref windowWidth, value);
        }
        double windowWidth;
        internal double WindowLeft
        {
            get => windowLeft;
            set => SetProperty(ref windowLeft, value);
        }
        double windowLeft;
        internal double WindowTop
        {
            get => windowTop;
            set => SetProperty(ref windowTop, value);
        }
        double windowTop;

        #endregion
        /// <summary>
        /// локализация
        /// </summary>
        internal Languages Language { set => SetProperty(ref language, value); get => language; }
        Languages language;

        /// <summary>
        /// текущая кодировка
        /// </summary>
        internal Encoding HomeEncoding
        {
            get => homeEncoding;
            set => SetProperty(ref homeEncoding, value);
        }
        private Encoding homeEncoding;

        //internal readonly HttpClient client;


        /// <summary>
        /// коллекция доступных кодировок
        /// </summary>
        internal ObservableCollection<EncodingInfo> HomeEncodings
        {
            get => homeEncodings ??= new ObservableCollection<EncodingInfo>();
            set => SetProperty(ref homeEncodings, value);
        }
        private ObservableCollection<EncodingInfo> homeEncodings;

        ViewModels.HomeMenuFileViewModel HomeMenuFileViewModel
        {
            get => homeMenuFileViewModel;
            set => homeMenuFileViewModel = value;
        }
        ViewModels.HomeMenuFileViewModel homeMenuFileViewModel;


        /// <summary>
        /// коллекция доступных для отображения страниц
        /// </summary>
        internal ObservableCollection<Page> FrameList
        {
            get => frameList ?? new ObservableCollection<Page>();
            set => frameList = value;
        }
        ObservableCollection<Page> frameList;

        /// <summary>
        /// отображаемая страница
        /// </summary>
        internal Page CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }
        private Page currentPage;

        public MainWindowModel()
        {
            FrameList = new ObservableCollection<Page>();
            //устанавливаем локализацию
            Language = new Languages();
            Language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            //восстанавливаем размеры и положение окна
            if (Properties.Settings.Default.FileOverviewFirstStart)
            {
                WindowHeight = 40;
                WindowWidth = 40;
                WindowLeft = 40;
                WindowTop = 40;
                Properties.Settings.Default.FileOverviewFirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.WindowHeight;
                WindowWidth = Properties.Settings.Default.WindowWidth;
                WindowLeft = Properties.Settings.Default.WindowLeft;
                WindowTop = Properties.Settings.Default.WindowTop;
            }
            //восстанавливаем состояние окна
            WindowState = Properties.Settings.Default.WindowState;
        }


        /// <summary>
        /// перемещение на следующую страницу
        /// </summary>
        /// <param name="obj"></param>
        internal bool CanExecute_Frmelist_GoForward( object obj )
        {
            try
            {
                bool c = false;
                c = FrameList.IndexOf((Page)CurrentPage) < FrameList.Count - 1;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_Frmelist_GoForward( object obj )
        {
            try
            {
                CurrentPage = FrameList[FrameList.IndexOf((Page)CurrentPage) + 1];
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// перемещение на предыдущую страницу
        /// </summary>
        /// <param name="obj"></param>
        internal bool CanExecute_Frmelist_GoBack( object obj )
        {
            try
            {
                bool c = false;
                c = FrameList.IndexOf((Page)CurrentPage) > 0;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_Frmelist_GoBack( object obj )
        {
            try
            {
                CurrentPage = FrameList[FrameList.IndexOf((Page)CurrentPage) - 1];
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// добавление страницы во фрейм
        /// </summary>
        /// <param name="page">добавляемая страница</param>
        /// <param name="pozition">позиция для вставки</param>
        /// <returns>добавленная страница</returns>
        internal bool CanExecute_FrameListAddPage( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_FrameListAddPage( object obj )
        {
            try
            {
                if (obj is Page p )
                {
                    CurrentPage = p;
                    FrameList.Add((Page)CurrentPage);
                }
            }
            catch 
            {
            }
        }

        /// <summary>
        /// удаление страницы из фрейма
        /// </summary>
        /// <param name="page">удаляемая страница</param>
        internal bool CanExecute_FrameListRemovePage( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_FrameListRemovePage( object obj )
        {
            try
            {
                if (obj is Page page)
                {
                    if (FrameList.Count > 1)
                    {
                        int i = FrameList.IndexOf(page);//индекс удаляемой страницы
                        if (CurrentPage.Equals(page))
                        {//текущая страница подлежит удалению
                            if (i == 0)
                            {//первый в коллекции
                                i = FrameList.Count > 1 ? i + 1 : i;//индекс новой текущей страницы
                            }
                            else
                            {//последний или в середине коллекции
                                i = FrameList.Count > 1 ? i - 1 : i;//индекс новой текущей страницы
                            }
                            CurrentPage = FrameList[i];
                        }
                        FrameList.Remove(page);
                    }
                    else
                    {
                        FrameList.Remove(page);
                    }
                }
                Application.Current.MainWindow.Focus();
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }


        /// <summary>
        /// закрытие выбранной страницы страницы
        /// </summary>
        /// <param name="obj">закрываемая страница</param>
        /// <returns></returns>
        internal bool CanExecute_PageClosed( object obj )
        {
            try
            {
                bool c = false;
                c = obj is Page;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClosed( object obj )
        {
            try
            {
                if (FrameList.Contains(obj))
                {
                    Execute_FrameListRemovePage(obj);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// окончание загрузки главного окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_MainWindowLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_MainWindowLoaded( object obj )
        {
            try
            {
                foreach (EncodingInfo info in Encoding.GetEncodings())
                    HomeEncodings.Add(info);
                HomeEncoding = Encoding.GetEncoding(Properties.Settings.Default.EncodingCodePage);
                Execute_FrameListAddPage(new Views.Home() { KeepAlive = true });
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// перед закрытием главного окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_MainWindowClosing( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_MainWindowClosing( object obj )
        {
            try
            {
                //размеры и положение окна
                if (WindowState.ToString() == "Normal")
                {
                    Properties.Settings.Default.WindowWidth = WindowWidth;
                    Properties.Settings.Default.WindowHeight = WindowHeight;
                    Properties.Settings.Default.WindowLeft = WindowLeft;
                    Properties.Settings.Default.WindowTop = WindowTop;
                }
                Properties.Settings.Default.LanguageKey = Language.Key;
                Properties.Settings.Default.WindowState = WindowState.ToString();
                int count = FrameList.Count;
                while (count > 0)
                {
                    var page = FrameList[--count];
                    if (page.DataContext is IPageViewModel viewmodel )
                        viewmodel.PageClose.Execute(page);
                }
                Properties.Settings.Default.Save();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_MainWindowClosed( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }catch(Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_MainWindowClosed( object obj )
        {
            try
            {
                DeletingTemporaryFiles();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// очистка временных файлов
        /// </summary>
        /// <returns></returns>
        private bool DeletingTemporaryFiles()
        {
            try
            {
                string path = $"{Environment.CurrentDirectory}/temp";
                if (Directory.Exists( path ))
                {
                    while (Directory.GetFiles( path ).Length > 0)
                    {
                        File.Delete( Directory.GetFiles( path ).FirstOrDefault() );
                    }
                }
                return Directory.GetFiles( path ).Length == 0;
            }
            catch { return false; }
        }

        /// <summary>
        /// Открытие окна выбора.
        /// Для скрытия кнопки вместо текста необходимо передать null
        /// </summary>
        /// <param name="left">текст на левой кнопке</param>
        /// <param name="center">текст на центральной кнопке</param>
        /// <param name="right">текст на правой кнопке</param>
        /// <returns>
        /// "leftbutton" - выбрана левая кнопка
        /// "centerbutton" - выбрана центральная кнопка
        /// "rightbutton" - выбрана правая кнопка
        /// </returns>
        internal string NewSelectWindow(string title, string message,  string left, string center, string right )
        {
            try
            {
                string result = null;
                var window = new Views.SelectWindow();
                var viewmodel = (ViewModels.SelectWindowViewModel)window.DataContext;
                viewmodel.Title = title;
                viewmodel.Message = message;
                viewmodel.LeftButtonContent = left;
                viewmodel.CenterButtonContent = center;
                viewmodel.RightButtonContent = right;
                window.Closing += ( s, e ) =>
                {
                    result = viewmodel.Result;
                };
                window.ShowDialog();
                return result;
            }
            catch(Exception e) { ErrorWindow(e); return null; }
        }

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"MainWindowModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
