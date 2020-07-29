using System;
using System.Reflection;
using System.Windows.Input;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.Behaviors
{
    [Preserve(AllMembers = true)]
    public class SfComboBoxDropDownBehavior : Behavior<SfComboBox>
    {


        #region Binable Properties

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SfComboBoxDropDownBehavior));
        #endregion

        #region Field

        private bool isCheckboxLoaded;

        #endregion

        #region Public Property

    
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        public SfComboBox ComboBox { get; private set; }

        #endregion

        #region Methods

        protected override void OnAttachedTo(SfComboBox comboBox)
        {
            if (comboBox != null)
            {
                base.OnAttachedTo(comboBox);
                this.ComboBox = comboBox;
                comboBox.BindingContextChanged += this.OnBindingContextChanged;
                comboBox.SelectionChanged += this.SelectionChanged;
            }
        }

        protected override void OnDetachingFrom(SfComboBox comboBox)
        {
            if (comboBox != null)
            {
                base.OnDetachingFrom(comboBox);
                comboBox.BindingContextChanged -= this.OnBindingContextChanged;
                comboBox.SelectionChanged -= this.SelectionChanged;
                this.ComboBox = null;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.BindingContext = this.ComboBox.BindingContext;
        }
        private void SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            int totalQuantity;
            int.TryParse(e.Value.ToString(), out totalQuantity);

            var bindingContext = (sender as SfComboBox).BindingContext;

            PropertyInfo propertyInfo = bindingContext.GetType().GetProperty("Quantity");
            propertyInfo.SetValue(bindingContext, totalQuantity);

            if (this.isCheckboxLoaded)
            {
                if (this.Command == null)
                {
                    return;
                }

                if (this.Command.CanExecute((sender as SfComboBox).BindingContext))
                {
                    this.Command.Execute((sender as SfComboBox).BindingContext);
                }
            }

            this.isCheckboxLoaded = true;
        }
        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            this.OnBindingContextChanged();
        }

        #endregion
    }
}
