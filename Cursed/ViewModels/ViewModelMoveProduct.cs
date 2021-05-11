using Cursed.Commands;
using Cursed.Enttities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cursed.ViewModels
{
    public class ViewModelMoveProduct : INotifyPropertyChanged
    {
        DB DB;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PartOfWarehouse> PartOfWarehouses { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public MiniCommand MoveProduct { get; set; }

        public ObservableCollection<Product> Numbers { get; set; }//new
        public ViewModelMoveProduct()
        {
            DB = DB.GetDb();
            PartOfWarehouses = new ObservableCollection<PartOfWarehouse>(DB.PartOfWarehouses);
            MoveProduct = new MiniCommand(() =>
            {
                DB.PartOfWarehouses.Add(selectedProduct.PartOfWarehouse); // проверить все binding в front'e
                //DB.
                //if(selectedPartOfWarehouse2 != null && selectedPartOfWarehouse != null)
                // {
               // selectedPartOfWarehouse.Add(Numbers);  КАК ЗАСТАВИТЬ ЭТУ КНОПКУ РАБОТАТЬ
                 //   DB.PartOfWarehouses.Add(selectedPartOfWarehouse2);
                 //   DB.SaveChanges();
                //}
               // else
               // {
               //     System.Windows.MessageBox.Show("Заполните все поля");
               // }
            });
        }
        private Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                SignalChanged();
            }
        }

        private PartOfWarehouse selectedPartOfWarehouse;
        public PartOfWarehouse SelectedPartOfWarehouse
        {
            get => selectedPartOfWarehouse;
            set
            {
                selectedPartOfWarehouse = value;
                try
                {
                    if (selectedPartOfWarehouse.Products == null)
                    {
                        System.Windows.MessageBox.Show("Вы выбрали пустой отсек");
                    }
                    else
                        Products = new ObservableCollection<Product>(selectedPartOfWarehouse.Products);
                        SignalChanged("Products");
                }
                catch(Exception ex)
                {
                }
            }
        }

        private PartOfWarehouse selectedPartOfWarehouse2;
        public PartOfWarehouse SelectedPartOfWarehouse2
        {
            get => selectedPartOfWarehouse2;
            set
            {
                selectedPartOfWarehouse2 = value;
                //SignalChanged("Products"); нам не надо
                System.Windows.MessageBox.Show("Вы выбрали");
            }
        }
    }
}
