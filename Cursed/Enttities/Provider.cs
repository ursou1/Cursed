using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed.Enttities
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Telephone { get; set; }
        public string Email { get; set; }

        public List<Product> Products { get; set; }
        public List<DeliveryNote> DeliveryNotes { get; set; }
    }
}
