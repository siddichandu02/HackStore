using StoreBuy.ViewModels.Catalog;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Catalog
{

    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class R10CategoryTilePage
    {
        public R10CategoryTilePage()
        {
            InitializeComponent();
            BindingContext = new R10CategoryViewModel(Navigation);
        }

       

        protected override void OnAppearing()
        {
            (this.BindingContext as R10CategoryViewModel).GetCategories();
        }       
    }
}