using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StoreBuy.ViewModels.Detail
{
    public class QRCodeViewModel : BaseViewModel
    {
        private string qrtext;

        private INavigation Navigation;

        public string QRText
        {
            get
            {
                return qrtext;
            }

            set
            {
                qrtext = value;
                NotifyPropertyChanged();
            }
        }

        public QRCodeViewModel(INavigation navigation,string SlotDateTime)
        {
            QRText = SlotDateTime;
            Navigation = navigation;
        }
    }
}
