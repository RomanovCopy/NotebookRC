using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace NotebookRCv001.Helpers
{
    public class ImageZoomBehavior : Behavior<Image>
    {
        private Point _lastMousePosition;
        private bool _isMouseDragging;
        private double scale = 1.0; // Глобальная переменная для масштаба

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseWheel += Image_MouseWheel;
            AssociatedObject.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            //AssociatedObject.MouseMove += Image_MouseMove;
            AssociatedObject.MouseLeftButtonUp += Image_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseWheel -= Image_MouseWheel;
            AssociatedObject.MouseLeftButtonDown -= Image_MouseLeftButtonDown;
            //AssociatedObject.MouseMove -= Image_MouseMove;
            AssociatedObject.MouseLeftButtonUp -= Image_MouseLeftButtonUp;
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var image = sender as Image;
            var transform = image.RenderTransform;

            ScaleTransform scaleTransform = transform as ScaleTransform;

            if (scaleTransform == null)
            {
                // Если RenderTransform не является экземпляром ScaleTransform, создаем новый экземпляр
                scaleTransform = new ScaleTransform();
                image.RenderTransform = scaleTransform;
            }

            var zoomIncrement = 0.2;
            var zoomDirection = e.Delta > 0 ? 1 : -1;

            var newScale = scaleTransform.ScaleX + zoomIncrement * zoomDirection;

            // Ограничиваем масштабирование в допустимых пределах
            var minScale = 0.2;
            var maxScale = 5.0;
            if (newScale < minScale)
                newScale = minScale;
            else if (newScale > maxScale)
                newScale = maxScale;

            // Применяем новый масштаб
            scaleTransform.ScaleX = newScale;
            scaleTransform.ScaleY = newScale;

            // Центрируем изображение в пределах контейнера
            CenterImage(image);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.CaptureMouse();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDragging)
            {
                var position = e.GetPosition(AssociatedObject);

                var transform = AssociatedObject.RenderTransform as MatrixTransform;
                var matrix = transform.Matrix;

                var offsetX = position.X - _lastMousePosition.X;
                var offsetY = position.Y - _lastMousePosition.Y;

                matrix.Translate(offsetX, offsetY);

                transform.Matrix = matrix;

                _lastMousePosition = position;
            }
        }

        private void CenterImage()
        {
            var transform = AssociatedObject.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;

            var centerOfViewport = new Point(AssociatedObject.ActualWidth / 2, AssociatedObject.ActualHeight / 2);
            var centerOfImage = transform.Transform(centerOfViewport);

            var offsetX = centerOfViewport.X - centerOfImage.X;
            var offsetY = centerOfViewport.Y - centerOfImage.Y;

            matrix.Translate(offsetX, offsetY);

            transform.Matrix = matrix;
        }

        private void CenterImage(Image image)
        {
            var container = VisualTreeHelper.GetParent(image) as FrameworkElement;
            var scaleTransform = image.RenderTransform as ScaleTransform;

            var imageWidth = image.ActualWidth * scaleTransform.ScaleX;
            var imageHeight = image.ActualHeight * scaleTransform.ScaleY;

            var offsetX = (container.ActualWidth - imageWidth) / 2;
            var offsetY = (container.ActualHeight - imageHeight) / 2;
            if (offsetX < 0 || offsetY < 0)
                return;
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            image.RenderTransform = new ScaleTransform(scaleTransform.ScaleX, scaleTransform.ScaleY, offsetX, offsetY);
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.ReleaseMouseCapture();
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageZoomBehavior),
                new PropertyMetadata(null, OnSourceChanged));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ImageZoomBehavior)d;
            behavior.AssociatedObject.Source = (ImageSource)e.NewValue;
        }
    }
}
