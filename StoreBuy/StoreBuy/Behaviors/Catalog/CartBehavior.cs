using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.Behaviors.Catalog
{
    [Preserve(AllMembers = true)]
    public class CartBehavior : Behavior<ContentPage>
    {
        #region Fields

        private ContentPage bindablePage;

        #endregion

        #region Method

        protected override void OnAttachedTo(ContentPage bindableContentPage)
        {
            if (bindableContentPage != null)
            {
                base.OnAttachedTo(bindableContentPage);
                this.bindablePage = bindableContentPage;
                bindableContentPage.Appearing += this.Bindable_Appearing;
            }
        }
       protected override void OnDetachingFrom(ContentPage bindableContentPage)
        {
            if (bindableContentPage != null)
            {
                base.OnDetachingFrom(bindableContentPage);
                bindableContentPage.Appearing -= this.Bindable_Appearing;
            }
        }

       
        private void Bindable_Appearing(object sender, EventArgs e)
        {
           
        }

        #endregion
    }
}
