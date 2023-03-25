using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Drawing = System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xaml;

using Microsoft.Win32;

using NotebookRCv001.Helpers;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Views;

namespace NotebookRCv001.Models
{
    internal class SelectAndPasteWindowModel : ViewModelBase, IDisplayProgressTarget
    {
        private object window;
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly RichTextBoxViewModel richTextBoxViewModel;
        private readonly BehaviorRichTextBox behaviorRichTextBox;
        private Languages language => mainWindowViewModel.Language;
        private MenuHomeViewModel menuHomeViewModel;
        private HomeMenuFileViewModel homeMenuFileViewModel;



        /// <summary>
        /// контейнер с изображением
        /// </summary>
        internal Figure CurrentFigure { get => currentFigure; set => SetProperty(ref currentFigure, value); }
        private Figure currentFigure;
        /// <summary>
        /// флаг: добавляется каталог с файлами
        /// </summary>
        private bool isDirectoryAdded { get; set; }
        private string lastDocumentDirectory => Path.Combine(Environment.CurrentDirectory, "temp");
        private string lastDocumentPath => Path.Combine(lastDocumentDirectory, "temp.xaml");

        #region ________________________Size position and window status_____________________________

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



        #endregion


        internal ObservableCollection<string> Headers => language.SelectAndPasteWindowHeaders;
        internal ObservableCollection<string> ToolTips => language.SelectAndPasteWindowToolTips;
        internal ObservableCollection<string> Messages => language.SelectAndPasteWindowMessages;

        internal ObservableCollection<Figure> Figures { get => figures ??= new ObservableCollection<Figure>(); set => SetProperty(ref figures, value); }
        private ObservableCollection<Figure> figures;

        internal string CurrentDirectory { get => currentDirectory; set => SetProperty(ref currentDirectory, value); }
        private string currentDirectory;
        /// <summary>
        /// путь к файлу изображения
        /// </summary>
        internal string PathToFile { get => pathToFile; set => SetProperty(ref pathToFile, value, new string[] { "PathToFile", "BitMap" }); }
        private string pathToFile;

        /// <summary>
        /// путь к выбранному каталогу
        /// </summary>
        internal string PathToDirectory { get => pathToDirectory; set => SetProperty(ref pathToDirectory, value); }
        private string pathToDirectory;

        /// <summary>
        /// класс для работы с изображениями
        /// </summary>
        internal BitmapImage BitMap { get => behaviorRichTextBox.Bitmap; set => behaviorRichTextBox.Bitmap = value; }

        /// <summary>
        /// режим взаимодействия с текстом
        /// </summary>
        private WrapDirection WrapDirection { get => WrapDirectionBoth ? WrapDirection.Both : WrapDirection.None; }
        internal bool WrapDirectionBoth { get => wrapDirectionBoth; set => SetProperty(ref wrapDirectionBoth, value); }
        private bool wrapDirectionBoth;

        internal bool Merge { get => merge; set => SetProperty(ref merge, value); }
        private bool merge;

        /// <summary>
        /// растяжение контента
        /// </summary>
        private Stretch Stretch { get => StretchFill ? Stretch.Fill : Stretch.None; }
        internal bool StretchFill { get => stretchFill; set => SetProperty(ref stretchFill, value); }
        private bool stretchFill;


        /// <summary>
        /// угол поворота 
        /// </summary>
        private Rotation Rotation
        {
            get => Rotation90 ? Rotation.Rotate90 : Rotation180 ? Rotation.Rotate180 :
                Rotation270 ? Rotation.Rotate270 : Rotation.Rotate0;
        }
        internal bool Rotation0 { get => rotation0; set => SetProperty(ref rotation0, value); }
        private bool rotation0;
        internal bool Rotation90 { get => rotation90; set => SetProperty(ref rotation90, value); }
        private bool rotation90;
        internal bool Rotation180 { get => rotation180; set => SetProperty(ref rotation180, value); }
        private bool rotation180;
        internal bool Rotation270 { get => rotation270; set => SetProperty(ref rotation270, value); }
        private bool rotation270;

