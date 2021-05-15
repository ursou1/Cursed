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

        public WinDeliveryNaklEd  WinDeliveryNaklEd = new WinDeliveryNaklEd();

        #region signalchanged

        protected void SignalChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion

        #region Commands

        public MiniCommand AddDeliveryNakl { get; set; } 
        public MiniCommand EdDeliveryNakl { get; set; } 
        public MiniCommand CheckDeliveryNakl { get; set; } 
        public MiniCommand DeleteDeliveryNakl { get; set; } 

        #endregion

        public ObservableCollection<DeliveryNote> DeliveryNotes { get; set; }
        public ViewModelDeliveryNakl()
        {
            DB = DB.GetDb();
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            SignalChanged("DeliveryNotes");

            #region Команда редактировать 
            EdDeliveryNakl = new MiniCommand(() =>
            {
                if(selectedDeliveryNote != null)
                {
                    window = new WinDeliveryNaklEd(SelectedDeliveryNote);
                    window.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Выберите поле");
                }
            });
            #endregion

            #region Команда удалить
            DeleteDeliveryNakl = new MiniCommand(() =>
            {
                DB.DeliveryNotes.Remove(selectedDeliveryNote);
                DB.SaveChanges();
                DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
                SignalChanged("DeliveryNotes");
            });
            #endregion

            #region Команда добавить
            AddDeliveryNakl = new MiniCommand(() =>
            {
                var deliveryNote = new DeliveryNote { };
                DB.DeliveryNotes.Add(deliveryNote);
                SelectedDeliveryNote = deliveryNote;
                DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
                SignalChanged("DeliveryNotes");
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
                window = new WinDeliveryNaklEd(SelectedDeliveryNote);
                window.ShowDialog();
            });
            #endregion

        }

        private DeliveryNote selectedDeliveryNote; /*= new DeliveryNote { };*/
        public DeliveryNote SelectedDeliveryNote
        {
            get => selectedDeliveryNote;
            set
            {
                selectedDeliveryNote = new DeliveryNote { };
                Set(ref selectedDeliveryNote, value);
                SignalChanged();
            }
        }



    }
}
