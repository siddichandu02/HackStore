using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms.Internals;

namespace StoreBuy.Models
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ItemCatalogueModel : INotifyPropertyChanged
    {
        
        [DataMember(Name = "ItemId")]
        public virtual long ItemId { get; set; }

        [DataMember(Name = "ItemName")]
        public virtual string ItemName { get; set; }

        [DataMember(Name = "Price")]
        public virtual float Price { get; set; }

        [DataMember(Name = "ItemDescription")]
        public virtual string ItemDescription { get; set; }

        [DataMember(Name = "CategoryName")]
        public virtual string CategoryName { get; set; }

        private long quantity;

        [DataMember(Name = "Quantity")]
        public virtual long Quantity {
            get
            {
                return  quantity;
            }
            set
            {
                 quantity = value;
                 NotifyPropertyChanged();
            }
        }


        [DataMember(Name = "ItemImage")]
        public virtual byte[] ItemImage { get; set; }

        [DataMember(Name = "IsInCart")]
        public virtual bool IsInCart { get; set; }


        [DataMember(Name = "quantities")]
        public List<object> Quantities { get; set; } = new List<object> { 1, 2, 3, 4, 5 };

        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember(Name = "TotalPrice")]
        public float TotalPrice { get; set; } 

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

