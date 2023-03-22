using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using Media = System.Windows.Media;
using Drawing = System.Drawing;
using controls = System.Windows.Controls;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Helpers;
using System.Runtime.CompilerServices;
using System.Threading;
using NotebookRCv001.Styles.CustomizedWindow;

namespace NotebookRCv001.Models
{

    public class HomeModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;
        private MenuHomeViewModel menuHomeViewModel;
        private RichTextBoxViewModel richTextBoxViewModel;

        /// <summary>
        /// статус шифрования
        /// </summary>
        internal string EncryptionStatus => menuHomeViewModel?.EncryptionStatus;
        /// <summary>
        /// текущая кодировка
        /// </summary>
        internal Encoding HomeEncoding => menuHomeViewModel?.HomeEncoding;
        /// <summary>
        /// коллекция доступных системных цветов
        /// </summary>
        internal ObservableCollection<Drawing.Color> HomeMyColors => menuHomeViewModel?.HomeMyColors;
        /// <summary>
        /// текущий цвет шрифта
        /// </summary>
        internal Media.Brush MyForeground { get => menuHomeViewModel?.MyForeground; set => menuHomeViewModel.MyForeground = value; }
        /// <summary>
        /// текущий цвет фона для шрифта
        /// </summary>
        internal Media.Brush MyFontBackground { get => menuHomeViewModel?.MyFontBackground; set => menuHomeViewModel.MyFontBackground = value; }
        /// <summary>
        /// текущий цвет фона для RichTextBox
        /// </summary>
        internal Media.Brush MyBackground
        {
            set => menuHomeViewModel.MyBackground = value;
            get => menuHomeViewModel?.MyBackground;
        }
        /// <summary>
        /// путь к рабочему каталогу
        /// </summary>
        internal string WorkingDirectory => menuHomeViewModel?.WorkingDirectory;
        /// <summary>
        /// имя рабочего каталога
        /// </summary>
        internal string WorkingDirectoryName => menuHomeViewModel?.WorkingDirectoryName;
        /// <summary>
        /// путь к последнему открытому или сохраненному файлу
        /// </summary>
        internal string PathToLastFile { get => menuHomeViewModel.PathToLastFile; set => menuHomeViewModel.PathToLastFile = value; }
        /// <summary>
        /// Имя последнего открытого или сохраненного файла
        /// </summary>
        internal string LastFileName { get => menuHomeViewModel.LastFileName; }

        /// <summary>
        /// делегат выполняемый после определения BehaviorFlowDocumentReader
        /// </summary>
        internal Action<object> BehaviorReady { get => behaviorReady; set => SetProperty(ref behaviorReady, value); }
        private Action<object> behaviorReady;

        public HomeModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
        }



        /// <summary>
        /// очистка страницы Home и строки пути к последненму открытому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_PageClear( object obj )
        {
            try
            {
                bool c = false;
                if (richTextBoxViewModel != null)
                    c = richTextBoxViewModel.PageClear.CanExecute(null);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear( object obj )
        {
            try
            {
                richTextBoxViewModel.PageClear.Execute(null);
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// окончание загрузки страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_HomeLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_HomeLoaded( object obj )
        {
            try
            {
                if (richTextBoxViewModel == null)
                {
                    if (obj is Views.Home home)
                    {
                        var menuhome = (MyControls.MenuHome)home.FindResource("menuhome");
                        richTextBoxViewModel ??= (RichTextBoxViewModel)((MyControls.RichTextBox)home.FindResource("richtextbox")).DataContext;
                        menuHomeViewModel ??= (MenuHomeViewModel)((MyControls.MenuHome)home.FindResource("menuhome")).DataContext;
                        menuHomeViewModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
                    }
                }
                if (BehaviorReady != null)
                {
                    BehaviorReady.Invoke(obj);
                    BehaviorReady = null;
                }
                OnPropertyChanged("LastFileName");
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// закрытие страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClose( object obj )
        {
            try
            {
                bool c = false;
                var list = mainWindowViewModel.FrameList.Where(( x ) => x is Views.Home);
                c = list.Count() > 1;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose( object obj )
        {
            try
            {
                if (obj is Views.Home home && richTextBoxViewModel!=null)
                {
                    Properties.Settings.Default.EncodingCodePage = HomeEncoding != null ? HomeEncoding.CodePage :
                        Properties.Settings.Default.EncodingCodePage;//сохранение текущей кодировки
                    Properties.Settings.Default.FontFamilyName = richTextBoxViewModel.BehaviorRichTextBox.FontFamily.Source;
                    Properties.Settings.Default.FontWeightName = richTextBoxViewModel.BehaviorRichTextBox.FontWeight ==
                        FontWeights.Bold ? "Bold" : "Normal";
                    Properties.Settings.Default.FontSize = richTextBoxViewModel.BehaviorRichTextBox.FontSize;
                    Properties.Settings.Default.FontStyleName = richTextBoxViewModel.BehaviorRichTextBox.FontStyle ==
                        FontStyles.Italic ? "Italic" : "Normal";
                    Properties.Settings.Default.TextDecoration = richTextBoxViewModel.BehaviorRichTextBox.TextDecoration ==
                        TextDecorations.Underline ? "Underline" : null;

                    var converter = new Converters.ColorToColorConverter();

                    Properties.Settings.Default.MyForeground = (System.Drawing.Color)converter.
                        Convert(MyForeground, typeof(System.Drawing.Color), null, null);

                    Properties.Settings.Default.MyBackground = (System.Drawing.Color)converter.
                        Convert(MyBackground, typeof(System.Drawing.Color), null, null);

                    Properties.Settings.Default.MyFontBackground = (System.Drawing.Color)converter.
                        Convert(MyFontBackground, typeof(System.Drawing.Color), null, null);

                    Properties.Settings.Default.HomeCurrentDirectoryOpen = menuHomeViewModel.CurrentDirectoryOpen;
                    Properties.Settings.Default.HomeCurrentDirectorySave = menuHomeViewModel.CurrentDirectorySave;
                    Properties.Settings.Default.WorkingDirectory =
                        !String.IsNullOrWhiteSpace(WorkingDirectory) ? WorkingDirectory : Properties.Settings.Default.WorkingDirectory;
                    Properties.Settings.Default.SetColorsForQuickAccess = new System.Collections.Specialized.StringCollection();
                    foreach(var color in richTextBoxViewModel.QuickSelectColorCollection)
                    {
                        var newcolor = richTextBoxViewModel.MyColors.
                            Where((x) => x.A == color.A && x.R == color.R && x.G == color.G && x.B == color.B).FirstOrDefault();
                        Properties.Settings.Default.SetColorsForQuickAccess.Add(newcolor.Name);
                    }
                    Properties.Settings.Default.Save();
                }
                if (mainWindowViewModel.PageClosed.CanExecute(obj))//удаеление страницы из коллекции страниц
                    mainWindowViewModel.PageClosed.Execute(obj);
            }
            catch (Exception e) { ErrorWindow(e); }
        }



        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        /// <param name="e"></param>
        /// <param name="name"></param>
        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"HomeModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