        /// <summary>
        /// имя открытого файла без пути к нему
        /// </summary>
        internal string FileName { get => fileName; set => SetProperty(ref fileName, value); }
        private string fileName;

        /// <summary>
        /// имя выбранного каталога
        /// </summary>
        internal string DirectoryName { get => directoryName; set => SetProperty(ref directoryName, value); }
        private string directoryName;


        /// <summary>
        /// процент изменения размера по горизонтали
        /// </summary>
        internal double PercentHeight { get => percentHeight; set => SetProperty(ref percentHeight, value); }
        private double percentHeight;


        /// <summary>
        /// процент изменения размера по вертикали
        /// </summary>
        internal double PercentWidth { get => percentWidth; set => SetProperty(ref percentWidth, value); }
        private double percentWidth;
        /// <summary>
        /// пропорциональное изменение размеров IsChecked
        /// </summary>
        internal bool Proportionally { get => proportionally; set => SetProperty(ref proportionally, value); }
        private bool proportionally;


        /// <summary>
        /// процент изменения начального размера до растяжения 
        /// </summary>
        internal double BeforeStretchingPercentWidth
        {
            get => beforeStretchingPercentWidth;
            set => SetProperty(ref beforeStretchingPercentWidth, value);
        }
        private double beforeStretchingPercentWidth;
        internal double BeforeStretchingPercentHeight
        {
            get => beforeStretchingPercentHeight;
            set => SetProperty(ref beforeStretchingPercentHeight, value);
        }
        private double beforeStretchingPercentHeight;


        /// <summary>
        /// начальный размер
        /// </summary>
        //internal double RealWidth { get => realWidth; set => SetProperty(ref realWidth, value); }
        //private double realWidth;
        internal double RealWidth
        {
            get => richTextBoxViewModel.BehaviorRichTextBox.RealWidth;
            set => richTextBoxViewModel.BehaviorRichTextBox.RealWidth = value;
        }
        internal double RealHeight
        {
            get => richTextBoxViewModel.BehaviorRichTextBox.RealHeight;
            set => richTextBoxViewModel.BehaviorRichTextBox.RealHeight = value;
        }
        //internal double RealHeight { get => realHeight; set => SetProperty(ref realHeight, value); }
        //private double realHeight;
        /// <summary>
        /// измененный размер
        /// </summary>
        internal double ChangedWidth { get => changedWidth; set => SetProperty(ref changedWidth, value); }
        private double changedWidth;
        internal double ChangedHeight { get => changedHeight; set => SetProperty(ref changedHeight, value); }
        private double changedHeight;
        /// <summary>
        /// выравнивание по горизонтали
        /// </summary>
        private FigureHorizontalAnchor figureHorizontalAnchor
        {
            get => HorizontalAlignmentRight ? FigureHorizontalAnchor.PageRight :
                HorizontalAlignmentLeft ? FigureHorizontalAnchor.PageLeft :
                FigureHorizontalAnchor.PageCenter;
        }
        internal bool HorizontalAlignmentLeft { get => horizontalAlignmentLeft; set => SetProperty(ref horizontalAlignmentLeft, value); }
        private bool horizontalAlignmentLeft;
        internal bool HorizontalAlignmentCenter { get => horizontalAlignmentCenter; set => SetProperty(ref horizontalAlignmentCenter, value); }
        private bool horizontalAlignmentCenter;
        internal bool HorizontalAlignmentRight { get => horizontalAlignmentRight; set => SetProperty(ref horizontalAlignmentRight, value); }
        private bool horizontalAlignmentRight;

        public double ProgressValue { get => progressValue; set => SetProperty(ref progressValue, value); }
        private double progressValue;

