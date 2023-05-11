using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        internal BitmapImage Source { get => (BitmapImage)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }
        public static readonly DependencyProperty SourceProperty;

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

            SourceProperty = DependencyProperty.Register("Source", typeof(BitmapImage), typeof(BehaviorImage),
                new PropertyMetadata(new PropertyChangedCallback(SourceChanged)));
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


        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if(d is BehaviorImage behavior && e.NewValue is BitmapImage bitmap)
                {
                    behavior.AssociatedObject.Source = bitmap;
                    //трансформируем изображение под размер страницы
                    var page = behavior.mainWindowViewModel.CurrentPage;
                    page.SizeChanged += (s,e) => { behavior.ImageTransformationToFitThePage(page, bitmap); };
                    behavior.ImageTransformationToFitThePage(page, bitmap);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// трансформация изображения под размер страницы
        /// </summary>
        /// <param name="page">страница</param>
        /// <param name="bitmap">изображение</param>
        /// <exception cref="Exception"></exception>
        private void ImageTransformationToFitThePage( Page page, BitmapImage bitmap )
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

                AssociatedObject.LayoutTransform = new ScaleTransform(scale, scale);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
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
                if (sender is Image img)
                {
                    double zoom = e.Delta > 0 ? .2 : -.2; // определяем направление масштабирования
                    double newScale = Math.Min(Math.Max(img.LayoutTransform.Value.M11 + zoom, .1), 10); // ограничиваем масштабирование
                    img.LayoutTransform = new ScaleTransform(newScale, newScale, 0, 0);
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
                KeyboardFocus();
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

        /// <summary>
        /// возврат фокуса клавиатуры(костыль!)
        /// </summary>
        private void KeyboardFocus()
        {
            var keyEvent = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual((ScrollViewer)AssociatedObject.Parent), 0, Key.Tab);
            keyEvent.RoutedEvent = Keyboard.KeyDownEvent;
            InputManager.Current.ProcessInput(keyEvent);
            keyEvent.RoutedEvent = Keyboard.KeyUpEvent;
            InputManager.Current.ProcessInput(keyEvent);
        }


    }
}
