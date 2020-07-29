using StoreBuy.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace StoreBuy.Models.Detail
{
    [Preserve(AllMembers = true)]
    public class Slot : BaseViewModel
    {
        #region Fields

        private bool isSlotFilled;
        private bool isSlotEmpty;

        #endregion

        #region Properties

        public string SlotTime { get; set; }
        public string SlotDate { get; set; }
        public string ButtonText { get; set; }
        public bool ButtonCheck { get; set; }      
        public bool IsSlotFilled
        {
            get
            {
                return isSlotFilled;
            }
            set
            {
                isSlotFilled = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsSlotEmpty
        {
            get
            {
                return isSlotEmpty;
            }
            set
            {
                isSlotEmpty = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion
    }
}