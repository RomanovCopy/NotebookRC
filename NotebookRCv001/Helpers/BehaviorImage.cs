using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

using Microsoft.Xaml.Behaviors;

namespace NotebookRCv001.Helpers
{
    public class BehaviorImage: Behavior<Image>
    {
        internal Point MousePosition { get => (Point)GetValue(MousePositionProperty); set => SetValue(MousePositionProperty, value); }
        public static readonly DependencyProperty MousePositionProperty;

        public BehaviorImage()
        {

        }

        static BehaviorImage()
        {
            MousePositionProperty=DependencyProperty.Register("MousePosition", typeof(Point), typeof(BehaviorImage),
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


        private static void MousePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private void AssociatedObject_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void AssociatedObject_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }



    }
}
