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
    public class ViewModelClient: ViewModelBase, INotifyPropertyChanged
    {
        DB DB;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Client> Clients { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        #region Список мини-команд
        public MiniCommand AddClient { get; set; } 
        public MiniCommand SaveClient { get; set; } 
        public MiniCommand DeleteClient { get; set; }
        #endregion

        #region Конструктор
        public ViewModelClient()
        {
            DB = DB.GetDb();
            Clients = new ObservableCollection<Client>(DB.Clients);

            #region Фильтр, поиск

            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Clients);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Client.Name))); // эта сортировка группирует по единице товара
            //EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Client.Code), ListSortDirection.Ascending)); // это сортировка по коду товара

            #endregion


            #region Команда с добавлением
            AddClient = new MiniCommand(() =>
            {
                var client = new Client { Name = "Имя", Female = "Фамилия", FatherName = "Отчество", Address = "Адрес", Email = "Email" };
                DB.Clients.Add(client);
                SelectedClient = client;
                RefreshListView();
            });
            #endregion

            #region Команда с сохранением
            SaveClient = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadClients();
                    RefreshListView();
                }
                catch(Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteClient = new MiniCommand(() =>
            {
                DB.Clients.Remove(selectedClient);
                DB.SaveChanges();
                LoadClients();
                RefreshListView();
            });
            #endregion

        }
        #endregion

        private Client selectedClient;
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                SignalChanged();
            }
        }

        private void LoadClients()
        {
            Clients = new ObservableCollection<Client>(DB.Clients);
            SignalChanged("Clients");
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
            if (obj is Client client)
            {
                string tl = client.Telephone.ToString();//это нужно, чтобы искать в поиске по коду товара, ибо штука снизу просит string, а не int..
                return client.Name.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || client.FatherName.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || client.Female.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || client.FullName.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || client.Address.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || client.Email.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || tl.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase);//new 3 argument
            }
            return false;
        }

        // Метод ниже обновляет листвью, чтобы работал поиск после редактирования листвью. (перезаписывает в icollection уже отредаченный список и поиск снова работает)
        private void RefreshListView()
        {
            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Clients);
            EmployeesCollectionView.Filter = FilterEmployees;
            EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Client.Name)));
           // EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Client.Name), ListSortDirection.Ascending));
        }

        #endregion
    }
}
