using Cursed.Commands;
using Cursed.Enttities;
using REghZyFramework.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cursed.ViewModels
{
    public class ViewModelSettings: INotifyPropertyChanged
    {
        DB DB;

        public event PropertyChangedEventHandler PropertyChanged;
        public int ProductCount { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<PartOfWarehouse> PartOfWarehouses { get; set; }
        public ObservableCollection<DepartNote> DepartNotes { get; set; }
        public ObservableCollection<DeliveryNote> DeliveryNotes { get; set; }
        public MiniCommand AllProduct { get; set; }
        public MiniCommand CountingProduct { get; set; }
        public MiniCommand DarkTheme { get; set; }
        public MiniCommand LightTheme { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public ViewModelSettings()
        {
            DB = DB.GetDb();
            PartOfWarehouses = new ObservableCollection<PartOfWarehouse>(DB.PartOfWarehouses);
            Products = new ObservableCollection<Product>(DB.Products);
            DepartNotes = new ObservableCollection<DepartNote>(DB.DepartNotes);
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            SignalChanged("PartOfWareHouses");
            SignalChanged("Products");
            SignalChanged("DepartNotes");
            SignalChanged("DeliveryNotes");

            //ProductCount = Products.Where(s => s.PartOfWarehouse == s.PartOfWarehouse).Count(); Если мы хотим сразу отобразить число товаров на складе
            //SignalChanged(nameof(ProductCount));

            CountingProduct = new MiniCommand(() =>
            {
                //ProductCount = Products.Where(s => s.PartOfWarehouse == s.PartOfWarehouse).Count();
                //SignalChanged(nameof(ProductCount));

                //for (int i = 0; i < Products.Count;i++)
                //{
                //    if (Products[i].DeliveryNotes != null & Products[i].DepartNotes == null)
                //    {
                //        ProductCount++;
                //    }
                //    else return;
                //}
                //ProductCount = 0;
                Products = new ObservableCollection<Product>(DB.Products);
                ProductCount = 0;
                foreach (var f in Products)
                {
                    if (f.DepartNotes == null)
                    {
                        ProductCount++;
                    }
                }
                SignalChanged(nameof(ProductCount));
            });


            //CountingProduct = new MiniCommand(() =>
            //{
            //    ProductCount = Products.Where(s => s.PartOfWarehouse == s.PartOfWarehouse).Count();
            //    SignalChanged(nameof(ProductCount));
            //});

            DarkTheme = new MiniCommand(() =>
            {
                ThemesController.SetTheme(ThemesController.ThemeTypes.ColourfulDark);
            });
            LightTheme = new MiniCommand(() =>
            {
                ThemesController.SetTheme(ThemesController.ThemeTypes.ColourfulLight);
            });

        }

    }
}
