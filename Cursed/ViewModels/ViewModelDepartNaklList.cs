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

                    }
                    else
                        Products = new ObservableCollection<Product>(selectedDepartNote.Products);
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
