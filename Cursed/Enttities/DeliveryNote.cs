using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed.Enttities
{
    public class DeliveryNote
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<Product> Products { get; set; }

    }
}
