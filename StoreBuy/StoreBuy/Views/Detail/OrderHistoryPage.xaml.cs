using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using StoreBuy.ViewModels.Detail;
using StoreBuy.ViewModels.Profile;

namespace StoreBuy.Views.Detail
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderHistoryPage
    {

        public OrderHistoryPage()
        {
            InitializeComponent();

            this.BindingContext = new OrderHistoryViewModel(Navigation);
        }

       
    }
}