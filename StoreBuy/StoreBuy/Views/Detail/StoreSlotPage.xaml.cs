using StoreBuy.Models.Detail;
using StoreBuy.ViewModels.Detail;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoreSlotPage : ContentPage
    {
        public StoreSlotPage(StoreModel SelectedStore)
        {
            InitializeComponent();
            BindingContext = new StoreSlotViewModel(Navigation, SelectedStore);
        }

        private void RadioButton_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                var SelectedButtonText = (sender as SfRadioButton).Text;

                (BindingContext as StoreSlotViewModel).Button_Clicked(SelectedButtonText);
            }



        }
    }
}