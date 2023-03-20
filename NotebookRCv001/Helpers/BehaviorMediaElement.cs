﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using Microsoft.Xaml.Behaviors;

using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Helpers
{
    public class BehaviorMediaElement : Behavior<MediaElement>
    {

        #region **************** Private properties *************************

        private readonly MainWindowViewModel mainWindowViewModel;

        #endregion

        #region **************** Public properties ************************

        public MediaElement MediaElement => AssociatedObject;


        #endregion

        #region *************** public methods ********************

        public void Play()
        {
            try
            {
                AssociatedObject.Play();
            }
            catch { }
        }

        public void Pause()
        {
            try
            {
                AssociatedObject.Pause();
            }
            catch { }
        }

        public void Stop()
        {
            try
            {
                AssociatedObject.Stop();
            }
            catch { }
        }

        #endregion

        #region ******************* Dependency properties ***************

        internal TimeSpan Position { get => (TimeSpan)GetValue( PositionProperty ); set => SetValue( PositionProperty, value ); }
        public static readonly DependencyProperty PositionProperty;

        #endregion

        #region ****************** Constructors ***************************

        public BehaviorMediaElement()
        {
        }

        static BehaviorMediaElement()
        {
            PositionProperty = DependencyProperty.Register( "Position", typeof( TimeSpan ),
                typeof( BehaviorMediaElement ), new PropertyMetadata( new PropertyChangedCallback( PositionChanged ) ) );
        }

        protected override void OnAttached()
        {

        }

        protected override void OnDetaching()
        {

        }


        #endregion


        #region ***************** Events *****************************

        public event RoutedEventHandler BuferingStarted
        {
            add
            {
                AssociatedObject.BufferingStarted += value;
            }
            remove
            {
                AssociatedObject.BufferingStarted -= value;
            }
        }

        public event RoutedEventHandler BuferingEnded
        {
            add
            {
                base.AssociatedObject.BufferingEnded += value;
            }
            remove
            {
                base.AssociatedObject.BufferingEnded -= value;
            }
        }

        #endregion


        #region *************** Event handlers *********************


        private static void PositionChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            if(d is BehaviorMediaElement behavior)
            {
                behavior.MediaElement.Position = (TimeSpan)e.NewValue;
            }
        }


        private void AssociatedObject_MediaFailed( object sender, ExceptionRoutedEventArgs e )
        {
        }


        private void AssociatedObject_MediaEnded( object sender, RoutedEventArgs e )
        {

        }


        private void AssociatedObject_MediaOpened( object sender, RoutedEventArgs e )
        {
            if(sender is MediaElement mediaElement)
            {
                
            }
        }


        #endregion







    }
}
