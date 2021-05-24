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
    public class ViewModelDepartNakl: ViewModelBase, INotifyPropertyChanged
    {
        DB DB;
        public Window window { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public ObservableCollection<DepartNote> DepartNotes { get; set; }
        public ObservableCollection<Client> Clients { get; set; }

        public MiniCommand AddDepartNakl { get; set; }
        public MiniCommand SaveDepartNakl { get; set; }
        public MiniCommand CheckDepartNakl { get; set; }
        public MiniCommand DeleteDepartNakl { get; set; }

        public ViewModelDepartNakl()
        {
            DB = DB.GetDb();
            DepartNotes = new ObservableCollection<DepartNote>(DB.DepartNotes);
            Clients = new ObservableCollection<Client>(DB.Clients);
            SignalChanged("DeliveryNotes");

            #region Фильтр, поиск

            EmployeesCollectionView = CollectionViewSource.GetDefaultView(DepartNotes);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(DepartNote.Clients))); // эта сортировка группирует по единице товара
           // EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending)); // это сортировка по коду товара

            #endregion

            #region Команда с добавлением

            AddDepartNakl = new MiniCommand(() =>
            {
                var departNote = new DepartNote { };
                DB.DepartNotes.Add(departNote);
                SelectedDepartNote = departNote;
                RefreshListView();
            });
            #endregion

            #region Команда с сохранением
            SaveDepartNakl = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadDepartNote();
                    RefreshListView();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteDepartNakl = new MiniCommand(() =>
            {
                DB.DepartNotes.Remove(selectedDepartNote);
                DB.SaveChanges();
                LoadDepartNote();
                RefreshListView();
            });
            #endregion

            #region команда с открытием листа с продуктами
            CheckDepartNakl = new MiniCommand(() =>
            {
                window = new WinDepartNaklList(); // окно пока не расписано
                window.ShowDialog();
            });
            #endregion

        }

        private DepartNote selectedDepartNote;
        public DepartNote SelectedDepartNote
        {
            get => selectedDepartNote;
            set
            {
                selectedDepartNote = value;
                SignalChanged();
            }
        }

        private void LoadDepartNote()
        {
            DepartNotes = new ObservableCollection<DepartNote>(DB.DepartNotes);
            SignalChanged("DepartNotes");
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
            if (obj is DepartNote departNote)
            {
                string nm = departNote.DepartDate.ToString();
                string dp = departNote.Number.ToString();
                return dp.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || nm.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        // Метод ниже обновляет листвью, чтобы работал поиск после редактирования листвью. (перезаписывает в icollection уже отредаченный список и поиск снова работает)
        private void RefreshListView()
        {
            EmployeesCollectionView = CollectionViewSource.GetDefaultView(DepartNotes);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(DepartNote.Clients)));
            //EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending));
        }

        #endregion

    }
}
