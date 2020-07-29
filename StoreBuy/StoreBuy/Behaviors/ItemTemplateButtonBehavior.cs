using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.Behaviors
{
    [Preserve(AllMembers = true)]
    public class ItemTemplateButtonBehavior : Behavior<SfButton>
    {
        #region Binable Properties

        public static readonly BindableProperty CommandProperty =
             BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemTemplateButtonBehavior));

        public static BindableProperty CommandParameterProperty =
         BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ItemTemplateButtonBehavior));

        public static BindableProperty ChildElementProperty =
         BindableProperty.Create(nameof(ChildElement), typeof(object), typeof(ItemTemplateButtonBehavior));

        public static BindableProperty ParentElementProperty =
         BindableProperty.Create(nameof(ParentElement), typeof(object), typeof(ItemTemplateButtonBehavior));

        private int selectedIndex;

        private int childrenCount;

        #endregion

        #region Public Property

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }
        public object ParentElement
        {
            get { return GetValue(ParentElementProperty); }
            set { this.SetValue(ParentElementProperty, value); }
        }

   
        public object ChildElement
        {
            get { return GetValue(ChildElementProperty); }
            set { this.SetValue(ChildElementProperty, value); }
        }

        #endregion

        #region Methods

        protected override void OnAttachedTo(SfButton button)
        {
            if (button != null)
            {
                base.OnAttachedTo(button);
                button.Clicked += Button_Clicked;
            }
        }
        protected override void OnDetachingFrom(SfButton button)
        {
            if (button != null)
            {
                base.OnDetachingFrom(button);
                button.Clicked -= Button_Clicked;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            SfButton sfButton = sender as SfButton;

            // Animate the item when remove from list.
            if (ParentElement != null && ChildElement != null)
            {
                StackLayout mainStack = null;
                SfListView mainListview = null;
                var selectedElement = ChildElement as Grid;
                if (ParentElement is StackLayout)
                {
                    mainStack = ParentElement as StackLayout;
                    selectedIndex = mainStack.Children.IndexOf(selectedElement);
                    childrenCount = mainStack.Children.Count;
                }
                else if (ParentElement is SfListView)
                {
                    mainListview = ParentElement as SfListView;
                    selectedIndex = mainListview.Children.IndexOf(selectedElement);
                    childrenCount = mainListview.Children.Count;
                }
                if (mainStack != null || mainListview != null)
                {
                    await selectedElement.TranslateTo(-100, 0, 100);
                    await selectedElement.FadeTo(0, 20);

                    List<Task> animations = new List<Task>();

                    for (int i = selectedIndex + 1; i < childrenCount; i++)
                    {
                        VisualElement elementToMove;
                        elementToMove = mainStack == null ? mainListview.Children[i] : mainStack.Children[i];
                        var boundsToMoveTo = elementToMove.Bounds;
                        boundsToMoveTo.Top -= selectedElement.Height;
                        animations.Add(elementToMove.LayoutTo(boundsToMoveTo, 200, Easing.Linear));
                    }
                    await selectedElement.FadeTo(0, 20);
                    await Task.WhenAll(animations);
                }
            }

            if (this.Command == null)
                return;
            this.Command.Execute(sfButton.CommandParameter);
        }

        #endregion
    }
}
