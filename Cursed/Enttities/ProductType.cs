using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed.Enttities
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
