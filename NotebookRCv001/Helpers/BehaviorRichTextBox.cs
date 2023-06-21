using Microsoft.Xaml.Behaviors;

using NotebookRCv001.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Drawing = System.Drawing;

namespace NotebookRCv001.Helpers
{
    public class BehaviorRichTextBox : Behavior<RichTextBox>
    {

        #region __________"Public Properties"__________


        internal RichTextBox RichTextBox => base.AssociatedObject;

        private RichTextBoxViewModel richTextBoxViewModel => (RichTextBoxViewModel)RichTextBox.DataContext;

        internal TextRange TextRange
        {
            get
            {
                if (Selection == null)
                {
                    if (Document != null)
                        return new TextRange( Document.ContentStart, Document.ContentEnd );
                    else
                        return null;
                }
                else
                {
                    if (Selection.IsEmpty)
                        return new TextRange( Document.ContentStart, Document.ContentEnd );
                    else
                        return new TextRange( Selection.Start, Selection.End );
                }
            }
        }

        internal TextSelection Selection => base.AssociatedObject?.Selection;

        /// <summary>
        /// элемент над которым открыто контекстное меню ContextMenuImage
        /// </summary>
        internal object SelectedImage { get => selectedImage; private set => selectedImage = value; }
        private object selectedImage;

        /// <summary>
        /// цель для вставки контента из вне
        /// </summary>
        internal object TargetToInsert { get => targetToInsert; set => targetToInsert = value; }
        private object targetToInsert;

        /// <summary>
        /// активация/деактивация элемента в контроле RichTextBox
        /// </summary>
        public Visibility Visibility { get => AssociatedObject.Visibility; set => AssociatedObject.Visibility = value; }

        internal Converters.ColorToColorConverter Converter => converter ??= new Converters.ColorToColorConverter();
        Converters.ColorToColorConverter converter;

        /// <summary>
        /// текст изменен
        /// </summary>
        internal bool OriginalTextChanged
        {
            get => originalTextChanged;
            set => originalTextChanged = value;
        }
        bool originalTextChanged;

        /// <summary>
        /// массив поддерживаемых служебных клавиш
        /// </summary>
        internal Key[] ServiceKeys
        {
            get => serviceKeys;
            set => serviceKeys = value;
        }
        Key[] serviceKeys;

        /// <summary>
        /// текущая служебная клавиша
        /// </summary>
        internal Key ServiceKey
        {
            get => serviceKey;
            set => serviceKey = value;
        }
        Key serviceKey;

        internal Figure CurrentFigure { get => currentFigure; set => currentFigure = value; }
        private Figure currentFigure;
        /// <summary>
        /// начальный размер
        /// </summary>
        internal double RealWidth { get => realWidth; set => realWidth = value; }
        private double realWidth;
        internal double RealHeight { get => realHeight; set => realHeight = value; }
        private double realHeight;
        /// <summary>
        /// измененный размер
        /// </summary>
        internal double ChangedWidth { get => changedWidth; set => changedWidth = value; }
        private double changedWidth;
        internal double ChangedHeight { get => changedHeight; set => changedHeight = value; }
        private double changedHeight;

        #endregion

        #region ______________Private Properties_______________

        private MainWindowViewModel mainWindowViewModel { get; set; }
        private HomeViewModel homeViewModel { get; set; }
        private bool newDocumentCreated { get; set; }
        private FigureHorizontalAnchor figureHorizontalAnchor { get; set; }
        private bool merge { get; set; }
        private FigureVerticalAnchor figureVerticalAnchor { get; set; }
        private Rotation rotation { get; set; }
        private Size size { get; set; }
        private Stretch stretch { get; set; }
        private WrapDirection wrapDirection { get; set; }
        private bool first { get; set; }

        #endregion

        #region __________"Dependency Properties"__________


        internal FlowDocument Document { get => (FlowDocument)GetValue( DocumentProperty ); set => SetValue( DocumentProperty, value ); }
        public static readonly DependencyProperty DocumentProperty;

        internal BitmapImage Bitmap { get => (BitmapImage)GetValue( BitmapProperty ); set => SetValue( BitmapProperty, value ); }
        public static readonly DependencyProperty BitmapProperty;

        /// <summary>
        /// источник загрузки изображения(либо путь к файлу(string), либо Source изображения (BitmapSource))
        /// </summary>
        internal object ImagePath
        {
            get => GetValue( ImagePathProperty );
            set => SetValue( ImagePathProperty, value );
        }
        public static readonly DependencyProperty ImagePathProperty;

        internal object ImageOptions
        {
            get => (object[])GetValue( ImageOptionsProperty );
            set => SetValue( ImageOptionsProperty, value );
        }
        public static readonly DependencyProperty ImageOptionsProperty;

        internal TextPointer CaretPosition
        {
            get => (TextPointer)GetValue( CaretPositionProperty );
            set => SetValue( CaretPositionProperty, value );
        }
        public static readonly DependencyProperty CaretPositionProperty;

        internal FontFamily FontFamily
        {
            get => (FontFamily)GetValue( FontFamilyProperty );
            set
            {
                if (Selection.IsEmpty)
                    SetValue( FontFamilyProperty, value );
                else
                    Selection.ApplyPropertyValue( TextElement.FontFamilyProperty, value );
            }
        }
        public static readonly DependencyProperty FontFamilyProperty;

        internal double FontSize
        {
            get => (double)GetValue( FontSizeProperty );
            set
            {
                if (Selection.IsEmpty)
                    SetValue( FontSizeProperty, value );
                else
                    Selection.ApplyPropertyValue( TextElement.FontSizeProperty, value );
            }
        }
        public static readonly DependencyProperty FontSizeProperty;

