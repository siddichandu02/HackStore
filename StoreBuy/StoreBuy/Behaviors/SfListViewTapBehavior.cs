using System.Windows.Input;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace StoreBuy.Behaviors
{
    public class SfListViewTapBehavior : Behavior<SfListView>
    {
        #region Properties

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SfListViewTapBehavior));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        #endregion

        #region Method

        protected override void OnAttachedTo(SfListView bindableListView)
        {
            if (bindableListView != null)
            {
                base.OnAttachedTo(bindableListView);
                bindableListView.ItemTapped += this.BindableListView_ItemTapped;
            }
        }

        protected override void OnDetachingFrom(SfListView bindableListView)
        {
            if (bindableListView != null)
            {
                base.OnDetachingFrom(bindableListView);
                bindableListView.ItemTapped -= this.BindableListView_ItemTapped;
            }
        }
        private void BindableListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (this.Command == null)
            {
                return;
            }

            if (this.Command.CanExecute(e.ItemData))
            {
                this.Command.Execute(e.ItemData);
            }
        }

        #endregion
    }
}
