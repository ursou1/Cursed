using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cursed.Enttities
{
    public class Product: INotifyPropertyChanged
    {
        private PartOfWarehouse partOfWarehouse;
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string Unit { get; set; }
        public PartOfWarehouse PartOfWarehouse { get => partOfWarehouse; set { partOfWarehouse = value; Signal(); } }
        public ProductType ProductType { get; set; }
        public DepartNote DepartNotes { get; set; }
        public DeliveryNote DeliveryNotes  { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}
