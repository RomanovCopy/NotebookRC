using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using Media = System.Windows.Media;
using NotebookRCv001.Infrastructure;
using System.Runtime.CompilerServices;
using System.Threading;
using NotebookRCv001.ViewModels;
using System.Reflection;

namespace NotebookRCv001.Models
{
    public class HomeMenuSettingsModel : ViewModelBase
    {
        private readonly MainWindowViewModel MainWindowViewModel;
        private HomeViewModel HomeViewModel;
        private RichTextBoxViewModel RichTextBoxViewModel;

        internal ObservableCollection<string> Headers => MainWindowViewModel.Language.HomeMenuSettings;
        internal ObservableCollection<string> ToolTips => MainWindowViewModel.Language.ToolTipsHomeMenuSettings;
        internal ObservableCollection<string> LanguagesKey => MainWindowViewModel.Language.LanguagesKey;

        /// <summary>
        /// текущая кодировка
        /// </summary>
        internal Encoding HomeEncoding
        {
            get => homeEncoding;
            set => SetProperty(ref homeEncoding, value);
        }
        private Encoding homeEncoding;

        /// <summary>
        /// коллекция доступных кодировок
        /// </summary>
        internal ObservableCollection<EncodingInfo> HomeEncodings
        {
            get => homeEncodings ??= new ObservableCollection<EncodingInfo>();
            set => SetProperty(ref homeEncodings, value);
        }
        private ObservableCollection<EncodingInfo> homeEncodings;

        /// <summary>
        /// коллекция доступных системных цветов
        /// </summary>
        internal ObservableCollection<Drawing.Color> HomeMyColors { get => homeMyColors ??= new ObservableCollection<Drawing.Color>(); 
            set => SetProperty(ref homeMyColors, value); }
        private ObservableCollection<Drawing.Color> homeMyColors;

        /// <summary>
        /// текущий цвет бумаги
        /// </summary>
        internal Media.Brush MyBackground { get => RichTextBoxViewModel.MyBackground;
            set => RichTextBoxViewModel.MyBackground = value;
        }
        /// <summary>
        /// текущий цвет чернил
        /// </summary>
        internal Media.Brush MyForeground { get => RichTextBoxViewModel.MyForeground; 
            set => RichTextBoxViewModel.MyForeground=value; }

        /// <summary>
        /// текущий цвет фона чернил
        /// </summary>
        internal Media.Brush MyFontBackground { get => RichTextBoxViewModel.MyFontBackground; 
            set => RichTextBoxViewModel.MyFontBackground=value; }


        public HomeMenuSettingsModel()
        {
            MainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            MainWindowViewModel.Language.PropertyChanged += ( s, e ) =>
            OnPropertyChanged(new string[] { "Headers", "ToolTips", "LanguagesKey" });
            foreach (EncodingInfo info in Encoding.GetEncodings())
                HomeEncodings.Add(info);
            foreach (PropertyInfo info in typeof(Drawing.Color).GetProperties())
            {
                if (info.PropertyType == typeof(Drawing.Color))
                    HomeMyColors.Add((Drawing.Color)info.GetValue(null));
            }
        }



        /// <summary>
        /// выбор цвета бумаги
        /// </summary>
        /// <param name="obj">выбранный цвет</param>
        /// <returns></returns>
        internal bool CanExecute_SelectColorBackground( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectColorBackground( object obj )
        {
            try
            {
                if (obj is Media.Brush brush)
                {
                    MyBackground = brush;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// выбор цвета чернил
        /// </summary>
        /// <param name="obj">выбранный цвет</param>
        /// <returns></returns>
        internal bool CanExecute_SelectForeground( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectForeground( object obj )
        {
            try
            {
                if (obj is Media.Brush brush)
                {
                    MyForeground = brush;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// выбор цвета фона шрифта
        /// </summary>
        /// <param name="obj">выбранный цвет</param>
        /// <returns></returns>
        internal bool CanExecute_SelectColorFontBackground( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectColorFontBackground( object obj )
        {
            try
            {
                if (obj is Media.Brush brush)
                {
                    MyFontBackground = brush;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }



        /// <summary>
        /// выбор кодировки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_SelectEncoding( object obj )
        {
            try
            {
                bool c = false;
                if (obj is EncodingInfo info && HomeEncoding != null)
                {
                    c = info.CodePage != HomeEncoding.CodePage;
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectEncoding( object obj )
        {
            try
            {
                if (obj is EncodingInfo info)
                {
                    HomeEncoding = info.GetEncoding();
                    Properties.Settings.Default.EncodingCodePage = HomeEncoding.CodePage;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_InstallingLocalization( object obj )
        {
            try
            {
                bool c = false;
                if (obj is string loc)
                {
                    c = loc != MainWindowViewModel.Language.Key;
                }
                return c;

            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_InstallingLocalization( object obj )
        {
            try
            {
                if (obj is string str && str != MainWindowViewModel.Language.Key)
                {
                    MainWindowViewModel.Language.Key = str;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_HomeMenuSettingsLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_HomeMenuSettingsLoaded( object obj )
        {
            try
            {
                if (MainWindowViewModel.CurrentPage is Views.Home page)
                {
                    HomeViewModel = (HomeViewModel)page.DataContext;
                    RichTextBoxViewModel = (RichTextBoxViewModel)((MyControls.RichTextBox)page.FindResource("richtextbox")).DataContext;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }







        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"HomeMenuSettingsModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
