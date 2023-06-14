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
    public class ImageZoomBehavior : Behavior<Image>
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



        static ImageZoomBehavior()
        {
            SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource),
                typeof(ImageZoomBehavior), new PropertyMetadata(null, OnSourceChanged));
        }

        public ImageZoomBehavior()
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

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var transform = AssociatedObject.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;

            var step = 0.1; // Размер шага масштабирования
            scale += step * (e.Delta > 0 ? 1 : -1); // Изменение масштаба

            var position = e.GetPosition(AssociatedObject);
            var newMatrix = Matrix.Identity;
            newMatrix.ScaleAt(scale, scale, position.X, position.Y);
            newMatrix.OffsetX = matrix.OffsetX;
            newMatrix.OffsetY = matrix.OffsetY;

            AssociatedObject.RenderTransform = new MatrixTransform(newMatrix);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(AssociatedObject);
            lastMousePosition = position;
            isMouseDragging = true;
            AssociatedObject.CaptureMouse();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDragging)
            {
                var position = e.GetPosition(AssociatedObject);

                var transform = AssociatedObject.RenderTransform as MatrixTransform;
                var matrix = transform.Matrix;

                var offsetX = position.X - lastMousePosition.X;
                var offsetY = position.Y - lastMousePosition.Y;

                matrix.Translate(offsetX, offsetY);

                transform.Matrix = matrix;

                lastMousePosition = position;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDragging = false;
            AssociatedObject.ReleaseMouseCapture();
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageZoomBehavior behavior)
            {
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
