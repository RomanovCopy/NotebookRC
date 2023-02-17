using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using Media = System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;

namespace NotebookRCv001.ViewModels
{
    public class HomeMenuSettingsViewModel: ViewModelBase
    {

        private readonly HomeMenuSettingsModel HomeMenuSettingsModel;
        /// <summary>
        /// коллекция заголовков
        /// </summary>
        public ObservableCollection<string> Headers => HomeMenuSettingsModel.Headers;
        /// <summary>
        /// коллекция подсказок
        /// </summary>
        public ObservableCollection<string> ToolTips => HomeMenuSettingsModel.ToolTips;
        /// <summary>
        /// коллекция установленных ключей локализации
        /// </summary>
        public ObservableCollection<string> LanguagesKey => HomeMenuSettingsModel.LanguagesKey;
        /// <summary>
        /// коллекция доступных кодировок
        /// </summary>
        public ObservableCollection<EncodingInfo> HomeEncodings => HomeMenuSettingsModel.HomeEncodings;
        /// <summary>
        /// текущая кодировка
        /// </summary>
        public Encoding HomeEncoding { get => HomeMenuSettingsModel.HomeEncoding; set => HomeMenuSettingsModel.HomeEncoding = value; }
        /// <summary>
        /// коллекция доступных системных цветов
        /// </summary>
        public ObservableCollection<Drawing.Color> HomeMyColors => HomeMenuSettingsModel.HomeMyColors;
        /// <summary>
        /// текущий цвет бумаги
        /// </summary>
        public Media.Brush MyBackground { get => HomeMenuSettingsModel.MyBackground;
             set => HomeMenuSettingsModel.MyBackground = value; }
        /// <summary>
        /// текущий цвет чернил
        /// </summary>
        public Media.Brush MyForeground { get => HomeMenuSettingsModel.MyForeground;
             set => HomeMenuSettingsModel.MyForeground = value; }
        /// <summary>
        /// текущий цвет фона шрифта
        /// </summary>
        public Media.Brush MyFontBackground { get => HomeMenuSettingsModel.MyFontBackground;
             set => HomeMenuSettingsModel.MyFontBackground = value; }


        public HomeMenuSettingsViewModel()
        {
            HomeMenuSettingsModel = new HomeMenuSettingsModel();
            HomeMenuSettingsModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// выбор цвета бумаги
        /// </summary>
        public ICommand SelectColorBackground => selectColorBackground ??= new RelayCommand(HomeMenuSettingsModel.Execute_SelectColorBackground, HomeMenuSettingsModel.CanExecute_SelectColorBackground);
        private RelayCommand selectColorBackground;
        /// <summary>
        /// выбор цвета шрифта (чернила )
        /// </summary>
        public ICommand SelectColorForeground => selectColorForeground ??= new RelayCommand(HomeMenuSettingsModel.Execute_SelectForeground, HomeMenuSettingsModel.CanExecute_SelectForeground);
        private RelayCommand selectColorForeground;
        /// <summary>
        /// выбор цвета фона шрифта
        /// </summary>
        public ICommand SelectColorFontBackground => selectColorFontBackground ??= new RelayCommand(HomeMenuSettingsModel.Execute_SelectColorFontBackground, HomeMenuSettingsModel.CanExecute_SelectColorFontBackground);
        private RelayCommand selectColorFontBackground;


        public ICommand SelectEncoding => selectEncoding ??= new RelayCommand(HomeMenuSettingsModel.Execute_SelectEncoding, HomeMenuSettingsModel.CanExecute_SelectEncoding);
        private RelayCommand selectEncoding;


        public ICommand InstallingLocalization => installingLocalization ??=
            new RelayCommand(HomeMenuSettingsModel.Execute_InstallingLocalization,
                HomeMenuSettingsModel.CanExecute_InstallingLocalization);
        private RelayCommand installingLocalization;


        public ICommand HomeMenuSettingsLoaded => homeMenuSettingsLoaded ??= new RelayCommand(
            HomeMenuSettingsModel.Execute_HomeMenuSettingsLoaded, HomeMenuSettingsModel.CanExecute_HomeMenuSettingsLoaded);
        private RelayCommand homeMenuSettingsLoaded;


    }
}