        internal SelectAndPasteWindowModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips", "Messages" });
            var home = mainWindowViewModel.CurrentPage;
            var menuHome = (MyControls.MenuHome)home.FindResource("menuhome");
            menuHomeViewModel = (MenuHomeViewModel)menuHome.DataContext;
            homeMenuFileViewModel = (HomeMenuFileViewModel)menuHome.FindResource("menufile");
            richTextBoxViewModel = (RichTextBoxViewModel)((MyControls.RichTextBox)home.FindResource("richtextbox")).DataContext;
            richTextBoxViewModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
            behaviorRichTextBox = richTextBoxViewModel.BehaviorRichTextBox;
            CurrentDirectory = Properties.Settings.Default.SelectAndPasteWindowDirectory;

            if (Properties.Settings.Default.SelectAndPasteWindowFirstStart)
            {
                WindowHeight = 50;
                WindowWidth = 55;
                WindowLeft = 30;
                WindowTop = 30;
                WindowState = "Normal";
                Properties.Settings.Default.SelectAndPasteWindowFirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.SelectAndPasteWindowHeight;
                WindowWidth = Properties.Settings.Default.SelectAndPasteWindowWidth;
                WindowLeft = Properties.Settings.Default.SelectAndPasteWindowLeft;
                WindowTop = Properties.Settings.Default.SelectAndPasteWindowTop;
                WindowState = Properties.Settings.Default.SelectAndPasteWindowState;
            }
            Directory.CreateDirectory(lastDocumentDirectory);
            homeMenuFileViewModel.SaveFile.Execute(lastDocumentPath);
        }



