using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed.Enttities
{
    public class Product
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int Cost { get; set; }
        public string Unit { get; set; }
        public PartOfWarehouse PartOfWarehouse { get; set; }
        public Provider Provider { get; set; }
        public ProductType ProductType { get; set; }
        public List<DepartNote> DepartNotes { get; set; }
        public List<DeliveryNote> DeliveryNotes  { get; set; }
        
       
    }
}
