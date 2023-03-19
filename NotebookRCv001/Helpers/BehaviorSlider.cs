using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

namespace NotebookRCv001.Helpers
{
    public class BehaviorSlider:Behavior<Slider>
    {

        #region *************** Private properties




        #endregion



        #region ***************** Public properties ***********************

        public Slider GetSlider => AssociatedObject; 

        public double Minimum { get => AssociatedObject.Minimum; set => AssociatedObject.Value = value; }
        public double Maximum { get => AssociatedObject.Maximum; set => AssociatedObject.Maximum = value; }


        #endregion

        #region ********************** Dependency properties ************************ 

        internal double Value { get => (double)GetValue( ValueProperty ); set => SetValue( ValueProperty, value ); }
        public static readonly DependencyProperty ValueProperty;

        #endregion


        #region *********************Construktors*********************



        static BehaviorSlider()
        {
            ValueProperty = DependencyProperty.Register( "Value", typeof( double ), typeof( BehaviorSlider ),
                new PropertyMetadata( new PropertyChangedCallback( ValueChanged ) ) );
        }


        public BehaviorSlider()
        {

        }

        protected override void OnAttached()
        {
        }

        protected override void OnDetaching()
        {

        }


        #endregion


        #region ***************** Events ***********************************


        #endregion

        #region ************************** Event handlers ***************************

        private static void ValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {

            }
            catch { }
        }

        #endregion

        #region **************** Public methods ******************************


        #endregion


        #region *********************** Private Methods *****************************



        #endregion


    }
}
