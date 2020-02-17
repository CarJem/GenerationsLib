using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GenerationsLib.WPF.Controls
{
    public class TimeoutPopupDecorator : PopupDecorator
    {
        #region Attached Properties

        public static DependencyProperty TimeoutTargetProperty =
            DependencyProperty.RegisterAttached(
                "TimeoutTarget",
                typeof(UIElement),
                typeof(TimeoutPopupDecorator),
                new FrameworkPropertyMetadata(null)
            );

        public static void SetTimeoutTarget(DependencyObject obj, UIElement value)
        {
            obj.SetValue(TimeoutTargetProperty, value);
        }

        public static UIElement GetTimeoutTarget(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(TimeoutTargetProperty);
        }

        #endregion

        #region Dependency Properties

        static public readonly DependencyProperty InitialTimeOutProperty;
        static public readonly DependencyProperty TimeOutProperty;
        static public readonly DependencyProperty IsTimeOutEnabledProperty;

        #endregion

        #region Static Constructor

        static TimeoutPopupDecorator()
        {
            // Register dependency properties

            InitialTimeOutProperty = DependencyProperty.Register("InitialTimeOut",
                typeof(TimeSpan),
                typeof(TimeoutPopupDecorator),
                new PropertyMetadata(TimeSpan.FromSeconds(5)));

            TimeOutProperty = DependencyProperty.Register("TimeOut",
                typeof(TimeSpan),
                typeof(TimeoutPopupDecorator),
                new PropertyMetadata(TimeSpan.FromSeconds(0.5)));

            IsTimeOutEnabledProperty = DependencyProperty.Register("IsTimeOutEnabled",
                typeof(bool),
                typeof(TimeoutPopupDecorator),
                new PropertyMetadata(true, new PropertyChangedCallback(OnTimeOutEnabledChanged)));
        }

        #endregion

        #region Static Protected Methods

        static protected void OnTimeOutEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeoutPopupDecorator tp = (TimeoutPopupDecorator)obj;

            // If IsTimeOutEnabled is set to true, and
            // the popup is open, and the mouse is not
            // currently over the popup, then start the
            // timeout timer.
            if ((bool)e.NewValue && tp.IsOpen && !tp.IsMouseOver)
            {
                tp.BeginTimeOut(true);
            }
            else
            {
                // Always cancel the timeout if IsTimeOUtEnabled is set to false
                tp.CancelTimeOut();
            }
        }

        #endregion

        #region Private Fields

        private DispatcherTimer _Dispatcher;
        private DateTime _MouseEnterTime;

        #endregion

        #region Public Properties

        public TimeSpan InitialTimeOut
        {
            get { return (TimeSpan)GetValue(InitialTimeOutProperty); }
            set { SetValue(InitialTimeOutProperty, value); }
        }

        public TimeSpan TimeOut
        {
            get { return (TimeSpan)GetValue(TimeOutProperty); }
            set { SetValue(TimeOutProperty, value); }
        }

        public bool IsTimeOutEnabled
        {
            get { return (bool)GetValue(IsTimeOutEnabledProperty); }
            set { SetValue(IsTimeOutEnabledProperty, value); }
        }

        #endregion 

        #region Private Methods

        private void BeginTimeOut(bool isInitial)
        {
            if (IsTimeOutEnabled)
            {
                _Dispatcher = new DispatcherTimer();
                _Dispatcher.Tick += new EventHandler(_Dispatcher_Tick);
                _Dispatcher.Interval = isInitial ? InitialTimeOut : TimeOut;
                _Dispatcher.Start();
            }
        }

        private void CancelTimeOut()
        {
            if (_Dispatcher != null)
            {
                _Dispatcher.Stop();
                _Dispatcher.Tick -= new EventHandler(_Dispatcher_Tick);
            }
        }

        private UIElement GetLocalTimeoutTarget()
        {
            UIElement timeoutTarget = GetTimeoutTarget(this);

            if (timeoutTarget == null &&
                Popup != null &&
                Popup is FrameworkElement)
                timeoutTarget = GetTimeoutTarget((FrameworkElement)Popup);

            if (timeoutTarget == null)
                timeoutTarget = Popup;

            return timeoutTarget;
        }

        #endregion

        #region Overrides

        public override UIElement Popup
        {
            get { return base.Popup; }
            set
            {
                if (base.Popup != value)
                {
                    UIElement timeoutTarget = GetLocalTimeoutTarget();

                    // Unregister the previous popup contents from our mouse handlers.
                    if (timeoutTarget != null)
                    {
                        timeoutTarget.MouseEnter -= new MouseEventHandler(Popup_MouseEnter);
                        timeoutTarget.MouseLeave -= new MouseEventHandler(Popup_MouseLeave);
                    }

                    base.Popup = value;

                    timeoutTarget = GetLocalTimeoutTarget();

                    // Register the popup with event handlers to track when the mouse is
                    // over the popup
                    if (timeoutTarget != null)
                    {
                        timeoutTarget.MouseEnter += new MouseEventHandler(Popup_MouseEnter);
                        timeoutTarget.MouseLeave += new MouseEventHandler(Popup_MouseLeave);
                    }
                }
            }
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            // Cancel the timeout first, if it hasn't been cancelled already
            CancelTimeOut();

            // Begin a new timeout
            BeginTimeOut(true);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CancelTimeOut();
        }

        #endregion

        #region Event Handlers

        void _Dispatcher_Tick(object sender, EventArgs e)
        {
            CancelTimeOut(); // Reset the timeout
            Close();         // Close the popup
        }

        void Popup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // The mouse must be over the window for at least 1/10 of a second
            if ((DateTime.Now - _MouseEnterTime).TotalMilliseconds > 100)
            {
                // Restart the timeout when the mouse leaves the popup
                BeginTimeOut(false);
            }
        }

        void Popup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Cancel the timeout while the mouse is over the popup
            CancelTimeOut();
            _MouseEnterTime = DateTime.Now;
        }

        #endregion
    }
}