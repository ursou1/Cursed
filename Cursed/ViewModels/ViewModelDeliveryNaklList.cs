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
    public class ViewModelDeliveryNaklList: INotifyPropertyChanged
    {

        DB DB;

        public ObservableCollection<DeliveryNote> DeliveryNotes { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public int DeliveryCount { get; set; }
        public int Counts;
        public int Prices;
        //public int monet;
        //public int sum1;
        public int sum2;
        public MiniCommand DeliveryCounting { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModelDeliveryNaklList()
        {
            DB = DB.GetDb();

            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);


            //DeliveryCounting = new MiniCommand(() =>
            //{
            //        int monet = 0;
            //        int sum1 = 0;
            //    foreach (var f in selectedDeliveryNote.Products)
            //    {
            //        monet = f.Count * f.Price;
            //        sum1 += monet;
            //        //Counts += f.Count;
            //        //Prices += f.Price;
            //        //DeliveryCount = Counts * Prices;
            //    }
            //        DeliveryCount = sum1;
            //        SignalChanged(nameof(DeliveryCount));
            //});

        }
        
        private DeliveryNote selectedDeliveryNote;

        public DeliveryNote SelectedDeliveryNote
        {
            get => selectedDeliveryNote;
            set
            {
                selectedDeliveryNote = value;
                try
                {
                    if (selectedDeliveryNote.Products == null || selectedDeliveryNote.Products.Count == 0)
                    {
                        System.Windows.MessageBox.Show("Список пуст");

                    }
                    else
                        Products = new ObservableCollection<Product>(selectedDeliveryNote.Products);
                        SignalChanged("Products");
                    int monet = 0;
                    int sum1 = 0;
                    foreach (var f in selectedDeliveryNote.Products)
                    {
                        monet = f.Count * f.Price;
                        sum1 += monet;
                    }
                    DeliveryCount = sum1;
                    SignalChanged(nameof(DeliveryCount));

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
