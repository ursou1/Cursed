using Cursed.Enttities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cursed.ViewModels
{
    public class ViewModelMoveProduct: INotifyPropertyChanged
    {
        DB DB;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PartOfWarehouse> PartOfWarehouses { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public ViewModelMoveProduct()
        {
            DB = DB.GetDb();
            PartOfWarehouses = new ObservableCollection<PartOfWarehouse>(DB.PartOfWarehouses);
        }

        private PartOfWarehouse selectedPartOfWarehouse;
        public PartOfWarehouse SelectedPartOfWarehouse
        {
            get => selectedPartOfWarehouse;
            set
            {
                selectedPartOfWarehouse = value;
                if (selectedPartOfWarehouse != null)
                    Products = new ObservableCollection<Product>(selectedPartOfWarehouse.Products);
                else
                    Products = new ObservableCollection<Product>();
                SignalChanged("Products");
            }
        }
    }
}
