using System;
using System.Collections.Generic;
using System.Text;

namespace StoreBuy.Models
{
    public class OrderModel
    {
        public  long OrderId { get; set; }
        public  string SlotTime { set; get; }
        public  string SlotDate
        { set; get; }
        public  string StoreName { set; get; }
        public  string Phone { get; set; }
        public List<ItemCatalogueModel> ListItems { set; get; }
        public  string StoreAddress { get; set; }
        public DateTime SlotDateTime { get; set; }
        public bool QRCodeIsVisible { get; set; }

    }
}
