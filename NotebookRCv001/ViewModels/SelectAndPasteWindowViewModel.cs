using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Documents;
using NotebookRCv001.Models;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace NotebookRCv001.ViewModels
{
    public class SelectAndPasteWindowViewModel : ViewModelBase, IWindowViewModel
    {
        private readonly SelectAndPasteWindowModel selectAndPasteWindowModel;

        #region _____________Size position and window status______________________________________

        public double WindowWidth { get => selectAndPasteWindowModel.WindowWidth; set => selectAndPasteWindowModel.WindowWidth = value; }
        public double WindowHeight { get => selectAndPasteWindowModel.WindowHeight; set => selectAndPasteWindowModel.WindowHeight = value; }
        public double WindowTop { get => selectAndPasteWindowModel.WindowTop; set => selectAndPasteWindowModel.WindowTop = value; }
        public double WindowLeft { get => selectAndPasteWindowModel.WindowLeft; set => selectAndPasteWindowModel.WindowLeft = value; }
        public object WindowState { get => selectAndPasteWindowModel.WindowState; set => selectAndPasteWindowModel.WindowState = value; }


        #endregion

        public Figure CurrentFigure => selectAndPasteWindowModel.CurrentFigure;

        public ObservableCollection<string> Headers => selectAndPasteWindowModel.Headers;
        public ObservableCollection<string> ToolTips => selectAndPasteWindowModel.ToolTips;
        public ObservableCollection<string> Messages => selectAndPasteWindowModel.Messages;

        public string FileName { get => selectAndPasteWindowModel.FileName; set => selectAndPasteWindowModel.FileName = value; }
        public string PathToFile { get => selectAndPasteWindowModel.PathToFile; set => selectAndPasteWindowModel.PathToFile = value; }
        public string DirectoryName { get => selectAndPasteWindowModel.DirectoryName; set => selectAndPasteWindowModel.DirectoryName = value; }
        public string PathToDirectory { get => selectAndPasteWindowModel.PathToDirectory; set => selectAndPasteWindowModel.PathToDirectory = value; }
        public BitmapImage BitMap { get => selectAndPasteWindowModel.BitMap; set => selectAndPasteWindowModel.BitMap = value; }
        public bool StretchFill { get => selectAndPasteWindowModel.StretchFill; set => selectAndPasteWindowModel.StretchFill = value; }
        public bool WrapDirectionBoth { get => selectAndPasteWindowModel.WrapDirectionBoth; set => selectAndPasteWindowModel.WrapDirectionBoth = value; }
        public bool Merge { get => selectAndPasteWindowModel.Merge; set => selectAndPasteWindowModel.Merge = value; }
        public bool Rotation0 { get => selectAndPasteWindowModel.Rotation0; set => selectAndPasteWindowModel.Rotation0 = value; }
        public bool Rotation90 { get => selectAndPasteWindowModel.Rotation90; set => selectAndPasteWindowModel.Rotation90=value; }
        public bool Rotation180 { get => selectAndPasteWindowModel.Rotation180; set => selectAndPasteWindowModel.Rotation180=value; }
        public bool Rotation270 { get => selectAndPasteWindowModel.Rotation270; set => selectAndPasteWindowModel.Rotation270=value; }
        public double PercentHeight { get => selectAndPasteWindowModel.PercentHeight; set => selectAndPasteWindowModel.PercentHeight = value; }
        public double PercentWidth { get => selectAndPasteWindowModel.PercentWidth; set => selectAndPasteWindowModel.PercentWidth = value; }
        public bool Proportionally { get => selectAndPasteWindowModel.Proportionally; set => selectAndPasteWindowModel.Proportionally = value; }
        public double RealWidth { get => selectAndPasteWindowModel.RealWidth; set => selectAndPasteWindowModel.RealWidth = value; }
        public double RealHeight { get => selectAndPasteWindowModel.RealHeight; set => selectAndPasteWindowModel.RealHeight = value; }
        public double ChangedWidth { get => selectAndPasteWindowModel.ChangedWidth; set => selectAndPasteWindowModel.ChangedWidth = value; }
        public double ChangedHeight { get => selectAndPasteWindowModel.ChangedHeight; set => selectAndPasteWindowModel.ChangedHeight = value; }

        public bool HorizontalAlignmentLeft
        {
            get => selectAndPasteWindowModel.HorizontalAlignmentLeft;
            set => selectAndPasteWindowModel.HorizontalAlignmentLeft = value;
        }
        public bool HorizontalAlignmentCenter
        {
            get => selectAndPasteWindowModel.HorizontalAlignmentCenter;
            set => selectAndPasteWindowModel.HorizontalAlignmentCenter = value;
        }
        public bool HorizontalAlignmentRight
        {
            get => selectAndPasteWindowModel.HorizontalAlignmentRight;
            set => selectAndPasteWindowModel.HorizontalAlignmentRight = value;
        }



        public SelectAndPasteWindowViewModel ( )
        {
            selectAndPasteWindowModel = new SelectAndPasteWindowModel ( );
            selectAndPasteWindowModel.PropertyChanged += ( s, e ) => OnPropertyChanged ( e.PropertyName );
        }


        public ICommand SelectFile => selectFile ??= new RelayCommand ( selectAndPasteWindowModel.Execute_SelectFile,
            selectAndPasteWindowModel.CanExecute_SelectFile );
        RelayCommand selectFile;

        public ICommand SelectDirectory => selectDirectory ??= new RelayCommand ( selectAndPasteWindowModel.Execute_SelectDirectory,
            selectAndPasteWindowModel.CanExecute_SelectDirectory );
        RelayCommand selectDirectory;

        public ICommand SizeChanged => sizeChanged ??= new RelayCommand ( selectAndPasteWindowModel.Execute_SizeChanged,
            selectAndPasteWindowModel.CanExecute_SizeChanged );
        RelayCommand sizeChanged;

        public ICommand ProportionallyChanged => proportionallyChanged ??= new RelayCommand ( selectAndPasteWindowModel.Execute_ProportionallyChanged,
            selectAndPasteWindowModel.CanExecute_ProportionallyChanged );
        RelayCommand proportionallyChanged;

        public ICommand WrapDirectionChanged => wrapDirectionChanged ??= new RelayCommand(selectAndPasteWindowModel.Execute_WrapDirectionChanged,
            selectAndPasteWindowModel.CanExecute_WrapDirectionChanged);
        RelayCommand wrapDirectionChanged;

        public ICommand StretchFillChanged => stretchFillChanged ??= new RelayCommand ( selectAndPasteWindowModel.Execute_StretchFillChanged,
            selectAndPasteWindowModel.CanExecute_StretchFillChanged );
        private RelayCommand stretchFillChanged;

        public ICommand HorizontalAlignment => horizontalAlignment ??= new RelayCommand ( selectAndPasteWindowModel.Execute_HorizontalAlignment,
            selectAndPasteWindowModel.CanExecute_HorizontalAlignment );
        private RelayCommand horizontalAlignment;

        public ICommand ImageRotation => imageRotation ??= new RelayCommand ( selectAndPasteWindowModel.Execute_ImageRotation,
            selectAndPasteWindowModel.CanExecute_ImageRotation );
        private RelayCommand imageRotation;

        public ICommand ClickButtonApply => clickButtonApply ??= new RelayCommand ( selectAndPasteWindowModel.Execute_ClickButtonApply,
            selectAndPasteWindowModel.CanExecute_ClickButtonApply );
        private RelayCommand clickButtonApply;

        public ICommand ClickButtonCancel => clickButtonCancel ??= new RelayCommand ( selectAndPasteWindowModel.Execute_ClickButtonCancel,
            selectAndPasteWindowModel.CanExecute_ClickButtonCancel );
        private RelayCommand clickButtonCancel;

        public ICommand ClickButtonSave => clickButtonSave ??= new RelayCommand ( selectAndPasteWindowModel.Execute_ClickButtonSave,
            selectAndPasteWindowModel.CanExecute_ClickButtonSave );
        private RelayCommand clickButtonSave;

        /// <summary>
        /// вставка и настройк изображения из буфера обмена(входной параметр должен быть типа BitmapSource)
        /// </summary>
        public ICommand PasteAnImageFromTheClipboard => pasteAnImageFromTheClipboard ??= new RelayCommand(
            selectAndPasteWindowModel.Execute_PasteAnImageFromTheClipboard, selectAndPasteWindowModel.CanExecute_PasteAnImageFromTheClipboard);
        private RelayCommand pasteAnImageFromTheClipboard;

        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand ( selectAndPasteWindowModel.Execute_WindowLoaded,
            selectAndPasteWindowModel.CanExecute_WindowLoaded );
        private RelayCommand windowLoaded;

        public ICommand WindowClosing => windowClosing ??= new RelayCommand ( selectAndPasteWindowModel.Execute_WindowClosing,
            selectAndPasteWindowModel.CanExecute_WindowClosing );
        private RelayCommand windowClosing;
    }
}
