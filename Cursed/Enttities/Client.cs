using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed.Enttities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Female { get; set; }
        public string FatherName { get; set; }
        public int Telephone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<DepartNote> DepartNotes { get; set; }
        public string FullName { get => $"{Name} {Female} {FatherName}"; }

    }
}
