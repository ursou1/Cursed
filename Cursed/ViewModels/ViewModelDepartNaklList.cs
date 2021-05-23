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
    public class ViewModelDepartNaklList: INotifyPropertyChanged
    {
        DB DB;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<DepartNote> DepartNotes { get; set; }
        public int DepartCount { get; set; }
        public int Counts;
        public int Prices;
        //public int monet;
        //public int sum1;
        public int sum2;
        public MiniCommand DepartCounting { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public ViewModelDepartNaklList()
        {
            DB = DB.GetDb();

            DepartNotes = new ObservableCollection<DepartNote>(DB.DepartNotes);
        }

        private DepartNote selectedDepartNote;

        public DepartNote SelectedDepartNote
        {
            get => selectedDepartNote;
            set
            {
                selectedDepartNote = value;
                try
                {
                    if (selectedDepartNote.Products == null || selectedDepartNote.Products.Count == 0)
                    {
                        System.Windows.MessageBox.Show("Список пуст");
                        Products = new ObservableCollection<Product>();// ЭТО ДЕЛАЕТ СПИСОК ПУСТЫМ ЕСЛИ МЫ ВЫБРАЛИ ПУСТОЙ ОТСЕК
                        SignalChanged("Products");
                        DepartCount = 0;
                        SignalChanged(nameof(DepartCount));
                    }
                    else
                    {
                        Products = new ObservableCollection<Product>(selectedDepartNote.Products);
                    SignalChanged("Products");
                    int monet = 0;
                    int sum1 = 0;
                    foreach (var f in selectedDepartNote.Products)
                    {
                        monet = f.Count * f.Price;
                        sum1 += monet;
                    }
                    DepartCount = sum1;
                    SignalChanged(nameof(DepartCount));
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