        internal FontWeight FontWeight
        {
            get => (FontWeight)GetValue( FontWeightProperty );
            set
            {
                if (Selection.IsEmpty)
                    SetValue( FontWeightProperty, value );
                else
                    Selection.ApplyPropertyValue( TextElement.FontWeightProperty, value );
            }
        }
        public static readonly DependencyProperty FontWeightProperty;

        internal FontStyle FontStyle
        {
            get => (FontStyle)GetValue( FontStyleProperty );
            set
            {
                if (Selection.IsEmpty)
                    SetValue( FontStyleProperty, value );
                else
                    Selection.ApplyPropertyValue( TextElement.FontStyleProperty, value );
            }
        }
        public static readonly DependencyProperty FontStyleProperty;

        internal TextDecorationCollection TextDecoration
        {
            get => (TextDecorationCollection)GetValue( TextDecorationsProperty );
            set
            {
                if (Selection.IsEmpty)
                    SetValue( TextDecorationsProperty, value );
                else
                    Selection.ApplyPropertyValue( Inline.TextDecorationsProperty, value );
            }
        }
        public static readonly DependencyProperty TextDecorationsProperty;

        /// <summary>
        /// межстрочный интервал
        /// </summary>
        internal Thickness LineSpacing
        {
            get => (Thickness)GetValue( LineSpacingProperty );
            set => SetValue( LineSpacingProperty, value );
        }
        public static readonly DependencyProperty LineSpacingProperty;

        internal Brush MyForeground
        {
            get => (Brush)GetValue( MyForegroundProperty );
            set
            {
                if (Selection.IsEmpty)
                    SetValue( MyForegroundProperty, value );
                else
                    Selection.ApplyPropertyValue( TextElement.ForegroundProperty, value );
            }
        }
        public static readonly DependencyProperty MyForegroundProperty;

        internal Brush MyBackground
        {
            get => (Brush)GetValue( MyBackgroundProperty );
            set
            {
                if (Selection != null && Selection.IsEmpty)
                    SetValue( MyBackgroundProperty, value );
                else if (Selection != null)
                    Selection.ApplyPropertyValue( Control.BackgroundProperty, value );
            }
        }
        public static readonly DependencyProperty MyBackgroundProperty;

        internal Brush MyFontBackground
        {
            get => (Brush)GetValue( MyFontBackgroundProperty );
            set
            {
                if (Selection != null && Selection.IsEmpty)
                    SetValue( MyFontBackgroundProperty, value );
                else if (Selection != null)
                    Selection.ApplyPropertyValue( TextElement.BackgroundProperty, value );
            }
        }
        public static readonly DependencyProperty MyFontBackgroundProperty;


        /// <summary>
        /// цвет выделения BorderBrush блоков документа 
        /// </summary>
        internal Brush MyHighlightColor
        {
            get => (Brush)GetValue( MyHighlightColorProperty );
            set => SetValue( MyHighlightColorProperty, value );
        }
        public static readonly DependencyProperty MyHighlightColorProperty;



        #endregion

        #region __________"Constructors"___________


        public BehaviorRichTextBox()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            MyHighlightColor = (Brush)Converter.Convert( Properties.Settings.Default.MyHighlightColor, typeof( Brush ), null, null );
            first = true;
        }

