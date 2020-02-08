using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace GenerationsLib.WPF.Controls
{
    public class PopupDecorator : AdornerDecorator
    {
        #region Dependency Properties

        public static readonly DependencyProperty CustomPopupPlacementCallbackProperty;
        public static readonly DependencyProperty HorizontalOffsetProperty;
        public static readonly DependencyProperty PlacementProperty;
        public static readonly DependencyProperty PlacementRectangleProperty;
        public static readonly DependencyProperty PlacementTargetProperty;
        //public static readonly DependencyProperty PopupAnimationProperty;
        public static readonly DependencyProperty RepositionWhenPopupResizesProperty;
        public static readonly DependencyProperty VerticalOffsetProperty;
        public static readonly DependencyProperty IsOpenProperty;

        #endregion

        #region Static Constructor

        static PopupDecorator()
        {
            CustomPopupPlacementCallbackProperty = DependencyProperty.Register(
                "CustomPopupPlacementCallback",
                typeof(CustomPopupPlacementCallback),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(null)
            );

            HorizontalOffsetProperty = DependencyProperty.Register(
                "HorizontalOffset",
                typeof(double),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(
                    0.0,
                    new PropertyChangedCallback(PopupDecorator.OnOffsetChanged)
                )
            );

            VerticalOffsetProperty = DependencyProperty.Register(
                "VerticalOffset",
                typeof(double),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(
                    0.0,
                    new PropertyChangedCallback(PopupDecorator.OnOffsetChanged)
                )
            );

            PlacementProperty = DependencyProperty.Register(
                "Placement",
                typeof(PlacementMode),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(
                    PlacementMode.Bottom,
                    new PropertyChangedCallback(PopupDecorator.OnPlacementChanged)
                ),
                new ValidateValueCallback(PopupDecorator.IsValidPlacementMode)
            );

            PlacementTargetProperty = DependencyProperty.Register(
                "PlacementTarget",
                typeof(UIElement),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(
                    null,
                    new PropertyChangedCallback(PopupDecorator.OnPlacementTargetChanged)
                )
            );

            PlacementRectangleProperty = DependencyProperty.Register(
                "PlacementRectangle",
                typeof(Rect),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(
                    Rect.Empty,
                    new PropertyChangedCallback(PopupDecorator.OnOffsetChanged)
                )
            );

            //PopupAnimationProperty = DependencyProperty.Register("PopupAnimation", typeof(PopupAnimation), typeof(PopupDecorator), new FrameworkPropertyMetadata(PopupAnimation.None, null, new CoerceValueCallback(PopupDecorator.CoercePopupAnimation)), new ValidateValueCallback(PopupDecorator.IsValidPopupAnimation));            

            RepositionWhenPopupResizesProperty = DependencyProperty.Register(
                "RepositionWhenPopupResizes",
                typeof(bool),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(false)
            );

            IsOpenProperty = DependencyProperty.Register(
                "IsOpen",
                typeof(bool),
                typeof(PopupDecorator),
                new FrameworkPropertyMetadata(
                    false,
                    new PropertyChangedCallback(IsOpenChanged)
                )
            );
        }

        #endregion

        #region Static Private Methods

        static private void OnOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((PopupDecorator)obj).Reposition();
        }

        static private void OnPlacementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((PopupDecorator)obj).Reposition();
        }

        static private bool IsValidPlacementMode(object value)
        {
            PlacementMode mode = (PlacementMode)value;
            if (
                    (
                        (
                            (
                                (mode != PlacementMode.Absolute) &&
                                (mode != PlacementMode.AbsolutePoint)
                             ) &&
                             (
                                (mode != PlacementMode.Bottom) &&
                                (mode != PlacementMode.Center)
                             )
                        ) &&
                        (
                            (
                                (mode != PlacementMode.Mouse) &&
                                (mode != PlacementMode.MousePoint)
                            ) &&
                            (
                                (mode != PlacementMode.Relative) &&
                                (mode != PlacementMode.RelativePoint)
                            )
                        )
                    ) &&
                    (
                        (
                            (mode != PlacementMode.Right) &&
                            (mode != PlacementMode.Left)
                        ) && (mode != PlacementMode.Top)
                    )
                )
            {
                return (mode == PlacementMode.Custom);
            }
            return true;
        }

        static private void IsOpenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            PopupDecorator pd = obj as PopupDecorator;
            if (pd != null)
            {
                if (!(bool)e.OldValue && (bool)e.NewValue)
                    pd.Open();
                else if ((bool)e.OldValue && !(bool)e.NewValue)
                    pd.Close();
            }
        }

        static private void OnPlacementTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((PopupDecorator)obj).Reposition();
        }

        /// <summary>
        /// Determines the bounding rectangle given 
        /// an array of interest points.
        /// </summary>        
        static private Rect GetBounds(Point[] interestPoints)
        {
            double right;
            double bottom;
            double x = right = interestPoints[0].X;
            double y = bottom = interestPoints[0].Y;
            for (int i = 1; i < interestPoints.Length; i++)
            {
                double tempx = interestPoints[i].X;
                double tempy = interestPoints[i].Y;
                if (tempx < x)
                {
                    x = tempx;
                }
                if (tempx > right)
                {
                    right = tempx;
                }
                if (tempy < y)
                {
                    y = tempy;
                }
                if (tempy > bottom)
                {
                    bottom = tempy;
                }
            }
            return new Rect(x, y, right - x, bottom - y);
        }

        /// <summary>
        /// Returns the number of placement combinations available
        /// given a PlacementMode.
        /// </summary>
        private static int GetNumberOfCombinations(PlacementMode placement)
        {
            switch (placement)
            {
                case PlacementMode.Bottom:
                case PlacementMode.Mouse:
                case PlacementMode.Top:
                    return 4;

                case PlacementMode.Right:
                case PlacementMode.AbsolutePoint:
                case PlacementMode.RelativePoint:
                case PlacementMode.MousePoint:
                case PlacementMode.Left:
                    return 4;

                case PlacementMode.Custom:
                    return 0;
            }
            return 1;
        }

        /// <summary>
        /// Swaps two different points.
        /// </summary>        
        private static void SwapPoints(ref Point p1, ref Point p2)
        {
            Point point = p1;
            p1 = p2;
            p2 = point;
        }

        #endregion

        #region Public Events

        public event EventHandler Opened;
        public event EventHandler Closed;

        #endregion

        #region Private Fields

        private UIElement _Popup = null;
        private UIElementAdorner _PopupAdorner = null;
        private SizeChangedEventHandler _PopupSizeChangedEventHandler;
        private bool _IsRightPositioned = false;
        private bool _IsBottomPositioned = false;
        private PointCombination _CurrentPointCombination;

        #endregion

        #region Public Properties

        virtual public UIElement Popup
        {
            get
            {
                return this._Popup;
            }
            set
            {
                if (this._Popup != value)
                {
                    // First close the popup, if we're changing the
                    // contents of the popup.
                    if (IsOpen)
                        Close();

                    if (this._Popup != null)
                        base.RemoveLogicalChild(this._Popup);

                    _Popup = value;
                    if (_Popup != null)
                    {
                        base.AddLogicalChild(value);

                        // Then, if the popup was displayed, re-open
                        // the popup with the new contents
                        if (IsOpen)
                            Open();
                    }

                    // Remeasure our object
                    InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not the popup is currently open.
        /// You can also call the Open() and Close() methods to show/hide
        /// the popup, respectively.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return ((bool)(base.GetValue(PopupDecorator.IsOpenProperty)));
            }
            set
            {
                SetValue(PopupDecorator.IsOpenProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a delegate handler method that positions the
        /// Popup control.
        /// 
        /// Return Value
        /// The CustomPopupPlacementCallback delegate method that provides
        /// placement information for the Popup control. The default value
        /// is null.
        /// </summary>
        [Category("Layout"), Bindable(false)]
        public CustomPopupPlacementCallback CustomPopupPlacementCallback
        {
            get { return (CustomPopupPlacementCallback)base.GetValue(CustomPopupPlacementCallbackProperty); }
            set { base.SetValue(CustomPopupPlacementCallbackProperty, value); }
        }

        /// <summary>
        /// Gets or sets the offset from the left of the area that is
        /// specified for the Popup control content by a combination of
        /// the Placement, PlacementTarget, and PlacementRectangle properties. 
        /// 
        /// Return Value
        /// An offset from the left of the area that is specified for the
        /// Popup control by a combination of the Placement, PlacementTarget,
        /// and PlacementRectangle properties. The default value is 0.0.
        /// </summary>
        [TypeConverter(typeof(LengthConverter)), Bindable(true), Category("Layout")]
        public double HorizontalOffset
        {
            get { return (double)base.GetValue(HorizontalOffsetProperty); }
            set { base.SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation of the Popup control when the control
        /// opens, and specifies the behavior of the Popup control when it
        /// overlaps screen boundaries.
        /// 
        /// Return Value
        /// A PlacementMode enumeration value that determines the orientation
        /// of the Popup control when the control opens, and that specifies
        /// how the control interacts with screen boundaries. The default value
        /// is Bottom. 
        /// </summary>
        [Category("Layout"), Bindable(true)]
        public PlacementMode Placement
        {
            get { return (PlacementMode)base.GetValue(PlacementProperty); }
            set { base.SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rectangle relative to which the Popup control
        /// is positioned when it opens.
        /// 
        /// Return Value
        /// The rectangle that is used to position the Popup control. The
        /// default value is null.
        /// 
        /// <remarks>
        /// The PlacementRectangle property specifies a rectangle relative
        /// to which the Popup control is positioned when it opens. If the
        /// PlacementTarget property is not a null reference (Nothing in
        /// Visual Basic), the rectangle is defined relative to the
        /// PlacementTarget object. Otherwise, the specified rectangle is
        /// defined relative to the PopupDecorator position.
        /// 
        /// If the Placement property is set to Mouse, the PlacementRectangle
        /// property has no effect because the placement of the Popup is
        /// with respect to the mouse pointer.
        /// </remarks>
        /// </summary>
        [Category("Layout"), Bindable(true)]
        public Rect PlacementRectangle
        {
            get { return (Rect)base.GetValue(PlacementRectangleProperty); }
            set { base.SetValue(PlacementRectangleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element relative to which the Popup is
        /// positioned when it opens.
        /// 
        /// Return Value
        /// The UIElement that is the logical parent of the Popup control.
        /// The default value is null.
        /// </summary>
        [Bindable(true), Category("Layout"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UIElement PlacementTarget
        {
            get { return (UIElement)base.GetValue(PlacementTargetProperty); }
            set { base.SetValue(PlacementTargetProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether or not the popup is repositioned
        /// when it is resized.
        /// </summary>
        [Bindable(true), Category("Layout")]
        public bool RepositionWhenPopupResizes
        {
            get { return (bool)base.GetValue(RepositionWhenPopupResizesProperty); }
            set { base.SetValue(RepositionWhenPopupResizesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the offset from top the area that is specified for
        /// the Popup control content by a combination of the Placement,
        /// PlacementTarget, and PlacementRectangle properties. 
        /// 
        /// Return Value
        /// An offset from the left of the area that is specified for the
        /// Popup control by a combination of the Placement, PlacementTarget,
        /// and PlacementRectangle properties. The default value is 0.0.
        /// </summary>
        [TypeConverter(typeof(LengthConverter)), Bindable(true), Category("Layout")]
        public double VerticalOffset
        {
            get { return (double)base.GetValue(VerticalOffsetProperty); }
            set { base.SetValue(VerticalOffsetProperty, value); }
        }

        #endregion

        #region Protected Properties

        virtual protected PointCombination CurrentPointCombination
        {
            get { return _CurrentPointCombination; }
            set { _CurrentPointCombination = value; }
        }

        #endregion

        #region Constructors

        public PopupDecorator()
        {
            _PopupSizeChangedEventHandler = new SizeChangedEventHandler(_PopupAdorner_SizeChanged);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the popup and displays it to the user.
        /// </summary>
        virtual public void Open()
        {
            if (_PopupAdorner != null)
                Close();

            if (_Popup != null)
            {
                // If the IsOpen property is false, change it to true!
                if (!IsOpen)
                    IsOpen = true;
                else
                {
                    // Reposition the popup
                    Reposition();

                    // Remove the popup from the logical tree
                    RemoveLogicalChild(_Popup);

                    // Add the popup to the logical tree of the UIElementAdorner
                    _PopupAdorner = new UIElementAdorner(Child, _Popup);
                    _PopupAdorner.SizeChanged += _PopupSizeChangedEventHandler;

                    // Show the popup
                    AdornerLayer.Add(_PopupAdorner);

                    // Fire the Opened event!
                    OnOpened(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Closes the popup and hides is from view.
        /// </summary>
        virtual public void Close()
        {
            if (_PopupAdorner != null)
            {
                if (IsOpen)
                {
                    // If the IsOpen property is true, set it to false!
                    IsOpen = false;
                }
                else
                {
                    // Hide the popup
                    AdornerLayer.Remove(_PopupAdorner);

                    // No longer track when the popup has changed size
                    _PopupAdorner.SizeChanged -= _PopupSizeChangedEventHandler;

                    // Remove the popup from the PopupAdorner's logical tree                
                    _PopupAdorner.Child = null;
                    _PopupAdorner = null;

                    // Add the hidden popup back into the logical tree
                    if (_Popup != null)
                        AddLogicalChild(_Popup);

                    // Fire the closed event!
                    OnClosed(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Repositions the popup based on the PlacementTarget
        /// or PlacementRectangle.
        /// </summary>
        virtual public void Reposition()
        {
            UpdatePosition();
        }

        #endregion

        #region Protected Methods

        virtual protected void OnOpened(EventArgs e)
        {
            if (Opened != null)
                Opened(this, e);
        }

        virtual protected void OnClosed(EventArgs e)
        {
            if (Closed != null)
                Closed(this, e);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the bounds of the PopupDecorator.
        /// </summary>
        /// <returns></returns>
        private Rect GetDecoratorBounds()
        {
            return new Rect(new Point(0, 0), RenderSize);
        }

        /// <summary>
        /// Returns an array of points representing the top left,
        /// top right, bottom left, bottom right, and center of
        /// the rectangle of the popup itself.
        /// </summary>        
        /// <returns>An array of points</returns>
        private Point[] GetPopupInterestPoints(PlacementMode placement)
        {
            UIElement popup = _Popup;
            if (popup == null)
                return InterestPointsFromRect(new Rect());

            // Using our popup render side, determine our interest points
            Point[] pointArray = InterestPointsFromRect(
                new Rect(
                    new Point(),
                    popup.DesiredSize
                )
            );

            // Determine the placement target for our popup
            UIElement placementTarget = this.GetTarget() as UIElement;

            if (
                    (
                        (placementTarget != null) &&
                        !IsAbsolutePlacementMode(placement)
                    ) &&
                    (
                        // Determine if the placementTarget's flow direction
                        // and our popup's flow direction match.  If not,
                        // we need to flip our popup!
                        ((FlowDirection)placementTarget.GetValue(FrameworkElement.FlowDirectionProperty)) != ((FlowDirection)popup.GetValue(FrameworkElement.FlowDirectionProperty))
                    )
                )
            {
                SwapPoints(ref pointArray[0], ref pointArray[1]);
                SwapPoints(ref pointArray[2], ref pointArray[3]);
            }

            //Vector animationOffset = this._popupRoot.Value.AnimationOffset;
            /*GeneralTransform transform = TransformToClient(popup, this);
            for (int i = 0; i < 5; i++)
            {
                //transform.TryTransform(pointArray[i] - animationOffset, out pointArray[i]);
                transform.TryTransform(pointArray[i], out pointArray[i]);
            }*/

            return pointArray;
        }

        /// <summary>
        /// Returns an array of points representing the top left,
        /// top right, bottom left, bottom right, and center of
        /// the rectangle of the popup placement target.
        /// </summary>        
        /// <returns>An array of points</returns>
        private Point[] GetPlacementTargetInterestPoints(PlacementMode placement)
        {
            // Determine the predetermined placement rectangle, if it is present.
            Rect placementRectangle = this.PlacementRectangle;

            // Get the placement target
            UIElement child = this.GetTarget() as UIElement;

            // Determine the offset from our placement target
            Vector vector = new Vector(this.HorizontalOffset, this.VerticalOffset);

            // If there's a valid placement target or we're in absolute
            // placement mode, then...
            if ((child == null) || IsAbsolutePlacementMode(placement))
            {
                if ((placement == PlacementMode.Mouse) ||
                    (placement == PlacementMode.MousePoint))
                {
                    // The placement rectangle is determined via
                    // the mouse point.  i.e. the original placement
                    // rectangle has no effect when positioned by mouse.
                    placementRectangle = new Rect(
                        Mouse.GetPosition(this),
                        new Size(
                            RenderSize.Width,
                            RenderSize.Height
                        )
                    );
                }
                else if (placementRectangle == Rect.Empty)
                {
                    // Ensure we have a placement rectangle
                    placementRectangle = new Rect();
                }

                // Offset the placement rectangle by the horizontal
                // and vertical offsets
                placementRectangle.Offset(vector);
                return InterestPointsFromRect(placementRectangle);
            }

            //
            // If no placement rectangle was provided,
            // then determine placement information...
            if (placementRectangle == Rect.Empty)
            {
                if ((placement != PlacementMode.Relative) &&
                    (placement != PlacementMode.RelativePoint))
                {
                    // The default placement rectangle is
                    // a duplicate of our placement target
                    // rectangle.
                    placementRectangle = new Rect(
                        0, 0,
                        child.RenderSize.Width,
                        child.RenderSize.Height);
                }
                else
                {
                    placementRectangle = new Rect();
                }
            }

            // Offset the placement rectangle by our horizontal and
            // vertical offsets.
            placementRectangle.Offset(vector);

            // Determine interest points from our placement rectangle
            Point[] pointArray = InterestPointsFromRect(placementRectangle);

            // Transform the points relatively to
            // the placement target and/or the placement
            // rectangle.
            GeneralTransform transform = TransformToClient(child, this);
            for (int i = 0; i < 5; i++)
                transform.TryTransform(pointArray[i], out pointArray[i]);

            // Return our list of points
            return pointArray;
        }

        /// <summary>
        /// Returns a PointCombination that represents the points that
        /// will be used to position the popup relative to its placement target.
        /// </summary>        
        private PointCombination GetPointCombination(PlacementMode placement, int i, out PopupPrimaryAxis axis)
        {
            bool menuDropAlignment = SystemParameters.MenuDropAlignment;
            switch (placement)
            {
                case PlacementMode.Relative:
                case PlacementMode.AbsolutePoint:
                case PlacementMode.RelativePoint:
                case PlacementMode.MousePoint:
                    axis = PopupPrimaryAxis.Horizontal;
                    if (!menuDropAlignment)
                    {
                        // Menus are right-aligned
                        if (i == 0)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopLeft);
                        }
                        if (i == 1)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
                        }
                        if (i == 2)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
                        }
                        if (i == 3)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomRight);
                        }
                        break;
                    }
                    // Menus are left-aligned
                    if (i != 0)
                    {
                        if (i == 1)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopLeft);
                        }
                        if (i == 2)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomRight);
                        }
                        if (i == 3)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
                        }
                        break;
                    }
                    return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);

                case PlacementMode.Bottom:
                case PlacementMode.Mouse:
                    axis = PopupPrimaryAxis.Horizontal;
                    if (!menuDropAlignment)
                    {
                        // Menus are right-aligned
                        if (i == 0)
                        {
                            return new PointCombination(InterestPoint.BottomLeft, InterestPoint.TopLeft);
                        }
                        if (i == 1)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
                        }
                        if (i == 2)
                        {
                            return new PointCombination(InterestPoint.BottomLeft, InterestPoint.TopRight);
                        }
                        if (i == 3)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomRight);
                        }
                        break;
                    }
                    // Menus are left aligned
                    if (i != 0)
                    {
                        if (i == 1)
                        {
                            return new PointCombination(InterestPoint.TopRight, InterestPoint.BottomRight);
                        }
                        if (i == 2)
                        {
                            return new PointCombination(InterestPoint.BottomRight, InterestPoint.TopLeft);
                        }
                        if (i == 3)
                        {
                            return new PointCombination(InterestPoint.TopRight, InterestPoint.BottomLeft);
                        }
                        break;
                    }
                    return new PointCombination(InterestPoint.BottomRight, InterestPoint.TopRight);

                case PlacementMode.Center:
                    axis = PopupPrimaryAxis.None;
                    return new PointCombination(InterestPoint.Center, InterestPoint.Center);

                case PlacementMode.Right:
                case PlacementMode.Left:
                    axis = PopupPrimaryAxis.Vertical;
                    //menuDropAlignment |= this.DropOpposite;
                    if ((!menuDropAlignment || (placement != PlacementMode.Right)) && (menuDropAlignment || (placement != PlacementMode.Left)))
                    {
                        // Menus are right-aligned
                        if (i == 0)
                        {
                            return new PointCombination(InterestPoint.TopRight, InterestPoint.TopLeft);
                        }
                        if (i == 1)
                        {
                            return new PointCombination(InterestPoint.BottomRight, InterestPoint.BottomLeft);
                        }
                        if (i == 2)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
                        }
                        if (i == 3)
                        {
                            return new PointCombination(InterestPoint.BottomLeft, InterestPoint.BottomRight);
                        }
                        break;
                    }
                    // Menus are left-aligned
                    if (i == 0)
                    {
                        return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
                    }
                    if (i == 1)
                    {
                        return new PointCombination(InterestPoint.BottomLeft, InterestPoint.BottomRight);
                    }
                    if (i == 2)
                    {
                        return new PointCombination(InterestPoint.TopRight, InterestPoint.TopLeft);
                    }
                    if (i != 3)
                    {
                        break;
                    }
                    return new PointCombination(InterestPoint.BottomRight, InterestPoint.BottomLeft);

                case PlacementMode.Top:
                    axis = PopupPrimaryAxis.Horizontal;
                    if (!menuDropAlignment)
                    {
                        // Menus are right-aligned
                        if (i == 0)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
                        }
                        if (i == 1)
                        {
                            return new PointCombination(InterestPoint.BottomLeft, InterestPoint.TopLeft);
                        }
                        if (i == 2)
                        {
                            return new PointCombination(InterestPoint.TopLeft, InterestPoint.BottomRight);
                        }
                        if (i == 3)
                        {
                            return new PointCombination(InterestPoint.BottomLeft, InterestPoint.TopRight);
                        }
                        break;
                    }
                    // Menus are left-aligned
                    if (i != 0)
                    {
                        if (i == 1)
                        {
                            return new PointCombination(InterestPoint.BottomRight, InterestPoint.TopRight);
                        }
                        if (i == 2)
                        {
                            return new PointCombination(InterestPoint.TopRight, InterestPoint.BottomLeft);
                        }
                        if (i == 3)
                        {
                            return new PointCombination(InterestPoint.BottomRight, InterestPoint.TopLeft);
                        }
                        break;
                    }
                    return new PointCombination(InterestPoint.TopRight, InterestPoint.BottomRight);

                default:
                    axis = PopupPrimaryAxis.None;
                    return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopLeft);
            }
            return new PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
        }

        /// <summary>
        /// Determines the placement target for the popup.
        /// </summary>
        /// <returns>The Visual target for the popup.</returns>
        private Visual GetTarget()
        {
            Visual placementTarget = this.PlacementTarget;
            if (placementTarget == null)
                placementTarget = Child;
            return placementTarget;
        }

        /// <summary>
        /// Returns an array of points representing the
        /// top left, top right, bottom left, bottom right,
        /// and center of the rectangle, in that order.
        /// </summary>
        /// <param name="rect">The rectangle from which to extract point information</param>
        /// <returns>An array of points</returns>
        private static Point[] InterestPointsFromRect(Rect rect)
        {
            return new Point[]
            {
                rect.TopLeft,
                rect.TopRight,
                rect.BottomLeft,
                rect.BottomRight,
                new Point(
                    rect.Left + (rect.Width / 2),
                    rect.Top + (rect.Height / 2)
                )
            };
        }

        /// <summary>
        /// Returns true of the popup is in an absolute placement mode.
        /// </summary>        
        private static bool IsAbsolutePlacementMode(PlacementMode placement)
        {
            switch (placement)
            {
                case PlacementMode.AbsolutePoint:
                case PlacementMode.Mouse:
                case PlacementMode.MousePoint:
                case PlacementMode.Absolute:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the popup requires repositioning.  This is especially
        /// useful if the popup has changed size.
        /// </summary>
        /// <returns>True if the popup requires repositioning, false otherwise.</returns>
        private bool RequiresRepositioning()
        {
            if (_Popup.RenderTransform != null)
            {
                TranslateTransform tt = (TranslateTransform)_Popup.RenderTransform;
                Rect popupBounds = new Rect(new Point(tt.X, tt.Y), _Popup.DesiredSize);
                Rect decoratorBounds = GetDecoratorBounds();

                Rect intersectingRect = Rect.Intersect(decoratorBounds, popupBounds);

                double popupArea = popupBounds.Width * popupBounds.Height;
                double intersectionArea =
                    (intersectingRect != Rect.Empty) ?
                    intersectingRect.Width * intersectingRect.Height :
                    0;

                return Math.Abs(intersectionArea - popupArea) > 0.01;
            }

            return false;
        }

        private void UpdatePosition()
        {
            if (_Popup != null)
            {
                int numberOfCombinations;

                // If the popup's size has not been determined yet, measure it!
                if (_Popup.DesiredSize == Size.Empty)
                    _Popup.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                // Determine the type of placement our popup is using
                PlacementMode placement = Placement;

                // Determine the interest points for our placement target
                Point[] interestPoints = GetPlacementTargetInterestPoints(placement);

                // Determine the interest points for our popup itself
                Point[] popupInterestPoints = GetPopupInterestPoints(placement);

                // Determine the bounding box of our placement area
                Rect placementBounds = GetBounds(interestPoints);

                // Determine the bounding box of our popup
                Rect popupBounds = GetBounds(popupInterestPoints);

                // Determine the bounding box for the decorator (our entire working area)
                Rect decoratorBounds = GetDecoratorBounds();

                // Determine the area of the popup
                double popupArea = popupBounds.Width * popupBounds.Height;

                double highestResultingArea = -1;
                PointCombination usedPointCombination = new PointCombination(InterestPoint.BottomLeft, InterestPoint.TopLeft);
                Vector offsetVector = new Vector(0, 0);

                // Determine custom popup placement
                CustomPopupPlacement[] placementArray = null;
                if (placement == PlacementMode.Custom)
                {
                    CustomPopupPlacementCallback customPopupPlacementCallback = CustomPopupPlacementCallback;
                    if (customPopupPlacementCallback != null)
                    {
                        placementArray = customPopupPlacementCallback(
                            popupBounds.Size,
                            placementBounds.Size,
                            new Point(this.HorizontalOffset, this.VerticalOffset)
                        );
                    }
                    numberOfCombinations = (placementArray == null) ? 0 : placementArray.Length;
                    if (!this.IsOpen)
                        return;
                }
                else
                {
                    // Determine the number of possible combinations            
                    numberOfCombinations = GetNumberOfCombinations(placement);
                }

                // Iterate through each possible combination
                for (int i = 0; i < numberOfCombinations; i++)
                {
                    Vector topLeftVector;
                    PopupPrimaryAxis primaryAxis;
                    _IsRightPositioned = false;
                    _IsBottomPositioned = false;
                    PointCombination pointCombination;

                    //
                    // Determine the topLeftVector for popup placement
                    // and the primary axis for the popup
                    //
                    if (placement == PlacementMode.Custom)
                    {
                        // Determine the top-left corner and primary axis for placement
                        topLeftVector = ((Vector)interestPoints[0]) + ((Vector)placementArray[i].Point);
                        primaryAxis = placementArray[i].PrimaryAxis;
                        pointCombination = new PointCombination(InterestPoint.BottomLeft, InterestPoint.TopLeft);
                    }
                    else
                    {
                        // Use the placement value to determine the relevant points for placement
                        pointCombination = GetPointCombination(placement, i, out primaryAxis);
                        InterestPoint targetInterestPoint = pointCombination.TargetInterestPoint;
                        InterestPoint childInterestPoint = pointCombination.ChildInterestPoint;
                        topLeftVector = (Vector)(
                            interestPoints[(int)targetInterestPoint] -
                            popupInterestPoints[(int)childInterestPoint]);

                        _IsRightPositioned = (childInterestPoint == InterestPoint.TopRight) || (childInterestPoint == InterestPoint.BottomRight);
                        _IsBottomPositioned = (childInterestPoint == InterestPoint.BottomLeft) || (childInterestPoint == InterestPoint.BottomRight);
                    }

                    // Determine the popup placement using this combination
                    Rect popupPlacementRect = Rect.Offset(popupBounds, topLeftVector);

                    // Determine if the popup placement intersects with the working area (will it be displayed?)
                    Rect intersectingRect = Rect.Intersect(decoratorBounds, popupPlacementRect);

                    // If the popup is displayed, the resultingArea tells us how much of it will be displayed
                    double resultingArea =
                        (intersectingRect != Rect.Empty) ?
                        intersectingRect.Width * intersectingRect.Height :
                        0;

                    // Does this combination display more of the popup than any of the previous combinations?
                    if ((resultingArea - highestResultingArea) > 0.01)
                    {
                        offsetVector = topLeftVector;
                        highestResultingArea = resultingArea;
                        usedPointCombination = pointCombination;

                        /*this.AnimateFromRight = flag;
                        this.AnimateFromBottom = flag2;*/

                        // Determine if we're displaying the full popup area
                        // using this combination.  If so, then break!
                        if (Math.Abs((double)(resultingArea - popupArea)) < 0.01)
                        {
                            break;
                        }
                    }
                }

                // Determine if the entire area of the popup is displayed.
                // If not, then we need to flip it!
                if (Math.Abs(highestResultingArea - popupArea) > 0.01)
                {
                    Point topLeft = interestPoints[0];
                    Point topRight = interestPoints[1];
                    Vector widthVector = (Vector)(topRight - topLeft);
                    widthVector.Normalize();
                    if (double.IsNaN(widthVector.Y) ||
                        Math.Abs(widthVector.Y) < 0.01)
                    {
                        if (popupBounds.Right + offsetVector.X > decoratorBounds.Right)
                        {
                            offsetVector.X = decoratorBounds.Right - popupBounds.Width;
                        }
                        else if (popupBounds.Left + offsetVector.X < decoratorBounds.Left)
                        {
                            offsetVector.X = decoratorBounds.Left;
                        }
                    }
                    else if (Math.Abs(widthVector.X) < 0.01)
                    {
                        if (popupBounds.Bottom + offsetVector.Y > decoratorBounds.Bottom)
                        {
                            offsetVector.Y = decoratorBounds.Bottom - popupBounds.Height;
                        }
                        else if (popupBounds.Top + offsetVector.Y < decoratorBounds.Top)
                        {
                            offsetVector.Y = decoratorBounds.Top;
                        }
                    }

                    Point bottomLeft = interestPoints[2];
                    Vector heightVector = (Vector)(topLeft - bottomLeft);
                    heightVector.Normalize();
                    if ((double.IsNaN(heightVector.X)) ||
                        (Math.Abs(heightVector.X) < 0.01))
                    {
                        if (popupBounds.Bottom + offsetVector.Y > decoratorBounds.Bottom)
                        {
                            offsetVector.Y = decoratorBounds.Bottom - popupBounds.Height;
                        }
                        else if (popupBounds.Top + offsetVector.Y < decoratorBounds.Top)
                        {
                            offsetVector.Y = 0;
                        }
                    }
                    else if (Math.Abs(heightVector.Y) < 0.01)
                    {
                        if (popupBounds.Right + offsetVector.X > decoratorBounds.Right)
                        {
                            offsetVector.X = decoratorBounds.Right - popupBounds.Width;
                        }
                        else if (popupBounds.Left + offsetVector.X < decoratorBounds.Left)
                        {
                            offsetVector.X = 0;
                        }
                    }
                }

                // Offset the popup rendering by the final placement rectangle
                _Popup.RenderTransform = new TranslateTransform(offsetVector.X, offsetVector.Y);

                // Set the point combinations that were used to place this popup
                CurrentPointCombination = usedPointCombination;
            }
        }

        /// <summary>
        /// Returns a transform that will offset the coordinates of a rectangle
        /// from the visual to the rootVisual.
        /// </summary>        
        private static GeneralTransform TransformToClient(Visual visual, Visual rootVisual)
        {
            return visual.TransformToAncestor(rootVisual);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Repositions the popup when the popup is closed,
        /// the user has requested that the popup is repositioned
        /// whenever the popup is resized, or if it is required
        /// to reposition the popup because it is right-positioned
        /// or bottom-positioned (and hence may be positioned incorrectly).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _PopupAdorner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsOpen ||
                RepositionWhenPopupResizes ||
                _IsBottomPositioned ||
                _IsRightPositioned ||
                RequiresRepositioning())
            {
                Reposition();
            }
        }

        #endregion

        #region Overrides

        protected override Size MeasureOverride(System.Windows.Size constraint)
        {
            constraint = base.MeasureOverride(constraint);

            // Measure the popup with infinite size
            if (_Popup != null)
                _Popup.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return constraint;
        }

        protected override Size ArrangeOverride(System.Windows.Size finalSize)
        {
            finalSize = base.ArrangeOverride(finalSize);

            // Arrange the popup with the DesiredSize of its contents
            if (_Popup != null)
                _Popup.Arrange(new Rect(new Point(0, 0), _Popup.DesiredSize));

            return finalSize;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (this._Popup == null)
                {
                    return base.LogicalChildren;
                }
                if (this.Child == null)
                {
                    return new SingleChildEnumerator(this._Popup);
                }
                return new DoubleChildEnumerator(this._Popup, this.Child);
            }
        }

        #endregion        

        #region Protected Structs

        [StructLayout(LayoutKind.Sequential)]
        protected struct PointCombination
        {
            public InterestPoint TargetInterestPoint;
            public InterestPoint ChildInterestPoint;
            public PointCombination(InterestPoint targetInterestPoint, InterestPoint childInterestPoint)
            {
                this.TargetInterestPoint = targetInterestPoint;
                this.ChildInterestPoint = childInterestPoint;
            }
        }

        #endregion

        #region Protected Enums

        protected enum InterestPoint
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center
        }

        #endregion

        #region Enumerators

        internal class SingleChildEnumerator : IEnumerator
        {
            #region Private Fields

            private object _child;
            private int _count;
            private int _index;

            #endregion

            #region Public Properties

            object IEnumerator.Current
            {
                get
                {
                    if (this._index != 0)
                    {
                        return null;
                    }
                    return this._child;
                }
            }

            #endregion

            #region Constructors

            internal SingleChildEnumerator(object Child)
            {
                this._child = Child;
                this._count = (Child == null) ? 0 : 1;
            }

            #endregion

            #region Public Methods

            bool IEnumerator.MoveNext()
            {
                this._index++;
                return (this._index < this._count);
            }

            void IEnumerator.Reset()
            {
                this._index = -1;
            }

            #endregion
        }

        internal class DoubleChildEnumerator : IEnumerator
        {
            #region Private Fields

            private object _child1;
            private object _child2;
            private int _index = -1;

            #endregion

            #region Public Properties

            object IEnumerator.Current
            {
                get
                {
                    switch (this._index)
                    {
                        case 0:
                            return this._child1;

                        case 1:
                            return this._child2;
                    }
                    return null;
                }
            }

            #endregion

            #region Constructors

            internal DoubleChildEnumerator(object child1, object child2)
            {
                this._child1 = child1;
                this._child2 = child2;
            }

            #endregion

            #region Public Methods

            bool IEnumerator.MoveNext()
            {
                this._index++;
                return (this._index < 2);
            }

            void IEnumerator.Reset()
            {
                this._index = -1;
            }

            #endregion
        }
    

        #endregion
    }
}