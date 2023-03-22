using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using Microsoft.Xaml.Behaviors;

namespace NotebookRCv001.Helpers
{
    public class BehaviorSlider:Behavior<Slider>
    {

        #region *************** Private properties




        #endregion



        #region ***************** Public properties ***********************


        public Slider Slider => AssociatedObject;
        public double Minimum { get => AssociatedObject.Minimum; set => AssociatedObject.Value = value; }
        public double Maximum { get => AssociatedObject.Maximum; set => AssociatedObject.Maximum = value; }
        public double Value { get => AssociatedObject.Value; set => AssociatedObject.Value = value; }


        #endregion

        #region ********************** Dependency properties ************************ 


        #endregion


        #region *********************Construktors*********************



        static BehaviorSlider()
        {
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


        #endregion

        #region **************** Public methods ******************************


        #endregion


        #region *********************** Private Methods *****************************



        #endregion


    }
}
