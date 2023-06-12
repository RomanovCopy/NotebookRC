using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

using Microsoft.Xaml.Behaviors;

using NotebookRCv001.MyControls;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Helpers
{
    public class BehaviorImage : Behavior<Image>
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public Image Image => AssociatedObject;
        private ScrollViewer scrollViewer { get; set; }

        private bool isDragging { get; set; }
        private Point startPoint { get; set; }

        internal Point MousePosition { get => (Point)GetValue(MousePositionProperty); set => SetValue(MousePositionProperty, value); }
        public static readonly DependencyProperty MousePositionProperty;

        internal BitmapImage Source { get => (BitmapImage)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }
        public static readonly DependencyProperty SourceProperty;

        internal Tuple<double, double> Scale { 
            get => (Tuple<double, double>)GetValue(ScaleProperty); 
            set => SetValue(ScaleProperty, value); }
        public static readonly DependencyProperty ScaleProperty;

        public BehaviorImage()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            mainWindowViewModel.FrameList.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems[0] is NotebookRCv001.MyControls.MediaPlayer player)
                {

                }
            };
            isDragging = false;
            Scale = new Tuple<double, double>(1.0, 1.0);
        }
        static BehaviorImage()
        {
            MousePositionProperty = DependencyProperty.Register("MousePosition", typeof(Point), typeof(BehaviorImage),
                new PropertyMetadata(new PropertyChangedCallback(MousePositionChanged)));

            SourceProperty = DependencyProperty.Register("Source", typeof(BitmapImage), typeof(BehaviorImage),
                new PropertyMetadata(SourceChanged));

            ScaleProperty = DependencyProperty.Register("Scale", typeof(Tuple<double, double>), typeof(BehaviorImage),
                new PropertyMetadata(ScaleChanged));
        }


        protected override void OnAttached()
        {
        }

        protected override void OnDetaching()
        {
        }


        public event RoutedEventHandler Loaded
        {
            add
            {
                AssociatedObject.Loaded += value;
            }
            remove
            {
                AssociatedObject.Loaded -= value;
            }
        }
        public event MouseEventHandler MouseMove
        {
            add
            {
                AssociatedObject.MouseMove += value;
            }
            remove
            {
                AssociatedObject.MouseMove -= value;
            }
        }
        public event MouseButtonEventHandler MouseDown
        {
            add
            {
                AssociatedObject.MouseDown += value;
            }
            remove
            {
                AssociatedObject.MouseDown -= value;
            }
        }
        public event MouseButtonEventHandler MouseUp
        {
            add
            {
                AssociatedObject.MouseUp += value;
            }
            remove
            {
                AssociatedObject.MouseUp -= value;
            }
        }
        public event MouseWheelEventHandler MouseWheel
        {
            add
            {
                AssociatedObject.MouseWheel += value;
            }
            remove
            {
                AssociatedObject.MouseWheel -= value;
            }
        }



        /// <summary>
        /// изменение источника             
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (d is BehaviorImage behavior && e.NewValue is BitmapImage bitmap)
                {
                    behavior.AssociatedObject.Source = bitmap;
                    var page = behavior.mainWindowViewModel.CurrentPage;
                    //трансформация изображения при изменении размеров страницы
                    page.SizeChanged += (s, e) => { behavior.ImageTransformationToFitThePage(page, bitmap); };
                    //начальная трансформация изображения под размер страницы
                    behavior.ImageTransformationToFitThePage(page, bitmap);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// изменение положения указателя мыши             
        /// </summary>
        /// <param name = "d" ></ param >
        /// < param name="e"></param>
        /// <exception cref = "Exception" ></ exception >
        private static void MousePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// изменение коэффициента масштабирования
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private static void ScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if(d is BehaviorImage behavior && behavior.AssociatedObject!=null)
                {
                    //var position = behavior.MousePosition;
                    var tuple = (Tuple<double, double>)e.NewValue;
                    var scale = Math.Min(tuple.Item1, tuple.Item2);

                    //var transformGroup = new TransformGroup();
                    //transformGroup.Children.Add(new TranslateTransform(0, 0));
                    //var scaleTransform = new ScaleTransform(scale, scale);
                    //var element = (Image)behavior.AssociatedObject;// объект, на котором происходит масштабирование
                    //var container = (ScrollViewer)element.Parent;// родительский контейнер объекта
                    //var transform = element.TransformToVisual(container);
                    //var position = transform.Transform(new Point(element.ActualWidth / 2, element.ActualHeight / 2));
                    //transformGroup.Children.Add(new ScaleTransform(scale, scale, position.X, position.Y));

                    var transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new TranslateTransform(0, 0));
                    //transformGroup.Children.Add(new ScaleTransform(scale, scale, position.X, position.Y));
                    if (behavior.AssociatedObject != null)
                        behavior.AssociatedObject.RenderTransform = transformGroup;
                }
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

                double zoom = e.Delta > 0 ? 0.1 : -0.1;
                var scale = Math.Min(Scale.Item1, Scale.Item2);
                scale += zoom;
                if (scale <= 5 && scale >= 0.5)
                {
                    //mediaPlayerViewModel.ScaleX = 1.1;
                    //mediaPlayerViewModel.ScaleY = 1.1;
                    //var position = e.GetPosition(image);
                    //var transformGroup = new TransformGroup();
                    //transformGroup.Children.Add(new TranslateTransform(0, 0));
                    //transformGroup.Children.Add(new ScaleTransform(scale, scale, position.X, position.Y));
                    //image.RenderTransform = transformGroup;
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
                //if (isDragging)
                //{
                //    if (image.RenderTransform is TransformGroup transform)
                //    {
                //        TranslateTransform translateTransform = null;
                //        foreach (var t in transform.Children)
                //        {
                //            if (t is TranslateTransform tt)
                //            {
                //                translateTransform = tt;
                //                break;
                //            }
                //        }
                //        if (translateTransform != null)
                //        {
                //            Point currentPoint = e.GetPosition(image);
                //            double diffX = currentPoint.X - startPoint.X;
                //            double diffY = currentPoint.Y - startPoint.Y;
                //            //scrollViewer.ScrollToVerticalOffset(diffY);
                //            //scrollViewer.ScrollToHorizontalOffset(diffX);
                //            translateTransform.X += diffX;
                //            translateTransform.Y += diffY;
                //            startPoint = currentPoint;
                //        }
                //    }
                //}

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private void AssociatedObject_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                //if (isDragging)
                //{
                //    var currentPoint = e.GetPosition(scrollViewer);
                //    double diffX = currentPoint.X - startPoint.X;
                //    double diffY = currentPoint.Y - startPoint.Y;
                //    scrollViewer.ScrollToVerticalOffset(diffY);
                //    scrollViewer.ScrollToHorizontalOffset(diffX);
                //}
                isDragging = false;
                KeyboardFocus();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                startPoint = e.GetPosition(scrollViewer);
                isDragging = true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void UpdateImagePosition()
        {
            //if (image.ActualWidth > scrollViewer.ViewportWidth)
            //{
            //    double horizontalOffset = (image.ActualWidth - scrollViewer.ViewportWidth) / 2;
            //    scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
            //}

            //if (image.ActualHeight > scrollViewer.ViewportHeight)
            //{
            //    double verticalOffset = (image.ActualHeight - scrollViewer.ViewportHeight) / 2;
            //    scrollViewer.ScrollToVerticalOffset(verticalOffset);
            //}
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
                Scale = new Tuple<double, double>(scale, scale);

                AssociatedObject.LayoutTransform = new ScaleTransform(Scale.Item1, Scale.Item2);
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
