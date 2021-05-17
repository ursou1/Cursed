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
    public class ViewModelProduct : ViewModelBase, INotifyPropertyChanged
    {
        DB DB;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<PartOfWarehouse> PartOfWarehouses { get; set; }
        public ObservableCollection<Provider> Providers { get; set; }
        public ObservableCollection<ProductType> ProductTypes { get; set; }
        public ObservableCollection<DeliveryNote> DeliveryNotes { get; set; }
        public ObservableCollection<DepartNote> DepartNotes { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        #region Список мини-команд
        public MiniCommand AddProduct { get; set; }
        public MiniCommand SaveProduct { get; set; }
        public MiniCommand DeleteProduct { get; set; }
        #endregion

        #region Конструктор
        public ViewModelProduct()
        {
            DB = DB.GetDb();
            Products = new ObservableCollection<Product>(DB.Products);
            PartOfWarehouses = new ObservableCollection<PartOfWarehouse>(DB.PartOfWarehouses);
            Providers = new ObservableCollection<Provider>(DB.Providers);
            ProductTypes = new ObservableCollection<ProductType>(DB.ProductTypes);
            DeliveryNotes = new ObservableCollection<DeliveryNote>(DB.DeliveryNotes);
            DepartNotes = new ObservableCollection<DepartNote>(DB.DepartNotes);

            #region Фильтр, поиск

            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Products);
            EmployeesCollectionView.Filter = FilterEmployees;
            //EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Product.Unit))); // эта сортировка группирует по единице товара
            EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending)); // это сортировка по коду товара

            #endregion

            #region Команда с добавлением
            AddProduct = new MiniCommand(() =>
            {
                var product = new Product { Code = 0, Name = "Наименование", Count = 0, Price = 0, Unit = "Ед.изм." };
                DB.Products.Add(product);
                SelectedProduct = product;
                RefreshListView();
                
            });
            #endregion

            #region Команда с сохранением
            SaveProduct = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadProducts();
                    RefreshListView();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteProduct = new MiniCommand(() =>
                {
                    DB.Products.Remove(selectedProduct);
                    DB.SaveChanges();
                    LoadProducts();
                    RefreshListView();
                });
            #endregion
        }

        #endregion

        public void Shit()
        {
            foreach (Product Products in Products)
            {
                if (Products.DepartNotes != null)
                {
                    Products.PartOfWarehouse = null;
                }
                else
                    return;
            }
        }

        private PartOfWarehouse selectedPartOfWarehouse;
        public PartOfWarehouse SelectedPartOfWarehouse
        {
            get => selectedPartOfWarehouse;
            set
            {
                selectedPartOfWarehouse = value;
            }
        }

        //new
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

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                SignalChanged();
            }
        }
        //new
        private void LoadProducts()
        {
            Products = new ObservableCollection<Product>(DB.Products);
            SignalChanged("Products");
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
            if (obj is Product product)
            {
                string pc = product.Code.ToString();//это нужно, чтобы искать в поиске по коду товара, ибо штука снизу просит string, а не int..
                return product.Name.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || product.Unit.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || pc.Contains(EmployeesFilter, StringComparison.Ordinal);//new 3 argument
            }
            return false;
        }

        // Метод ниже обновляет листвью, чтобы работал поиск после редактирования листвью. (перезаписывает в icollection уже отредаченный список и поиск снова работает)
        private void RefreshListView()
        {
            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Products);
            EmployeesCollectionView.Filter = FilterEmployees;
            //EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Product.Unit)));
            EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending));
        }

        #endregion

    }
}
