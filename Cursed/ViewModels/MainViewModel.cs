using Cursed.Enttities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cursed.ViewModels
{
    public class MainViewModel
    {
        DB DB;
        public ObservableCollection<Product> Products { get; set; }
        public MainViewModel()
        {
            DB = DB.GetDb();
            Products = new ObservableCollection<Product>(DB.Products);

        }
    }
}
