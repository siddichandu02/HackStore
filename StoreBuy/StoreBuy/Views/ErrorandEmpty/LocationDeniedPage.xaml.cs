using StoreBuy.ViewModels.ErrorAndEmpty;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.ErrorAndEmpty
{

    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationDeniedPage
    {

        public LocationDeniedPage()
        {
            InitializeComponent();
            BindingContext = new LocationDeniedPageViewModel(Navigation);
        }


        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    ErrorImage.IsVisible = false;
                }
            }
            else
            {
                ErrorImage.IsVisible = true;
            }
        }
    }
}