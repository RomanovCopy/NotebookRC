using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using NotebookRCv001.Helpers;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;



namespace NotebookRCv001.Models
{
    internal class TextEditorModel : ViewModelBase
    {
        private readonly Languages language;
        private object currentWindow { get; set; }

        private RichTextBoxViewModel richTextBoxViewModel { get; set; }
        private BehaviorRichTextBox behaviorRichTextBox => richTextBoxViewModel?.BehaviorRichTextBox;

        internal double WindowWidth { get => windowWidth; set => SetProperty(ref windowWidth, value); }
        private double windowWidth;
        internal double WindowHeight { get => windowHeight; set => SetProperty(ref windowHeight, value); }
        private double windowHeight;
        internal double WindowTop { get => windowTop; set => SetProperty(ref windowTop, value); }
        private double windowTop;
        internal double WindowLeft { get => windowLeft; set => SetProperty(ref windowLeft, value); }
        private double windowLeft;
        internal object WindowState { get => windowState; set => SetProperty(ref windowState, value); }
        private object windowState;


        internal ObservableCollection<string> Headers => null;

        internal ObservableCollection<string> ToolTips => null;

        internal FlowDocument Document => behaviorRichTextBox?.Document;


        /// <summary>
        /// путь к последнему открытому или сохраненному файлу
        /// </summary>
        internal string PathToLastFile { get => pathToLastFile; set => SetProperty(ref pathToLastFile, value); }
        private string pathToLastFile;
        /// <summary>
        /// Имя последнего открытого или сохраненного файла
        /// </summary>
        internal string LastFileName { get => lastFileName; set => SetProperty(ref lastFileName, value); }
        private string lastFileName;


        internal TextEditorModel()
        {
            //устанавливаем локализацию
            language = new Languages();
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            //восстанавливаем размеры и положение окна
            if (Properties.Settings.Default.TextEditorFirstStart)
            {
                WindowHeight = 40;
                WindowWidth = 40;
                WindowLeft = 10;
                WindowTop = 10;
                Properties.Settings.Default.TextEditorFirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.TextEditorHeight;
                WindowWidth = Properties.Settings.Default.TextEditorWidth;
                WindowLeft = Properties.Settings.Default.TextEditorLeft;
                WindowTop = Properties.Settings.Default.TextEditorTop;
            }
            //восстанавливаем состояние окна
            WindowState = Properties.Settings.Default.TextEditorState;
        }






        internal bool CanExecute_WindowLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowLoaded(object obj)
        {
            try
            {
                if (obj is Window window)
                {
                    currentWindow = window;
                    var richtextbox = (MyControls.RichTextBox)window.FindResource("richtextbox");
                    richtextbox.Loaded += (s, e) => { richTextBoxViewModel = (RichTextBoxViewModel)richtextbox.DataContext; };
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_WindowClosing(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowClosing(object obj)
        {
            try
            {
                //размеры и положение окна
                if (WindowState.ToString() == "Normal")
                {
                    Properties.Settings.Default.TextEditorWidth = WindowWidth;
                    Properties.Settings.Default.TextEditorHeight = WindowHeight;
                    Properties.Settings.Default.TextEditorLeft = WindowLeft;
                    Properties.Settings.Default.TextEditorTop = WindowTop;
                }
                Properties.Settings.Default.TextEditorState = WindowState.ToString();
            }
            catch (Exception e) { ErrorWindow(e); }
        }




        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"TextEditorModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
