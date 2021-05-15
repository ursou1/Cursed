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
    public class ViewModelDeliveryNaklEd: INotifyPropertyChanged
    {
        DB DB;

        public ObservableCollection<DeliveryNote> DeliveryNotes { get; set; }
        public MiniCommand SaveDeliveryNote { get; set; }

        #region selected

        private DeliveryNote selectedDeliveryNote;
        public DeliveryNote SelectedDeliveryNote
        {
            get => selectedDeliveryNote;
            set
            {
                selectedDeliveryNote = value;
                SignalChanged();
            }
        }

        #endregion

        public ViewModelDeliveryNaklEd()
        {
            DB = DB.GetDb();
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            SignalChanged("DeliveryNotes");

            SaveDeliveryNote = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
                    SignalChanged("DeliveryNotes");

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });

        }

        public ViewModelDeliveryNaklEd(DeliveryNote selectedDeliveryNote) : this()
        {
            SelectedDeliveryNote = selectedDeliveryNote;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
