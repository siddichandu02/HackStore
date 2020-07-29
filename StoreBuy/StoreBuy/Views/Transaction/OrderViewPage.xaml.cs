using StoreBuy.Models.Detail;
using StoreBuy.ViewModels.Transaction;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Transaction
{

    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderViewPage : ContentPage
    {
     
        public OrderViewPage(string SlotDate,string SlotTime,StoreModel Store)
        {
            InitializeComponent();
            this.BindingContext = new OrderViewModel(Navigation, SlotDate, SlotTime, Store);
        }
    }
}