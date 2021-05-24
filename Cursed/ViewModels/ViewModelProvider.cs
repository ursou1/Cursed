using Cursed.Commands;
using Cursed.Enttities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;

namespace Cursed.ViewModels
{
    public class ViewModelProvider: ViewModelBase, INotifyPropertyChanged
    {
        DB DB;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Provider> Providers { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        #region Список мини-команд
        public MiniCommand AddProvider { get; set; } 
        public MiniCommand SaveProvider { get; set; } 
        public MiniCommand DeleteProvider { get; set; } 
        #endregion

        #region Конструктор
        public ViewModelProvider()
        {
            DB = DB.GetDb();
            Providers = new ObservableCollection<Provider>(DB.Providers);

            #region Фильтр, поиск

            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Providers);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Provider.Name))); // эта сортировка группирует по единице товара
           // EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending)); // это сортировка по коду товара

            #endregion

            #region Команда с добавлением
            AddProvider = new MiniCommand(() =>
            {
                var provider = new Provider { Name="Имя", Email="Email" };
                DB.Providers.Add(provider);
                SelectedProvider = provider;
                RefreshListView();
            });
            #endregion

            #region Команда с сохранением
            SaveProvider = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadProviders();
                    RefreshListView();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteProvider = new MiniCommand(() =>
            {
                DB.Providers.Remove(selectedProvider);
                DB.SaveChanges();
                LoadProviders();
                RefreshListView();
            });
            #endregion
        }

        #endregion

        private Provider selectedProvider;
        public Provider SelectedProvider
        {
            get => selectedProvider;
            set
            {
                selectedProvider = value;
                SignalChanged();
            }
        }
        private void LoadProviders()
        {
            Providers = new ObservableCollection<Provider>(DB.Providers);
            SignalChanged("Providers");
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
            if (obj is Provider provider)
            {
                string tl = provider.Telephone.ToString();
                return provider.Name.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || provider.Email.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || tl.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase);//new 3 argument
            }
            return false;
        }

        // Метод ниже обновляет листвью, чтобы работал поиск после редактирования листвью. (перезаписывает в icollection уже отредаченный список и поиск снова работает)
        private void RefreshListView()
        {
            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Providers);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Provider.Name)));
            //EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending));
        }

        #endregion

    }
}
