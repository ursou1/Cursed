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

        
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModelDeliveryNaklList()
        {
            DB = DB.GetDb();

            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            Products = new ObservableCollection<Product>(DB.Products);

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
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
