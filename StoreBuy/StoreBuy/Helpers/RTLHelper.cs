using System.ComponentModel;
using Syncfusion.XForms.Border;
using Syncfusion.XForms.Cards;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.Helpers
{
    
    [Preserve(AllMembers = true)]
    public static class RTLHelper
    {
        #region Bindable Properties

        
        public static readonly BindableProperty MarginProperty = BindableProperty.CreateAttached("Margin",
            typeof(Thickness), typeof(RTLHelper), ZeroThickness, propertyChanged: OnMarginPropertyChanged);

        public static readonly BindableProperty PaddingProperty = BindableProperty.CreateAttached("Padding",
            typeof(Thickness), typeof(RTLHelper), ZeroThickness, propertyChanged: OnPaddingPropertyChanged);

        
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached("CornerRadius",
            typeof(Thickness), typeof(RTLHelper), ZeroThickness, propertyChanged: OnCornerRadiusPropertyChanged);

        #endregion

        #region Private Fields

        private static readonly Thickness ZeroThickness = new Thickness();

        #endregion

        #region Methods

       
        public static Thickness GetMargin(BindableObject bindable)
        {
            return (Thickness)bindable?.GetValue(MarginProperty);
        }

        public static Thickness GetPadding(BindableObject bindable)
        {
            return (Thickness)bindable?.GetValue(PaddingProperty);
        }

        public static Thickness GetCornerRadius(BindableObject bindable)
        {
            return (Thickness)bindable?.GetValue(CornerRadiusProperty);
        }

        public static void SetMargin(BindableObject bindable, Thickness value)
        {
            if (value != ZeroThickness)
            {
                bindable?.SetValue(MarginProperty, value);
            }
            else
            {
                bindable?.ClearValue(MarginProperty);
            }
        }
        public static void SetPadding(BindableObject bindable, Thickness value)
        {
            if (value != ZeroThickness)
            {
                bindable?.SetValue(PaddingProperty, value);
            }
            else
            {
                bindable?.ClearValue(PaddingProperty);
            }
        }

        public static void SetCornerRadius(BindableObject bindable, Thickness value)
        {
            if (value != ZeroThickness)
            {
                bindable?.SetValue(CornerRadiusProperty, value);
            }
            else
            {
                bindable?.ClearValue(CornerRadiusProperty);
            }
        }

        private static void OnMarginPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view)) return;

            var previousMargin = (Thickness)oldValue;
            var currentMargin = (Thickness)newValue;

            UpdateMargin(view);

            if (currentMargin != ZeroThickness)
            {
                if (previousMargin == ZeroThickness)
                {
                    OnElementAttached(view);
                }
            }
            else
            {
                OnElementDetached(view);
            }
        }

        private static void OnPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Layout layout)) return;

            var previousPadding = (Thickness)oldValue;
            var currentPadding = (Thickness)newValue;

            UpdatePadding(layout);

            if (currentPadding != ZeroThickness)
            {
                if (previousPadding == ZeroThickness)
                {
                    OnElementAttached(layout);
                }
            }
            else
            {
                OnElementDetached(layout);
            }
        }

        private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view)) return;

            var previousCornerRadius = (Thickness)oldValue;
            var currentCornerRadius = (Thickness)newValue;

            UpdateCornerRadius(view);

            if (currentCornerRadius != ZeroThickness)
            {
                if (previousCornerRadius == ZeroThickness)
                {
                    OnElementAttached(view);
                }
            }
            else
            {
                OnElementDetached(view);
            }
        }

        private static void UpdateMargin(VisualElement view)
        {
            var controller = (IVisualElementController)view;
            var margin = GetMargin(view);

            if (margin != ZeroThickness)
            {
                if (controller.EffectiveFlowDirection == EffectiveFlowDirection.RightToLeft)
                {
                    margin = new Thickness(margin.Right, margin.Top, margin.Left, margin.Bottom);
                }

                view.SetValue(View.MarginProperty, margin);
            }
            else
            {
                view.ClearValue(View.MarginProperty);
            }
        }
        private static void UpdatePadding(View layout)
        {
            var controller = (IVisualElementController)layout;
            var padding = GetPadding(layout);

            if (padding != ZeroThickness)
            {
                if (controller.EffectiveFlowDirection == EffectiveFlowDirection.RightToLeft)
                {
                    padding = new Thickness(padding.Right, padding.Top, padding.Left, padding.Bottom);
                }

                layout.SetValue(Layout.PaddingProperty, padding);
            }
            else
            {
                layout.ClearValue(Layout.PaddingProperty);
            }
        }

        private static void UpdateCornerRadius(View view)
        {
            var controller = (IVisualElementController)view;
            var cornerRadius = GetCornerRadius(view);

            if (cornerRadius != ZeroThickness)
            {
                if (controller.EffectiveFlowDirection == EffectiveFlowDirection.RightToLeft)
                {
                    cornerRadius = new Thickness(cornerRadius.Top, cornerRadius.Left, cornerRadius.Bottom,
                        cornerRadius.Right);
                }

                if (view is SfCardView)
                {
                    view.SetValue(SfCardView.CornerRadiusProperty, cornerRadius);
                }
                else if (view is SfBorder)
                {
                    view.SetValue(SfBorder.CornerRadiusProperty, cornerRadius);
                }
            }
            else
            {
                if (view is SfCardView)
                {
                    view.ClearValue(SfCardView.CornerRadiusProperty);
                }
                else if (view is SfBorder)
                {
                    view.ClearValue(SfBorder.CornerRadiusProperty);
                }
            }
        }

        private static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is View view)
            {
                if (e.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
                {
                    UpdateMargin(view);
                    UpdatePadding(view);
                    UpdateCornerRadius(view);
                }
            }
        }

        private static void OnElementAttached(View element)
        {
            element.PropertyChanged += OnElementPropertyChanged;
        }

        private static void OnElementDetached(View element)
        {
            element.PropertyChanged -= OnElementPropertyChanged;
        }

        #endregion
    }
}