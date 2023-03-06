using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Models
{
    public class ToolBarStatusModel : ViewModelBase
    {
        readonly MainWindowViewModel mainWindowViewModel;
        Languages language => mainWindowViewModel.Language;

        private HomeViewModel homeViewModel;


        internal string HomeEncoding => homeViewModel?.HomeEncoding.EncodingName;
        internal string EncryptionStatus => homeViewModel?.EncryptionStatus;
        internal string WorkingDirectory => homeViewModel?.WorkingDirectory;
        internal string WorkingDirectoryName => homeViewModel?.WorkingDirectoryName;
        internal string CurrentInputLanguageName { get => currentInputLanguageName; set => SetProperty( ref currentInputLanguageName, value ); }
        private string currentInputLanguageName;


        internal ObservableCollection<string> Headers => language.MainWindowToolBar;
        internal ObservableCollection<string> ToolTips => language.ToolTipsMainWindowToolBar;

        public ToolBarStatusModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            mainWindowViewModel.Language.PropertyChanged += ( s, e ) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            InputLanguageManager.Current.InputLanguageChanged += ( s, e ) => { CurrentInputLanguageName = e.NewLanguage.Name; };
        }


        internal bool CanExecute_ToolBarStatusLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ToolBarStatusLoaded( object obj )
        {
            try
            {
                //устанавливаем привязку ToolBarStatus к домашней странице Home
                var page = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).LastOrDefault();
                page.Loaded += ( s, e ) =>
                {
                    homeViewModel = (HomeViewModel)page.DataContext;
                    homeViewModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
                    CurrentInputLanguageName = InputLanguageManager.Current.CurrentInputLanguage.Name;
                };

            }
            catch (Exception e) { ErrorWindow(e); }
        }


        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"ToolBarStatusModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