        /// <summary>
        /// выбор файла
        /// </summary>
        /// <param name="obj">null</param>
        /// <returns></returns>
        internal bool CanExecute_SelectFile(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectFile(object obj)
        {
            try
            {
                string path = null;
                Size size = Size.Empty;
                path = homeMenuFileViewModel.OpenFileDialog.Invoke("All files (*.*)|*.*|Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...", CurrentDirectory);
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                {
                    behaviorRichTextBox.ImageOptions = Size.Empty;
                    SetImageOptions();
                    behaviorRichTextBox.ImagePath = path;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_SelectDirectory(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectDirectory(object obj)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                var viewmodel = (FolderBrowserDialogViewModel)dialog.DataContext;
                dialog.Closing += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(viewmodel.WorkingDirectory))
                    {
                        if (Directory.Exists(viewmodel.WorkingDirectory))
                        {
                            PathToDirectory = viewmodel.WorkingDirectory;
                            DirectoryName = new DirectoryInfo(PathToDirectory).Name;
                            var files = Directory.GetFiles(PathToDirectory);
                            isDirectoryAdded = files.Length > 1;
                            if (files.Length > 0)
                            {
                                behaviorRichTextBox.ImageOptions = Size.Empty;
                                SetImageOptions();
                                behaviorRichTextBox.ImagePath = files[0];
                            }
                        }
                    }
                };
                dialog.ShowDialog();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// изменение размеров добавляемого контента
        /// </summary>
        /// <param name="obj">процент от первоначального размера</param>
        /// <returns></returns>
        internal bool CanExecute_SizeChanged(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SizeChanged(object obj)
        {
            try
            {
                if (obj is string name)
                {
                    if (name == "width")
                    {
                        ChangedWidth = RealWidth / 100 * PercentWidth;
                        PercentHeight = Proportionally ? PercentWidth : PercentHeight;
                        ChangedHeight = Proportionally ? RealHeight / 100 * PercentHeight : ChangedHeight;
                    }
                    else if (name == "height")
                    {
                        ChangedHeight = RealHeight / 100 * PercentHeight;
                        PercentWidth = Proportionally ? PercentHeight : PercentWidth;
                        ChangedWidth = Proportionally ? RealWidth / 100 * PercentWidth : ChangedWidth;
                    }
                    behaviorRichTextBox.ImageOptions = new Size(ChangedWidth, ChangedHeight);
                }

            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_ProportionallyChanged(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ProportionallyChanged(object obj)
        {
            try
            {
                if ((bool)obj)
                {
                    PercentWidth = PercentWidth > PercentHeight ? PercentWidth : PercentWidth < PercentHeight ? PercentHeight : PercentWidth;
                    PercentHeight = PercentWidth;
                    var size = new Size(RealWidth / 100 * PercentWidth, RealHeight / 100 * PercentHeight);
                    behaviorRichTextBox.ImageOptions = size;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// обработка горизонтального выравнивания
        /// </summary>
        /// <param name="obj">вид выравнивания</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_HorizontalAlignment(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_HorizontalAlignment(object obj)
        {
            try
            {
                if (obj is string alignment)
                {
                    //выключаем не выбранные чекбоксы
                    if (alignment != "merge")
                    {
                        HorizontalAlignmentLeft = alignment == "left";
                        HorizontalAlignmentCenter = alignment == "center";
                        HorizontalAlignmentRight = alignment == "right";
                        behaviorRichTextBox.ImageOptions = figureHorizontalAnchor;
                    }
                    else
                    {
                        behaviorRichTextBox.ImageOptions = Merge ? "MergeOn" : "MergeOff";
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// поворот изображения
        /// </summary>
        /// <param name="obj">имя выбранного CheckBox</param>
        /// <returns></returns>
        internal bool CanExecute_ImageRotation(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ImageRotation(object obj)
        {
            try
            {
                if (obj is string rotate)
                {
                    Rotation0 = rotate == "rotate0";
                    Rotation90 = rotate == "rotate90";
                    Rotation180 = rotate == "rotate180";
                    Rotation270 = rotate == "rotate270";
                    behaviorRichTextBox.ImageOptions = Rotation;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_WrapDirectionChanged(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WrapDirectionChanged(object obj)
        {
            try
            {
                WrapDirectionBoth = (bool)obj;
                behaviorRichTextBox.ImageOptions = WrapDirection;
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_StretchFillChanged(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_StretchFillChanged(object obj)
        {
            try
            {
                if (StretchFill = (bool)obj)
                {
                    BeforeStretchingPercentWidth = PercentWidth;
                    BeforeStretchingPercentHeight = PercentHeight;
                }
                else if (BeforeStretchingPercentWidth > 0 && BeforeStretchingPercentHeight > 0)
                {
                    PercentWidth = BeforeStretchingPercentWidth;
                    PercentHeight = BeforeStretchingPercentHeight;
                    BeforeStretchingPercentWidth = 0;
                    BeforeStretchingPercentHeight = 0;
                    Execute_SizeChanged("width");
                    Execute_SizeChanged("height");
                }
                behaviorRichTextBox.ImageOptions = Stretch;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// примененин изменений
        /// </summary>
        /// <param name="obj">null</param>
        /// <returns></returns>
        internal bool CanExecute_ClickButtonApply(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal async void Execute_ClickButtonApply(object obj)
        {
            try
            {
                var win = window as Window;
                if (isDirectoryAdded)
                {
                    DisplayProgress progress = null;
                    await Task.Run(() =>
                    {//этот поток осущетсвляет ожжидание запуска окна DisplayProgress
                        Thread thread = new Thread(() =>
                        {//этот поток необходим для совместимости потока с отображаемым окном(ApartmentState.STA)
                            progress = new DisplayProgress();
                            var viewmodel = (DisplayProgressViewModel)progress.DataContext;
                            //подписываем окно на изменения в этом классе
                            PropertyChanged += (s, e) => viewmodel.OnPropertyChanged(e.PropertyName);
                            //отправляем ссылку на этот класс
                            viewmodel.Target = this;
                            progress.ShowDialog();
                        });
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    });
                    //получаем коллекцию всех файлов в заданной директории
                    var files = Directory.GetFiles(PathToDirectory);
                    for (double i = 1; i < files.Length; i++)
                    {//добавление контента находящегося в папке
                     //задаем параметры вставки изображений
                        SetImageOptions();
                        //отправляем путь к файлу для его (файла) получения, обработки и добавления
                        behaviorRichTextBox.ImagePath = files[(int)i];
                        //определяем процент уже добаленных файлов к общему их числу в папке
                        //для окна DisplayProgress
                        //обновление DisplayProgress по INotifyPropertyChanged
                        ProgressValue = i / (double)files.Length * 100.00;
                    }
                    behaviorRichTextBox.Document.Blocks.Remove(behaviorRichTextBox.Document.Blocks.FirstBlock);
                }
                //закрываем окно настройки добаляемых изображений
                win.Close();
                homeMenuFileViewModel.SaveFile.Execute(lastDocumentPath);
                //получив такое значение окно DisplayProgress закроется самостоятельно
                ProgressValue = 100;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// отмена изменений
        /// </summary>
        /// <param name="obj">null</param>
        /// <returns></returns>
        internal bool CanExecute_ClickButtonCancel(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorRichTextBox.CurrentFigure != null && !string.IsNullOrWhiteSpace(lastDocumentPath);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickButtonCancel(object obj)
        {
            try
            {
                homeMenuFileViewModel.OpenFile.Execute(lastDocumentPath);
                BitMap = null;
                OnPropertyChanged("Bitmap");
                PercentHeight = 100;
                PercentWidth = 100;
                ChangedWidth = 0;
                ChangedHeight = 0;
                behaviorRichTextBox.CurrentFigure = null;
                PathToFile = null;
                FileName = null;
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// сохранение полученного элемента
        /// </summary>
        /// <param name="obj">null</param>
        /// <returns></returns>
        internal bool CanExecute_ClickButtonSave(object obj)
        {
            try
            {
                bool c = false;
                c = CurrentFigure != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickButtonSave(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// вставка изображения из буфера обмена
        /// </summary>
        /// <param name="obj">BitmapSource</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PasteAnImageFromTheClipboard(object obj)
        {
            try
            {
                bool c = false;
                c = obj is BitmapSource;
                return c;
            }
            catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PasteAnImageFromTheClipboard(object obj)
        {
            try
            {
                if(obj is BitmapSource source)
                {
                    behaviorRichTextBox.ImageOptions = Size.Empty;
                    SetImageOptions();
                    behaviorRichTextBox.ImagePath = source;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// задание параметров для вставляемого изображения
        /// </summary>
        private void SetImageOptions()
        {
            try
            {
                behaviorRichTextBox.ImageOptions = figureHorizontalAnchor;
                behaviorRichTextBox.ImageOptions = new Size(ChangedWidth, ChangedHeight);
                behaviorRichTextBox.ImageOptions = Rotation;
                behaviorRichTextBox.ImageOptions = Stretch;
                behaviorRichTextBox.ImageOptions = WrapDirection;
                behaviorRichTextBox.ImageOptions = Merge ? "MergeOn" : "MergeOff";
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// окончание загрузки окна
        /// </summary>
        /// <param name="obj">Window</param>
        /// <returns></returns>
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
                    this.window = window;
                else
                    throw new Exception(language.MessagesMyMessages[3]);
                PercentWidth = 100;
                PercentHeight = 100;
                Rotation0 = true;
                WrapDirectionBoth = true;
                StretchFill = false;
                HorizontalAlignmentLeft = true;
                Proportionally = true;
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// перед закрытием окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
                homeMenuFileViewModel.SaveFile.Execute(lastDocumentPath);
                if (!string.IsNullOrWhiteSpace(CurrentDirectory))
                    Properties.Settings.Default.SelectAndPasteWindowDirectory = CurrentDirectory;
                Properties.Settings.Default.SelectAndPasteWindowHeight = WindowHeight;
                Properties.Settings.Default.SelectAndPasteWindowWidth = WindowWidth;
                Properties.Settings.Default.SelectAndPasteWindowLeft = WindowLeft;
                Properties.Settings.Default.SelectAndPasteWindowTop = WindowTop;
                Properties.Settings.Default.SelectAndPasteWindowState = WindowState.ToString();
                behaviorRichTextBox.CurrentFigure = null;
                behaviorRichTextBox.ImagePath = null;
                behaviorRichTextBox.Bitmap = null;
                richTextBoxViewModel.Document.Focus();
            }
            catch (Exception e) { ErrorWindow(e); }
        }


    }
}


