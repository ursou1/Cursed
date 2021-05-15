using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed.Enttities
{
    public class DepartNote
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime DepartDate { get; set; }
        public List<Product> Products { get; set; }//
    }
}
