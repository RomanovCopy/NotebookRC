using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using NotebookRCv001.Infrastructure;
using System.Threading;
using NotebookRCv001.Interfaces;

namespace NotebookRCv001.Models
{
    internal class DisplayProgressModel : ViewModelBase
    {
        private object window { get; set; }
        internal object Target { get; set; }
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

        internal double ProgressValue
        {
            get
            {
                if (Target is IDisplayProgressTarget target)
                {
                    WindowClose(target.ProgressValue);
                    return target.ProgressValue;
                }
                return 0;
            }
            set => SetProperty(ref progressValue, value);
        }
        private double progressValue;

        public ObservableCollection<string> Headers => throw new NotImplementedException();

        public ObservableCollection<string> ToolTips => throw new NotImplementedException();

        internal DisplayProgressModel()
        {
            if (Properties.Settings.Default.DisplayProgressFirstStart)
            {
                WindowTop = 10;
                WindowLeft = 10;
                WindowHeight = 10;
                WindowWidth = 30;
                Properties.Settings.Default.DisplayProgressFirstStart = false;
            }
            else
            {
                WindowTop = Properties.Settings.Default.DisplayProgressTop;
                WindowLeft = Properties.Settings.Default.DisplayProgressLeft;
                WindowHeight = Properties.Settings.Default.DisplayProgressHeight;
                WindowWidth = Properties.Settings.Default.DisplayProgressWidth;
            }
        }

        private void WindowClose( double value)
        {
            if (value ==100 && window is Window win)
                win.Close();
        } 

        internal void Execute_WindowLoaded(object obj)
        {
            try
            {
                if (obj is Views.DisplayProgress progress)
                    window = progress;
            }
            catch { }
        }

        internal void Execute_WindowClosing(object obj)
        {
            try
            {
                Properties.Settings.Default.DisplayProgressWidth = WindowWidth;
                Properties.Settings.Default.DisplayProgressHeight = WindowHeight;
                Properties.Settings.Default.DisplayProgressTop = WindowTop;
                Properties.Settings.Default.DisplayProgressLeft = WindowLeft;
                Properties.Settings.Default.Save();
            }
            catch { }
        }

    }
}
