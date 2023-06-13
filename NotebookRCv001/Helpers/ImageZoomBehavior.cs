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

            var scale = e.Delta > 0 ? 1.1 : 0.9; // Изменение масштаба (увеличение или уменьшение)
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
            _lastMousePosition = position;
            _isMouseDragging = true;
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

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDragging = false;
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
