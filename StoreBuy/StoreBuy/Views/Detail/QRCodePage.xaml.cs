using StoreBuy.ViewModels.Detail;
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
    public partial class QRCodePage : ContentPage
    {
        public QRCodePage(string SlotDateTime)
        {
            InitializeComponent();
            BindingContext = new QRCodeViewModel(Navigation, SlotDateTime);
        }
    }
}