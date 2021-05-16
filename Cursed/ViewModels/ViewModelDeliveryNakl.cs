using Cursed.Commands;
using Cursed.Enttities;
using Cursed.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Cursed.ViewModels
{
    public class ViewModelDeliveryNakl: INotifyPropertyChanged
    {
        DB DB;
        public event PropertyChangedEventHandler PropertyChanged;

        public Window window { get; set; }

        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        #region Commands

        public MiniCommand AddDeliveryNakl { get; set; } 
        public MiniCommand SaveDeliveryNakl { get; set; } 
        public MiniCommand CheckDeliveryNakl { get; set; } 
        public MiniCommand DeleteDeliveryNakl { get; set; } 

        #endregion

        public ObservableCollection<DeliveryNote> DeliveryNotes { get; set; }
        public ViewModelDeliveryNakl()
        {
            DB = DB.GetDb();
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            SignalChanged("DeliveryNotes");

            #region Команда с добавлением

            AddDeliveryNakl = new MiniCommand(() =>
            {
                var deliveryNote = new DeliveryNote {  };
                DB.DeliveryNotes.Add(deliveryNote);
                SelectedDeliveryNote = deliveryNote;
            });
            #endregion

            #region Команда с сохранением
            SaveDeliveryNakl = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadDeliveryNotes();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteDeliveryNakl = new MiniCommand(() =>
            {
                DB.DeliveryNotes.Remove(selectedDeliveryNote);
                DB.SaveChanges();
                LoadDeliveryNotes();
            });
            #endregion

            #region команда с открытием листа с продуктами
            CheckDeliveryNakl = new MiniCommand(() =>
            {
                window = new WinDeliveryNaklList();
                window.ShowDialog();
            });
            #endregion
        }

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

        private void LoadDeliveryNotes()
        {
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            SignalChanged("DeliveryNotes");
        }

    }
}
