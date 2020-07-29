using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace StoreBuy.Models.Detail
{
    [Preserve(AllMembers = true)]
    public class StoreModel
    {
        #region Properties
        public virtual long StoreId { get; set; }
        public virtual string StoreName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string StoreAddress { get; set; }
        public List<ItemCatalogueModel> ListItems { set; get; }
        public virtual string ItemsCount { set; get; }
        public long SlotDays { set; get; }
        public DateTime SlotStartTime { set; get; }
        public DateTime SlotEndTime { set; get; }
        public long SlotDuration { set; get; }
        public long Latitude { set; get; }
        public long Longitude { set; get; }

        #endregion
    }
}