        static BehaviorRichTextBox()
        {
            DocumentProperty = DependencyProperty.Register( "Document", typeof( FlowDocument ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( DocumentChanged ) ) );
            BitmapProperty = DependencyProperty.Register( "Bitmap", typeof( BitmapImage ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( BitmapChanged ) ) );
            ImagePathProperty = DependencyProperty.Register( "ImagePath", typeof( object ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( ImagePathChanged ) ) );
            ImageOptionsProperty = DependencyProperty.Register( "ImageOptions", typeof( object ),
                typeof( BehaviorRichTextBox ), new PropertyMetadata( new PropertyChangedCallback( ImageOptionsChanged ) ) );
            CaretPositionProperty = DependencyProperty.Register( "CaretPosition", typeof( TextPointer ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( CaretPositionChanged ) ) );

            FontFamilyProperty = DependencyProperty.Register( "FontFamily", typeof( FontFamily ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( FontFamilyChanged ) ) );

            FontSizeProperty = DependencyProperty.Register( "FontSize", typeof( double ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( FontSizeChanged ) ) );

            FontWeightProperty = DependencyProperty.Register( "FontWeight", typeof( FontWeight ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( default( FontWeight ), new PropertyChangedCallback( FontWeightChanged ) ) );

            FontStyleProperty = DependencyProperty.Register( "FontStyle", typeof( FontStyle ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( FontStyleChanged ) ) );

            TextDecorationsProperty = DependencyProperty.Register( "TextDecoration", typeof( TextDecorationCollection ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( TextDecorationChanged ) ) );

            LineSpacingProperty = DependencyProperty.Register( "LineSpacing", typeof( Thickness ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( default( Thickness ), new PropertyChangedCallback( LineSpacingChanged ) ) );
            //цвет текста
            MyForegroundProperty = DependencyProperty.Register( "MyForeground", typeof( Brush ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( MyForegroundChanged ) ) );
            //цвет фона текста
            MyBackgroundProperty = DependencyProperty.Register( "MyBackground", typeof( Brush ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( MyBackgroundChanged ) ) );
            //цвет бумаги
            MyFontBackgroundProperty = DependencyProperty.Register( "MyFontBackground", typeof( Brush ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( MyFontBackgroundChanged ) ) );
            //цвет выделения BorderBrush блоков документа
            MyHighlightColorProperty = DependencyProperty.Register( "MyHighlightColor", typeof( Brush ),
                typeof( BehaviorRichTextBox ),
                new PropertyMetadata( new PropertyChangedCallback( MyHighlightColorChanged ) ) );

        }


        protected override void OnAttached()
        {
            try
            {
                Document = AssociatedObject.Document;
                Document.IsHyphenationEnabled = true;
                CaretPosition = Document.ContentEnd;
                Document.IsOptimalParagraphEnabled = true;
                Document.IsHyphenationEnabled = true;
                FontFamily = new FontFamily( Properties.Settings.Default.FontFamilyName );
                FontSize = Properties.Settings.Default.FontSize;
                FontWeight = Properties.Settings.Default.FontWeightName == "Bold" ? FontWeights.Bold : FontWeights.Normal;
                FontStyle = Properties.Settings.Default.FontStyleName == "Italic" ? FontStyles.Italic : FontStyles.Normal;
                var array = Properties.Settings.Default.LineSpacing.Split( ',' );
                LineSpacing = new Thickness( double.Parse( array[0] ), double.Parse( array[1] ), double.Parse( array[2] ), double.Parse( array[3] ) );
                MyBackground = (Brush)Converter.Convert( Properties.Settings.Default.MyBackground, typeof( Brush ), null, null );
                MyFontBackground = (Brush)Converter.Convert( Properties.Settings.Default.MyFontBackground, typeof( Brush ), null, null );
                MyForeground = (Brush)Converter.Convert( Properties.Settings.Default.MyForeground, typeof( Brush ), null, null );
                SelectionChanged += BehaviorRichTextBox_SelectionChanged;
                AssociatedObject.Padding = new Thickness( 30 );
                AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
                AssociatedObject.ContextMenuOpening += AssociatedObject_ContextMenuOpening;
                AssociatedObject.ContextMenuClosing += AssociatedObject_ContextMenuClosing;
                TextChanged += BehaviorRichTextBox_TextChanged;
                PreviewKeyDown += BehaviorRichTextBox_PreviewKeyDown;
                ServiceKeys = new Key[] { Key.Enter, Key.Back, Key.Delete, Key.Insert, Key.Return };
                OriginalTextChanged = false;
                if(Document.Blocks.FirstBlock is Paragraph paragraph)
                {//задаем отступ первого знака от края документа
                    paragraph.TextIndent = 12;
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }


        protected override void OnDetaching()
        {
            try
            {

            }
            catch (Exception e)
            {
                ErrorWindow( e );
            }
        }

        #endregion

        #region ___________Events_______________

        public event MouseButtonEventHandler PreviewMouseButtonDoubleClick
        {
            add => base.AssociatedObject.PreviewMouseDoubleClick += value;
            remove => base.AssociatedObject.PreviewMouseDoubleClick -= value;
        }
        public event MouseButtonEventHandler PreviewMouseLeftButtonDown
        {
            add => RichTextBox.PreviewMouseLeftButtonDown += value;
            remove => RichTextBox.PreviewMouseLeftButtonDown -= value;
        }
        public event RoutedEventHandler Loaded
        {
            add => base.AssociatedObject.Loaded += value;
            remove => base.AssociatedObject.Loaded -= value;
        }
        public event RoutedEventHandler Unloaded
        {
            add => base.AssociatedObject.Unloaded += value;
            remove => base.AssociatedObject.Unloaded -= value;
        }
        public event RoutedEventHandler SelectionChanged
        {
            add => base.AssociatedObject.SelectionChanged += value;
            remove => base.AssociatedObject.SelectionChanged -= value;
        }
        public event TextChangedEventHandler TextChanged
        {
            add => base.AssociatedObject.TextChanged += value;
            remove => base.AssociatedObject.TextChanged -= value;
        }
        public event KeyEventHandler PreviewKeyDown
        {
            add => RichTextBox.PreviewKeyDown += value;
            remove => RichTextBox.PreviewKeyDown -= value;
        }
        public event KeyEventHandler PreviewKeyUp
        {
            add => RichTextBox.PreviewKeyUp += value;
            remove => RichTextBox.PreviewKeyUp -= value;
        }
        public event TextCompositionEventHandler PreviewTextInput
        {
            add => RichTextBox.PreviewTextInput += value;
            remove => RichTextBox.PreviewTextInput -= value;
        }
        public event EventHandler Selection_Changed
        {
            add => base.AssociatedObject.Selection.Changed += value;
            remove => base.AssociatedObject.Selection.Changed -= value;
        }
        public event DragEventHandler DragOver
        {
            add => AssociatedObject.DragOver += value;
            remove => AssociatedObject.DragOver -= value;
        }
        public event DragEventHandler Drop
        {
            add => AssociatedObject.Drop += value;
            remove => AssociatedObject.Drop -= value;
        }


        #endregion

        #region __________"Event Handlers"_________________




        private static void DocumentChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox behavior && e.OldValue != null)
                {
                    behavior.newDocumentCreated = !e.NewValue.Equals( e.OldValue );
                    behavior.AssociatedObject.Document = behavior.Document;
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private void AssociatedObject_IsVisibleChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            try
            {

            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void BitmapChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (e.NewValue is BitmapImage bitmap && d is BehaviorRichTextBox behavior)
                {
                    behavior.richTextBoxViewModel.OnPropertyChanged( "Bitmap" );
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void ImagePathChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                Image image = null;
                if (d is BehaviorRichTextBox behavior)
                {
                    if (e.NewValue is string path)
                    {
                        image = behavior.CreateImageFromPath( path );
                    }
                    else if (e.NewValue is BitmapSource source)
                    {
                        image = behavior.CreateImageFromBitmapSource( source );
                    }
                    if (image == null)
                        return;
                    Figure figure = behavior.CreateFigure( image );
                    behavior.AddingFigureToFlowdocument( figure );
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void ImageOptionsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox behavior)
                {
                    if (e.NewValue is FigureHorizontalAnchor horizontalAnchor)
                    {//выравнивание по горизонтали
                        behavior.figureHorizontalAnchor = horizontalAnchor;
                    }
                    else if (e.NewValue is string option && option.Contains( "Merge" ))
                    {//склеивание изображений
                        behavior.merge = option == "MergeOn";
                    }
                    else if (e.NewValue is FigureVerticalAnchor verticalAnchor)
                    {//выравнивание по вертикали
                        behavior.figureVerticalAnchor = verticalAnchor;
                    }
                    else if (e.NewValue is Rotation rotation)
                    {//поворот изображения
                        behavior.rotation = rotation;
                    }
                    else if (e.NewValue is Size size)
                    {//размер изображения
                        if (!size.IsEmpty && size != new Size( 0, 0 ))
                        {
                            behavior.ChangedWidth = size.Width;
                            behavior.ChangedHeight = size.Height;
                            behavior.size = new Size( behavior.ChangedWidth, behavior.ChangedHeight );
                        }
                        else
                        {
                            behavior.size = size;
                        }
                    }
                    else if (e.NewValue is Stretch stretch)
                    {
                        behavior.stretch = stretch;
                    }
                    else if (e.NewValue is WrapDirection wrap)
                    {
                        behavior.wrapDirection = wrap;
                    }
                    if (behavior.Bitmap != null)
                    {
                        var image = behavior.ImagePath is string ? behavior.CreateImageFromPath( (string)behavior.ImagePath ) :
                            behavior.CreateImageFromBitmapSource( (BitmapSource)behavior.ImagePath );
                        var parent = (Paragraph)behavior.CurrentFigure.Parent;
                        parent.Inlines.Remove( behavior.CurrentFigure );
                        behavior.CurrentFigure = behavior.CreateFigure( image );
                        var a = behavior.CurrentFigure.WrapDirection;
                        parent.Inlines.Add( behavior.CurrentFigure );
                        //if (behavior.wrapDirection == WrapDirection.Both)
                        //    parent.Inlines.Add(new Run(""));
                    }
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void CaretPositionChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox behavior && behavior.OriginalTextChanged)
                {//текст изменён
                    behavior.AssociatedObject.Focus();
                    if (behavior.newDocumentCreated)
                    {//создан новый документ
                        behavior.newDocumentCreated = false;
                        return;
                    }
                    if (e.NewValue is TextPointer newvalue && e.OldValue is TextPointer oldvalue)
                    {
                        TextRange textRange = null;
                        //когда CaretPosition, oldValue и newValue находятся в одной позиции
                        bool exception = (oldvalue.CompareTo( newvalue ) == 0 && behavior.CaretPosition.CompareTo( newvalue ) == 0);
                        //длина добавленного текста
                        int lengthText = new TextRange( oldvalue, newvalue ).Text.Trim().Length;
                        if (behavior.ServiceKey != Key.None || lengthText > 1)
                        {//задейсвована сервисная клавиша
                            if (behavior.ServiceKey == Key.Back)
                            {//удаление текста

                            }
                            else if (behavior.ServiceKey == Key.Enter)
                            {//перевод строки
                                //отмена сервисной клавиши, иначе произойдет
                                //зацикливание на этом событии
                                //после установки нового значения для CaretPosition
                                behavior.ServiceKey = Key.None;
                                //устанавливаем каретку в начало строки
                                behavior.CaretPosition = newvalue.GetLineStartPosition( 0 );
                            }
                            else if (behavior.ServiceKey == Key.Insert || lengthText > 1)
                            {//вставка текста

                            }
                            else if (behavior.ServiceKey == Key.Delete)
                            {
                                behavior.OnDeleteText();
                            }
                            //отмена сервисной клавиши
                            behavior.ServiceKey = Key.None;
                        }
                        else if (behavior.ServiceKey == Key.None && lengthText == 1)
                        {//ввод текста с клавиатуры
                            behavior.RichTextBox.Focus();
                            textRange = new TextRange( oldvalue, newvalue );
                        }
                        else if (exception && behavior.OriginalTextChanged)
                        {//когда CaretPosition, oldValue и newValue находятся в одной позиции
                            if (!behavior.Selection.IsEmpty)
                            {//при наличии выделения ничего не делаем, все выполнят сеттеры свойств зависимостей

                            }
                            else
                            {//при отсутсвии выделения
                                var back = newvalue.GetPositionAtOffset( -1 );
                                var forward = newvalue.GetPositionAtOffset( 1 );
                                var CaretPosition = behavior.CaretPosition;
                                behavior.OriginalTextChanged = false;
                                if (back != null)
                                {//позиция сзади существует
                                    behavior.RichTextBox.Focus();
                                    textRange = new TextRange( back, CaretPosition );
                                }
                                else if (forward != null)
                                {//позиция спереди существует
                                    behavior.RichTextBox.Focus();
                                    textRange = new TextRange( CaretPosition, forward );
                                }
                                else if (back == null && forward == null)
                                {//позиции сзади и спереди не существуют
                                    //расчет на то, что CaretPosition не может быть null
                                    //ищем позицию сзади
                                    back = CaretPosition.GetPositionAtOffset( -1 );
                                    if (back != null)
                                        textRange = new TextRange( back, CaretPosition );
                                    else
                                    {   //ищем позицию спереди, если позицию сзади установить не удалось
                                        forward = CaretPosition.GetPositionAtOffset( 1 );
                                        if (forward != null)
                                            textRange = new TextRange( CaretPosition, forward );
                                        else
                                        {   //крайняя мера
                                            //принудитльно задаем позищию сзади(начало строки) и получаем TextRange
                                            back = CaretPosition.GetLineStartPosition( 0 );
                                            if (back != null)
                                                textRange = new TextRange( back, CaretPosition );
                                        }
                                    }
                                }
                            }
                        }
                        if (textRange != null)
                            behavior.SetFontProperties( textRange );
                        else
                        {
                            textRange = new TextRange( behavior.Document.ContentStart, behavior.Document.ContentEnd );
                            if (textRange != null && behavior.first)
                                behavior.SetFontProperties( textRange );
                            behavior.first = false;
                        }
                    }
                    behavior.OriginalTextChanged = false;
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void FontFamilyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox && !textBox.Selection.IsEmpty)
                {
                    textBox.Selection.ApplyPropertyValue( TextElement.FontFamilyProperty, textBox.FontFamily );
                    textBox.OriginalTextChanged = false;
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void FontSizeChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox)
                {
                    if (!textBox.Selection.IsEmpty)
                    {
                        textBox.Selection.ApplyPropertyValue( TextElement.FontSizeProperty, textBox.FontSize * (96 / 72) );
                    }
                    textBox.OriginalTextChanged = false;
                    textBox.SetFontProperties( new TextRange( textBox.Selection.Start, textBox.Selection.End ) );
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void FontWeightChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox && !textBox.Selection.IsEmpty)
                {
                    textBox.Selection.ApplyPropertyValue( TextElement.FontWeightProperty, textBox.FontWeight );
                    textBox.OriginalTextChanged = false;
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void FontStyleChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox && !textBox.Selection.IsEmpty)
                {
                    textBox.Selection.ApplyPropertyValue( TextElement.FontStyleProperty, textBox.FontStyle );
                    textBox.OriginalTextChanged = false;
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void TextDecorationChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox && !textBox.Selection.IsEmpty)
                {
                    textBox.Selection.ApplyPropertyValue( Inline.TextDecorationsProperty, textBox.TextDecoration );
                    textBox.OriginalTextChanged = false;
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void LineSpacingChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textbox && e.NewValue is Thickness thickness && !textbox.Selection.IsEmpty)
                {
                    textbox.Selection.ApplyPropertyValue( Block.MarginProperty, thickness );
                    textbox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void MyBackgroundChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox)
                {
                    textBox.RichTextBox.Background = textBox.MyBackground;
                    var a1 = textBox.MyBackground.ToString();
                    var a2 = Brushes.Transparent.ToString();
                    Application.Current.MainWindow.Topmost = a1 == a2;
                    textBox.richTextBoxViewModel.OnPropertyChanged( "MyBackground" );
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void MyForegroundChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox)
                {
                    if (!textBox.Selection.IsEmpty)
                    {
                        textBox.Selection.ApplyPropertyValue( TextElement.ForegroundProperty, textBox.MyForeground );
                    }
                    else
                    {
                        new TextRange( textBox.CaretPosition, textBox.CaretPosition ).ApplyPropertyValue( TextElement.ForegroundProperty, textBox.MyForeground );
                        textBox.RichTextBox.CaretBrush = textBox.MyForeground;
                        textBox.richTextBoxViewModel.OnPropertyChanged( "MyForeground" );
                    }
                    textBox.OriginalTextChanged = false;
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private static void MyFontBackgroundChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox)
                {
                    textBox.Selection.ApplyPropertyValue( TextElement.BackgroundProperty, textBox.MyFontBackground );
                    textBox.OriginalTextChanged = false;
                    textBox.richTextBoxViewModel.OnPropertyChanged( "MyFontBackground" );
                    textBox.RichTextBox.Focus();
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }

        /// <summary>
        /// обработка изменения цвета выделения BorderBrush блоков документа
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void MyHighlightColorChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                if (d is BehaviorRichTextBox textBox && textBox.Document != null)
                {
                    foreach (var block in textBox.Document.Blocks)
                    {
                        if (block.BorderBrush != null && block.BorderBrush == (Brush)e.OldValue)
                        {
                            block.BorderBrush = (Brush)e.NewValue;
                        }
                    }
                    Properties.Settings.Default.MyHighlightColor = (System.Drawing.Color)textBox.Converter.
                        Convert( e.NewValue, typeof( System.Drawing.Color ), null, null );
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }

        private void BehaviorRichTextBox_TextChanged( object sender, TextChangedEventArgs e )
        {
            try
            {
                if (sender is RichTextBox textBox)
                {
                    OriginalTextChanged = true;//устанавливаем флаг изменения текста
                    CaretPosition = textBox.CaretPosition;
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private void BehaviorRichTextBox_SelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if (sender is RichTextBox textBox)
                {
                    CaretPosition = textBox.CaretPosition;
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private void BehaviorRichTextBox_PreviewKeyDown( object sender, KeyEventArgs e )
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    var richTextBox = (RichTextBox)sender;
                    var caretPosition = richTextBox.CaretPosition;

                    // Вставка символа новой строки
                    caretPosition.InsertLineBreak();

                    // Перевод курсора в начало добавленной строки
                    var lineStartPosition = caretPosition.GetLineStartPosition(1);
                    if (lineStartPosition != null)
                        richTextBox.CaretPosition = lineStartPosition;

                    e.Handled = true;
                }
                if (e.Key == Key.Delete)
                {
                    e.Handled = true;
                    return;
                }
                else if (ServiceKeys.Any( ( x ) => x == e.Key ))
                    ServiceKey = e.Key;
                else
                    ServiceKey = Key.None;
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private void AssociatedObject_ContextMenuOpening( object sender, ContextMenuEventArgs e )
        {
            try
            {//контекстное меню открыто, определяем над каким элементом
                //для разных типов элементов, контекстное меню будет разным
                if (e.Source is System.Windows.Controls.Image image)
                {//над изображением
                    SelectedImage = e.Source;
                }
                else
                {//над другими элементами
                    SelectedImage = null;
                }
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        private void AssociatedObject_ContextMenuClosing( object sender, ContextMenuEventArgs e )
        {
            try
            {

            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }

        #endregion

        #region __________"Public Methods"__________

        internal void OnLineSpacingBottomDecrease()
        {
            try
            {
                OnLineSpacingBottomIncrease();
                LineSpacing = new Thickness( LineSpacing.Left, LineSpacing.Top, LineSpacing.Right, (LineSpacing.Bottom > 1 ? LineSpacing.Bottom - 2 : 0) );
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        internal void OnLineSpacingBottomIncrease()
        {
            try
            {
                LineSpacing = new Thickness( LineSpacing.Left, LineSpacing.Top, LineSpacing.Right, (LineSpacing.Bottom + 1) );
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        internal void OnNormalText()
        {
            try
            {
                if (Selection.IsEmpty)
                    return;
                var textRange = new TextRange( Selection.Start, Selection.End );
                List<string> list = new();
                string[] array1 = textRange.Text.Split( new char[] { '\r', '\n' } );
                for (int i = 0; i < array1.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace( array1[i] ))
                        continue;
                    list.Add( (string)array1[i].Trim().Clone() );
                }
                string result = "";
                foreach (string str in list)
                {
                    result = string.IsNullOrWhiteSpace( result ) ? str + "\r\n" : result + str + "\r\n";
                }
                StringBuilder buffer = new StringBuilder( textRange.Text );
                buffer.Replace( textRange.Text, result );
                textRange.Text = buffer.ToString();
                FontFamily = new FontFamily( Properties.Settings.Default.NormalFontFamily );
                var array = Properties.Settings.Default.NormalLineSpacing.Split( ',' );
                LineSpacing = (Thickness)textRange.GetPropertyValue( Block.MarginProperty );
                LineSpacing = new Thickness( double.Parse( array[0] ), double.Parse( array[1] ), double.Parse( array[2] ), double.Parse( array[3] ) );
                FontSize = Properties.Settings.Default.NormalFontSize;
                FontStyle = Properties.Settings.Default.NormalFontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal;
                FontWeight = Properties.Settings.Default.NormalFontWeight == "Bold" ? FontWeights.Bold : FontWeights.Normal;
                TextDecoration = Properties.Settings.Default.NormalTextDecoration == "Underline" ? TextDecorations.Underline : null;
                textRange.ApplyPropertyValue( TextElement.ForegroundProperty, MyForeground );
                textRange.ApplyPropertyValue( TextElement.BackgroundProperty, converter.Convert( Properties.Settings.Default.NormalFontBackground,
                    typeof( System.Windows.Media.Brush ), null, null ) );
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        internal void OnDeleteText()
        {
            try
            {
                TextRange textRange = new TextRange( Selection.Start, Selection.End );
                textRange.Text = "";
                TextRange.ApplyPropertyValue( Block.MarginProperty, new Thickness( 5, 0, 0, 0 ) );
            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }
        internal void SetFocus()
        {
            AssociatedObject.Focus();
        }
        internal void AppendText( string text )
        {
            base.AssociatedObject.AppendText( text );
        }
        internal void AppendText( List<string> doc )
        {
            try
            {
                FlowDocument flowDocument = new FlowDocument();
                Paragraph paragraph = null;
                foreach (string str in doc)
                {
                    paragraph = new Paragraph( new Run( str ) );
                    flowDocument.Blocks.Add( paragraph );
                }
                Document = flowDocument;
            }
            catch (Exception e) { throw new Exception( e.Message ); }
        }
        /// <summary>
        /// преобразование в маркированный список
        /// </summary>
        internal void OnToggleBullets()
        {
            try
            {
                if (!Selection.IsEmpty)

                    EditingCommands.ToggleBullets.Execute( null, RichTextBox );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// преобразование в нумерованный список
        /// </summary>
        internal void OnToggleNumbering()
        {
            try
            {
                if (!Selection.IsEmpty)
                    EditingCommands.ToggleNumbering.Execute( null, RichTextBox );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// ContextMenuImage: Copy
        /// </summary>
        /// <returns></returns>
        internal bool SelectedImageCopy()
        {
            try
            {
                bool c = false;
                if (SelectedImage is Image image)
                {
                    Clipboard.SetImage( (BitmapSource)image.Source );
                }
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        /// <summary>
        /// Вырезать выбранное (CurrentElement) изображение в буфер обмена
        /// </summary>
        /// <returns></returns>
        internal bool SelectedImageCut()
        {
            try
            {
                Image image = null;
                if (SelectedImage is Image im)
                {
                    image = im;
                    var source = (BitmapSource)image.Source;
                    Clipboard.SetImage( source );
                }
                if (image == null)
                    return false;
                ApplicationCommands.Copy.Execute( SelectedImage, null );
                var parent = (Paragraph)((InlineUIContainer)image.Parent).Parent;
                if (parent != null && parent.Parent is Figure figure)
                {
                    var parentfigure = (Paragraph)figure.Parent;
                    if (parentfigure != null)
                        parentfigure.Inlines.Remove( figure );
                }
                return true;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        /// <summary>
        /// удаление выбранного изображения (SelectedImage)
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal bool SelectedImageDelete()
        {
            try
            {
                bool c = false;
                if (SelectedImage is Image image)
                {
                    if (Document.Blocks.Where( ( x ) => x.Equals( ParagraphSearchByImage( image ) ) ).FirstOrDefault() is Paragraph paragraph)
                    {
                        if (FigureSearchByImage( image ) is Figure figure)
                            c = paragraph.Inlines.Remove( figure );
                    }
                }
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }

        /// <summary>
        /// Вставка изображения из буфера обмена
        /// </summary>
        /// <returns></returns>
        internal bool PasteImageFromClipboard( object obj )
        {
            try
            {
                bool c = false;
                BitmapSource source = Clipboard.GetImage();
                if (source == null)
                    return false;
                var image = CreateImageFromBitmapSource( source );
                if (image == null)
                    return false;
                var figure = CreateFigure( image );
                var paragraph = ParagraphSearchByImage( (Image)SelectedImage );
                var targetfigure = FigureSearchByImage( (Image)SelectedImage );
                if ((string)obj == "before")
                    paragraph.Inlines.InsertBefore( targetfigure, figure );
                else if ((string)obj == "after")
                    paragraph.Inlines.InsertAfter( targetfigure, figure );
                else
                    return false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }

        public FlowDocument CloneDocument()
        {
            try
            {
                var document2 = new FlowDocument();
                var document = AssociatedObject.Document;
                if (document != null)
                {
                    TextRange range = new TextRange( document.ContentStart, document.ContentEnd );
                    MemoryStream stream = new MemoryStream();
                    System.Windows.Markup.XamlWriter.Save( range, stream );
                    range.Save( stream, DataFormats.XamlPackage );
                    TextRange range2 = new TextRange( document2.ContentEnd, document2.ContentEnd );
                    range2.Load( stream, DataFormats.XamlPackage );
                    stream.Close();
                }
                return document2;
            }
            catch (Exception e) { ErrorWindow( e ); return new FlowDocument(); }
        }

        #endregion

        #region __________"Private Methods"__________

        private SelectAndPasteWindowViewModel GetSelectAndPasteWindowViewModel()
        {
            try
            {
                SelectAndPasteWindowViewModel viewmodel = null;
                foreach (var win in Application.Current.Windows)
                {
                    if (win != null && win is Views.SelectAndPasteWindow window)
                        viewmodel = (SelectAndPasteWindowViewModel)window.DataContext;
                }
                return viewmodel;
            }
            catch { return null; }
        }

        /// <summary>
        /// создание нового параграфа, переход в его первую строку с отступом от края в 3 символа
        /// </summary>
        internal void NewParagraph()
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Margin = new Thickness(36, 0, 0, 0); // Отступ слева в 3 знака (30 пикселей)

            // Добавьте пустой Run, чтобы создать новую строку
            paragraph.Inlines.Add(new Run(Environment.NewLine));

            // Добавьте новый параграф в FlowDocument
            Document.Blocks.Add(paragraph);

            // Установите курсор в начало новой строки
            TextPointer newPosition = paragraph.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
            AssociatedObject.CaretPosition = newPosition;
        }


        /// <summary>
        /// установка свойств текущего символа
        /// </summary>
        /// <param name="textRange"></param>
        internal void SetFontProperties( TextRange textRange )
        {
            try
            {
                textRange.ApplyPropertyValue( TextElement.FontFamilyProperty, FontFamily );
                textRange.ApplyPropertyValue( TextElement.FontSizeProperty, (FontSize * (96 / 72)) );
                textRange.ApplyPropertyValue( TextElement.FontWeightProperty, FontWeight );
                textRange.ApplyPropertyValue( TextElement.FontStyleProperty, FontStyle );
                textRange.ApplyPropertyValue( Inline.TextDecorationsProperty, TextDecoration );
                textRange.ApplyPropertyValue( TextElement.ForegroundProperty, MyForeground );
                textRange.ApplyPropertyValue( TextElement.BackgroundProperty, MyFontBackground );
                textRange.ApplyPropertyValue( Block.MarginProperty, LineSpacing );

            }
            catch (Exception ex) { ErrorWindow( ex ); }
        }

        private Image CreateImageFromPath( string path )
        {
            try
            {
                if (string.IsNullOrEmpty( path ))
                    return null;
                var select = GetSelectAndPasteWindowViewModel();
                Image image = null;
                var bitmap = CreateBitmapImageFromPath( path );
                if (size.IsEmpty || size == new Size( 0, 0 ))
                {
                    RealWidth = bitmap.PixelWidth;
                    RealHeight = bitmap.PixelHeight;
                    ChangedWidth = bitmap.PixelWidth;
                    ChangedHeight = bitmap.PixelHeight;
                    select.SizeChanged.Execute( "width" );
                    select.SizeChanged.Execute( "height" );

                }
                Bitmap = bitmap;
                image = new();
                image.Source = bitmap;
                image.Loaded += ( s, e ) =>
                {
                    if (s is Image im)
                    {
                        if (im.IsLoaded)
                        {
                            im.Stretch = stretch;
                        }
                    }
                };
                return image;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private Image CreateImageFromBitmapSource( BitmapSource source )
        {
            try
            {
                Image image = new();
                image.Source = source;
                image.Loaded += ( s, e ) =>
                { image.Width = ChangedWidth; image.Height = ChangedHeight; };
                return image;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private Image CreateImageFromBitmap( Drawing.Bitmap bitmap )
        {
            try
            {
                Image image = new();
                BitmapImage bitmapImage = null;
                using (MemoryStream ms = new())
                {
                    bitmap.Save( ms, Drawing.Imaging.ImageFormat.Jpeg );
                    ms.Seek( 0, SeekOrigin.Begin );
                    bitmapImage = new();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    if (!size.IsEmpty)
                    {
                        bitmapImage.DecodePixelWidth = (int)size.Width;
                        bitmapImage.DecodePixelHeight = (int)size.Height;
                    }
                    bitmapImage.Rotation = rotation;
                    bitmapImage.EndInit();
                    if (size.IsEmpty || size == new Size( 0, 0 ))
                    {
                        RealWidth = bitmapImage.PixelWidth;
                        RealHeight = bitmapImage.PixelHeight;
                        ChangedWidth = bitmapImage.PixelWidth;
                        ChangedHeight = bitmapImage.PixelHeight;
                    }
                    image.Source = bitmapImage;
                    image.Loaded += ( s, e ) =>
                    {
                        if (image.IsLoaded)
                            image.Stretch = stretch;
                    };
                }
                return image;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private Drawing.Bitmap CreateBitmapFromImage( Image image )
        {
            try
            {
                Drawing.Bitmap bitmap = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    JpegBitmapEncoder encoder = new();
                    encoder.Frames.Add( BitmapFrame.Create( (BitmapSource)image.Source ) );
                    encoder.Save( ms );
                    using (Drawing.Bitmap bmp = new( ms ))
                    {
                        bitmap = new Drawing.Bitmap( bmp );
                    }
                }
                return bitmap;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private BitmapImage CreateBitmapImageFromImage( Image image )
        {
            try
            {
                var bitmap = CreateBitmapFromImage( image );
                BitmapImage bitmapImage = new();
                var select = GetSelectAndPasteWindowViewModel();
                using (MemoryStream ms = new())
                {
                    bitmap.Save( ms, System.Drawing.Imaging.ImageFormat.Jpeg );
                    ms.Seek( 0, SeekOrigin.Begin );
                    ms.Position = 0;

                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.UriSource = null;
                    bitmapImage.StreamSource = ms;
                    if (!size.IsEmpty && size != new Size( 0, 0 ))
                    {
                        bitmapImage.DecodePixelWidth = (int)size.Width;
                        bitmapImage.DecodePixelHeight = (int)size.Height;
                    }
                    bitmapImage.Rotation = rotation;
                    bitmapImage.EndInit();
                    if (size.IsEmpty || size == new Size( 0, 0 ))
                    {
                        RealWidth = bitmapImage.PixelWidth;
                        RealHeight = bitmapImage.PixelHeight;
                        ChangedWidth = bitmapImage.PixelWidth;
                        ChangedHeight = bitmapImage.PixelHeight;
                        select?.SizeChanged.Execute( "width" );
                        select?.SizeChanged.Execute( "height" );
                    }
                }
                return bitmapImage;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private BitmapImage CreateBitmapImageFromPath( string path )
        {
            try
            {
                BitmapImage bitmapImage = new();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri( path );
                bitmapImage.Rotation = rotation;
                if (!size.IsEmpty && size != new Size( 0, 0 ))
                {
                    if (!GetSelectAndPasteWindowViewModel().Proportionally)
                        bitmapImage.DecodePixelWidth = (int)size.Width;
                    bitmapImage.DecodePixelHeight = (int)size.Height;
                }
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }
        private Drawing.Bitmap CreateBitmapFromPath( string path )
        {
            try
            {
                Drawing.Bitmap bitmap = null;
                bitmap = new Drawing.Bitmap( path );
                return bitmap;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private Figure CreateFigure( Image image )
        {
            try
            {
                //BlockUIContainer blockUIContainer = new();
                //Figure figure = new();
                //blockUIContainer.Child = image;
                //figure.Width = new FigureLength(size.Width);
                //figure.Height = new FigureLength(size.Height);
                InlineUIContainer container = new();
                container.Child = image;
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add( container );
                Figure figure = new Figure();
                figure.VerticalAnchor = FigureVerticalAnchor.PageTop;
                figure.WrapDirection = wrapDirection;
                figure.HorizontalAnchor = figureHorizontalAnchor;
                figure.Blocks.Add( paragraph );
                //figure.Blocks.Add(blockUIContainer);
                return figure;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private Figure FigureSearchByImage( Image image )
        {
            try
            {
                Paragraph paragraph = null;
                InlineUIContainer container = null;
                Figure figure = null;
                if (image != null)
                {
                    container = (InlineUIContainer)image.Parent;
                    if (container != null)
                    {
                        paragraph = (Paragraph)container.Parent;
                        if (paragraph != null)
                        {
                            figure = (Figure)paragraph.Parent;
                        }
                    }
                }
                return figure;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private Paragraph ParagraphSearchByImage( Image image )
        {
            try
            {
                Paragraph paragraph = null;
                InlineUIContainer container = null;
                Paragraph paragraph1 = null;
                Figure figure = null;
                if (image != null)
                {
                    container = (InlineUIContainer)image.Parent;
                    if (container != null)
                    {
                        paragraph1 = (Paragraph)container.Parent;
                        if (paragraph1 != null)
                        {
                            figure = (Figure)paragraph1.Parent;
                            if (figure != null)
                                paragraph = (Paragraph)figure.Parent;
                        }
                    }
                }
                return paragraph;
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private void AddingFigureToFlowdocument( Figure figure )
        {
            try
            {
                Paragraph paragraph = null;
                if (TargetToInsert == null)
                {
                    paragraph = new();
                    CurrentFigure = figure;
                    paragraph.Inlines.Add( figure );
                    Document.Blocks.Add( paragraph );
                }
                else
                {
                    if (TargetToInsert is Paragraph par)
                    {
                        paragraph = par;
                        paragraph.Inlines.Add( figure );
                        CurrentFigure = figure;
                    }
                }
                RichTextBox.Focus();
                CaretPosition = paragraph.ContentEnd;
                Document.Blocks.Add( new Paragraph() );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        private static void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new Thread( () => MessageBox.Show( e.Message, $"BehaviorRichTextBox.{name}" ) );
            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
        }




        #endregion

    }
}
