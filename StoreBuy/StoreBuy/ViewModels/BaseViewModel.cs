using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Event handler

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
