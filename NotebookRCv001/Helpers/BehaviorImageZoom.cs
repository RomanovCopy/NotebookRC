﻿using System;
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
        private bool isDragging { get; set; }
        private bool isZoom { get; set; }
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
            isDragging = false;
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
            if (sender is Image image && image.Parent is ScrollViewer scroll)
            {
                var step = 0.1; // Размер шага масштабирования
                scale += step * (e.Delta > 0 ? 1 : -1); // Применяем масштаб
                image.LayoutTransform = new ScaleTransform(scale, scale);
            }
        }
        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {

            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastMousePosition = e.GetPosition(AssociatedObject);
            isDragging = true;
        }


        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BehaviorImageZoom behavior)
            {
                behavior.ImageTransformationToFitThePage(behavior.mainWindowViewModel.CurrentPage, (BitmapImage)e.NewValue);
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
                scale = Math.Min(scaleX, scaleY);
                AssociatedObject.LayoutTransform = new ScaleTransform(scale, scale);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
