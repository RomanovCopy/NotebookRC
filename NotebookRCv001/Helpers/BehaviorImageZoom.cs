using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Xaml.Behaviors;

using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Helpers
{
    public class BehaviorImageZoom : Behavior<Image>
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        /// <summary>
        /// автомасштаб
        /// </summary>
        private double autoScale { get; set; }
        /// <summary>
        /// текущий масштаб
        /// </summary>
        private double currentScale { get; set; }


        public object Parent => AssociatedObject.Parent;


        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty;



        static BehaviorImageZoom()
        {
            SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource),
                typeof(BehaviorImageZoom), new PropertyMetadata(null, OnSourceChanged));
        }

        public BehaviorImageZoom()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            mainWindowViewModel.FrameList.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems[0] is NotebookRCv001.MyControls.MediaPlayer player)
                {
                    player.SizeChanged += (s, e) => { 
                        if(currentScale==autoScale) 
                            ImageTransformationToFitThePage(); 
                    };
                }
            };
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseWheel += Image_MouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseWheel -= Image_MouseWheel;
        }


        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is Image image)
            {
                var step = 0.1; // Размер шага масштабирования
                currentScale += step * (e.Delta > 0 ? 1 : -1); // Применяем масштаб
                currentScale = currentScale >= 5 ? 5 : currentScale <= autoScale ? autoScale : currentScale;
                image.LayoutTransform = new ScaleTransform(currentScale, currentScale);
            }
        }
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BehaviorImageZoom behavior)
            {
                behavior.AssociatedObject.Source = (ImageSource)e.NewValue;
                if (behavior.autoScale == behavior.currentScale)
                    behavior.ImageTransformationToFitThePage();
            }
        }
        /// <summary>
        /// трансформация изображения под размер страницы
        /// </summary>
        /// <param name="page">страница</param>
        /// <param name="bitmap">изображение</param>
        /// <exception cref="Exception"></exception>
        internal void ImageTransformationToFitThePage()
        {
            try
            {
                if (Source is BitmapImage bitmap && mainWindowViewModel.CurrentPage is MyControls.MediaPlayer page)
                {
                    //трансформируем изображение под размер страницы
                    var height = page.ActualHeight;
                    var width = page.ActualWidth;
                    var imgHeight = bitmap.Height;
                    var imgWidth = bitmap.Width;
                    double scaleX = (double)width / imgWidth;
                    double scaleY = (double)height / imgHeight;
                    autoScale = currentScale = Math.Min(scaleX, scaleY);
                    AssociatedObject.LayoutTransform = new ScaleTransform(autoScale, autoScale);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
