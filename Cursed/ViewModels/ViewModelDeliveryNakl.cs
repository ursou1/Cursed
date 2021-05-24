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
using System.Windows.Data;

namespace Cursed.ViewModels
{
    public class ViewModelDeliveryNakl: ViewModelBase, INotifyPropertyChanged
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
        public ObservableCollection<Provider> Providers { get; set; }
        public ViewModelDeliveryNakl()
        {
            DB = DB.GetDb();
            Providers = new ObservableCollection<Provider>(DB.Providers);
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);

            #region Фильтр, поиск

            EmployeesCollectionView = CollectionViewSource.GetDefaultView(DeliveryNotes);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(DeliveryNote.Providers))); // эта сортировка группирует по единице товара
            //EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending)); // это сортировка по коду товара

            #endregion

            #region Команда с добавлением

            AddDeliveryNakl = new MiniCommand(() =>
            {
                var deliveryNote = new DeliveryNote {  };
                DB.DeliveryNotes.Add(deliveryNote);
                SelectedDeliveryNote = deliveryNote;
                RefreshListView();
            });
            #endregion

            #region Команда с сохранением
            SaveDeliveryNakl = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadDeliveryNotes();
                    RefreshListView();
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
                RefreshListView();
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

        private Provider selectedProvider;
        public Provider SelectedProvider
        {
            get => selectedProvider;
            set
            {
                selectedProvider = value;
            }
        }
        private void LoadDeliveryNotes()
        {
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            SignalChanged("DeliveryNotes");
        }

        #region поляна для работы с фильтрами, поиском
        public ICollectionView EmployeesCollectionView { get; set; }

        private string _employeesFilter = string.Empty;
        public string EmployeesFilter
        {
            get
            {
                return _employeesFilter;
            }
            set
            {
                _employeesFilter = value;
                OnPropertyChanged(nameof(EmployeesFilter));
                EmployeesCollectionView.Refresh();
            }
        }
        private bool FilterEmployees(object obj)
        {
            if (obj is DeliveryNote deliveryNote)
            {
                string dt = deliveryNote.DeliveryDate.ToString();//это нужно, чтобы искать в поиске по коду товара, ибо штука снизу просит string, а не int..
                string nm = deliveryNote.Number.ToString();//это нужно, чтобы искать в поиске по коду товара, ибо штука снизу просит string, а не int..
                return nm.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || dt.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase);//new 3 argument
            }
            return false;
        }

        // Метод ниже обновляет листвью, чтобы работал поиск после редактирования листвью. (перезаписывает в icollection уже отредаченный список и поиск снова работает)
        private void RefreshListView()
        {
            EmployeesCollectionView = CollectionViewSource.GetDefaultView(DeliveryNotes);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(DeliveryNote.Providers)));
            //EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending));
        }

        #endregion

    }
}
