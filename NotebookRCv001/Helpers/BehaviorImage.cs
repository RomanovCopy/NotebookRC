using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Microsoft.Xaml.Behaviors;

using NotebookRCv001.MyControls;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Helpers
{
    public class BehaviorImage : Behavior<Image>
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        private MediaPlayerViewModel mediaPlayerViewModel { get; set; }
        private bool isDragging { get; set; }
        private Point startPoint { get; set; }

        internal Point MousePosition { get => (Point)GetValue(MousePositionProperty); set => SetValue(MousePositionProperty, value); }
        public static readonly DependencyProperty MousePositionProperty;

        public BehaviorImage()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            mainWindowViewModel.FrameList.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems[0] is NotebookRCv001.MyControls.MediaPlayer player)
                {
                    mediaPlayerViewModel = (MediaPlayerViewModel)player.DataContext;
                }
            };
            isDragging = false;
        }

        static BehaviorImage()
        {
            MousePositionProperty = DependencyProperty.Register("MousePosition", typeof(Point), typeof(BehaviorImage),
                new PropertyMetadata(new PropertyChangedCallback(MousePositionChanged)));

        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            AssociatedObject.MouseUp += AssociatedObject_MouseUp;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
        }

        protected override void OnDetaching()
        {
        }



        /// <summary>
        /// изменение положения указателя мыши             
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private static void MousePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        /// <summary>
        /// прокручивание колесика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void AssociatedObject_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {
                if (sender is Image image)
                {
                    ScrollViewer scrollViewer = (ScrollViewer)image.Parent;
                    double zoom = e.Delta > 0 ? .2 : -.2; // определяем направление масштабирования
                    double newScale = Math.Min(Math.Max(image.LayoutTransform.Value.M11 + zoom, .1), 10); // ограничиваем масштабирование
                    scrollViewer.RenderTransformOrigin = e.GetPosition(image);
                    ScaleTransform scaleTransform = new ScaleTransform(newScale, newScale);
                    image.LayoutTransform = scaleTransform;
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// перемещение указателя мыши над изображением                           
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                if (sender is Image img)
                {
                    //MousePosition = e.GetPosition(img);
                    if (isDragging)
                    {
                        Point currentPoint = e.GetPosition(null);
                        double diffX = startPoint.X - currentPoint.X;
                        double diffY = startPoint.Y - currentPoint.Y;

                        ScrollViewer scrollViewer = img.Parent as ScrollViewer;
                        scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + diffX);
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + diffY);

                        startPoint = currentPoint;
                    }
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void AssociatedObject_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                isDragging = false;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                startPoint = e.GetPosition(null);
                isDragging = true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


    }
}
