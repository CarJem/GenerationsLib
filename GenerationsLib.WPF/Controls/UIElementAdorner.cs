using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System;

namespace GenerationsLib.WPF.Controls
{
    public class UIElementAdorner :
        Adorner,
        IDisposable
    {
        #region Private Fields

        private UIElement _Child;

        #endregion

        #region Public Properties

        public UIElement Child
        {
            get { return _Child; }
            set
            {
                if (_Child != value)
                {
                    if (_Child != null)
                    {
                        base.RemoveLogicalChild(_Child);
                        base.RemoveVisualChild(_Child);
                    }

                    _Child = value;
                    if (_Child != null)
                    {
                        base.AddLogicalChild(_Child);
                        base.AddVisualChild(_Child);
                    }
                }
            }
        }

        #endregion

        #region Constructors

        public UIElementAdorner(UIElement adornedElement, UIElement content)
            : base(adornedElement)
        {
            Child = content;
        }

        #endregion

        #region Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_Child != null)
            {
                _Child.Measure(availableSize);
                return _Child.DesiredSize;
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_Child != null)
            {
                _Child.Arrange(new Rect(finalSize));
                return finalSize;
            }
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _Child;
        }

        protected override int VisualChildrenCount
        {
            get { return (_Child != null) ? 1 : 0; }
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                ArrayList list = new ArrayList();
                if (_Child != null)
                    list.Add(_Child);
                return (IEnumerator)list.GetEnumerator();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // Disconnect the Child from the logical/visual tree
            // of this element.
            Child = null;
        }

        #endregion
    }
}