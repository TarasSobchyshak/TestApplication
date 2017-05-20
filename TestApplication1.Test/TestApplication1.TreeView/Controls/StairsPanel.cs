using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TestApplication1.Test.Controls
{
    public class StairsPanel : Panel
    {
        private double _maxWidth;
        private double _maxHeight;
        private Rect[] childrenLocations;
        
        public static readonly DependencyProperty OffsetProperty = DependencyProperty
            .Register(nameof(Offset), typeof(double), typeof(StairsPanel), new PropertyMetadata(0.5, OffsetPropertyCallBack));

        private static void OffsetPropertyCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stairsPanel = d as StairsPanel;
            if (stairsPanel == null) return;
            double newValue = double.Parse(e.NewValue.ToString());

            if (newValue < 0) stairsPanel.Offset = 0;
            if (newValue > 1) stairsPanel.Offset = 1;
        }

        /// <summary>
        /// Offset is value between 0.0 and 1.0 which determines the offset of items.
        /// </summary>
        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (!Children.Any()) return new Size();

            for (int i = 0; i < childrenLocations.Length; ++i)
                Children[i].Arrange(childrenLocations[i]);

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!Children.Any()) return new Size();

            foreach (UIElement item in Children)
            {
                item.Measure(availableSize);

                if (_maxHeight < item.DesiredSize.Height)
                    _maxHeight = item.DesiredSize.Height;

                if (_maxWidth < item.DesiredSize.Width)
                    _maxWidth = item.DesiredSize.Width;
            }

            EvaluateLocations(availableSize);
            var last = childrenLocations.Last();

            availableSize.Height = last.Bottom;

            return availableSize;
        }

        private void EvaluateLocations(Size availableSize)
        {
            var top = new Point(0, 0);
            var n = Children.Count;
            var columnsNumber = (int)Math.Floor(availableSize.Width / _maxWidth);
            var isReverseDirection = false;
            var itemSize = new Size(_maxWidth, _maxHeight);
            var i = 0;
            childrenLocations = new Rect[n];

            while (i < n)
            {
                if (isReverseDirection)
                {
                    top.X -= itemSize.Width;
                    top.Y += (1 - 2 * Offset) * itemSize.Height;

                    for (int j = 0; j < columnsNumber - 2 && i < n; ++j, ++i)
                    {
                        childrenLocations[i] = new Rect(top, itemSize);
                        top.X -= itemSize.Width;
                        top.Y += Offset * itemSize.Height;
                    }

                    top.Y += (1 - 2 * Offset) * itemSize.Height;

                    isReverseDirection = !isReverseDirection;
                }
                else
                {
                    for (int j = 0; j < columnsNumber && i < n; ++j, ++i)
                    {
                        childrenLocations[i] = new Rect(top, itemSize);

                        if (j != columnsNumber - 1)
                            top.X += itemSize.Width;

                        top.Y += Offset * itemSize.Height;
                    }

                    isReverseDirection = !isReverseDirection;
                }
            }
        }
    }
}
