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
        private Point lastMousePosition { get; set; }
        private bool isMouseDragging { get; set; }
        private double scale { get; set; }


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

                }
            };
            isMouseDragging = false;
            scale = 1;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseWheel += Image_MouseWheel;
            AssociatedObject.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            AssociatedObject.MouseMove += Image_MouseMove;
            AssociatedObject.MouseLeftButtonUp += Image_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseWheel -= Image_MouseWheel;
            AssociatedObject.MouseLeftButtonDown -= Image_MouseLeftButtonDown;
            AssociatedObject.MouseMove -= Image_MouseMove;
            AssociatedObject.MouseLeftButtonUp -= Image_MouseLeftButtonUp;
        }


        private Point center; // Центр масштабирования

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var transform = AssociatedObject.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;
            var position = e.GetPosition(AssociatedObject);

            var step = 0.1; // Размер шага масштабирования
            var scaleFactor = e.Delta > 0 ? 1 + step : 1 - step; // Фактор масштабирования

            scale *= scaleFactor; // Применяем масштаб

            // Пересчитываем центр масштабирования относительно изображения
            var imagePosition = position - new Point(AssociatedObject.ActualWidth / 2, AssociatedObject.ActualHeight / 2);
            var scaledImagePosition = new Vector(imagePosition.X * scaleFactor, imagePosition.Y * scaleFactor);
            center = position - scaledImagePosition;

            // Корректируем смещение, чтобы изображение не выходило за пределы контейнера
            var offsetX = AssociatedObject.ActualWidth / 2 - center.X;
            var offsetY = AssociatedObject.ActualHeight / 2 - center.Y;

            matrix = new Matrix();
            matrix.ScaleAt(scale, scale, center.X, center.Y);
            matrix.OffsetX = offsetX;
            matrix.OffsetY = offsetY;

            AssociatedObject.RenderTransform = new MatrixTransform(matrix);

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(AssociatedObject);
            lastMousePosition = position;
            isMouseDragging = true;
            //AssociatedObject.CaptureMouse();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            //if (isMouseDragging)
            //{
            //    var position = e.GetPosition(AssociatedObject);

            //    var transform = AssociatedObject.RenderTransform as MatrixTransform;
            //    var matrix = transform.Matrix;

            //    var offsetX = position.X - lastMousePosition.X;
            //    var offsetY = position.Y - lastMousePosition.Y;

            //    matrix.Translate(offsetX, offsetY);

            //    transform.Matrix = matrix;

            //    lastMousePosition = position;
            //}

            if (isMouseDragging)
            {
                var image = AssociatedObject;
                if (image.RenderTransform is TransformGroup transform)
                {
                    TranslateTransform translateTransform = null;
                    foreach (var t in transform.Children)
                    {
                        if (t is TranslateTransform tt)
                        {
                            translateTransform = tt;
                            break;
                        }
                    }
                    if (translateTransform != null)
                    {
                        Point currentPoint = e.GetPosition(image);
                        double diffX = currentPoint.X - lastMousePosition.X;
                        double diffY = currentPoint.Y - lastMousePosition.Y;
                        //scrollViewer.ScrollToVerticalOffset(diffY);
                        //scrollViewer.ScrollToHorizontalOffset(diffX);
                        translateTransform.X += diffX;
                        translateTransform.Y += diffY;
                        lastMousePosition = currentPoint;
                    }
                }
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDragging = false;
            //AssociatedObject.ReleaseMouseCapture();
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BehaviorImageZoom behavior)
            {
                //behavior.ImageTransformationToFitThePage(behavior.mainWindowViewModel.CurrentPage, (BitmapImage)e.NewValue);
                behavior.AssociatedObject.Source = (ImageSource)e.NewValue;

            }
        }


        /// <summary>
        /// трансформация изображения под размер страницы
        /// </summary>
        /// <param name="page">страница</param>
        /// <param name="bitmap">изображение</param>
        /// <exception cref="Exception"></exception>
        private void ImageTransformationToFitThePage(Page page, BitmapImage bitmap)
        {
            try
            {
                //трансформируем изображение под размер страницы
                var height = page.ActualHeight;
                var width = page.ActualWidth;
                var imgHeight = bitmap.Height;
                var imgWidth = bitmap.Width;
                double scaleX = (double)width / imgWidth;
                double scaleY = (double)height / imgHeight;
                double scale = Math.Min(scaleX, scaleY);
                scale = Math.Min(scaleX, scaleY);

                AssociatedObject.LayoutTransform = new ScaleTransform(scale, scale);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
